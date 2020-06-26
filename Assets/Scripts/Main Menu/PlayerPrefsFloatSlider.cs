using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlayerPrefsFloatSlider : MonoBehaviour
{
    public void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat(playerPrefName, slider.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefName == null || playerPrefName == "")
        {
            Debug.LogError("No playerPrefs name assigned to toggler!");
            Destroy(this);
        }
        slider = GetComponent<Slider>();

        slider.value = PlayerPrefs.GetFloat(playerPrefName, 0);
    }

    [SerializeField]
    string playerPrefName;
    Slider slider;
}
