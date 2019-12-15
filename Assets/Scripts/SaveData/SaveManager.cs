using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{

    public static List<LevelData> testContainer;

    private static string dataPath = string.Empty;
    private static string miscInfoDataPath = string.Empty;

    public static Level[] levelList;

    public static MiscInfo miscInfo;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        dataPath = System.IO.Path.Combine(Application.persistentDataPath, "levels.json");
        miscInfoDataPath = System.IO.Path.Combine(Application.persistentDataPath, "miscinfo.json");

        levelList = GetComponentsInChildren<Level>();
        miscInfo = GetComponentInChildren<MiscInfo>();


        for (int i = 0; i < levelList.Length; i++)
        {
            SaveData.levelContainer.levels.Add(new LevelData());
        }

        if (File.Exists(dataPath))
        {
            LoadLevels();
        }

        if (File.Exists(miscInfoDataPath))
        {
            LoadMiscInfo();
        }


        SaveMiscInfo();
        SaveLevels();





    }




    public static void SaveLevels()
    {
        SaveData.Save(dataPath, SaveData.levelContainer);
    }

    public static void LoadLevels()
    {
        SaveData.Load(dataPath);
    }

    public static void SaveMiscInfo()
    {
        SaveMiscData.Save(miscInfoDataPath, SaveMiscData.miscInfoContainer);
    }

    public static void LoadMiscInfo()
    {
        SaveMiscData.Load(miscInfoDataPath);
    }

    public static string GetDataPath()
    {
        return dataPath;
    }


}



