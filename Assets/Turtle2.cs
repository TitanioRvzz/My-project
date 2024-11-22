using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Turtle2 : MonoBehaviour
{
    [SerializeField] Tilemap terrenoTilemap;             
    [SerializeField] RuleTile arenaMojadaRuleTile;        
    [SerializeField] TileBase aguaTile;                    
    [SerializeField] float moveInterval = 1f;            
    private Vector3Int currentCell;                       
    public Vector2 respawnposition;
    public float dilayrespawn = 1f;  
    public GameObject objectToClone;
    public float probabilityToCloneTwo = 0.2f;
    public Transform respawnPoint, secondpoint;


    private void Start()
    {
        currentCell = terrenoTilemap.WorldToCell(transform.position);
        StartCoroutine(MoveRoutine()); 
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);
            if (IsOnMojadaTile())
            {
                MoveAlongMojadaPath();
            }
            else
            {
                MoveRandomly();
            }
        }
    }

    private bool IsOnMojadaTile()
    {
        TileBase tileAtCurrentCell = terrenoTilemap.GetTile(currentCell);
        return tileAtCurrentCell != null && tileAtCurrentCell == arenaMojadaRuleTile;
    }

    private void MoveAlongMojadaPath()
    {
        Vector3Int[] directions = { Vector3Int.down, Vector3Int.left, Vector3Int.right };

        bool moved = false;
        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
            {
                currentCell = adjacentCell;
                transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
                moved = true;
                break;
            }

            if (!moved)
            {
             CheckForWater();
            }
        }
    }

    private void MoveRandomly()
    {
        Vector3Int[] randomDirections = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int randomDirection = randomDirections[Random.Range(0, randomDirections.Length)];

        Vector3Int adjacentCell = currentCell + randomDirection;
        TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);
        if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
        {
            currentCell = adjacentCell;
            transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
        }
    }

    private void CheckForWater()
    {
        Vector3Int[] directions = { Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == aguaTile)
            {
                currentCell = adjacentCell;
                transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
                Debug.Log("Tortuga se mueve hacia el agua!");
                Destroy(gameObject);
                return;
            }
        }
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
        float randomValue = Random.value;

        if (randomValue <= probabilityToCloneTwo)
        {
            Instantiate(objectToClone, respawnPoint.position, respawnPoint.rotation);  
            Instantiate(objectToClone, respawnPoint.position + new Vector3(2f, 0f, 0f), respawnPoint.rotation); 
          
        }
        else
        {
            
            Instantiate(objectToClone, respawnPoint.position, respawnPoint.rotation);
         
        }
    }
    
  private void InstantiateAndActivateComponents(Vector3 position)
    {
      
        GameObject clone = Instantiate(objectToClone, position, Quaternion.identity);
        Collider2D collider = clone.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true; 
        }
        MonoBehaviour[] scripts = clone.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
    }
    
}