using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!startingTile)
        {
            Debug.LogError("No starting tile specified for character " + gameObject.name);
            return;
        }

        gameObject.transform.position = startingTile.gameObject.transform.position;
        occupyingTile = startingTile;

        currentActionPoints = baseActionPoints;

        if(!(playerHighlilghter = GameObject.Find("PlayerHighlighter")))
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
        if(playerHighlilghter)
        {
            playerHighlilghter.transform.position = gameObject.transform.position;
        }
    }

    public void OnDeselected()
    {
        if(playerHighlilghter)
        {
            playerHighlilghter.transform.position = playerHighlighterDefaultPosition;
        }
    }

    public int currentActionPoints { get; set; }
    public CTile occupyingTile { get; set; }

    [SerializeField]
    private int movementPerAction;
    [SerializeField]
    private int baseActionPoints;
    [SerializeField]
    private CTile startingTile;

    static GameObject playerHighlilghter;
    static Vector3 playerHighlighterDefaultPosition;
}
