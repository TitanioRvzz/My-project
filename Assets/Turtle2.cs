using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Turtle2 : MonoBehaviour
{
    [SerializeField] Tilemap terrenoTilemap;               // Tilemap del entorno
    [SerializeField] RuleTile arenaMojadaRuleTile;         // Tile de arena mojada
    [SerializeField] TileBase aguaTile;                    // Tile de agua
    [SerializeField] float moveInterval = 1f;              // Intervalo de movimiento (ajustable según sea necesario)

    private Vector3Int currentCell;                         // Posición de la tortuga en la cuadrícula
    private bool isMovingToWater = false;                   // Indica si la tortuga está moviéndose hacia el agua
    private bool isInHorizontalMovement = false;            // Indica si la tortuga está en un movimiento horizontal

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

            // Si la tortuga está en arena mojada, mueve hacia el agua, de lo contrario busca la arena mojada
            if (IsOnMojadaTile())
            {
                MoveToWater(); 
            }
            else
            {
                CheckForMojadaTiles();  // Busca tiles de arena mojada
            }
        }
    }

    private bool IsOnMojadaTile()
    {
        // Verifica si la tortuga está sobre un tile de arena mojada
        TileBase tileAtCurrentCell = terrenoTilemap.GetTile(currentCell);
        return tileAtCurrentCell != null && tileAtCurrentCell == arenaMojadaRuleTile;
    }

    private void MoveToWater()
    {
        // Si la tortuga está sobre un tile de arena mojada, sigue el camino hacia abajo o hacia el agua
        Vector3Int[] directions = { Vector3Int.down, Vector3Int.left, Vector3Int.right };  // Busca hacia abajo y horizontalmente
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

        // Si no se mueve hacia el agua, sigue buscando el camino hacia abajo o horizontalmente
        MoveDownToWater();
    }

    private void MoveDownToWater()
    {
        // Si la tortuga está sobre un tile de arena mojada, sigue el camino hacia abajo
        Vector3Int downCell = currentCell + Vector3Int.down;
        TileBase tileAtDownCell = terrenoTilemap.GetTile(downCell);

        // Si encuentra un tile de arena mojada hacia abajo, se mueve hacia allí
        if (tileAtDownCell != null && tileAtDownCell == arenaMojadaRuleTile)
        {
            currentCell = downCell;
            transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
            isInHorizontalMovement = false;  // No está en movimiento horizontal
        }
        else
        {
            // Si no hay arena mojada abajo, busca un camino horizontal
            MoveHorizontally();
        }
    }

    private void MoveHorizontally()
    {
        // Primero, intenta mover horizontalmente (izquierda o derecha)
        Vector3Int[] horizontalDirections = { Vector3Int.left, Vector3Int.right };
        foreach (var direction in horizontalDirections)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
            {
                // Si encuentra un tile de arena mojada en la dirección horizontal, se mueve
                currentCell = adjacentCell;
                transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
                isInHorizontalMovement = true; // Está en movimiento horizontal
                return; // Sale después de mover
            }
        }

        // Si no hay movimiento horizontal, sigue buscando el camino hacia abajo
        if (!isInHorizontalMovement)
        {
            MoveDownToWater();
        }
    }

    private void CheckForMojadaTiles()
    {
        // Comprobamos si el tile en la celda actual es de arena mojada
        TileBase tileAtCurrentCell = terrenoTilemap.GetTile(currentCell);
        if (tileAtCurrentCell != null && tileAtCurrentCell == arenaMojadaRuleTile)
        {
            // Si la tortuga ya está sobre arena mojada, comienza el movimiento hacia abajo
            MoveToWater();
        }
        else
        {
            // Si la tortuga no está sobre un tile de arena mojada, busca tiles adyacentes para moverse
            Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
            foreach (var direction in directions)
            {
                Vector3Int adjacentCell = currentCell + direction;

                // Verifica si el tile adyacente es de tipo arena mojada
                TileBase tileAtAdjacentCell = terrenoTilemap.GetTile(adjacentCell);
                if (tileAtAdjacentCell != null && tileAtAdjacentCell == arenaMojadaRuleTile)
                {
                    // Si encuentra un tile de arena mojada, comienza el movimiento
                    currentCell = adjacentCell;
                    transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
                    return;  // Sale del bucle después de mover la tortuga
                }
            }
        }
    }
}