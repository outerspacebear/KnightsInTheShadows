using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterActionController : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        UIEvents.actionButtonClickedEvent.AddListener(OnActionButtonClicked);
        MapLoadedEvent.Get().AddListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.AddListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.AddListener(Cleanup);
        TileEvents.tileClickedOnEvent.AddListener(OnTileClickedOn);
        TeamEvents.teamTurnStartedEvent.AddListener(OnTeamTurnStarted);
        CharacterEvents.characterRightClickedEvent.AddListener(OnCharacterRightClickedOn);
        LevelEvents.levelIsMultiplayerEvent.AddListener(LevelIsMultiplayer);
    }

    private void OnDestroy()
    {
        UIEvents.actionButtonClickedEvent.RemoveListener(OnActionButtonClicked);
        MapLoadedEvent.Get().RemoveListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.RemoveListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.RemoveListener(Cleanup);
        TileEvents.tileClickedOnEvent.RemoveListener(OnTileClickedOn);
        TeamEvents.teamTurnStartedEvent.RemoveListener(OnTeamTurnStarted);
        CharacterEvents.characterRightClickedEvent.RemoveListener(OnCharacterRightClickedOn);
        LevelEvents.levelIsMultiplayerEvent.RemoveListener(LevelIsMultiplayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelIsMultiplayer() => isLevelMultiplayer = true;

    void OnActionButtonClicked(ECharacterAction action)
    {
        Cleanup();
        currentlySelectedAction = action;
        switch(action)
        {
            case ECharacterAction.MOVE:
                OnMoveButtonClicked();
                break;
            case ECharacterAction.ATTACK:
                OnAttackButtonClicked();
                break;
            default:
                Debug.LogError("Action " + action.ToString() + " not covered in switch case! Nothing will happen when you click on the button!");
                break;
        }
    }

    void OnMoveButtonClicked()
    {
        tilesInRange = TileMapTools.GetTilesWithinMovementRange(map, currentlySelectedCharacter.occupyingTile
            , currentlySelectedCharacter.GetMovementPerAction());
        foreach (var tile in tilesInRange)
        {
            if(!IsAnyCharacterOnTile(tile))
            {
                tile.EnableMovementRangeHighlight(currentlySelectedCharacter.tileHighlightColor);
            }
        }
    }

    void OnAttackButtonClicked()
    {
        tilesInRange = TileMapTools.GetTilesWithinMovementRange(map, currentlySelectedCharacter.occupyingTile
            , currentlySelectedCharacter.GetAttackRange());
        foreach(var tile in tilesInRange)
        {
            if(IsEnemyCharacterOnTile(tile))
            {
                tile.EnableMovementRangeHighlight(currentlySelectedCharacter.tileHighlightColor);
            }
        }
    }

    void OnMapLoaded(TileMap map)
    {
        this.map = map;
        allTeams = levelManager.GetAllTeams();
    }

    void OnCharacterSelected(CCharacter character) => currentlySelectedCharacter = character;

    void OnTeamTurnStarted(TeamBase team) => currentlySelectedTeam = team;

    void Cleanup(CCharacter deselectedCharacter = null)
    {
        foreach(var tile in tilesInRange)
        {
            tile.RemoveAllHighlights();
        }
    }

    void OnTileClickedOn(CTile tile)
    {
        switch(currentlySelectedAction)
        {
            case ECharacterAction.MOVE:
                if (currentlySelectedCharacter.CanTakeAction(ECharacterAction.MOVE)
                    && tilesInRange.Contains(tile)
                    && !IsAnyCharacterOnTile(tile))
                {
                    if(!isLevelMultiplayer)
                    {
                        currentlySelectedCharacter.MoveTo(tile);
                    }
                    else
                    {
                        PhotonView photonView = currentlySelectedCharacter.GetComponent<PhotonView>();
                        photonView.RPC("MoveToTileAtPosition", RpcTarget.All, tile.transform.position);
                    }
                }
                break;
            default:
                Debug.LogError("The case for action " + currentlySelectedAction.ToString() + " has not been covered!");
                break;
        }
        
    }

    void OnCharacterRightClickedOn(CCharacter character)
    {
        switch(currentlySelectedAction)
        {
            case ECharacterAction.ATTACK:
                if (currentlySelectedCharacter.CanTakeAction(ECharacterAction.ATTACK)
                    && tilesInRange.Contains(character.occupyingTile)
                    && IsEnemyCharacterOnTile(character.occupyingTile))
                {
                    currentlySelectedCharacter.Attack(character);
                }
                break;
        }
    }

    public bool IsAnyCharacterOnTile(CTile tile)
    {
        foreach(var team in allTeams)
        {
            if(team.IsAnyCharacterOnTile(tile))
            {
                return true;
            }
        }

        return false;
    }

    //Enemy refers to any team other than thecurrently selected one
    public bool IsEnemyCharacterOnTile(CTile tile)
    {
        foreach (var team in allTeams)
        {
            if(team == currentlySelectedTeam)
            {
                continue;
            }

            if (team.IsAnyCharacterOnTile(tile))
            {
                return true;
            }
        }

        return false;
    }

    public CCharacter GetCharacterOnTile(CTile tile)
    {
        foreach (var team in allTeams)
        {
            CCharacter character = team.GetCharacterOnTile(tile);
            if (character != null)
            {
                return character;
            }
        }

        return null;
    }

    ECharacterAction currentlySelectedAction;
    TileMap map;
    CCharacter currentlySelectedCharacter;
    List<TeamBase> allTeams;
    TeamBase currentlySelectedTeam;

    List<CTile> tilesInRange = new List<CTile>();

    [SerializeField]
    LevelManager levelManager;

    bool isLevelMultiplayer = false;
}
