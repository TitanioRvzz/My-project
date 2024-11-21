using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Mov3 : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Radio0;
    [SerializeField] LayerMask obstaculo;
    [SerializeField] LayerMask tortugaLayer;
    [SerializeField] Tilemap terrenoTilemap;
    [SerializeField] TileBase arenaSecaTile;
    [SerializeField] TileBase arenaMojadaTile;
    [SerializeField] Slider humedadSlider;
    [SerializeField] LayerMask aguaLayer;
    [SerializeField] Animator anim;

    private Vector3Int puntodemov; // Posición del tile objetivo
    private bool moviendo = false; // Controla si el jugador está en movimiento
    private Vector2 lastMoveDirection; // Última dirección en la que se movió el jugador

    private void Start()
    {
        anim = GetComponent<Animator>();

        // Centrar al jugador en el tile inicial
        puntodemov = terrenoTilemap.WorldToCell(transform.position);
        transform.position = terrenoTilemap.GetCellCenterWorld(puntodemov);

        humedadSlider.value = 0;
    }

    void Update()
    {
        // Solo permitir entrada de movimiento si no está moviéndose
        if (!moviendo)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            // Detectar movimiento en una sola dirección (prioridad horizontal sobre vertical)
            if (moveX != 0)
            {
                lastMoveDirection = new Vector2(moveX, 0);
            }
            else if (moveY != 0)
            {
                lastMoveDirection = new Vector2(0, moveY);
            }
            else
            {
                anim.SetBool("Walk", false);
                return; // No hay movimiento, salir del método
            }

            // Calcular el próximo tile objetivo
            Vector3Int nextCell = puntodemov + new Vector3Int((int)lastMoveDirection.x, (int)lastMoveDirection.y, 0);

            // Validar que el próximo tile no tenga obstáculos
            if (!Physics2D.OverlapCircle(terrenoTilemap.GetCellCenterWorld(nextCell), Radio0, obstaculo))
            {
                puntodemov = nextCell; // Actualizar el objetivo del movimiento
                moviendo = true;
                anim.SetBool("Walk", true);

                // Cambiar dirección visual (flipX)
                if (lastMoveDirection.x > 0)
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                else if (lastMoveDirection.x < 0)
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        // Movimiento hacia el tile objetivo
        if (moviendo)
        {
            Vector3 targetPosition = terrenoTilemap.GetCellCenterWorld(puntodemov);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);

            // Comprobar si se ha alcanzado el centro del tile
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Alinear posición exacta
                moviendo = false;

                // Verificar si el jugador está en agua para rellenar la barra de humedad
                if (Physics2D.OverlapCircle(transform.position, Radio0, aguaLayer))
                {
                    humedadSlider.value = humedadSlider.maxValue;
                }
                else if (humedadSlider.value > 0)
                {
                    Vector3Int currentCell = terrenoTilemap.WorldToCell(transform.position);
                    TileBase currentTile = terrenoTilemap.GetTile(currentCell);

                    if (currentTile == arenaSecaTile)
                    {
                        humedadSlider.value -= 0.2f;
                        terrenoTilemap.SetTile(currentCell, arenaMojadaTile);
                    }
                    else if (currentTile == arenaMojadaTile)
                    {
                        humedadSlider.value -= 0.1f;
                    }
                }
            }
        }

        // Ataque del jugador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HitPlayerAttack();
            anim.SetTrigger("Hit");
        }
    }

    private void HitPlayerAttack()
    {
        Vector2 attackPosition = (Vector2)transform.position + lastMoveDirection;
        Collider2D hitTortuga = Physics2D.OverlapCircle(attackPosition, Radio0, tortugaLayer);

        if (hitTortuga != null)
        {
            if (hitTortuga.TryGetComponent<TurtleMovement>(out TurtleMovement turtle))
            {
                turtle.HitByPlayer(lastMoveDirection);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (terrenoTilemap != null)
        {
            Vector3Int currentCell = terrenoTilemap.WorldToCell(transform.position);
            Vector3 cellCenter = terrenoTilemap.GetCellCenterWorld(currentCell);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cellCenter, new Vector3(1, 1, 0)); // Dibujar un cuadro que represente el tile actual
        }
    }
}

