using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TortugaOF : MonoBehaviour
{
    public int points = 8;

    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase arenaTile;
    [SerializeField] TileBase arenaMojadaTile;
    [SerializeField] TileBase aguaTile;
    [SerializeField] LayerMask obstaculo;
    [SerializeField] float speed; // Velocidad base
    [SerializeField] float mojadaSpeedMultiplier; // Velocidad en arena mojada
    [SerializeField] Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    private Vector3Int currentCell;
    private Vector3Int lastDirection = Vector3Int.zero;

    private bool isMoving = false;
    private Animator anim;

    public float probabilityToCloneTwo;
    [SerializeField] GameObject[] objectsToSpawn; // Array de objetos para elegir
    public Transform respawnPoint, secondpoint, Fin;



    public bool isHit = false;
    public float Radio = 0.4f;
    public Vector2 offsetpunto;



    private void Start()
    {
        anim = GetComponent<Animator>();
        currentCell = tilemap.WorldToCell(transform.position);
    }

    private void Update()
    {
        if (isMoving) return;

        // Prioridad: Seguir el camino mojado si es posible
        if (FollowPath())
        {
            return;
        }

        // Movimiento aleatorio en arena seca
        MoveRandomly();

        
    }

        private bool CanMoveToCell(Vector3Int cell)
    {
        TileBase tileAtCell = tilemap.GetTile(cell);
        if (tileAtCell == null) return false; // No hay tile en esa celda
        if (Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(cell), 0.5f, obstaculo)) return false; // Obstáculo presente
        return true; // Celda válida para moverse
    }

    private bool FollowPath()
    {
        // Direcciones con prioridad
        Vector3Int[] priorityDirections = { Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (var direction in priorityDirections)
        {
            Vector3Int targetCell = currentCell + direction;
            TileBase tileAtTarget = tilemap.GetTile(targetCell);

            if (tileAtTarget != null && tileAtTarget == arenaMojadaTile && CanMoveToCell(targetCell))
            {
                // Asegurar que no regrese inmediatamente a la dirección opuesta
                if (isMoving && direction == -lastDirection)
                {
                    continue; // Ignorar esta dirección para evitar alternancia
                }

                // Registrar la dirección actual para evitar alternar
                lastDirection = direction;
                StartCoroutine(Movement(targetCell, speed * mojadaSpeedMultiplier));
                return true; // Movimiento hacia el camino mojado
            }
        }
        return false; // No encontró tiles mojados válidos
    }


    private void MoveRandomly()
    {
        Vector3Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3Int targetCell = currentCell + randomDirection;

        if (CanMoveToCell(targetCell))
        {
            StartCoroutine(Movement(targetCell, speed));
        }
    }

    private IEnumerator Movement(Vector3Int targetCell, float moveSpeed)
    {

        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCell);
        Vector2 direction = (targetPosition - startPosition).normalized;

        // Actualizar parámetros del Blend Tree
        anim.SetFloat("Xinput", direction.x);
        anim.SetFloat("Yinput", direction.y);
        anim.SetFloat("lastMoveX", direction.x);
        anim.SetFloat("lastMoveY", direction.y);

        if (direction.x != 0)
        {
            GetComponent<SpriteRenderer>().flipX = direction.x > 0; // Flip en el eje X
        }

        float elapsedTime = 0f;
        float duration = 1f / moveSpeed; // Ajustar duración según velocidad

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Alinear posición exacta
        currentCell = targetCell;

        float Stop = Random.Range(1,5);

        // Verificar si el tile actual es arena mojada
        TileBase currentTile = tilemap.GetTile(currentCell);
        if (currentTile != arenaMojadaTile)
        {
            yield return new WaitForSeconds(Stop); // Cambia 1f por el tiempo deseado para arena seca
        }

        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Water"))
        {
            CloneObjectWithProbability();
        }
    }

    public void CloneObjectWithProbability()
    {
        // Determinar el punto de respawn basado en la probabilidad
        Transform chosenRespawnPoint = Random.value <= probabilityToCloneTwo ? respawnPoint : secondpoint;

        // Respawnear la tortuga en el punto elegido
        Instantiate(this.gameObject, chosenRespawnPoint.position, chosenRespawnPoint.rotation);

        // Con probabilidad, respawnear una tortuga duplicada
        if (Random.value <= probabilityToCloneTwo)
        {
            Instantiate(gameObject, chosenRespawnPoint.position + Vector3.right * 4f, chosenRespawnPoint.rotation);
        }
    }

    public void HitByPlayer(Vector2 direction)
    {
        isHit = true;

        Vector3Int hitDirection = new Vector3Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), 0);
        Vector3Int targetCell = currentCell + hitDirection * 2;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCell);

        if (//tilemap.HasTile(targetCell) && 
            !Physics2D.OverlapCircle((Vector2)targetPosition + offsetpunto, Radio, obstaculo))
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

}
