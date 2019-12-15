using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingTrigger : MonoBehaviour {

    Ray ray;
    RaycastHit hitInfo;

    GameObject bridge;

    Direction direction;

    int hitCount = 0;
    bool externalActivation = false;

    Vector3 initialBridgePosition;

    int i = 0;

    private void Update()
    {

        i++;

        ray = new Ray(transform.position, Vector3.up);

        if (Physics.Raycast(ray, out hitInfo, 1))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if (hitObject.tag == "Player" && hitObject.transform.position.y == 0.5f)
            {
                if (hitCount < 1)
                {
                    bridge.GetComponent<BridgeController>().RotateBridge(direction);
                    direction = TriggerButton.DetermineReverseDirection(direction);
                    externalActivation = false;
                    hitCount++;
                }
            }
        }
        else
        {
            hitCount = 0;
        }

        if (externalActivation)
        {
            bridge.GetComponent<BridgeController>().RotateBridge(direction);
            direction = TriggerButton.DetermineReverseDirection(direction);
            externalActivation = false;
        }

        if (bridge.transform.position == initialBridgePosition)
        {
            bridge.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            bridge.GetComponent<MeshRenderer>().enabled = true;  
        }
    }

    public void SetInitialDirection(Direction _direction)
    {
        direction = _direction;
        bridge.GetComponent<BridgeController>().SetDirection(direction);
        initialBridgePosition = bridge.transform.position;
    }

    public void SetBridgeConnectionTag(string tag)
    {
        bridge = GameObject.FindGameObjectWithTag(tag);
    }

    public void ExternalTriggerActivation(bool activate)
    {
        externalActivation = activate;
    }

}
