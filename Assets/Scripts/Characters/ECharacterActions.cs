using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterAction
{
    MOVE, ATTACK, SPRINT, SLASH
}

public static class CharacterActions
{
    public static Dictionary<ECharacterAction, int> actionCostMap = new Dictionary<ECharacterAction, int>()
    { { ECharacterAction.MOVE, 1 },
        { ECharacterAction.ATTACK, 1 },
        { ECharacterAction.SPRINT, 2 },
        { ECharacterAction.SLASH, 2} };

    public static Dictionary<ECharacterAction, string> actionDescriptionMap = new Dictionary<ECharacterAction, string>()
    { {ECharacterAction.MOVE, "Move the character to a tile within movement range"},
        {ECharacterAction.ATTACK, "Attack a hostile character" },
        {ECharacterAction.SPRINT, "Sprint [Uses two action points]" },
        { ECharacterAction.SLASH, "Slash your sword around [Damages everyone around you]"} };
}
