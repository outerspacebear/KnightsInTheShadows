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

        foreach (var character in characters)
        {
            XElement characterXML = GetCharacterStateAsXML(character);
            teamStateXML.Add(characterXML);
        }

        return teamStateXML;
    }

    public void LoadTeamStateFromXML(XDocument xLoadFile)
    {
        XElement root = xLoadFile.Element(SaveManager.XMLFields.Header);
        XElement xThisTeam = null;

        foreach (var xTeam in root.Elements(XMLFields.Team))
        {
            if (xTeam.Attribute(XMLFields.TeamName).Value == this.name)
            {
                xThisTeam = xTeam;
                break;
            }
        }

        if (xThisTeam == null)
        {
            Debug.LogWarning("No saved state found for team " + name);
            return;
        }

        var xCharacters = xThisTeam.Elements(XMLFields.Character);
        List<CCharacter> deadCharacters = new List<CCharacter>();

        foreach (var character in characters)
        {
            bool foundSavedStateForCharacter = false;
            foreach(var xCharacter in xCharacters)
            {
                string xName = xCharacter.Element(XMLFields.CharacterName).Value; ;
                if(character.name == xName)
                {
                    LoadCharacterStateFromXML(character, xCharacter);
                    foundSavedStateForCharacter = true;
                    break;
                }
            }

            if(!foundSavedStateForCharacter)
            {
                deadCharacters.Add(character);
            }
        }

        foreach(var deadCharacter in deadCharacters)
        {
            characters.Remove(deadCharacter);
            Destroy(deadCharacter.gameObject);
        }

        loadedState = true;
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

    void LoadCharacterStateFromXML(CCharacter character, XElement xCharacter)
    {
        //Action points
        string actionPointsStr = xCharacter.Element(XMLFields.ActionPoints).Value;
        int actionPoints;

        if (!int.TryParse(actionPointsStr, out actionPoints))
        {
            Debug.LogError("Action points for character " + character.name + " not saved as a string!");
            return;
        }

        //Hit points
        string hitPointsStr = xCharacter.Element(XMLFields.HitPoints).Value;
        int hitPoints;

        if (!int.TryParse(hitPointsStr, out hitPoints))
        {
            Debug.LogError("Hit points for character " + character.name + " not saved as a string!");
            return;
        }

        //Position
        XElement xPosition = xCharacter.Element(XMLFields.Position);

        string posXStr = xPosition.Element(XMLFields.PositionX).Value;
        string posYStr = xPosition.Element(XMLFields.PositionY).Value;
        string posZStr = xPosition.Element(XMLFields.PositionZ).Value;
        int posX, posY, posZ;

        if (!int.TryParse(posXStr, out posX) || !int.TryParse(posYStr, out posY) || !int.TryParse(posZStr, out posZ))
        {
            Debug.LogError("Position of character " + character.name + " not saved as string!");
            return;
        }

        //Update character state
        character.currentActionPoints = actionPoints;
        character.SetHitPoints(hitPoints);
        Vector3 loadPosition = new Vector3(posX, posY, posZ);
        character.SetStartingPosition(loadPosition);
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
    public void Start()
    {
        TeamEvents.loadTeamStateFromXMLEvent.AddListener(LoadTeamStateFromXML);
    }

    private void OnDestroy()
    {
        TeamEvents.loadTeamStateFromXMLEvent.RemoveListener(LoadTeamStateFromXML);
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

    protected bool loadedState = false;
}
