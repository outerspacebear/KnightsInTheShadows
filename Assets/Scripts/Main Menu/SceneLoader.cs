using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(sceneName == null || sceneName == "")
        {
            Debug.LogError("No scene name assigned to LevelLoader on gameobject " + gameObject.name);
            Destroy(this);
        }
    }

    [SerializeField]
    string sceneName;
}
