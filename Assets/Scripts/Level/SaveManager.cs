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

    public void LoadGameFromMenu()
    {
        if (!loadGameDropdownList)
        {
            Debug.LogError("Load game list not assigned to save manager; unable to load from menu!");
            return;
        }

        LoadGame(loadGameDropdownList.options[loadGameDropdownList.value].text);
    }

    void SaveGame(string name)
    {
        Debug.Log("Attempting to save game!");

        XElement xRoot = new XElement(XMLFields.Header);
        xRoot.Add(new XElement(XMLFields.SceneIndex, SceneManager.GetActiveScene().buildIndex));

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

        PopulateLoadGameList();
    }
    void LoadGame(string name)
    {
        Debug.Log("Attempting to load game!");

        //Make sure file exists and that it is indeed a saved level
        //Make this SaveManager persist over scenes
        //Get all the stored info and hold it
        //Switch scenes, and then pass on the info to each team
    }

    static class XMLFields
    {
        public const string Header = "save_game";
        public const string SceneIndex = "scene_index";
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!levelManager)
        {
            Debug.LogError("Level manager not assigned to SaveManager!");
            return;
        }

        PopulateLoadGameList();
    }

    void PopulateLoadGameList()
    {
        DirectoryInfo saveFolder = new DirectoryInfo(saveFolderName);
        FileInfo[] saveFiles = saveFolder.GetFiles("*.xml");
        List<string> saveFileNames = new List<string>();

        foreach (var saveFile in saveFiles)
        {
            saveFileNames.Add(saveFile.Name);
        }

        loadGameDropdownList.AddOptions(saveFileNames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    LevelManager levelManager = null;
    [SerializeField]
    Text saveNameText = null;
    [SerializeField]
    Dropdown loadGameDropdownList = null;
    [SerializeField][Tooltip("Folder in which saves are stored. Used for both saving and loading.")]
    string saveFolderName = "Saved Games";
}
