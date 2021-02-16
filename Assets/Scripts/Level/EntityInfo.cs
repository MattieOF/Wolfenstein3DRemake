using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Entity", menuName = "Game/Entity")]
public class EntityInfo : ScriptableObject
{
    [Header("Editor Values")]
    public string editorIconName;

    [Header("Entity Properties")]
    public string entityPrefabName;
    public string entityName;
    public bool isEnemy = false;
    public bool useEditorIcon = false;
    public bool collidable = true;

    [JsonIgnore]
    [HideInInspector]
    public Texture texture;
}
