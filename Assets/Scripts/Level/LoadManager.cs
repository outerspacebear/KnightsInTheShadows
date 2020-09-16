using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class LoadManager : MonoBehaviour
{
    public void LoadGameFromMenu()
    {
        if (!loadGameDropdownList)
        {
            Debug.LogError("Load game list not assigned to save manager; unable to load from menu!");
            return;
        }

        LoadGame(loadGameDropdownList.options[loadGameDropdownList.value].text);
    }

    public void RefreshList()
    {
        if (!loadGameDropdownList)
        {
            Debug.LogError("Load game list not assigned to save manager; unable to refresh load list!");
            return;
        }

        PopulateLoadGameList();
    }

    void LoadGame(string name)
    {
        Debug.Log("Attempting to load game!");

        string fileName = loadGameDropdownList.options[loadGameDropdownList.value].text;
        string loadFilePath = saveFolderName + "/" + fileName + ".xml";

        xLoadFile = XDocument.Load(loadFilePath);
        if (xLoadFile == null)
        {
            Debug.LogError("No save game file found at path " + loadFilePath + "; Loading failed!");
            return;
        }

        XElement xRoot = xLoadFile.Element(SaveManager.XMLFields.Header);
        string sceneName = xRoot.Element(SaveManager.XMLFields.SceneName).Value;

        DontDestroyOnLoad(this);
        PlayerPrefs.SetInt("shouldLoadSavedGame", 1);

        SceneManager.LoadScene(sceneName);
    }

    void SendLoadStateEvent()
    {
        TeamEvents.loadTeamStateFromXMLEvent.Invoke(xLoadFile);
        Destroy(this);
    }

    void PopulateLoadGameList()
    {
        DirectoryInfo saveFolder = new DirectoryInfo(saveFolderName);
        FileInfo[] saveFiles = saveFolder.GetFiles("*.xml");
        List<string> saveFileNames = new List<string>();

        foreach (var saveFile in saveFiles)
        {
            saveFileNames.Add(saveFile.Name.Replace(saveFile.Extension, ""));
        }

        loadGameDropdownList.options.Clear();
        loadGameDropdownList.AddOptions(saveFileNames);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("shouldLoadSavedGame") != 0)
        {
            PlayerPrefs.SetInt("shouldLoadSavedGame", 0);
            Invoke("SendLoadStateEvent", 0.7f);
        }
        else
        {
            PopulateLoadGameList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    Dropdown loadGameDropdownList = null;
    [SerializeField]
    [Tooltip("Folder in which to look for saves.")]
    string saveFolderName = "Saved Games";

    static XDocument xLoadFile = null;
}
