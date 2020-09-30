using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!contentPanelTransform || !logTextPrefab)
        {
            Debug.LogError("LogDisplayer missing one or more references! Will not display anything for now.");
            return;
        }

        CharacterEvents.actionTakenEvent.AddListener((CCharacter character, ECharacterAction action) => 
            AddLog("Character " + character.name + " took action " + action.ToString()));
        CharacterEvents.characterAttackedEvent.AddListener((CCharacter character) =>
            AddLog("Character " + character.name + " was attacked!"));
        CharacterEvents.characterDeathEvent.AddListener((CCharacter character) =>
            AddLog("Character " + character.name + " has died!"));

        TeamEvents.teamTurnStartedEvent.AddListener(OnTeamTurnStarted);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTeamTurnStarted(TeamBase team)
    {
        AddLog("Team " + team.name + " is starting their turn!");
    }

    void AddLog(string text)
    {
        var textObject = GameObject.Instantiate(logTextPrefab, contentPanelTransform);
        logs.Add(textObject);
        textObject.GetComponent<Text>().text = text;
        Invoke("DeleteOldestLog", logLifetime);
    }

    void DeleteOldestLog()
    {
        if (logs.Count > 0)
        {
            Destroy(logs[0]);
            logs.RemoveAt(0);
        }
    }

    [SerializeField]
    Transform contentPanelTransform = null;
    [SerializeField]
    GameObject logTextPrefab = null;

    List<GameObject> logs = new List<GameObject>();
    [SerializeField][Tooltip("Each entry will be deleted after this amount of time.")]
    float logLifetime = 1.3f;
}
