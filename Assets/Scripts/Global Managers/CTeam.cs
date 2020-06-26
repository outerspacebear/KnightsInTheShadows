using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTeam : MonoBehaviour
{
    public void BeginTurn()
    {
        Debug.Log("Beginning turn for team " + name);

        isTeamsTurn = true;
        foreach(var character in characters)
        {
            character.ResetActionPoints();
        }
        currentlySelectedCharacter = null;

        TrySelectNextAvailableCharacter();

        TeamEvents.teamTurnStartedEvent.Invoke(this);
    }

    public void OnEndTurn()
    {
        Debug.Log("Ending turn for team " + name);
        currentlySelectedCharacter.OnDeselected();

        isTeamsTurn = false;
    }

    public bool IsAnyCharacterOnTile(CTile tile)
    {
        foreach (var character in characters)
        {
            if (character.occupyingTile == tile)
            {
                return true;
            }
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterClickedOnEvent.Get().AddListener(OnCharacterClickedOn);
        CharacterEvents.actionTakenEvent.AddListener(OnCharacterActionTaken);
    }

    ~CTeam()
    {
        CharacterClickedOnEvent.Get().RemoveListener(OnCharacterClickedOn);
        CharacterEvents.actionTakenEvent.RemoveListener(OnCharacterActionTaken);
    }

    void OnCharacterActionTaken(CCharacter character, ECharacterActions action)
    {
        if(!isTeamsTurn)
        {
            return;
        }

        Debug.Log("Character " + character.name + " did action " + action.ToString() 
            + " and now has " + character.currentActionPoints.ToString() + " action points");

        if (character.currentActionPoints > 0)
        {
            SelectCharacter(character);
            return;
        }

        if (HaveAllCharactersEndedTurn())
        {
            TeamEvents.teamTurnEndedEvent.Invoke(this);
            return;
        }

        TrySelectNextAvailableCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTeamsTurn)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            TrySelectNextAvailableCharacter();
        }
    }

    bool HaveAllCharactersEndedTurn()
    {
        foreach(var character in characters)
        {
            if(character.currentActionPoints > 0)
            {
                return false;
            }
        }

        return true;
    }

    void OnCharacterClickedOn(CCharacter character)
    {
        if(!isTeamsTurn)
        {
            return;
        }

        if(character.currentActionPoints > 0 && currentlySelectedCharacter != character)
        {
            SelectCharacter(character);
        }
    }

    bool TrySelectNextAvailableCharacter()
    {
        Debug.Log("Trying to select next available character!");
        foreach (var character in characters)
        {
            if (character != currentlySelectedCharacter && character.currentActionPoints > 0)
            {
                SelectCharacter(character);
                return true;
            }
        }

        return false;
    }

    void SelectCharacter(CCharacter character)
    {
        if(currentlySelectedCharacter)
        {
            currentlySelectedCharacter.OnDeselected();
        }

        currentlySelectedCharacter = character;
        currentlySelectedCharacter.OnSelected();
    }

    [SerializeField]
    List<CCharacter> characters;
    CCharacter currentlySelectedCharacter;

    bool isTeamsTurn = false;
}
