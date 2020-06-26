using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PlayerPrefsIntToggler : MonoBehaviour
{
    public void OnToggle()
    {
        int boolToInt = toggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(playerPrefName, boolToInt);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playerPrefName == null || playerPrefName == "")
        {
            Debug.LogError("No playerPrefs name assigned to toggler!");
            Destroy(this);
        }
        toggle = GetComponent<Toggle>();

        int savedValue = PlayerPrefs.GetInt(playerPrefName, 0);
        toggle.isOn = savedValue == 1 ? true : false;
    }

    [SerializeField]
    string playerPrefName;
    Toggle toggle;
}
