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

        MapLoadedEvent.Get().AddListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.AddListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.AddListener(OnCharacterDeselected);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMapLoaded(TileMap map)
    {
        actionPanel.SetActive(false);
    }

    void OnCharacterSelected(CCharacter character)
    {
        actionPanel.SetActive(true);
        availableActions = new List<ECharacterActions>(character.availableActions);
        UpdatePanel();
    }

    void OnCharacterDeselected(CCharacter character)
    {
        actionPanel.SetActive(false);
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

    [SerializeField]
    GameObject actionPanel;
    [SerializeField]
    List<CCharacterActionButton> actionButtons;

    List<ECharacterActions> availableActions;

    const string actionIconsPath = "Prefabs/UI/Actions";
    Dictionary<ECharacterActions, string> actionIconPrefabNames = 
        new Dictionary<ECharacterActions, string>{ { ECharacterActions.MOVE, "Move" } };
    Sprite[] actionIcons;
}
