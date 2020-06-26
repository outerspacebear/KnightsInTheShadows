using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAdder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
        originalText = textComponent.text;

        InvokeRepeating("AddText", 0.3f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddText()
    {
        if(timesAdded == maxRepetitions)
        {
            textComponent.text = originalText;
            timesAdded = 0;
        }

        textComponent.text += textToAdd;
        ++timesAdded;
    }

    [SerializeField]
    string textToAdd = ".";
    [SerializeField]
    int maxRepetitions = 3;
    int timesAdded = 0;
    string originalText;

    Text textComponent;
}
