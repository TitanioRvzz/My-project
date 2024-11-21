using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Turtle2 : MonoBehaviour
{
    [SerializeField] Tilemap terrenoTilemap;               // Tilemap del entorno
    [SerializeField] RuleTile arenaMojadaRuleTile;         // Tile de arena mojada
    [SerializeField] TileBase aguaTile;                    // Tile de agua
    [SerializeField] float moveInterval = 1f;              // Intervalo de movimiento

    private Vector3Int currentCell;                         // Posición de la tortuga en la cuadrícula

    private void Start()
    {
        // Establece la posición inicial de la tortuga según el Tilemap
        currentCell = terrenoTilemap.WorldToCell(transform.position);
        StartCoroutine(MoveRoutine());  // Comienza el ciclo de movimiento
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            // Comprobamos si la tortuga está sobre un tile de arena mojada
            if (IsOnMojadaTile())
            {
                // Si está sobre un tile de arena mojada, mueve hacia el agua
                MoveAlongMojadaPath();
            }
            else
            {
                // Si no está sobre un tile de arena mojada, realiza un movimiento aleatorio
                MoveRandomly();
            }
        }
    }

    private bool IsOnMojadaTile()
    {
        // Verifica si la tortuga está sobre un tile de arena mojada
        TileBase tileAtCurrentCell = terrenoTilemap.GetTile(currentCell);
        return tileAtCurrentCell != null && tileAtCurrentCell == arenaMojadaRuleTile;
    }

    private void MoveAlongMojadaPath()
    {
        // Si la tortuga está sobre un tile de arena mojada, sigue el camino hacia abajo o hacia el agua
        Vector3Int[] directions = { Vector3Int.down, Vector3Int.left, Vector3Int.right };  // Busca hacia abajo y horizontalmente

        bool moved = false;
        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
            {
                // Si encuentra un tile de arena mojada, se mueve hacia allí
                currentCell = adjacentCell;
                transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
                moved = true;
                break; // Solo se mueve a la primera casilla disponible
            }
        }

        if (!moved)
        {
            // Si no se mueve, intenta buscar el agua
            CheckForWater();
        }
    }

    private void MoveRandomly()
    {
        // Realiza un movimiento aleatorio (en cualquier dirección)
        Vector3Int[] randomDirections = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int randomDirection = randomDirections[Random.Range(0, randomDirections.Length)];

        Vector3Int adjacentCell = currentCell + randomDirection;
        TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);

        // Si encuentra un tile de arena mojada en la dirección aleatoria, se mueve hacia allí
        if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
        {
            currentCell = adjacentCell;
            transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
        }
    }

    private void CheckForWater()
    {
        // Verifica si la tortuga está en la casilla del agua
        Vector3Int[] directions = { Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == aguaTile)
            {
                // Si encuentra agua, se mueve hacia allí y destruye el objeto
                currentCell = adjacentCell;
                transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
                Debug.Log("Tortuga se mueve hacia el agua!");
                Destroy(gameObject);
                return;
            }
        }
    }
}