using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{
    public Tilemap doorTilemap;
    public Key.KeyColor requiredKeyColor;
    public TileBase doorTile;

    private PlayerKeys playerKeys;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerKeys = other.GetComponent<PlayerKeys>();
            if (playerKeys.HasKey(requiredKeyColor))
            {
                RemoveDoorTiles();
            }
        }
    }

    private void RemoveDoorTiles()
    {
        BoundsInt bounds = doorTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = doorTilemap.GetTile(cellPosition);

                // If the tile matches the door tile, remove it
                if (tile == doorTile)
                {
                    doorTilemap.SetTile(cellPosition, null);
                }
            }
        }
    }
}
