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
}
