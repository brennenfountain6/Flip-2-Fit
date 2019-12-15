using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAssist : MonoBehaviour
{


    public static bool IsStanding(Vector3 rot)
    {
        if (rot.z < 0.001f)
        {
            rot = new Vector3(rot.x, rot.y, 0f);
        }
        if (rot.y < 0.001f)
        {
            rot = new Vector3(rot.x, 0f, rot.z);
        }
        if (rot.x < 0.001f)
        {
            rot = new Vector3(0f, rot.y, rot.z);
        }
        if (rot.x == 0.0 && rot.y == 180.0 && rot.z == 180.0)
        {
            return true;
        }
        if (rot.x == 0 && (rot.y == 0 || rot.y == 90 || rot.y == 270) && rot.z == 0)
        {
            return true;
        }
        if (rot.x == 0 && rot.y == 180 && rot.z == 0)
        {
            return true;
        }
        if (rot.x == 0 && (rot.y == 0 || rot.y == 90 || rot.y == 270) && rot.z == 180)
        {
            return true;
        }


        return false;
    }

    public static bool IsLayingVertically(Vector3 rot)
    {
        if (rot.z < 0.001f)
        {
            rot = new Vector3(rot.x, rot.y, 0f);
        }
        if (rot.y < 0.001f)
        {
            rot = new Vector3(rot.x, 0f, rot.z);
        }
        if (rot.x < 0.001f)
        {
            rot = new Vector3(0f, rot.y, rot.z);
        }

        if ((rot.x == 270 || rot.x == 90) && rot.y == 0 && rot.z == 0)
        {
            return true;
        }
        if ((rot.x == 270 || rot.x == 90) && rot.y == 180 && rot.z == 0)
        {
            return true;
        }

        //90
        if (rot.x == 0 && rot.y == 90 && (rot.z == 90 || rot.z == 270))
        {
            return true;
        }
        if (rot.x == 0 && rot.y == 270 && (rot.z == 270 || rot.z == 90))
        {
            return true;
        }

        return false;
    }

    public static bool IsLayingHorizontally(Vector3 rot)
    {
        if (rot.z < 0.001f)
        {
            rot = new Vector3(rot.x, rot.y, 0f);
        }
        if (rot.y < 0.001f)
        {
            rot = new Vector3(rot.x, 0f, rot.z);
        }
        if (rot.x < 0.001f)
        {
            rot = new Vector3(0f, rot.y, rot.z);
        }

        if (rot.x == 0 && rot.y == 0 && (rot.z == 90 || rot.z == 270))
        {
            return true;
        }
        if (rot.x == 0 && rot.y == 180 && (rot.z == 90 || rot.z == 270))
        {
            return true;
        }
      
        //Involving 90 degree rotation from horizontal flip
        if (rot.x == 90 && (rot.y == 89.99999f || rot.y == 90 || rot.y == 90.00001f) && rot.z == 0)
        {
            return true;
        }
        if (rot.x == 270 && (rot.y == 270 || rot.y == 90.00001f || rot.y == 90) && rot.z == 0)
        {
            return true;
        }
        if (rot.x == 90 && rot.y == 270 && rot.z == 0)
        {
            return true;
        }
        if (rot.x == 0 && rot.y == 0 && rot.z == 270)
        {
            return true;
        }

        return false;
    }

    public static float DetermineRotation(float rot)
    {
        float dif = 90 - rot;
        if (dif >= -30 && dif <= 30)
        {
            return rot + dif; 
        }

        dif = 180 - rot;
        if (dif >= -30 && dif <= 30)
        {
            return rot + dif;
        }

        dif = 270 - rot;
        if (dif >= -30 && dif <= 30)
        {
            return rot + dif;
        }

        return 0;



    }

}
