using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    float loadTime;
    float minimumLogoTime = 2f;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Application.targetFrameRate = 60;

        if (Time.time < minimumLogoTime)
        {
            loadTime = minimumLogoTime;
        }
        else
        {
            loadTime = Time.time;
        }

        //AdManager.instance.RequestImageInterstitial();
    }

    private void Update()
    {
        if (Time.time > minimumLogoTime && loadTime != 0)
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}


