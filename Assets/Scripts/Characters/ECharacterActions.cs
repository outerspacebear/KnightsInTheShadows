using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterAction
{
    MOVE, ATTACK
}

public static class CharacterActions
{
    public static Dictionary<ECharacterAction, int> actionCostMap = new Dictionary<ECharacterAction, int>()
    { { ECharacterAction.MOVE, 1 },
        { ECharacterAction.ATTACK, 1 } };

    public static Dictionary<ECharacterAction, string> actionDescriptionMap = new Dictionary<ECharacterAction, string>()
    { {ECharacterAction.MOVE, "Move the character to a tile within movement range"},
        {ECharacterAction.ATTACK, "Attack a hostile character" } };
}
