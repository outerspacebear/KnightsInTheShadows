using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterClickedOnEvent : UnityEvent<CCharacter> 
{
    public static CharacterClickedOnEvent Get()
    {
        if(mEvent == null)
        {
            mEvent = new CharacterClickedOnEvent();
        }
        return mEvent;
    }

    static CharacterClickedOnEvent mEvent = null;
}
