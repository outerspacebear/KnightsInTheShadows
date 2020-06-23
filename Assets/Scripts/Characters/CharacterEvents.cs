using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CharacterEvents
{
    public class ActionTakenEvent : UnityEvent<CCharacter, CCharacter.EActions> { }

    public static ActionTakenEvent actionTakenEvent { get; } = new ActionTakenEvent();
}
