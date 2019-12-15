using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float speed;

    Vector3 axis;
    Vector3 startPosition;
    float rotateBuffer = .005f;


    int rotateCount = 0;

    Direction direction;


    bool readyToMove;

    private void Start()
    {
        startPosition = transform.position;
        readyToMove = false;
    }

    void Update ()
    {
        if (rotateCount % 2 == 0)
        {
            rotateBuffer = 0.1f;
        }
        if (readyToMove)
        {
            rotateCount++;
            switch(direction)
            {
                case Direction.FORWARD:
                    RotateForward();
                    break;
                case Direction.BACK:
                    RotateBack();
                    break;
                case Direction.RIGHT:
                    RotateRight();
                    break;
                case Direction.LEFT:
                    RotateLeft();
                    break;
            }
        }
    }

    public void RotateBridge(Direction _direction)
    {
        readyToMove = true;
        AudioManager.instance.PlayBridgeSound();
        direction = _direction;
    }

    private void RotateForward()
    {
        axis = new Vector3(startPosition.x, startPosition.y, startPosition.z + (transform.localScale.x / 2));

        if (transform.position.z < (startPosition.z + transform.localScale.x) - rotateBuffer)
        {
            transform.RotateAround(axis, Vector3.left, speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(startPosition.x, -0.16f, startPosition.z + transform.localScale.x);
            transform.eulerAngles = new Vector3(0f, 90f, -180f);

            readyToMove = false;
            startPosition = transform.position;
        }

    }

    private void RotateBack()
    {
        axis = new Vector3(startPosition.x, startPosition.y, startPosition.z - (transform.localScale.x / 2));

        if (transform.position.z > (startPosition.z - transform.localScale.x) + rotateBuffer)
        {
            transform.RotateAround(axis, Vector3.right, speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(startPosition.x, -0.16f, startPosition.z - transform.localScale.x);
            transform.eulerAngles = new Vector3(0f, 90f, 180f);

            readyToMove = false;
            startPosition = transform.position;
        }

    }


    private void RotateRight()
    {
        axis = new Vector3(startPosition.x + (transform.localScale.x / 2), startPosition.y, startPosition.z);

        if (transform.position.x < (startPosition.x + transform.localScale.x) - rotateBuffer)
        {
            transform.RotateAround(axis, Vector3.forward, speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(startPosition.x + transform.localScale.x, -0.16f, startPosition.z );
            transform.eulerAngles = new Vector3(0f, 0f, 180f);

            readyToMove = false;
            startPosition = transform.position;
        }

    }

    private void RotateLeft()
    {
        axis = new Vector3(startPosition.x - (transform.localScale.x / 2), startPosition.y, startPosition.z);

        if (transform.position.x > (startPosition.x - transform.localScale.x) + rotateBuffer)
        {
            transform.RotateAround(axis, Vector3.back, speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(startPosition.x - transform.localScale.x, -0.16f, startPosition.z);
            transform.eulerAngles = new Vector3(0f, 0f, -180f);

            readyToMove = false;
            startPosition = transform.position;
        }

    }

    public void SetDirection(Direction _direction)
    {
        direction = _direction;
    }

    public Direction GetDirection()
    {
        return direction;
    }
}
