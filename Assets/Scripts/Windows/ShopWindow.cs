using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ShopWindow : MonoBehaviour {

    public Text greenCount;
    public Text nextUnlockCount;
    public Text nextUnlockText;
    public Image nextUnlockImage;

    public Image[] colorImages;
    private bool[] buttonsUnlockStatus;

    private int count;
    private int unlockCount;

    int previousSelected;

    private void Awake()
    {
        if (SaveManager.miscInfo.newColorUnlocked)
        {
            SaveManager.miscInfo.newColorUnlocked = false;
            SaveManager.SaveMiscInfo();
        }

        //foreach (Level level in SaveManager.levelList)
        //{
        //    if (level.levelWinType == LevelWinType.BEST)
        //    {
        //        count++;
        //    }
        //}

        count = SaveManager.miscInfo.worldAPoints;

        int index = count;

        if (count < 120)
        {
            if (count % 12 != 0)
            {
                while (index % 12 != 0)
                {
                    index++;
                }
                unlockCount = index;
            }
            else
            {
                unlockCount = index + 12;
            }
        }
        else
        {
            if (count % 24 != 0)
            {
                while (index % 24 != 0)
                {
                    index++;
                }
                unlockCount = index;
            }
            else
            {
                unlockCount = 144;
            }
        }

        buttonsUnlockStatus = new bool[colorImages.Length];



        nextUnlockCount.text = "" + unlockCount;
        greenCount.text = "" + count;

        //Set unselected buttons
        for (int i = 0; i < colorImages.Length - 1; i++)
        {
            if (i * 12 <= count)
            {
                buttonsUnlockStatus[i] = true;
                colorImages[i].GetComponentInChildren<Button>().image.sprite = Resources.Load<Sprite>("ShopButtons/ShopUnselected");
            }
        }

        //Check if the black color is unlocked
        if (count == 144)
        {
            nextUnlockText.text = "All Colors Unlocked!";
            Destroy(nextUnlockCount);
            Destroy(nextUnlockImage);
            nextUnlockText.rectTransform.transform.position = new Vector3(transform.position.x, nextUnlockText.rectTransform.position.y - 36, transform.position.z);
            buttonsUnlockStatus[11] = true;
            colorImages[11].GetComponentInChildren<Button>().image.sprite = Resources.Load<Sprite>("ShopButtons/ShopUnselected");
        }

        //Set selected button
        colorImages[SaveManager.miscInfo.shopColorChoiceIndex].GetComponentInChildren<Button>().image.sprite = Resources.Load<Sprite>("ShopButtons/ShopSelected");
        previousSelected = SaveManager.miscInfo.shopColorChoiceIndex;
    }


    public void OnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnColorButton()
    {
        AudioManager.instance.PlayButtonClick();
        var go = EventSystem.current.currentSelectedGameObject;
        Button btn = go.GetComponent<Button>();

        int btnIndex = Convert.ToInt32(btn.name);

        if (buttonsUnlockStatus[btnIndex] && btnIndex != previousSelected)
        {
            colorImages[previousSelected].GetComponentInChildren<Button>().image.sprite = Resources.Load<Sprite>("ShopButtons/ShopUnselected");
            btn.image.sprite = Resources.Load<Sprite>("ShopButtons/ShopSelected");
            previousSelected = btnIndex;

            SaveManager.miscInfo.shopColorChoiceIndex = btnIndex;
        }

        SaveManager.SaveMiscInfo();



    }


}
