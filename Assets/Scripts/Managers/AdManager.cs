using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager instance = null;


    [SerializeField] string gameId = "1515194";

    bool inGameAdHasBeenShown = false;
    bool adsDisabled;

    int gamesPlayed = 1;
    int retryCount;

    float nextAdTime = 0;
    float adWaitTime = 35;

    //AdMob
    InterstitialAd interstitial;
    InterstitialAd imageInterstitial;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        //InitiateAd();
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        adsDisabled = SaveManager.miscInfo.adsDisabled;
    }

    public void LoadUnityAd(string zone = "")
    {
        if (Time.time > nextAdTime)
        {
            if (!adsDisabled)
            {
                print("Ad deployed");
                if (string.Equals(zone, ""))
                    zone = null;

                //If the two values equal each other, then show a non skipable ad
                int rand = Random.Range(1, 6);
                int rand2 = Random.Range(1, 6);
                if (rand == rand2)
                {
                    zone = "rewardedVideo";
                    print("---SHOW REWARD AD---");
                }


                if (Advertisement.IsReady(zone))
                {
                    Advertisement.Show(zone);
                    print("---ADVERTISEMENT READY---");
                }

                nextAdTime = Time.time + adWaitTime;
            }
        }
        else
        {
            print("---NOT ENOUGH TIME---");
        }
    }

    public void DetermineIfAdShouldShowInGame()
    {
        retryCount++;

        if (retryCount >= 2)
        {
            //If the two random values equal each other, show an ad
            int rand = Random.Range(1, 4);
            int rand2 = UnityEngine.Random.Range(1, 4);
            if (rand == rand2 && !inGameAdHasBeenShown)
            {
                LoadUnityAd();
                inGameAdHasBeenShown = true;
            }
        }
    }

    public void InitiateUnityAd()
    {
        print(gamesPlayed);
        if (!adsDisabled)
        {

            print("Initiate ad called");
            //This is called when the game is won and when the user leaves the game
            retryCount = 0;
            gamesPlayed++;
            if (gamesPlayed % 2 == 0)
                inGameAdHasBeenShown = true;
            else
                inGameAdHasBeenShown = false;


            Advertisement.Initialize(gameId, true);
            
        }
    }

    public void InitiateAdForMainWindow()
    {
        print("Initiate ad for menu called");

        Advertisement.Initialize(gameId, true);
    }

    public void DisableAds()
    {
        SaveManager.miscInfo.adsDisabled = true;
        adsDisabled = true;
        SaveManager.SaveMiscInfo();
    }


    //ADMOB

    public void RequestInterstitial()
    {
        if (!adsDisabled)
        {


            #if UNITY_ANDROID
            string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
            #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-5594107864016260/8944099579";
            #else
            string adUnitId = "unexpected_platform";
            #endif

            // Initialize an InterstitialAd.
            interstitial = new InterstitialAd(adUnitId);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);
        }
    
    }

    public void RequestImageInterstitial()
    {
        if (!adsDisabled)
        {
            #if UNITY_ANDROID
            string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
            #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-5594107864016260/5383395996";
            #else
            string adUnitId = "unexpected_platform";
            #endif

            // Initialize an InterstitialAd.
            imageInterstitial = new InterstitialAd(adUnitId);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            imageInterstitial.LoadAd(request);
        }
    }

    public void ShowInterstitial()
    {
        if (Time.time > nextAdTime)
        {
            if (!adsDisabled && interstitial.IsLoaded())
            {
                interstitial.Show();
                print("-------REGULAR INTERSTITIAL SHOWN------");
            }
            nextAdTime = Time.time + adWaitTime;
        }
        else
        {
            print("---NOT ENOUGH TIME PASSED");
        }
    }

    public void ShowImageInterstitial()
    {
        if (!adsDisabled && imageInterstitial.IsLoaded())
        {
            imageInterstitial.Show();
            print("------IMAGE INTERSTITIAL SHOWN--------");
        }
    }

    public void DestroyInterstitial()
    { 
        if (interstitial != null)
            interstitial.Destroy();
    }


}
