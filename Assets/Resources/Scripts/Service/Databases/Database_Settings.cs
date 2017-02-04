using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class Database_Settings : Database
{
    private Setting currentSetting;
    private JsonData json_settingsData;
    void Awake()
    {
        json_settingsData = LoadBase("Settings");
        Initialise_Settings_Database(json_settingsData.ToJson(), "Settings");
    }



    public Setting get_Current_Setting()
    {
        return currentSetting;
    }

    public void Initialise_Settings_Database(string file, string baseName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (file == "")
            {
                currentSetting = new Setting(new RandomStats(0.25, 0.5), new RandomStats(0.1, 0.1), new RandomStats(10.0, 10.0), 0, 5);

                string filepath = Application.persistentDataPath + "/" + baseName + ".json";
                if (!File.Exists(filepath))

                {

                    WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + baseName + ".json");
                    //TODO A coroutine instead of this dangerous cliff
                    while (!loadDB.isDone) { }
                    File.WriteAllBytes(filepath, loadDB.bytes);
                }
                File.WriteAllText(filepath, PrettyPrint(currentSetting));
            }
            else
            {
                currentSetting = JsonMapper.ToObject<Setting>(json_settingsData.ToJson());
            }

        }
        else
        {
            if (file == "")
            {
                currentSetting = new Setting(new RandomStats(0.25, 0.5), new RandomStats(0.1, 0.1), new RandomStats(10.0, 10.0), 0, 5);
                File.WriteAllText(Application.dataPath + "/" + baseName + ".json", PrettyPrint(currentSetting));
            }
            else
            {
                currentSetting = JsonMapper.ToObject<Setting>(json_settingsData.ToJson());
            }

        }
    }


}

[System.Serializable]
public class Setting
{
    public RandomStats OnClick_Time_To_Appear;
    public RandomStats Static_Time_To_Appear;
    public RandomStats Static_Time_To_Dissappear;
    public int Starting_Torches_Count;
    public int Target_Count;

    public Setting(RandomStats onClick, RandomStats static_Appear, RandomStats static_Dissappear,int starting_Torches_Count, int target_Count)
    {
        OnClick_Time_To_Appear = onClick;
        Static_Time_To_Appear = static_Appear;
        Static_Time_To_Dissappear = static_Dissappear;
        Starting_Torches_Count = starting_Torches_Count;
        Target_Count = target_Count;
    }
    public Setting()
    {

    }
}
[System.Serializable]
public class RandomStats
{
    public double Min;
    public double Max;

    public RandomStats(double min, double max)
    {
        Max = max;
        Min = min;
    }
    public RandomStats()
    {

    }
}