using System;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
[CreateAssetMenu(fileName = "New Tile", menuName = "Game/Tile")]
public class TileInfo : ScriptableObject
{
    [Header("Tile Properties")]
    public string textureName;
    public string tileName;

    [JsonIgnore]
    [HideInInspector]
    public Texture texture;

    [HideInInspector]
    public bool moveableTile = false;
    [HideInInspector]
    public Vector2 tileMoveTo = new Vector2();

    public TileInfo()
    { }

    // TODO Possible support for directional textures?
}
