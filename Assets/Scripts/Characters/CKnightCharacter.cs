using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKnightCharacter : CCharacter
{
    public override ECharacterAction[] availableActions { get; } =
        { ECharacterAction.MOVE, ECharacterAction.ATTACK, ECharacterAction.SLASH };

    public override void Slash(List<CCharacter> targets)
    {
        Debug.Log("Character " + name + " is slashing everyone around them!");
        currentActionPoints -= CharacterActions.actionCostMap[ECharacterAction.SLASH];

        foreach(var character in targets)
        {
            if (!isMultiplayerLevel)
            {
                character.OnAttacked(attackDamage);
            }
            else
            {
                PhotonView photonView = character.GetComponent<PhotonView>();
                photonView.RPC("OnAttacked", RpcTarget.All, attackDamage);
            }
        }
        

        CharacterEvents.actionTakenEvent.Invoke(this, ECharacterAction.SLASH);
    }
}
