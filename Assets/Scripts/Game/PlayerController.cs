using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Direction
{
    FORWARD, RIGHT, LEFT, BACK
}

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float rotateBuffer;

    //Timing for next move
    [HideInInspector]
    public float moveDuration;

    float startTime;

    [HideInInspector]
    public bool canMoveAgain = true;

    //Boolean to prepare movement
    bool readyToMove = false;

    //Scaling variables
    float xScale, yScale, zScale;

    Vector3 startPosition, startRotation;
    Vector3 axis;
    Direction direction;

    //Death variables
    [HideInInspector]
    public int checkForDeathCount = 0;




    // Use this for initialization
    void Start ()
    {
        yScale = transform.localScale.y;
        xScale = transform.localScale.x;
        zScale = transform.localScale.z;

        startPosition = transform.position;
        startRotation = transform.localEulerAngles;

        direction = Direction.FORWARD;
        speed *= 100;

        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (readyToMove)
        {
            if (direction == Direction.FORWARD)
            {
                MoveForward();
            }
            else if (direction == Direction.BACK)
            {
                MoveBack();
            }
            else if (direction == Direction.LEFT)
            {
                MoveLeft();
            }
            else if (direction == Direction.RIGHT)

            {
                MoveRight();
            }
        }
    }


    public void SetDirection(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                direction = Direction.FORWARD;
                break;
            case KeyCode.D:
                direction = Direction.RIGHT;
                break;
            case KeyCode.S:
                direction = Direction.BACK;
                break;
            case KeyCode.A:
                direction = Direction.LEFT;
                break;
        }

       

        startRotation = transform.eulerAngles;

        readyToMove = true;
        canMoveAgain = false;
        checkForDeathCount = 0;
    }


    private void MoveForward()
    {
        //If the block is standing up straight
        if (RotateAssist.IsStanding(startRotation))
        {
            axis = new Vector3(startPosition.x, startPosition.y - (yScale / 2), startPosition.z + (zScale / 2));

            if (transform.position.y > (xScale / 2) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.right, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (xScale / 2), startPosition.z + (yScale * (3f / 4f)));

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying down vertical
        else if (RotateAssist.IsLayingVertically(startRotation))
        {
            axis = new Vector3(startPosition.x, startPosition.y - (yScale / 4), startPosition.z + zScale);

            if (transform.position.z < (startPosition.z + yScale * (3f / 4f)) - rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.right, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (yScale / 2), startPosition.z + (yScale * (3f / 4f)));

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
                
            }
        }
        //If the block is laying horizontally
        else if (RotateAssist.IsLayingHorizontally(startRotation))
        {
            axis = new Vector3(startPosition.x, startPosition.y - (yScale / 4), startPosition.z + (zScale / 2));

            if (transform.position.z < (startPosition.z + zScale) - rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.right, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (yScale / 4), startPosition.z + zScale);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(axis, 0.1f);
    }

    private void MoveRight()
    {
        //If the block is standing up straight
        if (RotateAssist.IsStanding(startRotation))
        {

            axis = new Vector3(startPosition.x + (xScale / 2), startPosition.y - (yScale / 2), startPosition.z);

            if (transform.position.y > (zScale / 2) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.back, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(startPosition.x + (yScale * (3f / 4f)), (xScale / 2), transform.position.z);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying down horizontal
        else if (RotateAssist.IsLayingHorizontally(startRotation))               
        {
            axis = new Vector3(startPosition.x + xScale, startPosition.y - (yScale / 4), startPosition.z);

            if (transform.position.x < (startPosition.x + yScale * (3f / 4f)) - rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.back, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(startPosition.x + (yScale * (3f / 4f)), (yScale / 2), transform.position.z);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying vertically
        else if (RotateAssist.IsLayingVertically(startRotation))
        {
            axis = new Vector3(startPosition.x + (xScale / 2), startPosition.y - (yScale / 4), startPosition.z);

            if (transform.position.x < startPosition.x + xScale - rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.back, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(startPosition.x + xScale, (yScale / 4), transform.position.z);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }

    }

    private void MoveBack()
    {
        //If the block is standing up straight
        if (RotateAssist.IsStanding(startRotation))
        {
            axis = new Vector3(startPosition.x, startPosition.y - (yScale / 2), startPosition.z - (zScale / 2));

            if (transform.position.y > (xScale / 2) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.left, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (xScale / 2), startPosition.z - (yScale * (3f / 4f)));

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying down vertical
        else if (RotateAssist.IsLayingVertically(startRotation))
        {
            axis = new Vector3(startPosition.x, startPosition.y - (yScale / 4), startPosition.z - zScale);

            if (transform.position.z > (startPosition.z - yScale * (3f / 4f)) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.left, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (yScale / 2), startPosition.z - (yScale * (3f / 4f)));

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying horizontally
        else if (RotateAssist.IsLayingHorizontally(startRotation))
        {
            axis = new Vector3(startPosition.x, startPosition.y - (yScale / 4), startPosition.z - (zScale / 2));

            if (transform.position.z > (startPosition.z - zScale) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.left, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, (yScale / 4), startPosition.z - zScale);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
    }

    private void MoveLeft()
    {
        //If the block is standing up straight
        if (RotateAssist.IsStanding(startRotation))
        {
            axis = new Vector3(startPosition.x - (xScale / 2), startPosition.y - (yScale / 2), startPosition.z);

            if (transform.position.y > (zScale / 2) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.forward, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(startPosition.x - (yScale * (3f / 4f)), (xScale / 2), transform.position.z);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying down horizontal
        else if (RotateAssist.IsLayingHorizontally(startRotation))
        {
            axis = new Vector3(startPosition.x - xScale, startPosition.y - (yScale / 4), startPosition.z);

            if (transform.position.x > (startPosition.x - yScale * (3f / 4f)) + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.forward, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(startPosition.x - (yScale * (3f / 4f)), (yScale / 2), transform.position.z);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
        //If the block is laying down vertically
        else if (RotateAssist.IsLayingVertically(startRotation))
        {
            axis = new Vector3(startPosition.x - (xScale / 2), startPosition.y - (yScale / 4), startPosition.z);

            if (transform.position.x > startPosition.x - xScale + rotateBuffer)
            {
                transform.RotateAround(axis, Vector3.forward, speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(startPosition.x - xScale, (yScale / 4), transform.position.z);

                float xAngle = RotateAssist.DetermineRotation(transform.eulerAngles.x);
                float yAngle = RotateAssist.DetermineRotation(transform.eulerAngles.y);
                float zAngle = RotateAssist.DetermineRotation(transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);

                readyToMove = false;
                canMoveAgain = true;
                startPosition = transform.position;
            }
        }
    }

    public Direction GetDirection()
    {
        return direction;
    }

}
