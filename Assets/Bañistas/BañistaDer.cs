using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BañistaDer : MonoBehaviour
{
    public Tilemap terrenoTilemap;
    public Vector3Int startPosition;
    public int tilesToMove;
    public float waitTime;
    public float waitBeforeReturnTime;
    public float moveSpeed;
    private Vector3Int currentCell;
    private bool isReturning = false;

    private void Start()
    {

        currentCell = startPosition;
        transform.position = terrenoTilemap.GetCellCenterWorld(currentCell);


        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {

            for (int i = 0; i < tilesToMove; i++)
            {
                if (isReturning) yield break;


                Vector3Int nextCell = currentCell + Vector3Int.left;


                yield return MoveToCell(nextCell);
            }


            yield return new WaitForSeconds(waitTime);


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