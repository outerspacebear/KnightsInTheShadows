using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CScoutCharacter : CCharacter
{
    public override ECharacterAction[] availableActions { get; } = 
        { ECharacterAction.MOVE, ECharacterAction.ATTACK, ECharacterAction.SPRINT };

    [Photon.Pun.PunRPC]
    public override void SprintToTileAtPosition(Vector3 tilePosition)
    {
        CTile targetTile = map.TryGetTileAt(tilePosition);
        if (!targetTile)
        {
            Debug.LogError("Could not find tile at " + tilePosition.ToString() + "; something might have gone wrong!");
            return;
        }

        SprintTo(targetTile);
    }

    public override void SprintTo(CTile tile)
    {
        transform.position = tile.transform.position;
        occupyingTile = tile;
        currentActionPoints -= CharacterActions.actionCostMap[ECharacterAction.SPRINT];

        Debug.Log("Character " + name + " sprinted to " + tile.transform.position.ToString());
        CharacterEvents.actionTakenEvent.Invoke(this, ECharacterAction.SPRINT);
    }
}
