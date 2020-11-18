using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string name = "Untitled Level";
    public TileInfo[][] tiles;
    public Vector3 playerPosition = new Vector3(-1, -1, -1);
    public Vector2 levelSize;

    public LevelData()
    { }

    public LevelData(int xSize, int ySize)
    {
        tiles = new TileInfo[xSize][];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileInfo[ySize];
        }
        levelSize = new Vector2(xSize, ySize);
    }

    public TileInfo GetTileAt(int x, int y)
    {
        return tiles[x][y];
    }

    public bool TileExistsAt(int x, int y)
    {
        if (tiles[x][y] == null) return false;
        else return true;
    }

    public void SetTileAt(int x, int y, TileInfo tile)
    {
        tiles[x][y] = tile;
    }

    public void RemoveTileAt(int x, int y)
    {
        tiles[x][y] = null;
    }
}
