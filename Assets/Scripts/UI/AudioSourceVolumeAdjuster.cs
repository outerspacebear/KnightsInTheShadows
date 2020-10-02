using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceVolumeAdjuster : MonoBehaviour
{
    public void UpdateVolume()
    {
        float finalVolume = 1f;
        foreach (string pref in playerPrefNames)
        {
            finalVolume = finalVolume * PlayerPrefs.GetFloat(pref);
        }

        GetComponent<AudioSource>().volume = finalVolume;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    List<string> playerPrefNames;
}
