using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MapProperties = MapOperationsManager.MapProperties;
using System.Security.Cryptography;

public class LevelManager : MonoBehaviour
{
    public List<CTeam> GetAllTeams()
    {
        return teams;
    }

    // Start is called before the first frame update
    void Start()
    {
        TeamEvents.teamTurnEndedEvent.AddListener(OnTeamTurnEnded);
    }

    void OnDestroy()
    {
        TeamEvents.teamTurnEndedEvent.RemoveListener(OnTeamTurnEnded);
    }

    void StartLevel()
    {
        if (hasLevelStarted || !TryLoadLevel())
        {
            return;
        }
        MapLoadedEvent.Get().Invoke(map);
        Debug.Log("Map loaded successfully!");

        if (teams.Count == 0)
        {
            Debug.LogWarning("No teams assigned to the level manager");
            return;
        }
        teams[currentTeamIndex].BeginTurn();
        hasLevelStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasLevelStarted && Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartLevel();
        }
    }

    bool TryLoadLevel()
    {
        mapLoadProperties = new MapProperties();
        mapLoadProperties.tileWidth = tileWidth;
        mapLoadProperties.tileHeight = tileHeight;
        mapLoadProperties.startingPosition = mapLoadStartingPosition;

        MapLoader mapLoader = new MapLoader(mapLoadProperties, GetAllTilePrefabs(), mapLoadTileContainer, mapLoadBaseGround);
        var loadedMap = mapLoader.LoadMap(levelName);

        if(loadedMap == null)
        {
            Debug.LogError("Could not load map " + levelName);
            return false;
        }

        map = loadedMap;
        return true;
    }

    GameObject[] GetAllTilePrefabs()
    {
        var tilePrefabs = Resources.LoadAll("Prefabs/Tiles", typeof(GameObject)).Cast<GameObject>().ToArray();
        return tilePrefabs;
    }

    void BeginNextTeamTurn()
    {
        if(teams.Count == 0)
        {
            return;
        }

        teams[currentTeamIndex].OnEndTurn();
        currentTeamIndex = (currentTeamIndex + 1) % teams.Count;
        teams[currentTeamIndex].BeginTurn();
    }

    void OnTeamTurnEnded(CTeam team)
    {
        if(team != teams[currentTeamIndex])
        {
            Debug.LogError("The team whose turn ended wasn't the currently selected team in the LevelManager! This should never happen!");
            return;
        }

        BeginNextTeamTurn();
    }

    [SerializeField]
    List<CTeam> teams;
    int currentTeamIndex = 0;

    //Loading the level
    [SerializeField]
    string levelName;
    [SerializeField]
    float tileWidth = 1.0f;
    [SerializeField]
    float tileHeight = 1.0f;
    [SerializeField]
    Vector3 mapLoadStartingPosition = Vector3.zero;
    [SerializeField]
    [Tooltip("Instantiated tiles will be parented to this transform when loading a map.")]
    Transform mapLoadTileContainer;
    [SerializeField]
    [Tooltip("GameObject to be generated as the base ground below each cell. Leave blank if no base ground should be generated.")]
    GameObject mapLoadBaseGround = null;

    MapProperties mapLoadProperties;
    TileMap map;

    bool hasLevelStarted = false;
}
