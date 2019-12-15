using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    PlayerController controller;
    DeathCheck deathCheck;

    [HideInInspector]
    public event Action increaseMoveEvent;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    //Timing in between swipes
    float waitTime = .17f;
    float timeTilNextSwipe = 0;


    bool justSwiped;

    enum SwipeDirection
    {
        RIGHT, LEFT, UP, DOWN
    }

    SwipeDirection swipeDirection;


    private void Start()
    {
        dragDistance = Screen.height * 2 / 100;
        swipeDirection = SwipeDirection.UP;
        justSwiped = false;
        controller = GetComponent<PlayerController>();
        deathCheck = GetComponent<DeathCheck>();
    }

    private void Update()
    {



        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");

                            if (Time.time > timeTilNextSwipe)
                            {
                                swipeDirection = SwipeDirection.RIGHT;
                                justSwiped = true;
                            }

                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            if (Time.time > timeTilNextSwipe)
                            {
                                swipeDirection = SwipeDirection.LEFT;
                                justSwiped = true;
                            }

                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");

                            if (Time.time > timeTilNextSwipe)
                            {
                                swipeDirection = SwipeDirection.UP;
                                justSwiped = true;
                            }
                            

                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");

                            if (Time.time > timeTilNextSwipe)
                            {
                                swipeDirection = SwipeDirection.DOWN;
                                justSwiped = true;
                            }

                        }
                    }
                }
                else
                {   
                    Debug.Log("Tap");
                }
            }
        }



        if (!deathCheck.isDead)
        {
            if (controller.canMoveAgain)
            {
                if (Input.GetKeyDown(KeyCode.W) || (swipeDirection == SwipeDirection.UP && justSwiped))
                {
                    controller.SetDirection(KeyCode.W);
                    justSwiped = false;
                    if (increaseMoveEvent != null)
                    {
                        increaseMoveEvent();
                    }
                    AudioManager.instance.PlayPlayerMovementSound();
                    timeTilNextSwipe = Time.time + waitTime;

                }
                else if (Input.GetKeyDown(KeyCode.D) || (swipeDirection == SwipeDirection.RIGHT && justSwiped))
                {
                    controller.SetDirection(KeyCode.D);
                    justSwiped = false;

                    if (increaseMoveEvent != null)
                    {
                        increaseMoveEvent();
                    }
                    AudioManager.instance.PlayPlayerMovementSound();
                    timeTilNextSwipe = Time.time + waitTime;

                }
                else if (Input.GetKeyDown(KeyCode.S) || (swipeDirection == SwipeDirection.DOWN && justSwiped))
                {
                    controller.SetDirection(KeyCode.S);
                    justSwiped = false;

                    if (increaseMoveEvent != null)
                    {
                        increaseMoveEvent();
                    }
                    AudioManager.instance.PlayPlayerMovementSound();
                    timeTilNextSwipe = Time.time + waitTime;

                }
                else if (Input.GetKeyDown(KeyCode.A) || (swipeDirection == SwipeDirection.LEFT && justSwiped))
                {
                    controller.SetDirection(KeyCode.A);
                    justSwiped = false;

                    if (increaseMoveEvent != null)
                    {
                        increaseMoveEvent();
                    }
                    AudioManager.instance.PlayPlayerMovementSound();
                    timeTilNextSwipe = Time.time + waitTime;

                }


            }
        }



    }

}
