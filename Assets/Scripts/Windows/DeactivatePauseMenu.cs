using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeactivatePauseMenu : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        PauseWindow pausePanel = GetComponentInParent<PauseWindow>();
        pausePanel.SetAnimationBoolean(false);
        print(pausePanel);

        gameObject.SetActive(false);

    }
}
