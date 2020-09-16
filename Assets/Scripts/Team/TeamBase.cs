using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

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

    public XElement GetTeamStateAsXML()
    {
        XElement teamStateXML = new XElement(XMLFields.Team);
        teamStateXML.SetAttributeValue(XMLFields.TeamName, gameObject.name);

        foreach(var character in characters)
        {
            XElement characterXML = GetCharacterStateAsXML(character);
            teamStateXML.Add(characterXML);
        }

        return teamStateXML;
    }

    public void LoadTeamStateFromXML()
    {

    }

    protected static class XMLFields
    {
        public const string Team = "team";
        public const string TeamName = "name";
        public const string Character = "character";
        public const string CharacterName = "c_name";
        public const string ActionPoints = "action_points";
        public const string HitPoints = "hit_points";
        public const string Position = "position";
        public const string PositionX = "x";
        public const string PositionY = "y";
        public const string PositionZ = "z";

    }

    XElement GetCharacterStateAsXML(CCharacter character)
    {
        XElement xCharacter = new XElement(XMLFields.Character,
            new XElement(XMLFields.CharacterName, character.name),
            new XElement(XMLFields.ActionPoints, character.currentActionPoints),
            new XElement(XMLFields.HitPoints, character.GetHitPoints()));

        Vector3 characterPosition = character.transform.position;
        XElement xPosition = new XElement(XMLFields.Position,
            new XElement(XMLFields.PositionX, characterPosition.x),
            new XElement(XMLFields.PositionY, characterPosition.y),
            new XElement(XMLFields.PositionZ, characterPosition.z));

        xCharacter.Add(xPosition);
        return xCharacter;
    }

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
