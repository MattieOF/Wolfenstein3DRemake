using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string name = "Untitled Level";
    public TileInfo[][] tiles;
    public EntityInfo[][] entities;
    public Vector2 levelSize;

    public LevelData()
    { }

    public LevelData(int xSize, int ySize)
    {
        tiles = new TileInfo[xSize][];
        entities = new EntityInfo[xSize][];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileInfo[ySize];
        }
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = new EntityInfo[ySize];
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

    public EntityInfo GetEntityAt(int x, int y)
    {
        return entities[x][y];
    }

    public bool EntityExistsAt(int x, int y)
    {
        if (entities[x][y] == null) return false;
        else return true;
    }

    public void SetEntityAt(int x, int y, EntityInfo entity)
    {
        entities[x][y] = entity;
    }

    public void RemoveEntityAt(int x, int y)
    {
        entities[x][y] = null;
    }

    public bool ItemExistsAt(int x, int y)
    {
        return !(tiles[x][y] == null) | !(entities[x][y] == null);
    }

    public void RemoveItemAt(int x, int y)
    {
        if (EntityExistsAt(x, y)) RemoveEntityAt(x, y);
        else if (TileExistsAt(x, y)) RemoveTileAt(x, y);
    }
}
