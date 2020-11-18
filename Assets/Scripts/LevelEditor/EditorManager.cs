using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject saveFirstObject;
    public TilePalette tilePalette;

    [Header("Asset References")]
    public GameObject tilePrefab;

    // [Header("Level Properties")]
    public string LevelName
    {
        get { return level.name; }
        set { level.name = value; }
    }

    [Header("Properties")]
    public string menuSceneName = "Game";

    private LevelData level = new LevelData(100, 100);

    void Start()
    {
        saveFirstObject.SetActive(false);
    }

    public void SetName(string name)
    {
        level.name = name;
    }

    public void Save()
    {
        Debug.Log("Saving level.");
        // SAVE LEVEL HERE
    }

    public void OpenSaveFirstMessage()
    {
        saveFirstObject.SetActive(true);
    }

    public void CloseSaveFirstMessage()
    {
        saveFirstObject.SetActive(false);
    }

    public void CloseEditor()
    {
        // CLEANUP HERE
        SceneManager.LoadScene(menuSceneName);
    }

    public void PlaceTile(Vector3 location)
    {

        if (tilePalette.selectedTile == null) return;
        if (level.TileExistsAt((int)location.x, (int)location.z)) return;
        level.SetTileAt((int)location.x, (int)location.z, tilePalette.selectedTile);

        // Debug.Log("Placing tile at " + location);

        GameObject go = Instantiate(tilePrefab);
        go.transform.position = location;
        go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", tilePalette.selectedTile.texture);
    }

    public void RemoveTile(Vector3 location)
    {
        if (!level.TileExistsAt((int)location.x, (int)location.z)) return;
        level.RemoveTileAt((int)location.x, (int)location.z);

        Collider[] colliders = Physics.OverlapSphere(location, 0.2f);
        if (colliders.Length != 0)
        {
            foreach (Collider collider in colliders)
            {
                // TODO - MANAGE PLAYER START REMOVED
                if (collider.tag != "Tile") continue;
                Destroy(collider.gameObject);
            }
        }
        else return;
    }
}
