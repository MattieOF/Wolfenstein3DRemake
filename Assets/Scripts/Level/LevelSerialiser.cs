using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LevelSerialiser
{
    public static void Save(LevelData level)
    {
        string data = JsonConvert.SerializeObject(level, Formatting.Indented, new JsonSerializerSettings()
                                                                              {
                                                                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                              });
        if (!Directory.Exists(Application.persistentDataPath + "/Wolf3DLevels/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Wolf3DLevels/");
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Wolf3DLevels/" + level.name + ".json");
        writer.Write(data);
        writer.Close();
    }

    public static LevelData LoadLevel(string path, bool fromResources)
    {
        StreamReader reader;
        if (fromResources)
        {
            TextAsset level = Resources.Load<TextAsset>($"Maps/{path}");
            reader = new StreamReader(new MemoryStream(level.bytes));
        } 
        else
        {
            reader = new StreamReader(path);
        }

        LevelData data = JsonConvert.DeserializeObject<LevelData>(reader.ReadToEnd());
        reader.Close();
        return data;
    }
}
