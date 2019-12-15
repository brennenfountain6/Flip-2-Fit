using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingBlockController : MonoBehaviour
{
    public float disappearSpeed;

    Ray ray;
    RaycastHit hitInfo;

    int hitCount = 0;
    int disappearCount = 0;
    int appearCount = 0;

    bool shouldDisappear = false;
    bool shouldAppear = false;

    Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }


    private void Update()
    {
        ray = new Ray(transform.position, Vector3.up);

        if (Physics.Raycast(ray, out hitInfo, 1))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if (hitObject.tag == "Player" && ((hitObject.transform.position.y == 0.5f || hitObject.transform.position.y == 0.25f)))
            {
                hitCount++;
            }
        }
        else if (hitCount > 0)
        {
            MakeBlockDisappear();
        }

        if (shouldAppear)
        {
            if (appearCount < 1)
            {
                gameObject.layer = LayerMask.NameToLayer("Ground");
                appearCount++;
            }




            if (transform.localScale.x < startScale.x - 0.05f)
            {
                float yScale;
                if (transform.localScale.y < 0.1f)
                {
                    yScale = transform.localScale.y + disappearSpeed * Time.deltaTime;
                }
                else
                {
                    yScale = 0.01f;
                }
                transform.localScale = new Vector3(transform.localScale.x + disappearSpeed * Time.deltaTime, yScale, transform.localScale.z + disappearSpeed * Time.deltaTime);
            }
            else
            {
                transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
                shouldAppear = false;
                appearCount = 0;
            }
        }
        
    }


    private void MakeBlockDisappear()
    {
        if (disappearCount < 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            disappearCount++;
        }


        

        if (transform.localScale.x > 0.05)
        {
            float yScale;
            if (transform.localScale.y > 0.05f)
            {
                 yScale = transform.localScale.y - disappearSpeed * Time.deltaTime;
            }
            else
            {
                yScale = 0.009f;
            }
            transform.localScale = new Vector3(transform.localScale.x - disappearSpeed * Time.deltaTime, yScale, transform.localScale.z - disappearSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
            hitCount = 0;
            disappearCount = 0;
        }

    }

    public void MakeBlockAppear()
    {
        shouldAppear = true;
    }

}
