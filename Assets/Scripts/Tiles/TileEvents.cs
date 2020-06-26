using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public static class TileEvents
{
    public class TileClickedOnEvent : UnityEvent<CTile> { }

    public static TileClickedOnEvent tileClickedOnEvent { get; } = new TileClickedOnEvent();
}
