using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Text moveText;
    public GameObject endGameTextPanel;
    public Image scoreColor;


    DeathCheck deathChecker;

    GameObject previousPlayer;
    

    LevelManager levelManager;

    Level currentLevel;

    LevelWinType initialWinType;

    public static int moves = 0;
    public static int totalPointsUnlocked;

    int retryCount = 0;

    private void Start()
    {
        totalPointsUnlocked = 0;
        moves = 0;
        levelManager = FindObjectOfType<LevelManager>();
        deathChecker = FindObjectOfType<DeathCheck>();
        deathChecker.levelLostEvent += LevelLost;
        deathChecker.levelWonEvent += LevelWon;
        previousPlayer = GameObject.FindGameObjectWithTag("Player");
        previousPlayer.GetComponent<Player>().increaseMoveEvent += IncrementMoveCount;
        endGameTextPanel.GetComponentInChildren<Text>().text = "Level " + LevelSelectWindow.GetLevel() + " Complete";
        currentLevel = SaveManager.levelList[levelManager.GetCurrentLevel() - 1];
        moveText.text = "0/" + currentLevel.bestMove;


        AdManager.instance.DestroyInterstitial();


    }


    private void LevelWon()
    {
        //Stop movement
        Player pc = previousPlayer.GetComponent<Player>();
        Destroy(pc);
        endGameTextPanel.SetActive(true);
        StartCoroutine(WaitToTransitionToEndGame());

        initialWinType = currentLevel.levelWinType;

        DetermineLevelDataUpdate();

        CheckForNewBlockUnlock();

        AdManager.instance.InitiateUnityAd();
        

    }

    private void LevelLost()
    {
        AudioManager.instance.PlayDeathSound();
        moves = 0;
        StartCoroutine(WaitToDeployAd());
        StartCoroutine(WaitToRespawn());
        

    }

    private void IncrementMoveCount()
    {
        moves++;
        moveText.text = "" + moves;
        DetermineSquareColor();
    }

    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(1.0f);

        Respawn();

    }

    IEnumerator WaitToTransitionToEndGame()
    {
        yield return new WaitForSeconds(3.15f);


        SceneManager.LoadScene("EndGameMenu");

    }

    IEnumerator WaitToDeployAd()
    {
        yield return new WaitForSeconds(0.4f);
        HandleAdInfo();
    }


    private void Respawn()
    {
        levelManager.ResetBridges();
        levelManager.ResetDisappearingBlocks();

        Destroy(previousPlayer);
        moveText.text = "" + moves;
        DetermineSquareColor();
        previousPlayer = levelManager.SpawnBlock();



        //Reassign events
        deathChecker = previousPlayer.GetComponent<DeathCheck>();
        deathChecker.levelLostEvent += LevelLost;
        deathChecker.levelWonEvent += LevelWon;
        previousPlayer.GetComponent<Player>().increaseMoveEvent += IncrementMoveCount;
    }

    private void DetermineLevelDataUpdate()
    {
        //Update the next level if it is locked
        if (levelManager.GetCurrentLevel() != SaveManager.levelList.Length && SaveManager.levelList[levelManager.GetCurrentLevel()].levelWinType == LevelWinType.LOCKED)
        {
            if (levelManager.GetCurrentLevel() != SaveManager.levelList.Length + 1)
            {
                SaveManager.levelList[levelManager.GetCurrentLevel()].levelWinType = LevelWinType.UNLOCKED;
                SaveManager.SaveLevels();
            }
        }

        //Level currentLevel = SaveManager.levelList[levelManager.GetCurrentLevel() - 1];

        if (moves < currentLevel.bestPersonalMove)
        {
            currentLevel.bestPersonalMove = moves;
        }

        if (moves <= currentLevel.bestMove)
        {
            currentLevel.levelWinType = LevelWinType.BEST;
        }
        else if (moves <= currentLevel.mediumMove)
        {
            if (currentLevel.levelWinType != LevelWinType.BEST)
            {
                currentLevel.levelWinType = LevelWinType.MEDIUM;
            }
        }
        else
        {
            if (currentLevel.levelWinType != LevelWinType.BEST && currentLevel.levelWinType != LevelWinType.MEDIUM)
            {
                currentLevel.levelWinType = LevelWinType.WORST;
            }
        }


        SaveManager.SaveLevels();
       
    }

    private void HandleAdInfo()
    {
        AdManager.instance.DetermineIfAdShouldShowInGame();
    }

    private void DetermineSquareColor()
    {
        currentLevel = SaveManager.levelList[levelManager.GetCurrentLevel() - 1];

        if (moves <= currentLevel.bestMove)
        {
            scoreColor.color = new Color(0.10980f, 0.905882f, 0.10980f);
            moveText.text += "/" + currentLevel.bestMove;
        }
        else if (moves <= currentLevel.mediumMove)
        {
            scoreColor.color = new Color(0, 0.54901961f, 0.82745098f);
            moveText.text += "/" + currentLevel.mediumMove;
        }
        else
        {
            scoreColor.color = new Color(0, 0.196f, 0.69f);

        }

    }

    public void OnRespawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HandleAdInfo();

    }

    public void CheckForNewBlockUnlock()
    {
        int previousPoints = SaveManager.miscInfo.worldAPoints;
        //Set points unlocked for world A
        if (initialWinType == LevelWinType.BEST)
        {
            totalPointsUnlocked = 0;
        }
        else if (initialWinType == LevelWinType.MEDIUM)
        {
            if (currentLevel.levelWinType == LevelWinType.BEST)
            {
                totalPointsUnlocked = 1;
            }
        }
        else if (initialWinType == LevelWinType.WORST)
        {
            if (currentLevel.levelWinType == LevelWinType.BEST)
            {
                totalPointsUnlocked = 2;
            }

            if (currentLevel.levelWinType == LevelWinType.MEDIUM)
            {
                totalPointsUnlocked = 1;
            }
        }
        else
        {
            print("In the else");

            if (currentLevel.levelWinType == LevelWinType.BEST)
            {
                totalPointsUnlocked = 3;
            }

            if (currentLevel.levelWinType == LevelWinType.MEDIUM)
            {
                totalPointsUnlocked = 2;
            }

            if (currentLevel.levelWinType == LevelWinType.WORST)
            {
                totalPointsUnlocked = 1;
            }
        }

        int newTotalPoints = previousPoints + totalPointsUnlocked;

        int i = previousPoints + 1;
        while (i % 12 != 0)
        {
            i++;
        }



        //Check if a new color is unlocked from the new points assigned
        if (newTotalPoints >= i)
        {
            if (i < 132)
            {
                SaveManager.miscInfo.newColorUnlocked = true;
            }
            else
            {
                if (newTotalPoints == 144)
                {
                    SaveManager.miscInfo.newColorUnlocked = true;

                }
            }
            
        }
            

        //save
        SaveManager.miscInfo.worldAPoints = newTotalPoints;
        SaveManager.SaveMiscInfo(); 




        SaveManager.SaveMiscInfo();

    }



}

