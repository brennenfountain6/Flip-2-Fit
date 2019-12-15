using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriggerButton {

    public static Direction DetermineReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.FORWARD:
                return Direction.BACK;
            case Direction.BACK:
                return Direction.FORWARD;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.LEFT:
                return Direction.RIGHT;
            default:
                return Direction.FORWARD;
        }
    }
}
