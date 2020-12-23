using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PaletteTab
{
    Tiles,
    Entities
}

public enum ItemType
{
    None,
    Tile,
    Entity
}

public class TilePalette : MonoBehaviour
{
    [Header("Scene References")]
    public Transform contentParent;
    public RectTransform tilesContent, entitiesContent;
    public Button tilesTabButton, entitiesTabButton;
    public ScrollRect paletteRect;
    public GameObject tileInfoBox;
    public TextMeshProUGUI tileNameText;
    public RawImage tileIcon;

    [Header("Asset References")]
    public GameObject itemPrefab;
    public GameObject entityPrefab;
    
    private TileInfo[] tiles;
    private EntityInfo[] entities;
    public TileInfo selectedTile;
    public EntityInfo selectedEntity;
    public PaletteItem selectedItem;

    [Header("Properties")]
    public PaletteTab currentTab = PaletteTab.Tiles;
    public ItemType selectedItemType = ItemType.None;

    void Start()
    {
        LoadTiles();
        LoadEntities();
        SwitchTab(currentTab);
        SetTileInfoBoxActive(false);
    }

    public void SetTileInfoBoxActive(bool active)
    {
        tileInfoBox.SetActive(active);
    }

    public void SetTileInfoToCurrent()
    {
        SetTileInfoBoxActive(true);
        switch (selectedItemType)
        {
            case ItemType.Entity:
                SetTileInfoBox(selectedEntity);
                break;
            case ItemType.Tile:
                SetTileInfoBox(selectedTile);
                break;
            default:
                SetTileInfoBoxActive(false);
                break;
        }
    }

    public void SetTileInfoBox(EntityInfo info)
    {
        SetTileInfoBoxActive(true);
        tileIcon.texture = info.texture;
        tileNameText.text = info.entityName;
    }

    public void SetTileInfoBox(TileInfo info)
    {
        SetTileInfoBoxActive(true);
        tileIcon.texture = info.texture;
        tileNameText.text = info.tileName;
    }

    public void SwitchToEntityTab()
    {
        currentTab = PaletteTab.Entities;
        paletteRect.content = entitiesContent;
        tilesContent.gameObject.SetActive(false);
        entitiesContent.gameObject.SetActive(true);
    }

    public void SwitchToTilesTab()
    {
        currentTab = PaletteTab.Tiles;
        paletteRect.content = tilesContent;
        tilesContent.gameObject.SetActive(true);
        entitiesContent.gameObject.SetActive(false);
    }

    public void SwitchTab(PaletteTab tab)
    {
        currentTab = tab;
        switch (tab)
        {
            case PaletteTab.Entities:
                tilesContent.gameObject.SetActive(false);
                entitiesContent.gameObject.SetActive(true);
                break;
            case PaletteTab.Tiles:
                tilesContent.gameObject.SetActive(true);
                entitiesContent.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void LoadTiles()
    {
        tiles = Resources.LoadAll<TileInfo>("Tiles");

        foreach (TileInfo tile in tiles)
        {
            tile.texture = Resources.Load("Textures/" + tile.textureName, typeof(Texture)) as Texture;
            GameObject go = Instantiate(itemPrefab, tilesContent);
            go.GetComponent<RawImage>().texture = tile.texture;
            TilePaletteItem item = go.GetComponent<TilePaletteItem>();
            item.tileInfo = tile;
            item.tilePalette = this;
        }
    }

    public void LoadEntities()
    {
        entities = Resources.LoadAll<EntityInfo>("Entities");

        foreach (EntityInfo entity in entities)
        {
            entity.texture = Resources.Load("Textures/" + entity.editorIconName, typeof(Texture)) as Texture;
            GameObject go = Instantiate(entityPrefab, entitiesContent);
            go.GetComponent<RawImage>().texture = entity.texture;
            EntityPaletteItem item = go.GetComponent<EntityPaletteItem>();
            item.entityInfo = entity;
            item.tilePalette = this;
        }
    }

    public void SelectTile(TilePaletteItem item)
    {
        if (selectedItem == item) return;
        if (selectedItem) selectedItem.Deselect();
        selectedItem = item;
        selectedItemType = ItemType.Tile;
        selectedTile = item.tileInfo;
    }

    public void SelectEntity(EntityPaletteItem item)
    {
        if (selectedItem == item) return;
        if (selectedItem) selectedItem.Deselect();
        selectedItem = item;
        selectedItemType = ItemType.Entity;
        selectedEntity = item.entityInfo;
    }
}
