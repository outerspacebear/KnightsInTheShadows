using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CCharacter))]
public class CCharacterUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(characterCanvas == null || hitPointsText == null || actionPointsText == null)
        {
            Debug.LogError("CharacterUiManager not initialised properly! One or more properties have not been assigned!");
            return;
        }

        thisCharacter = GetComponent<CCharacter>();

        CharacterEvents.characterSelectedEvent.AddListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.AddListener(OnCharacterDeselected);
        CharacterEvents.actionTakenEvent.AddListener(OnCharacterActionTaken);

        characterCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        CharacterEvents.characterSelectedEvent.RemoveListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.RemoveListener(OnCharacterDeselected);
        CharacterEvents.actionTakenEvent.RemoveListener(OnCharacterActionTaken);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCharacterSelected(CCharacter character)
    {
        if(character == thisCharacter)
        {
            characterCanvas.SetActive(true);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        Debug.Log("Updating UI for character " + thisCharacter.name);

        int hitPoints = thisCharacter.GetHitPoints();
        hitPointsText.text = "HP: " + hitPoints.ToString();

        string actionText = "";
        for(int actionPoints = thisCharacter.currentActionPoints; actionPoints > 0; --actionPoints)
        {
            actionText += ">";
        }
        actionPointsText.text = actionText;
    }

    void OnCharacterDeselected(CCharacter character)
    {
        if(character == thisCharacter)
        {
            characterCanvas.SetActive(false);
        }
    }

    void OnCharacterActionTaken(CCharacter character, ECharacterAction action)
    {
        if(character == thisCharacter)
        {
            UpdateUI();
        }
    }

    [SerializeField]
    GameObject characterCanvas;
    [SerializeField]
    Text hitPointsText;
    [SerializeField]
    Text actionPointsText;

    CCharacter thisCharacter;
}
