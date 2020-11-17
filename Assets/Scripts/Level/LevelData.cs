using System;

[Serializable]
public class LevelData
{
    public string name;
    public TileInfo[,] tiles;

    public LevelData(int xSize, int ySize)
    {
        tiles = new TileInfo[xSize, ySize];
    }

    public TileInfo GetTileAt(int x, int y)
    {
        return tiles[x, y];
    }

    public bool TileExistsAt(int x, int y)
    {
        return tiles[x, y] != null;
    }
}
