using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!characterInfoPanel)
        {
            Debug.LogError("No character info panel assigned to the manager! It won't work!");
            return;
        }

        CharacterEvents.characterSelectedEvent.AddListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.AddListener((CCharacter character) => characterInfoPanel.SetActive(false));

        characterInfoPanel.SetActive(false);
    }

    void OnCharacterSelected(CCharacter character)
    {
        nameText.text = character.name;
        movementRangeText.text = "Movement Range: " + character.GetMovementPerAction().ToString();
        attackRangeText.text = "Attack Range: " + character.GetAttackRange().ToString();

        characterInfoPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    GameObject characterInfoPanel = null;
    [SerializeField]
    Text nameText = null;
    [SerializeField]
    Text movementRangeText = null;
    [SerializeField]
    Text attackRangeText = null;
}
