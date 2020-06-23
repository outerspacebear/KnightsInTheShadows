using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TileClickedOnEvent : UnityEvent<CTile>
{
    public static TileClickedOnEvent Get()
    {
        if (mEvent == null)
        {
            mEvent = new TileClickedOnEvent();
        }
        return mEvent;
    }

    static TileClickedOnEvent mEvent = null;
}
