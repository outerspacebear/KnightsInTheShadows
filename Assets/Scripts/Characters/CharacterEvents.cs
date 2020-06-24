using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CharacterEvents
{
    public class ActionTakenEvent : UnityEvent<CCharacter, ECharacterActions> { }
    public class CharacterSelectedEvent : UnityEvent<CCharacter> { }
    public class CharacterDeselectedEvent : UnityEvent<CCharacter> { }

    public static ActionTakenEvent actionTakenEvent { get; } = new ActionTakenEvent();
    public static CharacterSelectedEvent characterSelectedEvent { get; } = new CharacterSelectedEvent();
    public static CharacterDeselectedEvent characterDeselectedEvent { get; } = new CharacterDeselectedEvent();
}
