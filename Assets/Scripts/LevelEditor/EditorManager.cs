using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EditorManager : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject saveFirstObject;
    public TilePalette tilePalette;
    public LevelNameControl levelNameControl;
    public TMP_InputField levelLoadInput;
    public GameObject levelLoadInputObject;
    public EditorCameraControl cameraControl;

    [Header("Asset References")]
    public GameObject tilePrefab;
    public GameObject playerTilePrefab;

    [HideInInspector]
    public static bool loadLevelOnStart = false;
    [HideInInspector]
    public static string levelToLoadOnStart = "";

    private List<GameObject> objects = new List<GameObject>();

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
        levelLoadInputObject.SetActive(false);
        if (loadLevelOnStart)
        {
            LoadLevel(levelToLoadOnStart);
        }
    }

    public static bool LevelExists(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return File.Exists(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml");
    }

    public void SetName(string name)
    {
        level.name = name;
    }

    public void Save()
    {
        Debug.Log("Saving level.");
        LevelSerialiser.Save(level);
        // SAVE LEVEL HERE
    }

    public void ShowLoadLevelInput()
    {
        cameraControl.acceptInput = false;
        cameraControl.ResetInput();
        levelLoadInputObject.SetActive(true);
    }

    public void HideLoadLevelInput()
    {
        cameraControl.acceptInput = true;
        levelLoadInputObject.SetActive(false);
    }

    public void LoadFromInput()
    {
        LoadLevel(levelLoadInput.text);
        HideLoadLevelInput();
    }

    public void LoadLevel(string name)
    {
        if (!File.Exists(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml"))
        {
            Debug.Log("Level " + Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml not found.");
            return;
        }

        Debug.Log("Opening level " + Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml");
        StreamReader file = new StreamReader(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".xml");

        ClearEditor();
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        level = (LevelData)serializer.Deserialize(file.BaseStream);
        levelNameControl.SetName(level.name);
        file.Close();
        LoadTiles();
        // TODO Load player start object
    }

    public void LoadTiles()
    {
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.tiles[x][y] != null)
                {
                    GameObject go = Instantiate(tilePrefab);
                    go.transform.position = new Vector3(x, 1, y);
                    level.tiles[x][y].texture = Resources.Load("Textures/" + level.tiles[x][y].textureName, typeof(Texture)) as Texture;
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", level.tiles[x][y].texture);
                    objects.Add(go);
                }
            }
        }

        if (level.playerPosition != Vector3.one * -1)
        {
            GameObject go = Instantiate(playerTilePrefab);
            go.transform.position = level.playerPosition;
            objects.Add(go);
        }
    }

    public void ClearEditor()
    {
        foreach (GameObject go in objects) Destroy(go);
        objects.Clear();
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

    public void RemovePlayerTile()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            objects.Remove(go);
            Destroy(go);
        }
    }

    public void PlaceTile(Vector3 location)
    {
        if (tilePalette.selectedTile == null && !tilePalette.selectedItem.specialTile) return;
        if (level.TileExistsAt((int)location.x, (int)location.z)) return;
        if (level.playerPosition == location) return;

        if (tilePalette.selectedItem.specialTile)
        {
            switch (tilePalette.selectedItem.specialTileType)
            {
                case SpecialTiles.PlayerStart:
                    if (level.playerPosition == location) return;
                    RemovePlayerTile();
                    level.playerPosition = location;
                    GameObject go = Instantiate(playerTilePrefab);
                    go.transform.position = location;
                    objects.Add(go);
                    break;
            }
        } else
        {
            level.SetTileAt((int)location.x, (int)location.z, tilePalette.selectedTile);

            GameObject go = Instantiate(tilePrefab);
            go.transform.position = location;
            go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", tilePalette.selectedTile.texture);
            objects.Add(go);
        }
    }

    public void RemoveTile(Vector3 location)
    {
        if (level.playerPosition == location)
        {
            level.playerPosition = Vector3.one * -1;
        } else
        {
            if (!level.TileExistsAt((int)location.x, (int)location.z)) return;
            level.RemoveTileAt((int)location.x, (int)location.z);
        }

        Collider[] colliders = Physics.OverlapSphere(location, 0.2f);
        if (colliders.Length != 0)
        {
            foreach (Collider collider in colliders)
            {
                // TODO - MANAGE PLAYER START REMOVED
                if (collider.tag == "Tile" || collider.tag == "Player")
                {
                    objects.Remove(collider.gameObject);
                    Destroy(collider.gameObject);
                }
            }
        }
        else return;
    }
}
