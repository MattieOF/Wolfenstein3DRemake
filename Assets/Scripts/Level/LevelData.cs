using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string name = "Untitled Level";
    public Tile[,] tiles;
    public Entity[,] entities;
    public List<Vector2> moveableTileEndPositions;
    public Vector2 levelSize;

    public LevelData()
    { }

    public LevelData(int xSize, int ySize)
    {
        tiles = new Tile[xSize, ySize];
        entities = new Entity[xSize, ySize];
        moveableTileEndPositions = new List<Vector2>();
        levelSize = new Vector2(xSize, ySize);
    }

    public Tile GetTileAt(int x, int y)
    {
        return tiles[x, y];
    }

    public bool TileExistsAt(int x, int y)
    {
        if (tiles[x, y] == null) return false;
        else return true;
    }

    public void SetTileAt(int x, int y, Tile tile)
    {
        tiles[x, y] = tile;
    }

    public void SetTileTypeAt(int x, int y, TileInfo tile)
    {
        tiles[x, y].tileType = tile.name;
    }

    public void RemoveTileAt(int x, int y)
    {
        tiles[x, y] = null;
    }

    public Entity GetEntityAt(int x, int y)
    {
        return entities[x, y];
    }

    public bool EntityExistsAt(int x, int y)
    {
        if (entities[x, y] == null) return false;
        else return true;
    }

    public void SetEntityAt(int x, int y, Entity entity)
    {
        entities[x, y] = entity;
    }

    public void SetEntityTypeAt(int x, int y, EntityInfo entity)
    {
        entities[x, y].entityType = entity.name;
    }

    public void RemoveEntityAt(int x, int y)
    {
        entities[x, y] = null;
    }

    public bool ItemExistsAt(int x, int y)
    {
        return !(tiles[x, y] == null) | !(entities[x, y] == null) | MoveableTileEndPosAt(x, y);
    }

    public void RemoveItemAt(int x, int y)
    {
        if (EntityExistsAt(x, y)) RemoveEntityAt(x, y);
        else if (TileExistsAt(x, y)) RemoveTileAt(x, y);
    }

    public bool MoveableTileEndPosAt(int x, int y)
    {
        return moveableTileEndPositions.Contains(new Vector2(x, y));
    }

    public void AddMoveableTileEndPos(int x, int y)
    {
        moveableTileEndPositions.Add(new Vector2(x, y));
    }

    public void SetTileMoveable(int x, int y, int endX, int endY)
    {
        if (!TileExistsAt(x, y)) return;
        if (TileExistsAt(endX, endY)) return;

        tiles[x, y].moveableTile = true;
        tiles[x, y].tileMoveTo = new Vector2(endX, endY);

        AddMoveableTileEndPos(endX, endY);
    }

    public void RemoveTileMoveability(int x, int y)
    {
        if (!TileExistsAt(x, y) || !tiles[x, y].moveableTile) return;

        tiles[x, y].moveableTile = false;
        moveableTileEndPositions.Remove(new Vector2(tiles[x, y].tileMoveTo.x, tiles[x, y].tileMoveTo.y));
        tiles[x, y].tileMoveTo = Vector2.zero;
    }
}
