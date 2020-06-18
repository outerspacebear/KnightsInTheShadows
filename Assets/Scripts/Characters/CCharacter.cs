using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MapLoadedEvent.Get().AddListener(OnMapLoaded);

        currentActionPoints = baseActionPoints;

        if(!(playerHighlilghter = GameObject.Find("PlayerHighlighter")))
        {
            Debug.LogError("Coudln't find the player highlighter!");
            return;
        }
        playerHighlighterDefaultPosition = playerHighlilghter.transform.position;

        tilesInMovementRange = new List<CTile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Character " + gameObject.name + "clicked on!");
        CharacterClickedOnEvent.Get().Invoke(this);
    }

    public void OnSelected()
    {
        if(playerHighlilghter)
        {
            playerHighlilghter.transform.position = gameObject.transform.position;
        }

        UpdateTilesWithinMovementRange();

        foreach(var tile in tilesInMovementRange)
        {
            tile.EnableMovementRangeHighlight();
        }
    }

    public void OnDeselected()
    {
        if(playerHighlilghter)
        {
            playerHighlilghter.transform.position = playerHighlighterDefaultPosition;
        }

        foreach (var tile in tilesInMovementRange)
        {
            tile.DisableMovementRangeHighlight();
        }
    }

    public void MoveTo(CTile tile)
    {

    }

    void UpdateTilesWithinMovementRange()
    {
        tilesInMovementRange.Clear();
        tilesInMovementRange = TileMapTools.GetTilesWithinMovementRange(map, occupyingTile, movementPerAction);
    }

    void OnMapLoaded(TileMap map)
    {
        if(CCharacter.map == null)
        {
            CCharacter.map = map;
        }

        transform.position = startingPosition;

        occupyingTile = null;
        if (!(occupyingTile = map.TryGetTileAt(transform.position)))
        {
            Debug.LogError("Character " + gameObject.name + " is not placed on a vald tile");
            return;
        }
    }

    public int currentActionPoints { get; set; }
    public CTile occupyingTile { get; set; }
    public List<CTile> tilesInMovementRange { get; private set; }

    [SerializeField]
    private int movementPerAction;
    [SerializeField]
    private int baseActionPoints;
    [SerializeField]
    Vector3 startingPosition;

    static GameObject playerHighlilghter;
    static Vector3 playerHighlighterDefaultPosition;
    static TileMap map;
}
