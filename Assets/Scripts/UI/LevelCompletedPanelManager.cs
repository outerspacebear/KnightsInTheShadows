﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        LevelEvents.levelCompletedEvent.AddListener(OnLevelCompleted);
    }

    private void OnDestroy()
    {
        LevelEvents.levelCompletedEvent.RemoveListener(OnLevelCompleted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLevelCompleted()
    {
        levelCompletedPanel.SetActive(true);
    }

    [SerializeField]
    GameObject levelCompletedPanel;
}