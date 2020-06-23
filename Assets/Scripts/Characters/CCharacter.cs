using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CCharacter : MonoBehaviour
{
    public enum EActions
    {
        MOVE
    }

    public static Dictionary<EActions, int> actionCostMap = new Dictionary<EActions, int>() { { EActions.MOVE, 1 } };

    public void ResetActionPoints()
    {
        currentActionPoints = baseActionPoints;
    }

    public bool CanTakeAction(EActions action)
    {
        if(currentActionPoints >= actionCostMap[action])
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        MapLoadedEvent.Get().AddListener(OnMapLoaded);

        currentActionPoints = baseActionPoints;

        if (!(playerHighlilghter = GameObject.Find("PlayerHighlighter")))
        {
            Debug.LogError("Coudln't find the player highlighter!");
            return;
        }
        playerHighlighterDefaultPosition = playerHighlilghter.transform.position;
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
        if (playerHighlilghter)
        {
            playerHighlilghter.transform.position = gameObject.transform.position;
        }

        UpdateTilesWithinMovementRange();

        foreach (var tile in tilesInMovementRange)
        {
            tile.EnableMovementRangeHighlight(tileHighlightColor);
        }

        Debug.Log("Character " + name + " selected!");
        CharacterSelectedEvent.Get().Invoke(this);
    }

    public void OnDeselected()
    {
        if (playerHighlilghter)
        {
            playerHighlilghter.transform.position = playerHighlighterDefaultPosition;
        }

        foreach (var tile in tilesInMovementRange)
        {
            tile.DisableMovementRangeHighlight();
        }

        Debug.Log("Character " + name + " de-selected!");
    }

    public void MoveTo(CTile tile)
    {
        transform.position = tile.transform.position;
        occupyingTile = tile;
        currentActionPoints -= actionCostMap[EActions.MOVE];

        Debug.Log("Character " + name + " moved to " + tile.transform.position.ToString());
        CharacterEvents.actionTakenEvent.Invoke(this, EActions.MOVE);
    }

    void UpdateTilesWithinMovementRange()
    {
        tilesInMovementRange.Clear();
        tilesInMovementRange = TileMapTools.GetTilesWithinMovementRange(map, occupyingTile, movementPerAction);
    }

    void OnMapLoaded(TileMap map)
    {
        if (CCharacter.map == null)
        {
            CCharacter.map = map;
        }

        transform.position = startingPosition;

        occupyingTile = null;
        if (!(occupyingTile = map.TryGetTileAt(transform.position)))
        {
            Debug.LogError("Character " + gameObject.name + " is not placed on a valid tile");
            return;
        }
    }

    public int currentActionPoints { get; set; }
    public CTile occupyingTile { get; set; }
    public List<CTile> tilesInMovementRange { get; private set; } = new List<CTile>();
    [SerializeField]
    Color tileHighlightColor = Color.cyan;

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
