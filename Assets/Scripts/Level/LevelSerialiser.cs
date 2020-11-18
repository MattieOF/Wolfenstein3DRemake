using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LevelSerialiser
{
    public static void Save(LevelData level)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        if (!Directory.Exists(Application.persistentDataPath + "/Wolf3DLevels/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Wolf3DLevels/");
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Wolf3DLevels/" + level.name + ".xml");
        serializer.Serialize(writer.BaseStream, level);
        writer.Close();
    }
}
