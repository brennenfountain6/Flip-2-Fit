using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscInfo : MonoBehaviour 
{

    public int shopColorChoiceIndex;
    public int worldAPoints;
    public bool adsDisabled;
    public bool newColorUnlocked;
    public bool soundDisabled;

    public MiscInfo()
    {
        shopColorChoiceIndex = 0;
        worldAPoints = 0;
        adsDisabled = false;
        newColorUnlocked = false;
        soundDisabled = false;
    }
}

public class MiscInfoData
{
    public int shopColorChoiceIndex;
    public int worldAPoints;
    public bool adsDisabled;
    public bool newColorUnlocked;
    public bool soundDisabled;

}
