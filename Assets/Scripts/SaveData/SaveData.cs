using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    public static LevelContainer levelContainer = new LevelContainer();

    public delegate void SerializeAction();
    public static event SerializeAction OnLoaded;
    public static event SerializeAction OnBeforeSave;


    public static void Save(string path, LevelContainer levels)
    {
        OnBeforeSave();
        SaveLevels(path, levels);

        //ClearLevelList();
    }
    public static void Load(string path)
    {

        int i = 0;


        string json = File.ReadAllText(path);


        foreach (LevelData data in JsonUtility.FromJson<LevelContainer>(json).levels)
        {
            //SaveManager.levelList[i].bestMove = data.bestMove;
            //SaveManager.levelList[i].mediumMove = data.mediumMove;
            //SaveManager.levelList[i].worstMove = data.worstMove;
            SaveManager.levelList[i].levelWinType = data.levelWinType;
            SaveManager.levelList[i].bestPersonalMove = data.bestPersonalMove;
            SaveManager.levelList[i].availablePoints = data.availablePoints;
            i++;
        }

    }

    public static void AddLevelData(LevelData data)
    {
        levelContainer.levels.Add(data);
    }


    private static LevelContainer LoadLevels(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<LevelContainer>(json);
    }

    private static void SaveLevels(string path, LevelContainer levels)
    {

        int i = 0;

        foreach (Level level in SaveManager.levelList)
        {
            //levels.levels[i].bestMove = level.bestMove;
            //levels.levels[i].mediumMove = level.mediumMove;
            //levels.levels[i].worstMove = level.worstMove;
            levels.levels[i].levelWinType = level.levelWinType;
            levels.levels[i].bestPersonalMove = level.bestPersonalMove;
            levels.levels[i].availablePoints = level.availablePoints;
            i++;
        }




        string json = JsonUtility.ToJson(levels);

        StreamWriter sw = File.CreateText(path);
        sw.Close();

        File.WriteAllText(path, json);
    }


}
