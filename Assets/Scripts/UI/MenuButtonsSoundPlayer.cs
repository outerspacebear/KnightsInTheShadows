using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MenuButtonsSoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach(var button in buttons)
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void OnButtonClicked()
    {
        audioSource.Play();
    }

    Button[] buttons;
    AudioSource audioSource;
}
