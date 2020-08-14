using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTeam : TeamBase
{
    public override void BeginTurn()
    {
        Debug.Log("Beginning turn for team " + name);

        isTeamsTurn = true;
        foreach(var character in characters)
        {
            character.ResetActionPoints();
        }
        currentlySelectedCharacter = null;

        if(!TrySelectNextAvailableCharacter())
        {
            shouldEndTurnNextUpdate = true;
        }

        TeamEvents.teamTurnStartedEvent.Invoke(this);
    }

    public override void OnEndTurn()
    {
        Debug.Log("Ending turn for team " + name);
        if(currentlySelectedCharacter)
        {
            currentlySelectedCharacter.OnDeselected();
        }

        isTeamsTurn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterEvents.characterClickedEvent.AddListener(OnCharacterClickedOn);
        CharacterEvents.actionTakenEvent.AddListener(OnCharacterActionTaken);
        CharacterEvents.characterDeathEvent.AddListener(OnCharacterDeath);
    }

    ~CTeam()
    {
        CharacterEvents.characterClickedEvent.RemoveListener(OnCharacterClickedOn);
        CharacterEvents.actionTakenEvent.RemoveListener(OnCharacterActionTaken);
        CharacterEvents.characterDeathEvent.RemoveListener(OnCharacterDeath);
    }

    void OnCharacterActionTaken(CCharacter character, ECharacterAction action)
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

        if (AreAllCharactersOutOfActions())
        {
            shouldEndTurnNextUpdate = true;
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TrySelectNextAvailableCharacter();
        }

        if (shouldEndTurnNextUpdate)
        {
            TeamEvents.teamTurnEndedEvent.Invoke(this);
            shouldEndTurnNextUpdate = false;
        }
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

    void OnCharacterDeath(CCharacter character)
    {
        if(characters.Contains(character))
        {
            CharacterEvents.characterDeselectedEvent.Invoke(character);
            characters.Remove(character);
            if(currentlySelectedCharacter == character)
            {
                currentlySelectedCharacter = null;
            }
            Debug.Log("Character " + character.name + " is dead!");
            Destroy(character.gameObject);
        }
    }

    bool TrySelectNextAvailableCharacter()
    {
        if(!isTeamsTurn)
        {
            return false;
        }

        Debug.Log("Trying to select next available character!");
        foreach (var character in characters)
        {
            if (character != currentlySelectedCharacter && character.currentActionPoints > 0)
            {
                SelectCharacter(character);
                return true;
            }
        }

        Debug.Log("No available character found!");
        return false;
    }

    void SelectCharacter(CCharacter character)
    {
        if(!isTeamsTurn)
        {
            return;
        }

        if(currentlySelectedCharacter)
        {
            currentlySelectedCharacter.OnDeselected();
        }

        currentlySelectedCharacter = character;
        currentlySelectedCharacter.OnSelected();
    }

    protected CCharacter currentlySelectedCharacter;

    protected bool isTeamsTurn = false;
    protected bool shouldEndTurnNextUpdate = false;
}
