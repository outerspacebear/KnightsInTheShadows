using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceVolumeAdjuster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float finalVolume = 1f;
        foreach(string pref in playerPrefNames)
        {
            finalVolume = finalVolume * PlayerPrefs.GetFloat(pref);
        }

        GetComponent<AudioSource>().volume = finalVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    List<string> playerPrefNames;
}
