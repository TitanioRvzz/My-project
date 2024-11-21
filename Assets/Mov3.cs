using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Mov3 : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Radio0;
    [SerializeField] Vector2 offsetpunto;
    [SerializeField] LayerMask obstaculo;
    [SerializeField] LayerMask aguaLayer;
    [SerializeField] LayerMask tortugaLayer;
    [SerializeField] Tilemap terrenoTilemap;
    [SerializeField] TileBase arenaSecaTile;
    [SerializeField] TileBase arenaMojadaTile;
    [SerializeField] Slider humedadSlider;
    [SerializeField] Animator anim;

    private Vector3Int targetCell;
    private bool moviendo = false;

    private void Start()
    {
        targetCell = terrenoTilemap.WorldToCell(transform.position);
        humedadSlider.value = 0;
    }

    private void Update()
    {
        if (!moviendo)
        {
            // Capturar input de movimiento
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // Asegurar movimiento en solo una dirección a la vez
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                input.y = 0;
            }
            else
            {
                input.x = 0;
            }

            // Si se presiona una tecla válida, intentar mover
            if (input != Vector2.zero)
            {
                Vector3Int moveDirection = new Vector3Int(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y), 0);
                Vector3Int newCell = targetCell + moveDirection;

                // Verificar colisión con obstáculos
                if (!Physics2D.OverlapCircle(terrenoTilemap.GetCellCenterWorld(newCell) + (Vector3)offsetpunto, Radio0, obstaculo))
                {
                    targetCell = newCell;
                    StartCoroutine(MoveToCell(terrenoTilemap.GetCellCenterWorld(newCell)));
                }

                // Actualizar animación y orientación
                if (input.x != 0)
                {
                    anim.SetBool("Walk", true);
                    gameObject.GetComponent<SpriteRenderer>().flipX = input.x < 0;
                }
                else if (input.y != 0)
                {
                    anim.SetBool("Walk", true);
                }
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }

        // Manejo del agua
        if (Physics2D.OverlapCircle(transform.position, Radio0, aguaLayer))
        {
            humedadSlider.value = humedadSlider.maxValue;
        }
    }

    private IEnumerator MoveToCell(Vector3 targetPosition)
    {
        moviendo = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        moviendo = false;

        // Actualizar tiles al llegar a la nueva posición
        UpdateTile();
    }

    private void UpdateTile()
    {
        Vector3Int currentCell = terrenoTilemap.WorldToCell(transform.position);
        TileBase currentTile = terrenoTilemap.GetTile(currentCell);

        if (currentTile == arenaSecaTile && humedadSlider.value > 0)
        {
            humedadSlider.value -= 0.15f; // Reducir humedad
            terrenoTilemap.SetTile(currentCell, arenaMojadaTile); // Cambiar a tile mojado
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)offsetpunto, Radio0);
    }
}
