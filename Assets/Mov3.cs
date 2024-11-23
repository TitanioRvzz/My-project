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
    [SerializeField] Rigidbody2D rb;
    private Vector3Int puntodemov;
    private bool moviendo = false;
    private Vector2 lastMoveDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        puntodemov = terrenoTilemap.WorldToCell(transform.position);
        transform.position = terrenoTilemap.GetCellCenterWorld(puntodemov);

        humedadSlider.value = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Hit", true);
            HitPlayerAttack();
        }
        else
        {
            anim.SetBool("Hit", false);
        }

        rb.velocity = Speed * Time.deltaTime * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        anim.SetFloat("Xinput", rb.velocity.x);
        anim.SetFloat("Yinput", rb.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        if (!moviendo)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

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
                return; // No hay movimiento, salir del método
            }

            Vector3Int nextCell = puntodemov + new Vector3Int((int)lastMoveDirection.x, (int)lastMoveDirection.y, 0);

            if (!Physics2D.OverlapCircle(terrenoTilemap.GetCellCenterWorld(nextCell), Radio0, obstaculo))
            {
                puntodemov = nextCell; // Actualizar el objetivo del movimiento
                moviendo = true;

                if (lastMoveDirection.x > 0)
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                else if (lastMoveDirection.x < 0)
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (moviendo)
        {
            Vector3 targetPosition = terrenoTilemap.GetCellCenterWorld(puntodemov);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Alinear posición exacta
                moviendo = false;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            anim.SetBool("InWater", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            anim.SetBool("InWater", false);
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

