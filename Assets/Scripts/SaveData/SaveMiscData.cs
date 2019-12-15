using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveMiscData : MonoBehaviour
{
    public static MiscInfoData miscInfoContainer = new MiscInfoData();

    public delegate void SerializeAction();
    public static event SerializeAction OnLoaded;
    public static event SerializeAction OnBeforeSave;


    public static void Save(string path, MiscInfoData info)
    {
        SaveMiscInfo(path, info);

        //ClearLevelList();
    }
    public static void Load(string path)
    {



        string json = File.ReadAllText(path);


        MiscInfoData miData = JsonUtility.FromJson<MiscInfoData>(json);

        SaveManager.miscInfo.adsDisabled = miData.adsDisabled;
        SaveManager.miscInfo.shopColorChoiceIndex = miData.shopColorChoiceIndex;
        SaveManager.miscInfo.newColorUnlocked = miData.newColorUnlocked;
        SaveManager.miscInfo.soundDisabled = miData.soundDisabled;
        SaveManager.miscInfo.worldAPoints = miData.worldAPoints;

    }


    private static void SaveMiscInfo(string path, MiscInfoData miscInfo)
    {
        miscInfo.adsDisabled = SaveManager.miscInfo.adsDisabled;
        miscInfo.shopColorChoiceIndex = SaveManager.miscInfo.shopColorChoiceIndex;
        miscInfo.newColorUnlocked = SaveManager.miscInfo.newColorUnlocked;
        miscInfo.soundDisabled = SaveManager.miscInfo.soundDisabled;
        miscInfo.worldAPoints = SaveManager.miscInfo.worldAPoints;


        string json = JsonUtility.ToJson(miscInfo);

        StreamWriter sw = File.CreateText(path);
        sw.Close();

        File.WriteAllText(path, json);
    }


}

