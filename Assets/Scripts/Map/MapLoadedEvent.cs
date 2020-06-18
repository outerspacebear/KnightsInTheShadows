using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MapLoadedEvent : UnityEvent<TileMap>
{
    public static MapLoadedEvent Get()
    {
        if (mEvent == null)
        {
            mEvent = new MapLoadedEvent();
        }
        return mEvent;
    }

    static MapLoadedEvent mEvent = null;
}
