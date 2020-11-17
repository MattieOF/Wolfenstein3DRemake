using UnityEngine;
using UnityEngine.UI;

public class TilePalette : MonoBehaviour
{
    [Header("Scene References")]
    public Transform contentParent;

    [Header("Asset References")]
    public GameObject itemPrefab;
    
    private TileInfo[] tiles;

    void Start()
    {
        LoadTiles();
    }

    public void LoadTiles()
    {
        tiles = Resources.LoadAll<TileInfo>("Tiles");

        foreach (TileInfo tile in tiles)
        {
            GameObject go = Instantiate(itemPrefab, contentParent);
            go.GetComponent<RawImage>().texture = tile.texture;
        }
    }
}
