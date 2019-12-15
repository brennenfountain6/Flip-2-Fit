using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    


    Text btnText;
    Vector3 startRectTransform;


    // Use this for initialization
    void Start ()
    {
        btnText = GetComponentInChildren<Text>();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        btnText.rectTransform.localPosition = new Vector3(0, -4.6f, 0);

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        btnText.rectTransform.localPosition = new Vector3(0.0f, 4.5f, 0.0f);
    }

}
