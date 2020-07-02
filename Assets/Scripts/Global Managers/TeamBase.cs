using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeamBase : MonoBehaviour
{
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
}
