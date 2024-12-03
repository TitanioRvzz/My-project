using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class COMPARAR: MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase aguaTile;
    [SerializeField] TileBase arenaMojadaTile;
    [SerializeField] float Radio;
    [SerializeField] LayerMask obstaculo;
    //[SerializeField] int moveHor = 1;
    private bool isMoving = false;

    public Animator anim;
    private Vector3Int currentCell;
    private Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    [SerializeField] float baseSpeed = 2f; // Velocidad en arena seca
    [SerializeField] float mojadaSpeedMultiplier = 1.5f; // Multiplicador de velocidad en arena mojada
    [SerializeField] float respawnProbability = 0.5f; // Probabilidad de duplicarse al respawn
    [SerializeField] GameObject tortugaPrefab; // Prefab de la tortuga para el respawn
    [SerializeField] Transform respawnPoint;


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
            float speed = IsOnMojadaTile() ? baseSpeed * mojadaSpeedMultiplier : baseSpeed;

            if (IsOnMojadaTile())
            {
                if (!MoveTowardsWater(speed))
                {
                    MoveRandomly(speed); // Si no encuentra agua, moverse al azar
                }
            }
            else
            {
                MoveRandomly(speed);
            }

            yield return new WaitForSeconds(0.5f / speed); // Control del intervalo de movimiento según la velocidad
        }
    }

    private bool IsOnMojadaTile()
    {
        TileBase tileAtCurrentCell = tilemap.GetTile(currentCell);
        return tileAtCurrentCell != null && tileAtCurrentCell == arenaMojadaTile;
    }

    private bool MoveTowardsWater(float speed)
    {
        foreach (var direction in directions)
        {
            Vector3Int adjacentCell = currentCell + direction;
            TileBase tileAtAdjacentCell = tilemap.GetTile(adjacentCell);

            if (tileAtAdjacentCell != null && tileAtAdjacentCell == aguaTile)
            {
                StartCoroutine(SmoothMove(adjacentCell, speed));
                return true; // Movimiento hacia el agua exitoso
            }
        }
        return false; // No encontró agua alrededor
    }

    private void MoveRandomly(float speed)
    {
        Vector3Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3Int targetCell = currentCell + randomDirection;

        if (CanMoveToCell(targetCell))
        {
            StartCoroutine(SmoothMove(targetCell, speed));
        }
    }

    private bool CanMoveToCell(Vector3Int cell)
    {
        TileBase tileAtCell = tilemap.GetTile(cell);
        if (tileAtCell == null) return false; // No hay tile en esa celda
        if (Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(cell), Radio, obstaculo)) return false; // Obstáculo presente
        return true; // Puede moverse
    }

    private IEnumerator SmoothMove(Vector3Int targetCell, float speed)
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
        float duration = 1f / speed; // Duración ajustada por la velocidad

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Alinear posición exacta
        currentCell = targetCell;

        isMoving = false;

        // Verificar si llegó al agua
        if (tilemap.GetTile(currentCell) == aguaTile)
        {
            RespawnOrDuplicate();
        }
    }

    private void RespawnOrDuplicate()
    {
        // Destruir la tortuga actual
        Destroy(gameObject);

        // Respawn de una o dos tortugas según la probabilidad
        if (Random.value <= respawnProbability)
        {
            Instantiate(tortugaPrefab, respawnPoint.position, Quaternion.identity);
            Instantiate(tortugaPrefab, respawnPoint.position + Vector3.right * 2, Quaternion.identity);
        }
        else
        {
            Instantiate(tortugaPrefab, respawnPoint.position, Quaternion.identity);
        }
    }
}
