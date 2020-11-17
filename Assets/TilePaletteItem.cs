using UnityEngine;

public class TilePaletteItem : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject highlight;
    public GameObject selectHighlight;
    public TilePalette tilePalette;

    [Header("Properties")]
    public TileInfo tileInfo;

    void Start()
    {
        highlight.SetActive(false);
        selectHighlight.SetActive(false);
    }

    public void PointerEnter()
    {
        highlight.SetActive(true);
    }

    public void PointerExit()
    {
        highlight.SetActive(false);
    }

    public void Select()
    {
        if (selectHighlight.activeSelf) return;
        selectHighlight.SetActive(true);
        tilePalette.SelectTile(this);
    }

    public void Deselect()
    {
        selectHighlight.SetActive(false);
    }
}
