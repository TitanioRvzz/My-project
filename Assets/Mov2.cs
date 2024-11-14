using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Mov2 : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Radio0;
    [SerializeField] Vector2 puntodemov;
    [SerializeField] Vector2 offsetpunto;
    [SerializeField] LayerMask obstaculo;
    [SerializeField] LayerMask tortugaLayer;
    [SerializeField] Tilemap terrenoTilemap;
    [SerializeField] TileBase arenaSecaTile;
    [SerializeField] TileBase arenaMojadaTile;
    [SerializeField] Slider humedadSlider;
    [SerializeField] LayerMask aguaLayer;
    [SerializeField] Animator anim;



    bool moviendo = false;
    Vector2 Move;
    Vector2 lastMoveDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
        puntodemov = transform.position;
        humedadSlider.value = 0;
    }

     void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0)
        {
            Move = new Vector2(moveX, 0);
            anim.SetBool("Walk", true);

            if (moveX > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if ( moveX < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }


        else if (moveY != 0)
        {
            Move = new Vector2(0, moveY);
        }
        else
        {
            Move = Vector2.zero;
            anim.SetBool("Walk", false);
        }

        if (moviendo)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntodemov, Speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, puntodemov) == 0)
            {
                moviendo = false;

                Vector3Int currentCell = terrenoTilemap.WorldToCell(transform.position);
                TileBase currentTile = terrenoTilemap.GetTile(currentCell);

                if (Physics2D.OverlapCircle(transform.position, Radio0, aguaLayer))
                {
                    humedadSlider.value = humedadSlider.maxValue;
                }
                else if (humedadSlider.value > 0)
                {
                    if (currentTile == arenaSecaTile)
                    {
                        humedadSlider.value -= 0.2f;
                        terrenoTilemap.SetTile(currentCell, arenaMojadaTile);
                    }
                    else if (currentTile == arenaMojadaTile)
                    {
                        humedadSlider.value -= 0.18f;
                    }
                }
            }
        }

        if (Move != Vector2.zero && !moviendo)
        {
            Vector2 puntoaevaluar = puntodemov + Move;

            if (!Physics2D.OverlapCircle(puntoaevaluar + offsetpunto, Radio0, obstaculo))
            {
                moviendo = true;
                puntodemov = puntoaevaluar;
            }

            lastMoveDirection = Move;
            //UpdateRotation(lastMoveDirection);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HitPlayerAttack();
            anim.SetTrigger("Hit");
        }
    }

    private void UpdateRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntodemov + offsetpunto, Radio0);
    }
}