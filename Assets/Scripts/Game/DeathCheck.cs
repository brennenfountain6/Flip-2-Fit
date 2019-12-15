using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DeathCheck : MonoBehaviour
{

    public float deathRotateSpeed, fallSpeed, horizontalFallSpeed, horizontalDimRate;
    public LayerMask groundLayer;
    public Transform[] rayOriginLocations;

    [HideInInspector]
    public event Action levelWonEvent;
    [HideInInspector]
    public event Action levelLostEvent;

    PlayerController controller;
    RotateAssist rotateAssist;

    [HideInInspector]
    public bool isDead;
    bool needsToWait;
    int deathCheckCount = 0;
    int fallCheckCount = 0;

    public float shrinkRate;
    bool shouldAnimatePlayer;

    bool shouldDecreaseMusicVolume;

    float nextHorizontalDimTime;

    //Fix error with disappearing block
    bool leftOrBackHit;
    bool rightOrFrontHit;

    Vector3 deadRotation;

    Ray rayTop, rayBottom, rayLeft1, rayLeft2, rayRight1, rayRight2, rayFront1, rayFront2, rayBack1, rayBack2;
    RaycastHit hitInfo;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        isDead = false;
        needsToWait = false;
        shouldAnimatePlayer = false;
    }

    private int NumberOfRaysReturningTrue(Ray ray1, Ray ray2, Ray ray3, Ray ray4, Ray ray5, Ray ray6, Ray ray7, Ray ray8)
    {
        int count = 0;

        if (Physics.Raycast(ray1, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray2, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray3, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray4, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray5, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray6, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray7, out hitInfo, 1, groundLayer))
        {
            count++;
        }
        if (Physics.Raycast(ray8, out hitInfo, 1, groundLayer))
        {
            count++;
        }


        return count;
    }

    private void Update()
    {

        DrawRays();

        if (shouldDecreaseMusicVolume)
        {
            AudioManager.instance.DecreaseMusicVolumeForWin();
        }

        //Create rays and check if cube is off
        if (controller.canMoveAgain && controller.checkForDeathCount < 1)
        {
            controller.checkForDeathCount++;

            rayTop = new Ray(rayOriginLocations[0].position, transform.up);
            rayBottom = new Ray(rayOriginLocations[1].position, -transform.up);
            rayLeft1 = new Ray(rayOriginLocations[0].position, -transform.right);
            rayLeft2 = new Ray(rayOriginLocations[1].position, -transform.right);
            rayRight1 = new Ray(rayOriginLocations[0].position, transform.right);
            rayRight2 = new Ray(rayOriginLocations[1].position, transform.right);
            rayFront1 = new Ray(rayOriginLocations[0].position, transform.forward);
            rayFront2 = new Ray(rayOriginLocations[1].position, transform.forward);
            rayBack1 = new Ray(rayOriginLocations[0].position, -transform.forward);
            rayBack2 = new Ray(rayOriginLocations[1].position, -transform.forward);


            if (RotateAssist.IsStanding(transform.eulerAngles))
            {
                //Check vertical
                if (!Physics.Raycast(rayBottom, out hitInfo, 1, groundLayer) && !Physics.Raycast(rayTop, out hitInfo, 1, groundLayer))
                {
                    isDead = true;
                }

                //Check for game over
                if (Physics.Raycast(rayBottom, out hitInfo, 1, groundLayer) || Physics.Raycast(rayTop, out hitInfo, 1, groundLayer))
                {
                    if (hitInfo.collider.gameObject.tag == "Goal")
                    {
                        if (levelWonEvent != null)
                        {
                            levelWonEvent();
                        }
                        StartCoroutine(WaitToFallThroughGoal());
                    }

                }

            }
            else
            {
                //If every side is off
                int numberOfRays = NumberOfRaysReturningTrue(rayLeft1, rayLeft2, rayRight1, rayRight2, rayFront1, rayFront2, rayBack1, rayBack2);
                if (numberOfRays < 2)
                {
                    isDead = true;

                    if (numberOfRays == 1)
                    {
                        needsToWait = true;
                    }
                }
            }


        }

        //If cube is off according to the rays
        if (isDead)
        {
            if (Time.time > nextHorizontalDimTime)
            {
                horizontalFallSpeed *= 0.70f;
                nextHorizontalDimTime += horizontalDimRate;
            }
            //The first time the cube is reported dead: call Game Over functions and switch orientation of the rays
            if (deathCheckCount < 1)
            {
                deathCheckCount++;
                if (levelLostEvent != null)
                {
                    levelLostEvent();
                }
                SwitchOrientation();
                deadRotation = transform.eulerAngles;
            }

            //Determine the direction the cube was heading 
            switch (controller.GetDirection())
            {
                case Direction.FORWARD:
                    //If the cube hits one block and should switch rotation to left or right
                    if (needsToWait && RotateAssist.IsLayingHorizontally(deadRotation))
                    {
                        if (LeftOrBackRayHit() || leftOrBackHit)
                        {
                            leftOrBackHit = true;
                            transform.Translate(Vector3.right * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.back, Time.deltaTime * deathRotateSpeed);
                        }
                        else if (RightOrFrontRayHit() || rightOrFrontHit)
                        {
                            rightOrFrontHit = true;
                            transform.Translate(Vector3.left * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * deathRotateSpeed);
                        }
                    }
                    //Cube fell off and should rotate forward
                    else
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime * horizontalFallSpeed, Space.World);
                        transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * deathRotateSpeed);
                    }
                    break;
                case Direction.BACK:
                    //If the cube hits one block and should switch rotation to left or right
                    if (needsToWait && RotateAssist.IsLayingHorizontally(deadRotation))
                    {
                        if (LeftOrBackRayHit() || leftOrBackHit)
                        {
                            leftOrBackHit = true;
                            transform.Translate(Vector3.right * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.back, Time.deltaTime * deathRotateSpeed);
                        }
                        else if (RightOrFrontRayHit() || rightOrFrontHit)
                        {
                            rightOrFrontHit = true;
                            transform.Translate(Vector3.left * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * deathRotateSpeed);
                        }
                    }
                    //Cube fell off and should rotate back
                    else
                    {
                        transform.Translate(Vector3.back * Time.deltaTime * horizontalFallSpeed, Space.World);
                        transform.RotateAround(transform.position, Vector3.left, Time.deltaTime * deathRotateSpeed);
                    }
                    break;
                case Direction.RIGHT:
                    //If the cube hits one block and should switch rotation to back or forward
                    if (needsToWait && RotateAssist.IsLayingVertically(deadRotation))
                    {
                        if (LeftOrBackRayHit() || leftOrBackHit)
                        {
                            leftOrBackHit = true;
                            transform.Translate(Vector3.forward * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * deathRotateSpeed);
                        }
                        else if (RightOrFrontRayHit() || rightOrFrontHit)
                        {
                            rightOrFrontHit = true;
                            transform.Translate(Vector3.back * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.left, Time.deltaTime * deathRotateSpeed);
                        }
                    }
                    //Cube fell off and should rotate right
                    else
                    {
                        transform.Translate(Vector3.right * Time.deltaTime * horizontalFallSpeed, Space.World);
                        transform.RotateAround(transform.position, Vector3.back, Time.deltaTime * deathRotateSpeed);
                    }
                    break;
                case Direction.LEFT:
                    //If the cube hits one block and should switch rotation to back or forward
                    if (needsToWait && RotateAssist.IsLayingVertically(deadRotation))
                    {
                        if (LeftOrBackRayHit() || leftOrBackHit)
                        {
                            leftOrBackHit = true;
                            transform.Translate(Vector3.forward * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * deathRotateSpeed);
                        }
                        else if (RightOrFrontRayHit() || rightOrFrontHit)
                        {
                            rightOrFrontHit = true;
                            transform.Translate(Vector3.back * Time.deltaTime * horizontalFallSpeed, Space.World);
                            transform.RotateAround(transform.position, Vector3.left, Time.deltaTime * deathRotateSpeed);
                        }
                    }
                    //Cube fell off and should rotate right
                    else
                    {
                        transform.Translate(Vector3.left * Time.deltaTime * horizontalFallSpeed, Space.World);
                        transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * deathRotateSpeed);
                    }
                    break;
            }


            //Checks to see if block needs to wait to fall in order to fix bug
            if (!needsToWait)
            {
                transform.Translate(Vector3.down * Time.deltaTime * fallSpeed, Space.World);
            }
            else if (needsToWait && fallCheckCount == 0)
            {
                StartCoroutine(WaitToFall());
            }
            else if (fallCheckCount == 1)
            {
                transform.Translate(Vector3.down * Time.deltaTime * fallSpeed, Space.World);
            }

        }

        if (shouldAnimatePlayer)
        {
            if (transform.localScale.y > 0 && transform.position.y > -0.01)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - (shrinkRate * Time.deltaTime), transform.localScale.z);
                transform.position = new Vector3(transform.position.x, transform.position.y - (shrinkRate / 2 * Time.deltaTime), transform.position.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                transform.position = new Vector3(transform.position.x, -0.01f, transform.position.z);
                shouldAnimatePlayer = false;
                AudioManager.instance.PlayGameWonSound();
            }
        }

    }

    IEnumerator WaitToFallThroughGoal()
    {
        shouldDecreaseMusicVolume = true;
        yield return new WaitForSeconds(0.15f);


        shouldAnimatePlayer = true;

    }

    IEnumerator WaitToFall()
    {
        yield return new WaitForSeconds(0.05f);
        transform.Translate(Vector3.down * Time.deltaTime * fallSpeed, Space.World);
        fallCheckCount = 1;

    }


    private void DrawRays()
    {
        //Top
        Debug.DrawLine(rayTop.origin, rayTop.direction * 100, Color.green);

        //Bottom
        Debug.DrawLine(rayBottom.origin, rayBottom.direction * 100, Color.green);

        //Left1
        Debug.DrawLine(rayLeft1.origin, rayLeft1.direction * 100, Color.blue);

        //Left2
        Debug.DrawLine(rayLeft2.origin, rayLeft2.direction * 100, Color.red);

        //Right1
        Debug.DrawLine(rayRight1.origin, rayRight1.direction * 100, Color.blue);

        //Right2
        Debug.DrawLine(rayRight2.origin, rayRight2.direction * 100, Color.red);

        //Front 1
        Debug.DrawLine(rayFront1.origin, rayFront1.direction * 100, Color.blue);

        //Front 2
        Debug.DrawLine(rayFront2.origin, rayFront2.direction * 100, Color.red);

        //Back
        Debug.DrawLine(rayBack1.origin, rayBack1.direction * 100, Color.blue);

        //Back 2
        Debug.DrawLine(rayBack2.origin, rayBack2.direction * 100, Color.red);
    }

    private void SwitchOrientation()
    {
        //1 is always on the left and closest to the camera

        Vector3 tempPosition = Vector3.zero;
        if (rayLeft1.origin.x > rayLeft2.origin.x)
        {
            tempPosition = rayLeft1.origin;

            //Left side
            rayLeft1.origin = rayLeft2.origin;
            rayLeft2.origin = tempPosition;

            //Right side
            rayRight1.origin = rayRight2.origin;
            rayRight2.origin = tempPosition;

            //Front side
            rayFront1.origin = rayFront2.origin;
            rayFront2.origin = tempPosition;

            //Back side
            rayBack1.origin = rayBack2.origin;
            rayBack2.origin = tempPosition;
        }

        if (rayLeft1.origin.z - rayLeft2.origin.z > .3f)
        {
            tempPosition = rayLeft1.origin;

            //Left side
            rayLeft1.origin = rayLeft2.origin;
            rayLeft2.origin = tempPosition;

            //Right side
            rayRight1.origin = rayRight2.origin;
            rayRight2.origin = tempPosition;

            //Front side
            rayFront1.origin = rayFront2.origin;
            rayFront2.origin = tempPosition;

            //Back side
            rayBack1.origin = rayBack2.origin;
            rayBack2.origin = tempPosition;

        }

    }

    private bool LeftOrBackRayHit()
    {
        if (Physics.Raycast(rayLeft1, out hitInfo, 1, groundLayer))
        {
            return true;
        }
        if (Physics.Raycast(rayRight1, out hitInfo, 1, groundLayer))
        {
            return true;
        }
        if (Physics.Raycast(rayFront1, out hitInfo, 1, groundLayer))
        {
            return true;
        }
        if (Physics.Raycast(rayBack1, out hitInfo, 1, groundLayer))
        {
            return true;
        }


        return false;
    }

    private bool RightOrFrontRayHit()
    {
        if (Physics.Raycast(rayLeft2, out hitInfo, 1, groundLayer))
        {
            return true;
        }
        if (Physics.Raycast(rayRight2, out hitInfo, 1, groundLayer))
        {
            return true;
        }
        if (Physics.Raycast(rayFront2, out hitInfo, 1, groundLayer))
        {
            return true;
        }
        if (Physics.Raycast(rayBack2, out hitInfo, 1, groundLayer))
        {
            return true;
        }

        return false;
    }
}