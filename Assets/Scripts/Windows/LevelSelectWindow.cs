using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelSelectWindow : MonoBehaviour
{
    public Button[] levelButtons;
    public Text worldAPointText;


    static int level;


    Button currentBtn;
    Vector3 startRectTransform;
    Text currentText;
    int textMoveCount = 0;



    protected void Awake()
    {
        SaveManager.LoadLevels();
        AudioManager.instance.SetMainMenuMusic();
    }

    private void Start()
    { 
        
        AssignColorToLevelButton();
        worldAPointText.text = "" + SaveManager.miscInfo.worldAPoints + "/144";
        
    }


    public void OnBack()
    {
        AudioManager.instance.PlayButtonClick();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnLevelSelect()
    {

        AudioManager.instance.PlayButtonClick();
        AdManager.instance.InitiateUnityAd();

        var go = EventSystem.current.currentSelectedGameObject;
        Button btn = go.GetComponent<Button>();
        Text buttonText = btn.GetComponentInChildren<Text>();
        if (go != null)
        {
            int levelSelection = Convert.ToInt32(buttonText.text.ToString());
            level = levelSelection;
            if (levelSelection > 0 && levelSelection <= 16)
            {
                if (SaveManager.levelList[levelSelection - 1].levelWinType != LevelWinType.LOCKED)
                    SceneManager.LoadScene("GameScene01");
            }
            else if (levelSelection > 16 && levelSelection <= 32)
            {
                if (SaveManager.levelList[levelSelection - 1].levelWinType != LevelWinType.LOCKED)
                    SceneManager.LoadScene("GameScene02");
            } 
            else if (levelSelection > 32 && levelSelection <= 48)
            {
                if (SaveManager.levelList[levelSelection - 1].levelWinType != LevelWinType.LOCKED)
                    SceneManager.LoadScene("GameScene03");
            }
            
        }

        AudioManager.instance.SetRandomInGameMusic();

    }

    public static int GetLevel()
    {
        return level;
    }

    public static void SetLevel(int _level)
    {
        level = _level;
    }


    private void AssignColorToLevelButton()
    {
        //Level[] loadedLevels = FindObjectOfType<SaveManager>().GetLevelArray();
        //for (int i = 0; i < levels.Length; i++)
        //{
        //    levels[i] = new Level(true, Level.WinState.MEDIUM);//loadedLevels[i];
        //    if (i % 3 == 0)
        //        levels[i] = new Level(true, Level.WinState.BEST);

        //    if ((i + 2) % 2 == 0)
        //        levels[i] = new Level(true, Level.WinState.WORST);


        //    Level currentLevel = levels[i];

        int i = 0;
        foreach (Level level in SaveManager.levelList)
        {

            switch (level.levelWinType)
            {
                case LevelWinType.BEST:
                    levelButtons[i].image.sprite = Resources.Load<Sprite>("LSButtons/GreenLevelButtonUnpressed");
                    Sprite pressedSprite = Resources.Load<Sprite>("LSButtons/GreenLevelButtonPressed");
                    SpriteState st = new SpriteState();
                    st.pressedSprite = pressedSprite;
                    levelButtons[i].spriteState = st;
                    break;
                case LevelWinType.MEDIUM:
                    levelButtons[i].image.sprite = Resources.Load<Sprite>("LSButtons/LightBlueLevelButtonUnpressed");
                    Sprite pressedSpriteM = Resources.Load<Sprite>("LSButtons/LightBlueLevelButtonPressed");
                    SpriteState stM = new SpriteState();
                    stM.pressedSprite = pressedSpriteM;
                    levelButtons[i].spriteState = stM;
                    break;
                case LevelWinType.WORST:
                    levelButtons[i].image.sprite = Resources.Load<Sprite>("LSButtons/BlueLevelButtonUnpressed");
                    Sprite pressedSpriteW = Resources.Load<Sprite>("LSButtons/BlueLevelButtonPressed");
                    SpriteState stW = new SpriteState();
                    stW.pressedSprite = pressedSpriteW;
                    levelButtons[i].spriteState = stW;
                    break;
                case LevelWinType.UNLOCKED:
                    levelButtons[i].image.sprite = Resources.Load<Sprite>("LSButtons/UnlockedLevelButtonUnpressed");
                    Sprite pressedSpriteU = Resources.Load<Sprite>("LSButtons/UnlockedLevelButtonPressed");
                    SpriteState stU = new SpriteState();
                    stU.pressedSprite = pressedSpriteU;
                    levelButtons[i].spriteState = stU;
                    break;
                case LevelWinType.LOCKED:
                    levelButtons[i].image.sprite = Resources.Load<Sprite>("LSButtons/LockedLevelButtonUnpressed");
                    Sprite pressedSpriteL = Resources.Load<Sprite>("LSButtons/LockedLevelButtonPressed");
                    SpriteState stL = new SpriteState();
                    stL.pressedSprite = pressedSpriteL;
                    levelButtons[i].spriteState = stL;
                    break;
            }

            i++;
        }






        //}
    }

}
