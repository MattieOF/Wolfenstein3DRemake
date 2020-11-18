using UnityEngine;
using UnityEngine.UI;

public enum SpecialTiles
{
    PlayerStart
}

public class TilePalette : MonoBehaviour
{
    [Header("Scene References")]
    public Transform contentParent;

    [Header("Asset References")]
    public GameObject itemPrefab;
    
    private TileInfo[] tiles;
    public TileInfo selectedTile;
    public TilePaletteItem selectedItem;

    void Start()
    {
        LoadTiles();
    }

    public void LoadTiles()
    {
        tiles = Resources.LoadAll<TileInfo>("Tiles");

        foreach (TileInfo tile in tiles)
        {
            tile.texture = Resources.Load("Textures/" + tile.textureName, typeof(Texture)) as Texture;
            GameObject go = Instantiate(itemPrefab, contentParent);
            go.GetComponent<RawImage>().texture = tile.texture;
            TilePaletteItem item = go.GetComponent<TilePaletteItem>();
            item.tileInfo = tile;
            item.tilePalette = this;
        }
    }

    public void SelectTile(TilePaletteItem item)
    {
        if (selectedItem == item) return;
        if (selectedItem) selectedItem.Deselect();
        selectedItem = item;
        if (!selectedItem.specialTile)
            selectedTile = selectedItem.tileInfo;
    }
}
