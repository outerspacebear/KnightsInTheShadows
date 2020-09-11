using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class LevelCompletedPanelManager : MonoBehaviour
{
    public void NavigateToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(levelCompletedPanel == null)
        {
            Debug.LogError("Level Completed Panel not assigned to Manager! Disabling manager.");
            return;
        }

        levelCompletedPanel.SetActive(false);
        LevelEvents.levelWonEvent.AddListener(OnLevelWon);
        LevelEvents.levelIsMultiplayerEvent.AddListener(LevelIsMultiplayer);
    }

    private void OnDestroy()
    {
        LevelEvents.levelWonEvent.RemoveListener(OnLevelWon);
        LevelEvents.levelIsMultiplayerEvent.RemoveListener(LevelIsMultiplayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLevelWon(TeamBase team)
    {
        bool didThisPlayerWin = false;

        if(!isMultiplayerLevel)
        {
            didThisPlayerWin = !team.IsTeamAI();
        }
        else
        {
            didThisPlayerWin = team.GetLocalPlayerNumber() == team.GetTeamNumber();
        }

        panelText.text = didThisPlayerWin ? levelWinText : levelLoseText;
        levelCompletedPanel.SetActive(true);
    }

    void LevelIsMultiplayer() => isMultiplayerLevel = true;

    [SerializeField]
    GameObject levelCompletedPanel;
    [SerializeField]
    Text panelText;

    bool isMultiplayerLevel = false;

    const string levelWinText = "You won! :D";
    const string levelLoseText = "You lost! :(";
}
