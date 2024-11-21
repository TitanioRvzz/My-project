using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class TurtleMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveInterval;
    public LayerMask obstaculo;
    public float Radio0;
    public Vector2 offsetpunto;
    public Transform jugador; 

    private Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
    private Vector3Int currentCell;
    private bool isHit = false; 
    public Vector2 respawnposition;
    public float dilayrespawn = 1f;  
    public GameObject objectToClone;
    public float probabilityToCloneTwo = 0.2f;
    public Transform respawnPoint;

    void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);
        StartCoroutine(MoveRoutine());
        
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (!isHit)
            {
                yield return new WaitForSeconds(moveInterval);
                MoveToRandomAdjacentCell();
            }
            yield return null;
        }
    }

    private void MoveToRandomAdjacentCell()
  {
        Vector3Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3Int targetCell = currentCell + randomDirection;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCell);

        Vector3Int jugadorCell = tilemap.WorldToCell(jugador.position);

        if (tilemap.HasTile(targetCell) && !Physics2D.OverlapCircle((Vector2)targetPosition + offsetpunto, Radio0, obstaculo) && targetCell != jugadorCell)
        {
            currentCell = targetCell;
            transform.position = targetPosition;
        }
    }

    public void HitByPlayer(Vector2 direction)
    {
        isHit = true;

        Vector3Int hitDirection = new Vector3Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), 0);
        Vector3Int targetCell = currentCell + hitDirection * 2;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCell);

        if (tilemap.HasTile(targetCell) && !Physics2D.OverlapCircle((Vector2)targetPosition + offsetpunto, Radio0, obstaculo))
        {
            currentCell = targetCell;
            transform.position = targetPosition;
        }

        Invoke(nameof(ResetHit), 0.5f);
    }

    private void ResetHit()
    {
        isHit = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       Gizmos.DrawWireSphere((Vector2)tilemap.GetCellCenterWorld(currentCell), Radio0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
    if(other.gameObject.CompareTag("Ocean"))
    {
           CloneObjectWithProbability();
           Destroy(this.gameObject);
       
     
    }    
    }
    private void Respawn()
    {
        if(objectToClone !=null)
        {
            Instantiate(gameObject, objectToClone.transform.position, Quaternion.identity);
        }

    }
     private void CloneObjectWithProbability()
    {
        // Generamos un número aleatorio entre 0 y 1
        float randomValue = Random.value;

        // Si el número aleatorio es menor o igual a la probabilidad para 2 clones, instanciamos 2 clones
        if (randomValue <= probabilityToCloneTwo)
        {
            Instantiate(objectToClone, respawnPoint.position, respawnPoint.rotation);  // Primer clon
            Instantiate(objectToClone, respawnPoint.position + new Vector3(2f, 0f, 0f), respawnPoint.rotation);  // Segundo clon, ligeramente desplazado
            Debug.Log("2 Clones generados.");
        }
        else
        {
            // Si no, solo instanciamos 1 clon
            Instantiate(objectToClone, respawnPoint.position, respawnPoint.rotation);
            Debug.Log("1 Clon generado.");
        }
    }
    
  private void InstantiateAndActivateComponents(Vector3 position)
    {
        // Instancia el clon en la posición deseada
        GameObject clone = Instantiate(objectToClone, position, Quaternion.identity);

        // Activamos el Collider y el Script del clon si están desactivados
        Collider2D collider = clone.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;  // Aseguramos que el Collider esté activado
        }

        // Activamos los scripts del clon
        MonoBehaviour[] scripts = clone.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;  // Aseguramos que todos los scripts estén activados
        }
    }
 
}




