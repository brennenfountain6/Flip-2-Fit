using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EndGameWindow : MonoBehaviour {

    public Image scoreColor;
    public Text moveText;
    public Text bestMoveText;
    public Image alertImage;
    public Text pointsAwarded;
    public Text totalPoints;
    public Image border;

    public Text levelCompleteText;
    public Button nextLevelButton;

    [SerializeField] string appID;

    Animator anim;

    float increaseRate = .14f;

    int levelNumber;

    int points, startPoints;
    float nextIncreaseTime = 0;

    bool hasAdBeenSet = false;
    bool shouldShowAd = false;

    float animDuration;
    float elapsedTime;
    bool bFinished;


    // Use this for initialization
    void Start()
    {

        AudioManager.instance.SetMusicVolumeToZero();
        anim = border.GetComponent<Animator>();
        levelNumber = LevelSelectWindow.GetLevel();
        levelCompleteText.text = "Level " + levelNumber + " Complete";
        moveText.text = "" + GameManager.moves;
        bestMoveText.text = "" + SaveManager.levelList[LevelSelectWindow.GetLevel() - 1].bestPersonalMove;
        DetermineScoreColor();

        CheckIfGameWonMenuShouldShow();

        if (SaveManager.miscInfo.newColorUnlocked)
        {
            alertImage.enabled = true;
        }
        else
        {
            alertImage.enabled = false;
        }

        points = GameManager.totalPointsUnlocked;
        startPoints = points;

        pointsAwarded.text = "+" + points;
        totalPoints.text = "" + (SaveManager.miscInfo.worldAPoints - points)  + "/144";


        int rand = Random.Range(1, 4);
        int rand2 = Random.Range(1, 4);
        if (rand == rand2)
        {
            AdManager.instance.RequestInterstitial();
            shouldShowAd = true;
        }

        animDuration = anim.GetCurrentAnimatorStateInfo(0).length - 2.78f;
        elapsedTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        bFinished = (elapsedTime > animDuration);


        appID = "1281303037";

        StartCoroutine(PlayScoreSound());
    }

    private void Update()
    {

        elapsedTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        bFinished = (elapsedTime > animDuration / 2f);

        if (bFinished)
        {
            if (points > 0 && Time.time > nextIncreaseTime)
            {
                totalPoints.text = "" + (SaveManager.miscInfo.worldAPoints - points + 1) + "/144";
                print(totalPoints.text);
                nextIncreaseTime = Time.time + increaseRate;
                points -= 1;
            }


        }


        if (Time.timeSinceLevelLoad > 3.95f)
        {
            if (!hasAdBeenSet)
            {
                hasAdBeenSet = true;

                if (shouldShowAd)
                {
                    AdManager.instance.ShowInterstitial();
                }
            }
        }
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

    public void OnPlayNext()
    {
        if (levelNumber < 48)
        {
            AudioManager.instance.SetRandomInGameMusic();
            int levelSelection = LevelSelectWindow.GetLevel() + 1;
            LevelSelectWindow.SetLevel(levelSelection);
            DetermineSceneToLoad(levelSelection);
        }
        else
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appID);
        }

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

    private void CheckIfGameWonMenuShouldShow()
    {
        if (levelNumber == 48)
        {
            nextLevelButton.image.sprite = Resources.Load<Sprite>("GameWonMenu/RateButtonStarUnpressed");
            Sprite pressedSprite = Resources.Load<Sprite>("GameWonMenu/RateButtonStarPressed");
            SpriteState st = new SpriteState();
            st.pressedSprite = pressedSprite;
            nextLevelButton.spriteState = st;
        }
    }

    private void DetermineScoreColor()
    {
        if (GameManager.moves <= SaveManager.levelList[LevelSelectWindow.GetLevel() - 1].bestMove)
        {
            scoreColor.color = new Color(0.10980f , 0.905882f , 0.10980f);
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
