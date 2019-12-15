using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWonWindow : MonoBehaviour {

    public Image scoreColor;
    public Text moveText;
    public Text bestMoveText;

    public Text levelCompleteText;


    // Use this for initialization
    void Start()
    {
        AudioManager.instance.SetMusicVolumeToZero();
        levelCompleteText.text = "Level " + LevelSelectWindow.GetLevel() + " Complete";
        print(LevelSelectWindow.GetLevel());
        print(levelCompleteText.text);
        moveText.text = "" + GameManager.moves;
        bestMoveText.text = "" + SaveManager.levelList[LevelSelectWindow.GetLevel() - 1].bestPersonalMove;
        DetermineScoreColor();

        StartCoroutine(PlayScoreSound());


    }

    public void OnHome()
    {
        AudioManager.instance.SetMainMenuMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestart()
    {
        AudioManager.instance.SetRandomInGameMusic();
        int levelSelection = LevelSelectWindow.GetLevel();
        DetermineSceneToLoad(levelSelection);
    }

    public void OnRate()
    {
        //Open up the app store
    }

    private void DetermineSceneToLoad(int level)
    {
        int levelSelection = level;
        if (levelSelection > 0 && levelSelection <= 16)
        {
            SceneManager.LoadScene("GameScene01");
        }
        else if (levelSelection > 16 && levelSelection <= 32)
        {
            SceneManager.LoadScene("GameScene02");
        }
        else if (levelSelection > 32 && levelSelection <= 48)
        {
            SceneManager.LoadScene("GameScene03");
        }
    }

    private void DetermineScoreColor()
    {
        if (GameManager.moves <= SaveManager.levelList[LevelSelectWindow.GetLevel() - 1].bestMove)
        {
            scoreColor.color = new Color(0.10980f, 0.905882f, 0.10980f);
        }
        else if (GameManager.moves <= SaveManager.levelList[LevelSelectWindow.GetLevel() - 1].mediumMove)
        {
            scoreColor.color = new Color(0, 0.54901961f, 0.82745098f);
        }
        else
        {
            scoreColor.color = new Color(0, 0.196f, 0.69f);
        }
    }

    IEnumerator PlayScoreSound()
    {
        yield return new WaitForSeconds(1.08f);

        AudioManager.instance.PlayScoreIndicatorSound();
    }

}
