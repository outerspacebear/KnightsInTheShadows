using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeamBase : MonoBehaviour
{
    public abstract bool IsTeamAI();

    public void InitialiseForSinglePlayerGame()
    {
        localPlayerNumber = teamNumber;
    }

    public void InitialiseForMultiplayerGame(int playerNumber)
    {
        localPlayerNumber = playerNumber;
        isMultiplayerGame = true;
    }

    public int GetLocalPlayerNumber() => localPlayerNumber;

    public int GetTeamNumber() => teamNumber;

    public bool IsAnyCharacterOnTile(CTile tile)
    {
        foreach (var character in characters)
        {
            if (character.occupyingTile == tile)
            {
                return true;
            }
        }

        return false;
    }

    public CCharacter GetCharacterOnTile(CTile tile)
    {
        foreach (var character in characters)
        {
            if (character.occupyingTile == tile)
            {
                return character;
            }
        }

        return null;
    }

    public abstract void BeginTurn();
    public abstract void OnEndTurn();

    protected bool AreAllCharactersOutOfActions()
    {
        foreach (var character in characters)
        {
            if (character.currentActionPoints > 0)
            {
                return false;
            }
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField]
    protected List<CCharacter> characters;

    [SerializeField]
    protected int teamNumber = 1;
    protected int localPlayerNumber;
    protected bool isMultiplayerGame = false;
}
