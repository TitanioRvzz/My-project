using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BañistaIzq : MonoBehaviour
{
    public Tilemap terrenoTilemap;
    public Vector3Int startPosition;
    public int tilesToMove;
    public float waitTime; // Permanece quieta al llegar al límite
    public float waitBeforeReturnTime; //  Permanece quieta al regresar
    public float moveSpeed;
    private Vector3Int currentCell;
    private bool isReturning = false; //  Saber si regresa
    private int moveDirection = 1;
    public Animator Bañista;

    private void Start()
    {

        currentCell = startPosition;
        transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);
        Bañista = GetComponent<Animator>();

        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            for (int i = 0; i < tilesToMove; i++)
            {
                if (isReturning) yield break;


                Vector3Int nextCell = currentCell + Vector3Int.right * moveDirection;
                Bañista.SetFloat("Xinput", 1);


                yield return MoveToCell(nextCell);
            }

            Bañista.SetFloat("Xinput", 0);
            Bañista.SetBool("Sit", true);

            yield return new WaitForSeconds(waitTime);

            print("se sienta");
            Bañista.SetBool("Sit", false);
            Bañista.SetFloat("Xinput", -1);

            isReturning = true;
            StartCoroutine(ReturnToStart());

        }
    }

    private IEnumerator ReturnToStart()
    {
        while (currentCell != startPosition)
        {

            Vector3 direction = (startPosition - currentCell);
            direction.Normalize();
            Vector3Int nextDirection = new Vector3Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), 0);

            Vector3Int nextCell = currentCell + nextDirection;


            yield return MoveToCell(nextCell);
        }

        Bañista.SetFloat("Xinput", 0);

        yield return new WaitForSeconds(waitBeforeReturnTime);



        isReturning = false;
        StartCoroutine(MoveRoutine());
    }


    private IEnumerator MoveToCell(Vector3Int targetCell)
    {

        Vector3 targetPosition = terrenoTilemap.GetCellCenterWorld(targetCell);


        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }


        transform.position = targetPosition;
        currentCell = targetCell;
    }
}



