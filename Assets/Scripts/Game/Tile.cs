using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Tile
{
    [Header("Properties")]
    public string tileType;

    [HideInInspector]
    public bool moveableTile = false;
    [HideInInspector]
    public Vector2 tileMoveTo = new Vector2();

    public Tile()
    { }

    [JsonIgnore]
    public static Dictionary<string, TileInfo> tileTypes = new Dictionary<string, TileInfo>();

    public static void InitTileTypes()
    {
        tileTypes.Clear();
        TileInfo[] tiles = Resources.LoadAll<TileInfo>("Tiles");

        foreach (TileInfo tile in tiles)
        {
            tile.texture = Resources.Load("Textures/" + tile.textureName, typeof(Texture)) as Texture;
            tileTypes.Add(tile.name, tile);
        }
    }
}
