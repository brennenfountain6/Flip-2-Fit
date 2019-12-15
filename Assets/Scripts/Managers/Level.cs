using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Level : MonoBehaviour
{

    public LevelData data = new LevelData();

    public int bestPersonalMove = 1;

    public int bestMove = 1;

    public int mediumMove = 1;

    public int worstMove = 1;

    public int availablePoints = 30;

    public LevelWinType levelWinType = LevelWinType.BEST;

    public void StoreData()
    {
        data.bestMove = bestMove;
        data.mediumMove = mediumMove;
        data.worstMove = worstMove;
        data.bestPersonalMove = bestPersonalMove;
        data.levelWinType = levelWinType;
        data.availablePoints = availablePoints;
    }

    //public void LoadData()
    //{
    //    print(data.bestMove);
    //    bestMove = data.bestMove;
    //    mediumMove = data.mediumMove;
    //    worstMove = data.worstMove;
    //    bestPersonalMove = data.bestPersonalMove;
    //    levelWinType = data.levelWinType;
    //    availablePoints = data.availablePoints;
    //}

    public void ApplyData()
    {
        //SaveData.AddLevelData(data);
    }

    private void OnEnable()
    {
        //SaveData.OnLoaded += LoadData;
        SaveData.OnBeforeSave += StoreData;
        SaveData.OnBeforeSave += ApplyData;
    }

    private void OnDisable()
    {
        //SaveData.OnLoaded -= LoadData;
        SaveData.OnBeforeSave -= StoreData;
        SaveData.OnBeforeSave -= ApplyData;
    }


}

public enum LevelWinType
{
    BEST, MEDIUM, WORST, UNLOCKED, LOCKED
}

[Serializable]
public class LevelData
{
    public int bestPersonalMove;

    public int bestMove;

    public int mediumMove;

    public int worstMove;

    public int availablePoints;

    public LevelWinType levelWinType;


}
