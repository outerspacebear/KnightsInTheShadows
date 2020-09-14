using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public void ResumeGame()
    {
        if(isPaused)
        {
            Unpause();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!pauseMenu)
        {
            Debug.LogError("Pause menu gameobject not assiged to PauseMenuController!");
            return;
        }

        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        LevelEvents.pauseLevelEvent.Invoke(LevelEvents.PauseActions.PAUSE);
        isPaused = true;
    }

    void Unpause()
    {
        pauseMenu.SetActive(false);
        LevelEvents.pauseLevelEvent.Invoke(LevelEvents.PauseActions.PLAY);
        isPaused = false;
    }

    [SerializeField]
    GameObject pauseMenu = null;

    bool isPaused = false;
}
