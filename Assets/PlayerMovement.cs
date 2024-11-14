using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Radio0;
    [SerializeField] Vector2 puntodemov;
    [SerializeField] Vector2 offsetpunto;
    [SerializeField] LayerMask obstaculo;
    [SerializeField] LayerMask tortugaLayer;  
    bool moviendo = false;
    Vector2 Move;
    Vector2 lastMoveDirection;

    private void Start()
    {
        puntodemov = transform.position;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0)
        {
            Move = new Vector2(moveX, 0);
        }
        else if (moveY != 0)
        {
            Move = new Vector2(0, moveY);
        }
        else
        {
            Move = Vector2.zero;
        }

        if (moviendo)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntodemov, Speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, puntodemov) == 0)
            {
                moviendo = false;
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
            UpdateRotation(lastMoveDirection);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HitPlayerAttack();
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