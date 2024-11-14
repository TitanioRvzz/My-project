using System.Collections;
using UnityEngine;
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
        Gizmos.DrawWireSphere((Vector2)tilemap.GetCellCenterWorld(currentCell) + offsetpunto, Radio0);
    }
}

