using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartWindow : MonoBehaviour
{

    //public Button playButton;

    public Image alertImage;
    public Button soundButton;
    public Image adMenu;

    [SerializeField] string appID;

    //For ads
    bool userIsActive;
    static int initiateCount, deployCount, startMenuLoadTimes;
    int startTime;

    int adMenuCount = 0;
    bool openMenu, closeMenu;

    float speed = 4.5f;


    protected void Awake()
    {
        AudioManager.instance.SetMainMenuMusic();
        if (SaveManager.miscInfo.newColorUnlocked)
        {
            alertImage.enabled = true;
        }
        else
        {
            alertImage.enabled = false;
        }



    }

    private void Start()
    {
        //Load interstitial ad if first time opening game
        //if (startMenuLoadTimes < 1)
        //{
        //    AdManager.instance.RequestInterstitial();
        //}


        appID = "1281303037";

        startMenuLoadTimes++;

        startTime = (int)Time.time;
        SetAudioButtonImage();


        //AdManager.instance.RequestInterstitial();


        //int rand = Random.Range(1, 3);
        //int rand2 = Random.Range(1, 3);
        //if (rand == rand2 && startMenuLoadTimes == 1)
        //{
        //    AdManager.instance.ShowImageInterstitial();
        //}

    }



    private void Update()
    {
        if ((int)Time.time == startTime + 10 && initiateCount == 0 && !userIsActive)
        {
            AdManager.instance.InitiateAdForMainWindow();
            //AdManager.instance.RequestInterstitial();
            initiateCount++;
        }

        if ((int)Time.time == startTime + 15 && deployCount == 0 && !userIsActive)
        {
            AdManager.instance.LoadUnityAd();
            //AdManager.instance.ShowInterstitial();
            deployCount++;
        }

        OpenOrCloseAdMenu();
    }

    private void SetAudioButtonImage()
    {
       
        if (SaveManager.miscInfo.soundDisabled)
        {
            soundButton.image.sprite = Resources.Load<Sprite>("AudioButtons/SoundDisabledUnpressed");
            Sprite pressedSprite = Resources.Load<Sprite>("AudioButtons/SoundDisabledPressed");
            SpriteState st = new SpriteState();
            st.pressedSprite = pressedSprite;
            soundButton.spriteState = st;
        }
        else
        {
            soundButton.image.sprite = Resources.Load<Sprite>("AudioButtons/SoundEnabledUnpressed");
            Sprite pressedSprite = Resources.Load<Sprite>("AudioButtons/SoundEnabledPressed");
            SpriteState st = new SpriteState();
            st.pressedSprite = pressedSprite;
            soundButton.spriteState = st;
        }
    }

    private void OpenOrCloseAdMenu()
    {
        if (openMenu)
        {

            if (adMenu.transform.localScale.x < 0.95)
            {
                adMenu.transform.localScale += Vector3.right * (speed * Time.deltaTime);
            }
            else
            {
                adMenu.transform.localScale = new Vector3(1, 1, 1);
                openMenu = false;
            }

        }

        if (closeMenu)
        {
            if (adMenu.transform.localScale.x > 0.05)
            {
                adMenu.transform.localScale -= Vector3.right * (speed * Time.deltaTime);
            }
            else
            {
                adMenu.transform.localScale = new Vector3(0, 1, 1);
                closeMenu = false;
            }
        }

    }

    public void OnPlay()
    {
        SceneManager.LoadScene("LevelSelect");

    }

    public void OnRate()
    {
        userIsActive = true;
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appID);

    }


    public void OnShop()
    {
        userIsActive = true;
        SceneManager.LoadScene("ShopMenu");
    }

    public void OnDisableAds()
    {
        userIsActive = true;

        print("CALLED");

        if (!openMenu && !closeMenu)
        {
            adMenuCount++;
            if (adMenuCount % 2 == 0)
            {
                closeMenu = true;
            }
            else
            {
                openMenu = true;
            }
        }
    }

    public void OnTwitter()
    {
        userIsActive = true;
        Application.OpenURL("http://twitter.com/brenbitgames");
        
    }

    public void OnSound()
    {
        startTime = (int)Time.time;
        SaveManager.miscInfo.soundDisabled = !SaveManager.miscInfo.soundDisabled;
        SaveManager.SaveMiscInfo();
        SetAudioButtonImage();
        AudioManager.instance.SetMusicVolumeSettings();
        
    }





    
    




}
