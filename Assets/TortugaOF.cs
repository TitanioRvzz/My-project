using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TortugaOF : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Animator anim;
    [SerializeField] TileBase arenaMojadaTile;
    [SerializeField] LayerMask obstaculo;
    private Vector3Int currentCell;
    private bool isMoving = false;
    private Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);

    }

    void Update()
    {
        Vector3Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3Int targetCell = currentCell + randomDirection;

        if (CanMoveToCell(targetCell))
        {
            StartCoroutine(Movement(targetCell));
        }

        else if (FollowPath(targetCell))
        {
            StartCoroutine(Movement(targetCell));
        }
    }
    private bool CanMoveToCell(Vector3Int cell)
    {
        TileBase tileAtCell = tilemap.GetTile(cell);
        if (tileAtCell == null) return false; // No hay tile en esa celda
        if (Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(cell), 0.5f, obstaculo)) return false; // Obstáculo presente

        return true; // Puede moverse
    }
    private bool FollowPath(Vector3Int cell)
    {
        TileBase tileAtCell = tilemap.GetTile(currentCell);
        if (Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(cell), 0.5f, obstaculo)) return false; // Obstáculo presente
        if (tileAtCell != null && tileAtCell == arenaMojadaTile) return true; // No hay tile en esa celda
        return true; // Puede moverse

    }
    private IEnumerator Movement (Vector3Int targetCell)
    {
    
        if (isMoving) yield break; // Evitar movimientos simultáneos
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCell);
        Vector2 direction = (targetPosition - startPosition).normalized;

        // Actualizar parámetros del Blend Tree
        anim.SetFloat("Xinput", direction.x);
        anim.SetFloat("Yinput", direction.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }
        if (direction.x != 0)
        {
            GetComponent<SpriteRenderer>().flipX = direction.x > 0; // Flip en el eje X
        }

        float elapsedTime = 0f;
        float duration = 4.5f; // Duración ajustada por la velocidad

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Alinear posición exacta
        currentCell = targetCell;

        isMoving = false;
    }
}
