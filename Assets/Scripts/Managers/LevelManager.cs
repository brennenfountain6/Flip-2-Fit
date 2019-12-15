using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    TextAsset textAsset;

    public GameObject player;

    //Platform objects
    public GameObject basicPlatform;  //0
    public GameObject goalPlatform;   //1
    public GameObject bridge;         //2
    public GameObject disappearingBlock;  //3
    public GameObject standingTrigger;//4

    //Remake bridge
    List<GameObject> bridgeInstances;
    List<GameObject> buttonInstances;
    Vector3[] bridgeStartPositions;

    //Remake Disappearing Blocks
    List<GameObject> disappearingInstances;

    //Direction[] bridgeStartDirections;

    GameManager gameManager;

    int currentLevel;
    Vector2 spawnPosition;

    string[] levelCreationInfo;

    List<GameObject> allMapObjects;

    private void Awake()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "GameScene01":
                textAsset = Resources.Load<TextAsset>("LevelLoaderText/LevelCreation01");
                break;
            case "GameScene02":
                textAsset = Resources.Load<TextAsset>("LevelLoaderText/LevelCreation02");
                break;
            case "GameScene03":
                textAsset = Resources.Load<TextAsset>("LevelLoaderText/LevelCreation03");
                break;
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        bridgeInstances = new List<GameObject>();
        buttonInstances = new List<GameObject>();
        allMapObjects = new List<GameObject>();
        disappearingInstances = new List<GameObject>();
        currentLevel = LevelSelectWindow.GetLevel();
        GenerateMap(currentLevel);
        bridgeStartPositions = new Vector3[bridgeInstances.Count];
        //bridgeStartDirections = new Direction[bridgeInstances.Count];
        GetBridgeStartPositionsAndDirections();

    }

    private void GenerateMap(int levelIndex)
    {
        bool shouldMoveMapDown = false;

        if (currentLevel > 0 && currentLevel <= 8)
            shouldMoveMapDown = true;
        else if (currentLevel > 16 && currentLevel <= 24)
            shouldMoveMapDown = true;
        else if (currentLevel > 32 && currentLevel <= 40)
            shouldMoveMapDown = true;
        else
            shouldMoveMapDown = false;

        levelCreationInfo = textAsset.text.Split('\n');

        //Find the starting index for the level
        int startingLineIndex = 0;
        for (int i = 0; i < levelCreationInfo.Length; i++)
        {
            string line = levelCreationInfo[i];
            if (line[0] == 'L')
            {
                string sLevel = line.Split(':')[1];
                int level = Convert.ToInt32(sLevel);
                if (level == levelIndex)
                {
                    startingLineIndex = i;
                    break;
                }
            }
        }

        //Get the spawn position for the block
        string sSpawnValues01 = levelCreationInfo[startingLineIndex + 1].Split(':')[1];
        string[] sSpawnValues02 = sSpawnValues01.Split(',');

        float xSpawnPosition = (float)Convert.ToDouble(sSpawnValues02[0]);
        float ySpawnPosition = (float)Convert.ToDouble(sSpawnValues02[1]);

        if (shouldMoveMapDown)
        {
            spawnPosition = new Vector2(xSpawnPosition, ySpawnPosition - 0.5f);
        }
        else
        {
            spawnPosition = new Vector2(xSpawnPosition, ySpawnPosition);
        }



        //Loop through the .txt file from the starting position and create 
        int j = startingLineIndex + 2;

        while (levelCreationInfo[j][0] != 'L')
        {
            string line = levelCreationInfo[j];
            string[] sValues = line.Split(',');
            float[] values = new float[sValues.Length];
            for (int i = 0; i < 3; i++)
            {
                double dValue = Convert.ToDouble(sValues[i]);
                values[i] = (float)dValue;
            }

            //Build the basic platform
            if (values[0] == 0)
            {
                Vector2 position;
                if (shouldMoveMapDown)
                {
                    position = new Vector2((float)values[1], (float)values[2] - 0.5f);
                }
                else
                {
                    position = new Vector2((float)values[1], (float)values[2]);
                }
                
                GameObject basicPlatformInstance = Instantiate(basicPlatform, new Vector3(position.x, -0.15f, position.y), Quaternion.identity);
                allMapObjects.Add(basicPlatformInstance);
            }
            else if (values[0] == 1)
            {
                Vector2 position;
                if (shouldMoveMapDown)
                {
                    position = new Vector2((float)values[1], (float)values[2] - 0.5f);
                }
                else
                {
                    position = new Vector2((float)values[1], (float)values[2]);
                }
                GameObject goalInstance = Instantiate(goalPlatform, new Vector3(position.x, -0.15f, position.y), Quaternion.identity);
                allMapObjects.Add(goalInstance);

            }
            else if (values[0] == 2)
            {
                Vector2 position;
                if (shouldMoveMapDown)
                {
                    position = new Vector2((float)values[1], (float)values[2] - 0.5f);
                }
                else
                {
                    position = new Vector2((float)values[1], (float)values[2]);
                }
                GameObject bridgeInstance = Instantiate(bridge, new Vector3(position.x, -0.16f, position.y), Quaternion.identity);

                float yRotation = (sValues[3] == "straight") ? 90 : 0;
                bridgeInstance.transform.eulerAngles = new Vector3(0f, yRotation, 0f);

                string tag = sValues[4];
                bridgeInstance.gameObject.tag = tag;

                bridgeInstances.Add(bridgeInstance);
                allMapObjects.Add(bridgeInstance);

            }
            else if (values[0] == 3)
            {
                Vector2 position;
                if (shouldMoveMapDown)
                {
                    position = new Vector2((float)values[1], (float)values[2] - 0.5f);
                }
                else
                {
                    position = new Vector2((float)values[1], (float)values[2]);
                }
                //Disappearing Block
                GameObject disappearingBlockInstance = Instantiate(disappearingBlock, new Vector3(position.x, -0.15f, position.y), Quaternion.identity);
                disappearingInstances.Add(disappearingBlockInstance);
                allMapObjects.Add(disappearingBlockInstance);

            }
            else if (values[0] == 4)
            {
                Vector2 position;
                if (shouldMoveMapDown)
                {
                    position = new Vector2((float)values[1], (float)values[2] - 0.5f);
                }
                else
                {
                    position = new Vector2((float)values[1], (float)values[2]);
                }
                //Standing trigger
                GameObject standingTriggerInstance = Instantiate(standingTrigger, new Vector3(position.x, -0.15f, position.y), Quaternion.identity);

                string tag = sValues[4];
                StandingTrigger triggerController = standingTriggerInstance.GetComponent<StandingTrigger>();
                triggerController.SetBridgeConnectionTag(tag);

                if (sValues[3] == "forward")
                {
                    triggerController.SetInitialDirection(Direction.FORWARD);
                }
                else if (sValues[3] == "back")
                {
                    triggerController.SetInitialDirection(Direction.BACK);
                }
                else if (sValues[3] == "right")
                {
                    triggerController.SetInitialDirection(Direction.RIGHT);
                }
                else if (sValues[3] == "left")
                {
                    triggerController.SetInitialDirection(Direction.LEFT);
                }

                buttonInstances.Add(standingTriggerInstance);
                allMapObjects.Add(standingTriggerInstance);
            }



            j++;
        }

        SpawnBlock();

    }

    private void GetBridgeStartPositionsAndDirections()
    {
        for (int i = 0; i < bridgeInstances.Count; i++)
        {
            bridgeStartPositions[i] = bridgeInstances[i].transform.position;
            //bridgeStartDirections[i] = bridgeInstances[i].GetComponent<BridgeController>().GetDirection();
        }
    }

    private void GenerateMapWithoutPlayer(int levelIndex)
    {
        levelCreationInfo = textAsset.text.Split('\n');

        //Find the starting index for the level
        int startingLineIndex = 0;
        for (int i = 0; i < levelCreationInfo.Length; i++)
        {
            string line = levelCreationInfo[i];
            if (line[0] == 'L')
            {
                string sLevel = line.Split(':')[1];
                int level = Convert.ToInt32(sLevel);
                if (level == levelIndex)
                {
                    startingLineIndex = i;
                    break;
                }
            }
        }

        //Get the spawn position for the block
        string sSpawnValues01 = levelCreationInfo[startingLineIndex + 1].Split(':')[1];
        string[] sSpawnValues02 = sSpawnValues01.Split(',');
        float xSpawnPosition = (float)Convert.ToDouble(sSpawnValues02[0]);
        float ySpawnPosition = (float)Convert.ToDouble(sSpawnValues02[1]);
        spawnPosition = new Vector2(xSpawnPosition, ySpawnPosition);

        //Loop through the .txt file from the starting position and create 
        int j = startingLineIndex + 2;

        while (levelCreationInfo[j][0] != 'L')
        {
            string line = levelCreationInfo[j];
            string[] sValues = line.Split(',');
            float[] values = new float[sValues.Length];
            for (int i = 0; i < 3; i++)
            {
                double dValue = Convert.ToDouble(sValues[i]);
                values[i] = (float)dValue;
            }

            //Build the basic platform
            if (values[0] == 0)
            {
                Vector2 position = new Vector2((float)values[1], (float)values[2]);
                GameObject basicPlatformInstance = Instantiate(basicPlatform, new Vector3(position.x, -0.05f, position.y), Quaternion.identity);
                allMapObjects.Add(basicPlatformInstance);
            }
            else if (values[0] == 1)
            {
                Vector2 position = new Vector2((float)values[1], (float)values[2]);
                GameObject goalInstance = Instantiate(goalPlatform, new Vector3(position.x, -0.05f, position.y), Quaternion.identity);
                allMapObjects.Add(goalInstance);

            }
            else if (values[0] == 2)
            {
                print("Create bridge here");
                //Bridge
                Vector2 position = new Vector2((float)values[1], (float)values[2]);
                GameObject bridgeInstance = Instantiate(bridge, new Vector3(position.x, -0.06f, position.y), Quaternion.identity);

                float yRotation = (sValues[3] == "straight") ? 90 : 0;
                bridgeInstance.transform.eulerAngles = new Vector3(0f, yRotation, 0f);

                string tag = sValues[4];
                bridgeInstance.gameObject.tag = tag;

                bridgeInstances.Add(bridgeInstance);
                allMapObjects.Add(bridgeInstance);

            }
            else if (values[0] == 3)
            {
                //Disappearing Block
                Vector2 position = new Vector2((float)values[1], (float)values[2]);
                GameObject disappearingBlockInstance = Instantiate(disappearingBlock, new Vector3(position.x, -0.05f, position.y), Quaternion.identity);
                disappearingInstances.Add(disappearingBlockInstance);
                allMapObjects.Add(disappearingBlockInstance);

            }
            else if (values[0] == 4)
            {
                //Standing trigger
                Vector2 position = new Vector2((float)values[1], (float)values[2]);
                GameObject standingTriggerInstance = Instantiate(standingTrigger, new Vector3(position.x, -0.05f, position.y), Quaternion.identity);

                string tag = sValues[4];
                StandingTrigger triggerController = standingTriggerInstance.GetComponent<StandingTrigger>();
                triggerController.SetBridgeConnectionTag(tag);

                if (sValues[3] == "forward")
                {
                    triggerController.SetInitialDirection(Direction.FORWARD);
                }
                else if (sValues[3] == "back")
                {
                    triggerController.SetInitialDirection(Direction.BACK);
                }
                else if (sValues[3] == "right")
                {
                    triggerController.SetInitialDirection(Direction.RIGHT);
                }
                else if (sValues[3] == "left")
                {
                    triggerController.SetInitialDirection(Direction.LEFT);
                }

                buttonInstances.Add(standingTriggerInstance);
                allMapObjects.Add(standingTriggerInstance);
            }



            j++;
        }



    }

    public void ResetBridges()
    {
        for (int i = 0; i < bridgeInstances.Count; i++)
        {
            if (bridgeInstances[i].transform.position != bridgeStartPositions[i])
            {
                buttonInstances[i].GetComponent<StandingTrigger>().ExternalTriggerActivation(true);

            }
        }
    }

    public void ResetDisappearingBlocks()
    {
        foreach (GameObject block in disappearingInstances)
        {
            Transform current = block.transform;
            if (current.localScale.x != 0.5)
            {
                block.GetComponent<DisappearingBlockController>().MakeBlockAppear();
            }
        }
    }





    public Vector2 GetSpawnPosition()
    {
        return spawnPosition;
    }

    public GameObject SpawnBlock()
    {
        GameObject prefab = Instantiate(player, new Vector3(spawnPosition.x, 0.5f, spawnPosition.y), player.transform.rotation);


        int colorIndex = SaveManager.miscInfo.shopColorChoiceIndex;
        if (colorIndex < 9)
        {

            string resourcePath = "PlayerMaterials/PlayerColor0" + (colorIndex + 1);
            prefab.GetComponent<Renderer>().material = Resources.Load<Material>(resourcePath);
        }
        else
        {
            string resourcePath = "PlayerMaterials/PlayerColor" + (colorIndex + 1);

            prefab.GetComponent<Renderer>().material = Resources.Load<Material>(resourcePath);
        }


        return prefab;

        //return Instantiate(player, new Vector3(spawnPosition.x, 0.5f, spawnPosition.y), player.transform.rotation);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }


}

