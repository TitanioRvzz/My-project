using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap terrenoTilemap;
    [SerializeField] private TileBase arenaMojadaTile;
    [SerializeField] private TileBase arenaSecaTile;
    [SerializeField] public float revertTime; // Tiempo en segundos para que un tile se revierta

    private Queue<Vector3Int> mojadosQueue = new Queue<Vector3Int>(); // Cola de tiles mojados

    public void AddMojadoTile(Vector3Int tilePosition)
    {
        // Añadir el tile a la cola
        if (!mojadosQueue.Contains(tilePosition))
        {
            mojadosQueue.Enqueue(tilePosition);
            StartCoroutine(RevertTile(tilePosition));
        }
    }

    private IEnumerator RevertTile(Vector3Int tilePosition)
    {
        // Esperar el tiempo definido antes de revertir el tile
        yield return new WaitForSeconds(revertTime);

        // Revertir el tile si todavía es arena mojada
        if (terrenoTilemap.GetTile(tilePosition) == arenaMojadaTile)
        {
            terrenoTilemap.SetTile(tilePosition, arenaSecaTile);
        }

        // Remover el tile de la cola
        mojadosQueue.Dequeue();
    }
}
