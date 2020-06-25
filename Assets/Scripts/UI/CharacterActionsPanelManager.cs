﻿using System.Collections;
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

    }

    void OnMapLoaded(TileMap map)
    {
        actionPanel.SetActive(false);
        actionDescriptionPanel.gameObject.SetActive(false);
    }

    void OnCharacterSelected(CCharacter character)
    {
        actionPanel.SetActive(true);
        availableActions = new List<ECharacterActions>(character.availableActions);
        UpdatePanel();

        //Click the first action button by default
        if(availableActions.Count > 0)
        {
            actionButtons[0].GetComponent<Button>().onClick.Invoke();
        }
    }

    void OnCharacterDeselected(CCharacter character)
    {
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
            ++actionButtonIndex;
        }

        //Disable buttons not being used
        for(; actionButtonIndex < actionButtons.Count; ++actionButtonIndex)
        {
            actionButtons[actionButtonIndex].ResetButton();
        }
    }

    Sprite GetActionIcon(ECharacterActions action)
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

    void OnActionButtonClicked(ECharacterActions action)
    {
        //Show description text
        string description = CharacterActions.actionDescriptionMap[action];
        actionDescriptionText.text = description;
        actionDescriptionPanel.gameObject.SetActive(true);
    }

    [SerializeField]
    GameObject actionPanel;
    [SerializeField]
    GameObject actionDescriptionPanel;
    Text actionDescriptionText;
    [SerializeField]
    List<CCharacterActionButton> actionButtons;

    List<ECharacterActions> availableActions;

    const string actionIconsPath = "Prefabs/UI/Actions";
    Dictionary<ECharacterActions, string> actionIconPrefabNames = 
        new Dictionary<ECharacterActions, string>{ { ECharacterActions.MOVE, "Move" } };
    Sprite[] actionIcons;
}