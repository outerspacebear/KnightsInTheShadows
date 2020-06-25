﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CCharacter : MonoBehaviour
{
    public ECharacterActions[] availableActions { get; } = { ECharacterActions.MOVE };

    public void ResetActionPoints()
    {
        currentActionPoints = baseActionPoints;
    }

    public bool CanTakeAction(ECharacterActions action)
    {
        if(!availableActions.Contains(action))
        {
            //This character class cannot perform this action
            return false;
        }

        if(currentActionPoints >= CharacterActions.actionCostMap[action])
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

        Debug.Log("Character " + name + " selected!");
        CharacterEvents.characterSelectedEvent.Invoke(this);
    }

    public void OnDeselected()
    {
        if (playerHighlilghter)
        {
            playerHighlilghter.transform.position = playerHighlighterDefaultPosition;
        }

        Debug.Log("Character " + name + " de-selected!");
        CharacterEvents.characterDeselectedEvent.Invoke(this);
    }

    public void MoveTo(CTile tile)
    {
        transform.position = tile.transform.position;
        occupyingTile = tile;
        currentActionPoints -= CharacterActions.actionCostMap[ECharacterActions.MOVE];

        Debug.Log("Character " + name + " moved to " + tile.transform.position.ToString());
        CharacterEvents.actionTakenEvent.Invoke(this, ECharacterActions.MOVE);
    }

    public int GetMovementPerAction() => movementPerAction;

    void OnMapLoaded(TileMap map)
    {
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
    [SerializeField]
    public Color tileHighlightColor = Color.cyan;

    [SerializeField]
    int movementPerAction;
    [SerializeField]
    private int baseActionPoints;
    [SerializeField]
    Vector3 startingPosition;

    static GameObject playerHighlilghter;
    static Vector3 playerHighlighterDefaultPosition;
}
