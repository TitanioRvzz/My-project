using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Turtle2 : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase aguaTile;
    [SerializeField] RuleTile arenaMojadaRuleTile;
    [SerializeField] float Radio0;
    [SerializeField] LayerMask obstaculo;
    [SerializeField] int moveHor = 1;

    private Animator anim;
    private Vector3Int currentCell;
    private Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    public int points = 8;
    [SerializeField] Vector2 offsetpunto;
    [SerializeField] float moveInterval = 5f;

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

       //Vector3Int[] directions = { Vector3Int.right, Vector3Int.left };


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

        anim.SetBool("Walk", false); // Detener animaci�n si no hay tiles mojados alrededor
    }
    private TileBase TileAtAdjacentCell(Vector3Int dir)
    {
        Vector3Int adjacentCell = currentCell + dir;
        return tilemap.GetTile(adjacentCell);
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
    private void MoveRandomly()
    {
        Vector3Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3Int targetCell = currentCell + randomDirection;

        if (CanMoveToCell(targetCell))
        {
            currentCell = targetCell;
            transform.position = tilemap.GetCellCenterWorld(currentCell);

            // Cambiar velocidad aleatoriamente
            moveInterval = Random.Range(3f, 5f);
            // anim.SetBool("Walk", true);
        }
        else
        {
            //  anim.SetBool("Walk", false); // Detener animaci�n si no se mueve
        }
    }

    private bool CanMoveToCell(Vector3Int cell)
    {
        TileBase tileAtCell = tilemap.GetTile(cell);
        if (tileAtCell == null) return false; // No hay tile en esa celda
        if (Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(cell) + (Vector3)offsetpunto, Radio0, obstaculo)) return false; // Obst�culo presente

        return true; // Puede moverse
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Debug.Log("Tortuga alcanz� el agua y fue salvada.");
            Destroy(gameObject);
        }
    }
}
