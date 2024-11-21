using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Turtle2 : MonoBehaviour
{
    [SerializeField] Tilemap terrenoTilemap;
    [SerializeField] RuleTile arenaMojadaRuleTile;
    [SerializeField] TileBase aguaTile;
    [SerializeField] float moveInterval = 2f;

    private Vector3Int currentCell;
    private bool isMovingToWater = false;

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

            if (!isMovingToWater)
            {
                CheckForMojadaTiles();
                if (!isMovingToWater)
                {
                    MoveRandom();
                }
            }
            else
            {
                MoveToWater();
            }
        }
    }

    private void MoveRandom()
    {
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3Int targetCell = currentCell + randomDirection;

        if (terrenoTilemap.HasTile(targetCell))
        {
            currentCell = targetCell;
            transform.position = terrenoTilemap.GetCellCenterWorld(targetCell);
        }
    }

    private void CheckForMojadaTiles()
    {
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            if (terrenoTilemap.GetTile(adjacentCell) == arenaMojadaRuleTile)
            {
                isMovingToWater = true;
                return;
            }
        }
    }

    private void MoveToWater()
    {
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            if (terrenoTilemap.GetTile(adjacentCell) == aguaTile)
            {
                // Acción de salvación
                SaveTurtle();
                return;
            }
        }
    }

    private void SaveTurtle()
    {
        Debug.Log("Tortuga salvada!");
        Destroy(gameObject);
        // Aquí puedes sumar puntos o activar animaciones
    }
}