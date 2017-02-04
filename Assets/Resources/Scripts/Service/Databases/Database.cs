using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class Database : MonoBehaviour
{

    private JsonData json_Data;


    // Use this for initialization
    void Start()
    {

    }

    public virtual JsonData LoadBase(string name_base)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string filepath = Application.persistentDataPath + "/" + name_base + ".json";

            if (!File.Exists(filepath))

            {

                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + name_base + ".json");

                //TODO A coroutine instead of this dangerous cliff
                while (!loadDB.isDone) { }

                File.WriteAllBytes(filepath, loadDB.bytes);
            }

            json_Data = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "/" + name_base + ".json"));
            return json_Data;


        }
        else
        {
            if (!File.Exists(Application.dataPath + "/" + name_base + ".json"))
                File.WriteAllText(Application.dataPath + "/" + name_base + ".json", JsonMapper.ToJson(new JsonData()));
            json_Data = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/" + name_base + ".json"));
            return json_Data;
        }


    }

    public string PrettyPrint(object obj)
    {
        JsonWriter writer = new JsonWriter();
        writer.PrettyPrint = true;
        JsonMapper.ToJson(obj, writer);
        return writer.ToString();
    }

}
