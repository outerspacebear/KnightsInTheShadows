using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public enum EType { DEFAULT, TEAM_ELIMINATION, CHARACTER_ELIMINATION }

    // Start is called before the first frame update
    void Start()
    {
        if(!IsObjectiveSetUpCorrectly())
        {
            Debug.LogWarning("Something is wrong with the level objective! It will not be activated!");
            return;
        }

        switch(type)
        {
            case EType.TEAM_ELIMINATION:
                TeamEvents.teamEliminatedEvent.AddListener(OnTeamEliminated);
                break;
            case EType.CHARACTER_ELIMINATION:
                CharacterEvents.characterDeathEvent.AddListener(OnCharacterDeath);
                break;
        }
    }

    private void OnDestroy()
    {
        switch (type)
        {
            case EType.TEAM_ELIMINATION:
                TeamEvents.teamEliminatedEvent.RemoveListener(OnTeamEliminated);
                break;
            case EType.CHARACTER_ELIMINATION:
                CharacterEvents.characterDeathEvent.RemoveListener(OnCharacterDeath);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsObjectiveSetUpCorrectly()
    {
        switch(type)
        {
            case EType.TEAM_ELIMINATION:
                if(teamToEliminate == null)
                {
                    Debug.LogError("Level objective type: Team Elimination - team to eliminate is null!");
                    return false;
                }
                break;
            case EType.CHARACTER_ELIMINATION:
                if (characterToEliminate == null)
                {
                    Debug.LogError("Level objective type: Character Elimination - character to eliminate is null!");
                    return false;
                }
                break;
            case EType.DEFAULT:
            default:
                Debug.LogError("Level objective (on " + gameObject.name + ") not set up!");
                return false;
        }

        return true;
    }

    void OnTeamEliminated(TeamBase team)
    {
        if(team == teamToEliminate)
        {
            OnObjectiveCompleted(teamToEliminate.name);
        }
    }

    void OnCharacterDeath(CCharacter character)
    {
        if(character == characterToEliminate)
        {
            OnObjectiveCompleted(characterToEliminate.name);
        }
    }

    void OnObjectiveCompleted(string objectiveName)
    {
        Debug.Log("Level objective " + type.ToString() + " " + objectiveName + " completed!");
        LevelEvents.objectiveCompleteEvent.Invoke(this);
        Destroy(this);
    }

    [SerializeField]
    private EType type = EType.DEFAULT;
    [SerializeField]
    private TeamBase teamToEliminate = null;
    [SerializeField]
    private CCharacter characterToEliminate = null;
}
