using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCharacterActionButton : MonoBehaviour
{
    public void OnActionButtonClicked()
    {
        Debug.Log("Action button for " + action.ToString() + " clicked!");
        UIEvents.actionButtonClickedEvent.Invoke(action);
    }

    public void SetAction(ECharacterAction action, Sprite iconImage)
    {
        this.action = action;
        iconImageComponent.sprite = iconImage;
        iconImageComponent.enabled = true;
        buttonComponent.enabled = true;
    }

    public void ResetButton()
    {
        iconImageComponent.enabled = false;
        buttonComponent.enabled = false;
    }

    private void Start()
    {
        var imageComponents = GetComponentsInChildren<Image>();
        foreach (var component in imageComponents)
        {
            if (component.gameObject != this.gameObject)
            {
                iconImageComponent = component;
            }
        }

        buttonComponent = GetComponent<Button>();

        ResetButton();
    }

    ECharacterAction action { get; set; }
    Image iconImageComponent;
    Button buttonComponent;
}
