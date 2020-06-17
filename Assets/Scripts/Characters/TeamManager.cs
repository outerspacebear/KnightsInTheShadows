using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!currentlySelectedCharacter)
        {
            TrySelectNextAvailableCharacter();
        }

        CharacterClickedOnEvent.Get().AddListener(OnCharacterClickedOn);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            TrySelectNextAvailableCharacter();
        }

        if (!currentlySelectedCharacter)
        {
            if (!TrySelectNextAvailableCharacter())
            {
                Debug.Log("All characters on this team have taken their turn");
            }
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

        currentlySelectedCharacter = null;
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

        Debug.Log("Character " + currentlySelectedCharacter.name + " selected!");
    }

    [SerializeField]
    List<CCharacter> characters;
    CCharacter currentlySelectedCharacter;
}
