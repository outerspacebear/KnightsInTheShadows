using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

public class CAITeam : TeamBase
{
    public override void BeginTurn()
    {
        Debug.Log("Beginning turn for AI team " + name);

        foreach (var character in characters)
        {
            if(!loadedState)
            {
                character.ResetActionPoints();
            }
        }

        if (AreAllCharactersOutOfActions())
        {
            shouldEndTurnNextUpdate = true;
        }

        TeamEvents.teamTurnStartedEvent.Invoke(this);

        ProcessTurnWithTimeouts();
    }

    public override void OnEndTurn()
    {
        Debug.Log("Ending turn for AI team " + name);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        MapLoadedEvent.Get().AddListener(OnMapLoaded);
        CharacterEvents.characterDeathEvent.AddListener(OnCharacterDeath);
        LevelEvents.pauseLevelEvent.AddListener(OnPauseOrResume);
    }

    private void OnDestroy()
    {
        MapLoadedEvent.Get().RemoveListener(OnMapLoaded);
        CharacterEvents.characterDeathEvent.RemoveListener(OnCharacterDeath);
        LevelEvents.pauseLevelEvent.RemoveListener(OnPauseOrResume);
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldEndTurnNextUpdate)
        {
            TeamEvents.teamTurnEndedEvent.Invoke(this);
            shouldEndTurnNextUpdate = false;
        }
    }

    void OnMapLoaded(TileMap loadedMap) => map = loadedMap;

    CCharacter TryGetNextAvailableCharacter()
    {
        Debug.Log("AI Team trying to find next available character!");
        foreach (var character in characters)
        {
            if (character.currentActionPoints > 0)
            {
                return character;
            }
        }

        Debug.Log("No available character found!");
        return null;
    }

    void ProcessTurnWithTimeouts()
    {
        if (!AreAllCharactersOutOfActions())
        {
            if (!gameIsPaused)
            {
                var currentCharacter = TryGetNextAvailableCharacter();
                TakeActionForCharacter(currentCharacter);

                Invoke("ProcessTurnWithTimeouts", actionTimeout);
            }
        }
        else
        {
            shouldEndTurnNextUpdate = true;
        }
    }

    void TakeActionForCharacter(CCharacter character)
    {
        Debug.Log("Determining course of action for character " + character.name);
        
        if(character.CanTakeAction(ECharacterAction.ATTACK))
        {
            var tilesInRange = TileMapTools.GetTilesWithinMovementRange(map,
                character.occupyingTile, character.GetAttackRange());

            foreach(var tile in tilesInRange)
            {
                if(actionController.IsEnemyCharacterOnTile(tile))
                {
                    var characterToAttack = actionController.GetCharacterOnTile(tile);
                    character.Attack(characterToAttack);
                    CharacterEvents.aiActionTakenEvent.Invoke(character, ECharacterAction.ATTACK);
                    return;
                }
            }
        }
        
        if(character.CanTakeAction(ECharacterAction.MOVE))
        {
            var tilesInRange = TileMapTools.GetTilesWithinMovementRange(map, 
                character.occupyingTile, character.GetMovementPerAction());
            int randomlySelectedIndex = UnityEngine.Random.Range(0, tilesInRange.Count);
            while(actionController.IsAnyCharacterOnTile(tilesInRange[randomlySelectedIndex]))
            {
                if(tilesInRange.Count == 0)
                {
                    Debug.LogWarning("Character could not find a tile to MOVE to! Weird!");
                    character.ExhaustActionPoints();
                    return;
                }

                tilesInRange.RemoveAt(randomlySelectedIndex);
                randomlySelectedIndex = UnityEngine.Random.Range(0, tilesInRange.Count);
            }
            character.MoveTo(tilesInRange[randomlySelectedIndex]);
            CharacterEvents.aiActionTakenEvent.Invoke(character, ECharacterAction.MOVE);
            return;
        }
    }
    void OnCharacterDeath(CCharacter character)
    {
        if (characters.Contains(character))
        {
            characters.Remove(character);
            Debug.Log("Character " + character.name + " is dead!");
            Destroy(character.gameObject);

            if(characters.Count == 0)
            {
                TeamEvents.teamEliminatedEvent.Invoke(this);
            }
        }
    }

    void OnPauseOrResume(LevelEvents.PauseActions action)
    {
        gameIsPaused = action == LevelEvents.PauseActions.PAUSE ? true : false;
    }

    public override bool IsTeamAI()
    {
        return false;
    }

    bool shouldEndTurnNextUpdate = false;

    TileMap map;

    bool gameIsPaused = false;

    [SerializeField][Tooltip("The time the AI will wait before performing each action")]
    float actionTimeout = 1.5f;

    [SerializeField]
    CharacterActionController actionController;
}
