using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurtuleMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveInterval;
    public LayerMask obstaculo;
    public float Radio0;
    public Vector2 offsetpunto;
    public Transform jugador;  // Asigna el transform del jugador en el Inspector

    private Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
    private Vector3Int currentCell;
    private bool isHit = false;  // Verificar si la tortuga ha sido golpeada para detener su movimiento aleatorio temporalmente

    void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Si la tortuga fue golpeada, espera antes de reanudar el movimiento aleatorio
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

        // Obtiene la celda en la que está el jugador
        Vector3Int jugadorCell = tilemap.WorldToCell(jugador.position);

        // Verifica si el targetCell tiene un tile, no está bloqueado, no coincide con la casilla del jugador y no tiene obstáculos
        if (tilemap.HasTile(targetCell) && !Physics2D.OverlapCircle((Vector2)targetPosition + offsetpunto, Radio0, obstaculo) && targetCell != jugadorCell)
        {
            currentCell = targetCell;
            transform.position = targetPosition;
        }
    }

    public void HitByPlayer(Vector2 direction)
    {
        isHit = true;  // Detiene temporalmente el movimiento aleatorio

        // Calculamos dos celdas de desplazamiento en la dirección del golpe
        Vector3Int hitDirection = new Vector3Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), 0);
        Vector3Int targetCell = currentCell + hitDirection * 2;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCell);

        // Verificamos si las celdas de destino están dentro de la grilla y sin obstáculos
        if (tilemap.HasTile(targetCell) && !Physics2D.OverlapCircle((Vector2)targetPosition + offsetpunto, Radio0, obstaculo))
        {
            currentCell = targetCell;
            transform.position = targetPosition;
        }

        // Restablecemos el movimiento aleatorio después de un pequeño tiempo de espera
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

