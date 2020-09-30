using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class CharacterEvents
{
    public class ActionTakenEvent : UnityEvent<CCharacter, ECharacterAction> { }
    public class CharacterSelectedEvent : UnityEvent<CCharacter> { }
    public class CharacterDeselectedEvent : UnityEvent<CCharacter> { }
    public class CharacterAttackedEvent : UnityEvent<CCharacter> { }
    public class CharacterDeathEvent : UnityEvent<CCharacter> { }
    public class CharacterClickedEvent : UnityEvent<CCharacter> { }
    public class CharacterRightClickedEvent: UnityEvent<CCharacter> { }
    public class AIActionTakenEvent : UnityEvent<CCharacter, ECharacterAction> { }


    public static ActionTakenEvent actionTakenEvent { get; } = new ActionTakenEvent();
    public static CharacterSelectedEvent characterSelectedEvent { get; } = new CharacterSelectedEvent();
    public static CharacterDeselectedEvent characterDeselectedEvent { get; } = new CharacterDeselectedEvent();
    public static CharacterAttackedEvent characterAttackedEvent { get; } = new CharacterAttackedEvent();
    public static CharacterDeathEvent characterDeathEvent { get; } = new CharacterDeathEvent();
    public static CharacterClickedEvent characterClickedEvent { get; } = new CharacterClickedEvent();
    public static CharacterRightClickedEvent characterRightClickedEvent { get; } = new CharacterRightClickedEvent();
    public static AIActionTakenEvent aiActionTakenEvent { get; } = new AIActionTakenEvent();
}
