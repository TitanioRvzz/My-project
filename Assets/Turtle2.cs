using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Turtle2 : MonoBehaviour
{
    public int points = 8;
    [SerializeField] RuleTile arenaMojadaRuleTile;
    [SerializeField] TileBase aguaTile;
    [SerializeField] float moveInterval = 1f;
    private Vector3Int currentCell;
    public Vector2 respawnposition;
    public float dilayrespawn = 1f;
    public GameObject objectToClone;
    public float probabilityToCloneTwo = 0.2f;
    public Transform respawnPoint, secondpoint;
    [SerializeField] int moveHor = 1;
    private Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
    public bool isHit = false;
    public Tilemap tilemap;
    public LayerMask obstaculo;
    public float Radio0;
    public Vector2 offsetpunto;
    public Transform jugador;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentCell = tilemap.WorldToCell(transform.position);
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

        TileBase tileAtCurrentCell = tilemap.GetTile(currentCell);
        return tileAtCurrentCell != null && tileAtCurrentCell == arenaMojadaRuleTile;
    }

    private void MoveAlongMojadaPath()
    {

        Vector3Int[] directions = { Vector3Int.right, Vector3Int.left };


        bool moved = false;

        if (TileAtAdjacentCell(Vector3Int.down) != null && TileAtAdjacentCell(Vector3Int.down) == arenaMojadaRuleTile)
        {

            currentCell += Vector3Int.down;
            transform.position = tilemap.GetCellCenterWorld(currentCell); 
            anim.SetBool("Walk", true);
            print("movimiento hacia " + Vector3Int.down);
            moved = true;
        }
        else
        {
            if (TileAtAdjacentCell(Vector3Int.left * moveHor) != null && TileAtAdjacentCell(Vector3Int.left * moveHor) == arenaMojadaRuleTile)
            {
                currentCell += Vector3Int.left * moveHor;
                transform.position = tilemap.GetCellCenterWorld(currentCell);
                print("movimiento hacia " + Vector3Int.left * moveHor);
                moved = true;
            }
            else
            {
                moveHor *= -1;
            }
        }

        if (!moved)
        {

            CheckForWater();
        }
    }

    private TileBase TileAtAdjacentCell(Vector3Int dir)
    {
        Vector3Int adjacentCell = currentCell + dir;
        return tilemap.GetTile(adjacentCell);
    }

    private void MoveRandomly()
    {

        Vector3Int[] randomDirections = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int randomDirection = randomDirections[Random.Range(0, randomDirections.Length)];
        Vector3Int adjacentCell = currentCell + randomDirection;
        TileBase tileAtAdjacentCell = tilemap.GetTile(adjacentCell);
        if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
        {
            currentCell = adjacentCell;
            transform.position = tilemap.GetCellCenterWorld(currentCell);
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);

        }
    }

        private void CheckForWater()
    {
        Vector3Int[] directions = { Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = tilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == aguaTile)
            {
                currentCell = adjacentCell;
                transform.position = tilemap.GetCellCenterWorld(currentCell);
                Debug.Log("Tortuga se mueve hacia el agua!");
                Destroy(gameObject);
                return;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            CloneObjectWithProbability();
            Destroy(this.gameObject);


        }
    }
    private void Respawn()
    {
        if (objectToClone != null)
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
    private IEnumerator Golpe()
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
        Gizmos.DrawWireSphere((Vector2)tilemap.GetCellCenterWorld(currentCell) + offsetpunto, Radio0);
    }
}