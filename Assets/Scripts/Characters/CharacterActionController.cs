using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIEvents.actionButtonClickedEvent.AddListener(OnActionButtonClicked);
        MapLoadedEvent.Get().AddListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.AddListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.AddListener(Cleanup);
        TileEvents.tileClickedOnEvent.AddListener(OnTileClickedOn);
        TeamEvents.teamTurnStartedEvent.AddListener(OnTeamSelected);
    }

    private void OnDestroy()
    {
        UIEvents.actionButtonClickedEvent.RemoveListener(OnActionButtonClicked);
        MapLoadedEvent.Get().RemoveListener(OnMapLoaded);
        CharacterEvents.characterSelectedEvent.RemoveListener(OnCharacterSelected);
        CharacterEvents.characterDeselectedEvent.RemoveListener(Cleanup);
        TileEvents.tileClickedOnEvent.RemoveListener(OnTileClickedOn);
        TeamEvents.teamTurnStartedEvent.RemoveListener(OnTeamSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnActionButtonClicked(ECharacterActions action)
    {
        Cleanup();
        currentlySelectedAction = action;
        switch(action)
        {
            case ECharacterActions.MOVE:
                OnMoveButtonClicked();
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
            tile.EnableMovementRangeHighlight(currentlySelectedCharacter.tileHighlightColor);
        }
    }

    void OnMapLoaded(TileMap map)
    {
        this.map = map;
        allTeams = levelManager.GetAllTeams();
    }

    void OnCharacterSelected(CCharacter character) => currentlySelectedCharacter = character;

    void OnTeamSelected(CTeam team) => currentlySelectedTeam = team;

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
            case ECharacterActions.MOVE:
                if (currentlySelectedCharacter.CanTakeAction(ECharacterActions.MOVE)
                    && tilesInRange.Contains(tile)
                    && !IsTileOccupied(tile))
                {
                    currentlySelectedCharacter.MoveTo(tile);
                }
                break;
            default:
                Debug.LogError("The case for action " + currentlySelectedAction.ToString() + " has not been covered!");
                break;
        }
        
    }

    bool IsTileOccupied(CTile tile)
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

    ECharacterActions currentlySelectedAction;
    TileMap map;
    CCharacter currentlySelectedCharacter;
    CTeam currentlySelectedTeam;
    List<CTeam> allTeams;

    List<CTile> tilesInRange = new List<CTile>();

    [SerializeField]
    LevelManager levelManager;
}
