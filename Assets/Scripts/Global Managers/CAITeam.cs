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
            character.ResetActionPoints();
        }

        if (AreAllCharactersOutOfActions())
        {
            shouldEndTurnNextUpdate = true;
        }

        TeamEvents.teamTurnStartedEvent.Invoke(this);

        ProcessTurn();
    }

    public override void OnEndTurn()
    {
        Debug.Log("Ending turn for AI team " + name);
    }

    // Start is called before the first frame update
    void Start()
    {
        MapLoadedEvent.Get().AddListener(OnMapLoaded);
    }

    private void OnDestroy()
    {
        MapLoadedEvent.Get().RemoveListener(OnMapLoaded);
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

    void ProcessTurn()
    {
        Debug.Log("AI team processing turn");
        while(!AreAllCharactersOutOfActions())
        {
            var currentCharacter = TryGetNextAvailableCharacter();
            TakeActionsForCharacter(currentCharacter);
        }

        shouldEndTurnNextUpdate = true;
    }

    void TakeActionsForCharacter(CCharacter character)
    {
        Debug.Log("Determining course of action for character " + character.name);
        while(character.currentActionPoints > 0)
        {
            //TODO: Make an actual system to do this properly
            if(character.CanTakeAction(ECharacterAction.MOVE))
            {
                var tilesInRange = TileMapTools.GetTilesWithinMovementRange(map, 
                    character.occupyingTile, character.GetMovementPerAction());
                int randomlySelectedIndex = UnityEngine.Random.Range(0, tilesInRange.Count);
                character.MoveTo(tilesInRange[randomlySelectedIndex]);
            }
        }
    }

    bool shouldEndTurnNextUpdate = false;

    TileMap map;
}
