using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterActionsPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        actionIcons = Resources.LoadAll(actionIconsPath, typeof(Sprite)).Cast<Sprite>().ToArray();

        actionDescriptionText = actionDescriptionPanel.GetComponentInChildren<Text>();
        if(!actionDescriptionText)
        {
            Debug.LogError("No Text component found in children of action description panel!");
        }

        int autoSelectIntValue = PlayerPrefs.GetInt("autoSelectFirstAction", 0);
        shouldAutoSelectFirstAction = autoSelectIntValue == 1 ? true : false;

        MapLoadedEvent.Get().AddListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.AddListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.AddListener(OnCharacterDeselected);
        UIEvents.actionButtonClickedEvent.AddListener(OnActionButtonClicked);
    }

    private void OnDestroy()
    {
        MapLoadedEvent.Get().RemoveListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.RemoveListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.RemoveListener(OnCharacterDeselected);
        UIEvents.actionButtonClickedEvent.RemoveListener(OnActionButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryClickActionButton(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryClickActionButton(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TryClickActionButton(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TryClickActionButton(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TryClickActionButton(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TryClickActionButton(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            TryClickActionButton(6);
        }
    }

    void TryClickActionButton(int index)
    {
        if (availableActions != null && availableActions.Count > index)
        {
            Button button = actionButtons[index].GetComponent<Button>();
            if(button == null)
            {
                Debug.LogError("No button component found on object with action button script!");
                return;
            }

            if(button.interactable)
            {
                button.Select();
                button.onClick.Invoke();
            }
        }
    }

    void OnMapLoaded(TileMap map)
    {
        actionPanel.SetActive(false);
        actionDescriptionPanel.gameObject.SetActive(false);
    }

    void OnCharacterSelected(CCharacter character)
    {
        currentlySelectedCharacter = character;
        actionPanel.SetActive(true);
        availableActions = new List<ECharacterAction>(character.availableActions);
        UpdatePanel();

        if(shouldAutoSelectFirstAction)
        {
            Invoke("TryClickDefaultActionButton", 0.1f);
        }
    }

    void TryClickDefaultActionButton()
    {
        TryClickActionButton(0);
    }

    void OnCharacterDeselected(CCharacter character)
    {
        currentlySelectedCharacter = null;
        actionPanel.SetActive(false);
        actionDescriptionPanel.gameObject.SetActive(false);
        availableActions = null;
        ResetAllButtons();
    }

    void UpdatePanel()
    {
        int actionButtonIndex = 0;
        foreach (var action in availableActions)
        {
            Sprite actionIcon = null;
            if(!(actionIcon = GetActionIcon(action)))
            {
                Debug.LogError("Cannot find action icon prefab for action " + action.ToString());
                return;
            }

            actionButtons[actionButtonIndex].SetAction(action, actionIcon);
            if(!currentlySelectedCharacter.CanTakeAction(action))
            {
                actionButtons[actionButtonIndex].DisableButton();
            }
            ++actionButtonIndex;
        }

        //Disable buttons not being used
        for(; actionButtonIndex < actionButtons.Count; ++actionButtonIndex)
        {
            actionButtons[actionButtonIndex].ResetButton();
        }
    }

    Sprite GetActionIcon(ECharacterAction action)
    {
        string actionIconName = actionIconPrefabNames[action];
        foreach(var iconPrefab in actionIcons)
        {
            if(iconPrefab.name == actionIconName)
            {
                return iconPrefab;
            }
        }

        return null;
    }

    void ResetAllButtons()
    {
        foreach(var button in actionButtons)
        {
            button.ResetButton();
        }
    }

    void OnActionButtonClicked(ECharacterAction action)
    {
        //Show description text
        string description = CharacterActions.actionDescriptionMap[action];
        actionDescriptionText.text = description;
        actionDescriptionPanel.gameObject.SetActive(true);
    }

    void OnCharacterActionTaken(CCharacter character, ECharacterAction action)
    {
        
    }

    [SerializeField]
    GameObject actionPanel;
    [SerializeField]
    GameObject actionDescriptionPanel;
    Text actionDescriptionText;
    [SerializeField]
    List<CCharacterActionButton> actionButtons;

    List<ECharacterAction> availableActions;
    CCharacter currentlySelectedCharacter;

    const string actionIconsPath = "Prefabs/UI/Actions";
    Dictionary<ECharacterAction, string> actionIconPrefabNames = 
        new Dictionary<ECharacterAction, string>{
            { ECharacterAction.MOVE, "Move" },
            {ECharacterAction.ATTACK, "Attack" },
            {ECharacterAction.SPRINT, "Sprint" },
            {ECharacterAction.SLASH, "Slash" } };
    Sprite[] actionIcons;

    //PlayerPrefs
    bool shouldAutoSelectFirstAction = false;
}
