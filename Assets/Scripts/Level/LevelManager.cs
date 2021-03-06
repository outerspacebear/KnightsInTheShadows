﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MapProperties = MapOperationsManager.MapProperties;
using System.Security.Cryptography;
using Photon.Pun;

public class LevelManager : MonoBehaviourPun
{
    public List<TeamBase> GetAllTeams()
    {
        return teams;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(targetLevelObjectives.Count == 0)
        {
            Debug.LogWarning("Level objectives not set! Level will never end!");
        }

        if(!isMultiplayerLevel)
        {
            foreach(var team in teams)
            {
                team.InitialiseForSinglePlayerGame();
            }
        }
        else
        {
            SetUpMultiplayerLevel();
        }

        TeamEvents.teamTurnEndedEvent.AddListener(OnTeamTurnEnded);
        LevelEvents.objectiveCompleteEvent.AddListener(OnLevelObjectiveCompleted);
        Invoke("StartLevel", 1f);
    }

    void OnDestroy()
    {
        TeamEvents.teamTurnEndedEvent.RemoveListener(OnTeamTurnEnded);
        LevelEvents.objectiveCompleteEvent.RemoveListener(OnLevelObjectiveCompleted);
    }

    public void StartLevel()
    {
        if (hasLevelStarted || !TryLoadLevel())
        {
            return;
        }
        MapLoadedEvent.Get().Invoke(map);
        Debug.Log("Map loaded successfully!");
        if(isMultiplayerLevel)
        {
            LevelEvents.levelIsMultiplayerEvent.Invoke();
        }

        if(loadingScreen)
        {
            loadingScreen.SetActive(false);
        }

        if (teams.Count == 0)
        {
            Debug.LogWarning("No teams assigned to the level manager");
            return;
        }
        teams[currentTeamIndex].BeginTurn();
        hasLevelStarted = true;
    }

    void SetUpMultiplayerLevel()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("Initialising teams for multiplayer. You are player number " + playerNumber.ToString());

        foreach (var team in teams)
        {
            team.InitialiseForMultiplayerGame(playerNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {

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

    [PunRPC]
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

    void OnTeamTurnEnded(TeamBase team)
    {
        if(team != teams[currentTeamIndex])
        {
            Debug.LogError("The team whose turn ended wasn't the currently selected team in the LevelManager! This should never happen!");
            return;
        }

        if(!isLevelCompleted)
        {
            if(isMultiplayerLevel)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("BeginNextTeamTurn", RpcTarget.All);
            }
            else
            {
                BeginNextTeamTurn();
            }
        }
    }

    void OnLevelObjectiveCompleted(LevelObjective objective)
    {
        if(targetLevelObjectives.Contains(objective))
        {
            var winningTeam = objective.GetObjectiveOwner();
            Debug.Log("Objective " + objective.GetObjectiveType() + " achieved by " + winningTeam.name);
            isLevelCompleted = true;
            LevelEvents.levelWonEvent.Invoke(winningTeam);
        }
    }

    [SerializeField]
    List<TeamBase> teams;
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

    [SerializeField]
    GameObject loadingScreen;

    [SerializeField]
    List<LevelObjective> targetLevelObjectives = new List<LevelObjective>();
    bool isLevelCompleted = false;

    [SerializeField]
    bool isMultiplayerLevel = false;
}
