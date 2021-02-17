using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Uween;

public enum MoveableTileEditStage
{
    None,
    PlaceStart,
    PlaceEnd
}

public class EditorManager : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject saveFirstObject;
    public TilePalette tilePalette;
    public LevelNameControl levelNameControl;
    public TMP_InputField levelLoadInput;
    public GameObject levelLoadInputObject;
    public EditorCameraControl cameraControl;
    public Image levelNameBG;
    public Color levelNameBGDefault;
    public GameObject moveableTileUI, moveableTileError;
    public TextMeshProUGUI moveableTileErrorText, moveableTileStartText, moveableTileEndText, moveableTileNotif;

    [Header("Asset References")]
    public GameObject tilePrefab;

    [HideInInspector]
    public static bool loadLevelOnStart = false;
    [HideInInspector]
    public static string levelToLoadOnStart = "";

    private List<GameObject> objects = new List<GameObject>();

    private MoveableTileEditStage moveableTileEditStage = MoveableTileEditStage.None;
    private Vector2 moveableTileStart, moveableTileEnd, currentHover = Vector2.zero;

    // [Header("Level Properties")]
    public string LevelName
    {
        get { return level.name; }
        set { level.name = value; }
    }

    [Header("Properties")]
    public string menuSceneName = "Menu";
    public string gameSceneName = "Game";

    private LevelData level = new LevelData(100, 100);

    void Start()
    {
        saveFirstObject.SetActive(false);
        levelLoadInputObject.SetActive(false);
        ToggleMoveableTileUI(); // Make sure it's off
        if (loadLevelOnStart)
        {
            LoadLevel(levelToLoadOnStart);
        }
    }

    public static bool LevelExists(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return File.Exists(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".json");
    }

    public void SetName(string name)
    {
        level.name = name;
    }

    public void Save()
    {
        Debug.Log("Saving level.");
        LevelSerialiser.Save(level);
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
        if (!File.Exists(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".json"))
        {
            Debug.Log("Level " + Application.persistentDataPath + "/Wolf3DLevels/" + name + ".json not found.");
            return;
        }

        Debug.Log("Opening level " + Application.persistentDataPath + "/Wolf3DLevels/" + name + ".json");

        ClearEditor();
        level = LevelSerialiser.LoadLevel(Application.persistentDataPath + "/Wolf3DLevels/" + name + ".json", false);
        levelNameControl.SetName(level.name);
        LoadTiles();
        LoadEntities();
    }

    public void LoadTiles()
    {
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.tiles[x, y] != null)
                {
                    GameObject go = Instantiate(tilePrefab);
                    go.transform.position = new Vector3(x, 1, y);
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", Tile.tileTypes[level.tiles[x, y].tileType].texture);
                    objects.Add(go);
                }
            }
        }
    }

    public void LoadEntities()
    {
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.entities[x, y] != null)
                {
                    GameObject go = Instantiate(tilePrefab);
                    go.transform.position = new Vector3(x, 1, y);
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", Entity.entityTypes[level.entities[x, y].entityType].texture);
                    objects.Add(go);
                }
            }
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

    public void Place(Vector3 location, bool buttonDown)
    {
        int x = (int)location.x;
        int y = (int)location.z;

        if (moveableTileEditStage == MoveableTileEditStage.PlaceStart && buttonDown)
        {
            if (!level.TileExistsAt(x, y))
            {
                SetMoveableTileError("A tile must exist at a moveable tile start position");
                return;
            }

            moveableTileStart = new Vector2(x, y);
            moveableTileStartText.text = $"Start pos: {moveableTileStart}";
            moveableTileEditStage = MoveableTileEditStage.PlaceEnd;
            return;
        }
        else if (moveableTileEditStage == MoveableTileEditStage.PlaceEnd && buttonDown)
        {
            if (level.TileExistsAt(x, y))
            {
                SetMoveableTileError("A tile must not exist at a moveable tile end positon");
                return;
            }

            moveableTileEnd = new Vector2(x, y);
            if (!MoveableTilePathValid()) return;

            Debug.Log($"Marked tile at {new Vector2((int)moveableTileStart.x, (int)moveableTileStart.y)} as moveable to location {new Vector2(x, y)}");
            level.SetTileMoveable((int)moveableTileStart.x, (int)moveableTileStart.y, x, y);
            moveableTileEditStage = MoveableTileEditStage.None;
            ToggleMoveableTileUI();
            return;
        }

        if (moveableTileEditStage != MoveableTileEditStage.None) return;
        if (tilePalette.selectedItemType == ItemType.None) return;
        if (level.ItemExistsAt(x, y)) return;
        if (tilePalette.selectedItemType == ItemType.Tile)
        {
            if (tilePalette.selectedTile == null) return;
            else PlaceTile(location);
        } else
        {
            if (tilePalette.selectedEntity == null) return;
            else PlaceEntity(location);
        }
    }

    public void PlaceTile(Vector3 location)
    {
        Tile t = new Tile();
        t.tileType = tilePalette.selectedTile.name;
        level.SetTileAt((int)location.x, (int)location.z, t);

        GameObject go = Instantiate(tilePrefab);
        go.transform.position = location;
        go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", tilePalette.selectedTile.texture);
        objects.Add(go);
    }

    public void PlaceEntity(Vector3 location)
    {
        Entity e = new Entity
        {
            entityType = tilePalette.selectedEntity.name
        };
        level.SetEntityAt((int)location.x, (int)location.z, e);

        GameObject go = Instantiate(tilePrefab);
        go.transform.position = location;
        go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", tilePalette.selectedEntity.texture);
        go.tag = "Entity";

        objects.Add(go);
    }

    public void RemoveItem(Vector3 location)
    {
        if (!level.ItemExistsAt((int)location.x, (int)location.z)) return;
        if (level.tiles[(int)location.x, (int)location.z] != null && level.tiles[(int)location.x, (int)location.z].moveableTile)
            level.moveableTileEndPositions.Remove(new Vector2((int)location.x, (int)location.y));
        level.RemoveItemAt((int)location.x, (int)location.z);

        Collider[] colliders = Physics.OverlapSphere(location, 0.2f);
        if (colliders.Length != 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Tile" || collider.tag == "Entity")
                {
                    objects.Remove(collider.gameObject);
                    Destroy(collider.gameObject);
                }
            }
        }
        else return;
    }

    public void FlashLevelName()
    {
        levelNameBG.color = Color.red;
        TweenC.Add(levelNameBG.gameObject, 1.5f, levelNameBGDefault);
        TweenA.Add(levelNameBG.gameObject, 1f, levelNameBGDefault.a);
    }

    public void PlayLevel()
    {
        if (LevelName == "Untitled Level")
        {
            FlashLevelName();
            return;
        }

        Save();
        LevelLoader.loadedFromEditor = true;
        LevelLoader.levelToLoad = LevelName;
        SceneManager.LoadScene(gameSceneName);
    }

    public void SaveButton()
    {
        if (LevelName == "Untitled Level")
            FlashLevelName();
        else
            Save();
    }

    public void ToggleMoveableTileUI()
    {
        moveableTileUI.SetActive(!moveableTileUI.activeSelf);
        moveableTileEditStage = moveableTileUI.activeSelf ? MoveableTileEditStage.PlaceStart : MoveableTileEditStage.None;
        moveableTileStartText.text = "Start pos: Selecting";
        moveableTileEndText.text = "End pos: Selecting";
        moveableTileError.SetActive(false);
    }

    public void SetMoveableTileError(string error)
    {
        moveableTileError.SetActive(true);
        moveableTileErrorText.text = error;
    }

    public bool MoveableTilePathValid()
    {
        bool moveX = false, moveY = false;

        if ((int)moveableTileStart.x != (int)moveableTileEnd.x) moveX = true;
        if ((int)moveableTileStart.y != (int)moveableTileEnd.y) moveY = true;

        if (moveX && moveY)
        {
            SetMoveableTileError("Moveable tiles must move in one axis only");
            return false;
        }

        return true;
    }

    public void HoverTile(int x, int y)
    {
        if (currentHover.x == x && currentHover.y == y) return;

        if (level.tiles[x, y] == null)
        {
            moveableTileNotif.text = "Tile is not moveable";
            return;
        }

        if (level.tiles[x, y].moveableTile) moveableTileNotif.text = "Tile is moveable"; else moveableTileNotif.text = "Tile is not moveable";

        currentHover = new Vector2(x, y);
    }
}
