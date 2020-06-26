using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterActions
{
    MOVE
}

public static class CharacterActions
{
    public static Dictionary<ECharacterActions, int> actionCostMap = new Dictionary<ECharacterActions, int>()
    { { ECharacterActions.MOVE, 1 } };

    public static Dictionary<ECharacterActions, string> actionDescriptionMap = new Dictionary<ECharacterActions, string>()
    { {ECharacterActions.MOVE, "Move the character to a tile within movement range"} };
}
