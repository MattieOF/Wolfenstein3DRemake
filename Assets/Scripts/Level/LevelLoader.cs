using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [HideInInspector]
    public static string levelToLoad = "";
    [HideInInspector]
    public static bool loadedFromEditor = false;

    [Header("Scene References")]
    public GameObject levelObjects;
    public GameObject playerObject, tilePrefab, multiplePlayerStartsWarning;
    public MapInfoPanel mapInfoPanel;
    public GameObject pauseMenu;
    public TextMeshProUGUI mapNameText, quitButtonText;
    public PlayerMouseLook mouseLook;

    [Header("Asset References")]
    public string menuSceneName = "Menu";
    public string editorSceneName = "Editor";
    public GameObject playerPrefab;

    private LevelData level;
    private int tilesCount = 0, enemyCount = 0, entityCount = 0;
    private bool pauseMenuActive = false;

    void Start()
    {
        LoadLevel(levelToLoad);
        if (loadedFromEditor) quitButtonText.text = "Return to Editor";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pauseMenuActive) ClosePauseMenu(); else OpenPauseMenu();
    }

    public void OpenPauseMenu()
    {
        pauseMenuActive = true;
        pauseMenu.SetActive(true);
        pauseMenu.GetComponent<Animator>().Play("OpenPauseMenu");
        mouseLook.UnlockCursor();
        mapInfoPanel.StopTimer();
    }

    public void ClosePauseMenu()
    {
        pauseMenuActive = false;
        pauseMenu.GetComponent<Animator>().Play("ClosePauseMenu");
        mouseLook.LockCursor();
        mapInfoPanel.StartTimer();
    }

    public void LoadLevel(string levelName)
    {
        if (!EditorManager.LevelExists(levelName)) return;

        string path = Application.persistentDataPath + "/Wolf3DLevels/" + levelName + ".xml";
        StreamReader file;
        if (File.Exists(path))
            file = new StreamReader(path);
        else
        {
            TextAsset level = Resources.Load<TextAsset>($"Maps/{levelName}");
            if (level)
            {
                file = new StreamReader(new MemoryStream(level.bytes));
                path = levelName;
            }
            else
            {
                Debug.LogError($"Couldn't find map with name {levelName}");
                return;
            }
        }

        Debug.Log("Opening level " + path);

        ClearLevel();
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        level = (LevelData)serializer.Deserialize(file.BaseStream);
        file.Close();
        LoadTiles();
        LoadEntities();
        mouseLook = playerObject.GetComponentInChildren<PlayerMouseLook>();
        mapInfoPanel.SetValues(level.name, tilesCount, enemyCount);
        mapNameText.text = level.name;
    }

    public void LoadTiles()
    {
        tilesCount = 0;
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.tiles[x][y] != null)
                {
                    tilesCount++;
                    GameObject go = Instantiate(tilePrefab, levelObjects.transform);
                    go.transform.position = new Vector3(x, 1, y);
                    level.tiles[x][y].texture = Resources.Load("Textures/" + level.tiles[x][y].textureName, typeof(Texture)) as Texture;
                    go.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", level.tiles[x][y].texture);

                    if (level.tiles[x][y].moveableTile)
                    {
                        MoveableTile mt = go.AddComponent<MoveableTile>();
                        mt.endPosition = level.tiles[x][y].tileMoveTo;
                    }

                    Vector2[] uvs = go.GetComponent<MeshFilter>().sharedMesh.uv;

                    uvs[6] = new Vector2(0, 0);
                    uvs[7] = new Vector2(1, 0);
                    uvs[10] = new Vector2(0, 1);
                    uvs[11] = new Vector2(1, 1);

                    go.GetComponent<MeshFilter>().sharedMesh.uv = uvs;
                }
            }
        }
    }

    public void LoadEntities()
    {
        bool loadedPlayer = false;
        
        entityCount = 0;
        enemyCount = 0;
        for (int x = 0; x < level.levelSize.x; x++)
        {
            for (int y = 0; y < level.levelSize.y; y++)
            {
                if (level.entities[x][y] != null)
                {
                    if (level.entities[x][y].entityName == "PlayerStart")
                    {
                        if (loadedPlayer)
                        {
                            Debug.LogWarning("Multiple player starts in the level.");
                            multiplePlayerStartsWarning.SetActive(true);
                            Destroy(playerObject);
                        }

                        Vector3 pos = new Vector3(x, 1, y);
                        playerObject = Instantiate(playerPrefab, pos, Quaternion.identity);
                        loadedPlayer = true;
                    } else
                    {
                        entityCount++;
                        if (level.entities[x][y].isEnemy) enemyCount++;
                        GameObject go = Instantiate(Resources.Load<GameObject>("Entities/" + level.entities[x][y].entityPrefabName), levelObjects.transform);
                        go.transform.position = new Vector3(x, 1, y);
                        if (level.entities[x][y].useEditorIcon)
                        {
                            level.entities[x][y].texture = Resources.Load("Textures/" + level.entities[x][y].editorIconName, typeof(Texture)) as Texture;
                            go.GetComponentInChildren<MeshRenderer>().materials[0].SetTexture("_MainTex", level.entities[x][y].texture);
                        }
                        if (!level.entities[x][y].collidable)
                            Destroy(go.GetComponentInChildren<BoxCollider>());
                    }
                }
            }
        }

        if (!loadedPlayer)
        {
            Debug.LogWarning("No player starts in the level, so the player is spawned at the center of the level");
            playerObject = Instantiate(playerPrefab);
            playerPrefab.transform.localPosition = new Vector3(level.tiles.Length / 2, 1, level.tiles[0].Length / 2); // Spawn in middle of the level
        }
    }

    public void ClearLevel()
    {
        foreach (Transform t in levelObjects.transform)
            Destroy(t.gameObject);
    }

    public void Quit()
    {
        if (loadedFromEditor)
        {
            EditorManager.levelToLoadOnStart = level.name;
            EditorManager.loadLevelOnStart = true;
            SceneManager.LoadScene(editorSceneName);
        } else
        {
            // TOOD Ask about saving
            SceneManager.LoadScene(menuSceneName);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
