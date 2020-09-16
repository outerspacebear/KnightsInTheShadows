using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public void SaveGameFromMenu()
    {
        if(!saveNameText)
        {
            Debug.LogError("Save name text field not assigned to save manager; unable to save from menu!");
            return;
        }

        SaveGame(saveNameText.text);
    }

    void SaveGame(string name)
    {
        Debug.Log("Attempting to save game!");

        XElement xRoot = new XElement(XMLFields.Header);
        xRoot.Add(new XElement(XMLFields.SceneName, SceneManager.GetActiveScene().name));

        var allTeams = levelManager.GetAllTeams();
        foreach(var team in allTeams)
        {
            XElement teamState = team.GetTeamStateAsXML();
            xRoot.Add(teamState);
        }

        XDocument xSaveGame = new XDocument(xRoot);

        string saveFileName = saveFolderName + "/" + name + ".xml";
        xSaveGame.Save(saveFileName);

        Debug.Log("Game saved as " + saveFileName);
    }

    public static class XMLFields
    {
        public const string Header = "save_game";
        public const string SceneName = "scene_name";
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!levelManager)
        {
            Debug.LogError("Level manager not assigned to SaveManager!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    LevelManager levelManager = null;
    [SerializeField]
    Text saveNameText = null;
    [SerializeField][Tooltip("Folder in which saves are to be stored.")]
    string saveFolderName = "Saved Games";
}
