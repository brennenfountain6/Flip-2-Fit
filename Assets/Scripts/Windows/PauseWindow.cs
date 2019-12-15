using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour
{
    public Image dp;

    int windowIndex = 0;

    bool moveButtonsDown = false;


    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPause()
    {
        moveButtonsDown = !moveButtonsDown;

        dp.gameObject.SetActive(moveButtonsDown);

        animator.SetBool("ShiftDown", moveButtonsDown);
    }

    public void OnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void OnLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");

    }

    public void SetAnimationBoolean(bool val)
    {
        moveButtonsDown = val;
        animator.SetBool("ShiftDown", moveButtonsDown);
    }


}