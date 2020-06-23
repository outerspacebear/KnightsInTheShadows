using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTile : MonoBehaviour
{
    public int GetId() { return id; }

    public int GetMovementCost() { return movementCost; }

    public bool CanMoveOn() { return canMoveOn; }

    public void EnableMovementRangeHighlight(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void DisableMovementRangeHighlight()
    {
        meshRenderer.material.color = meshRendererOriginalColour;
    }

    private void Awake()
    {
        if(!tileHighlighter)
        {
            tileHighlighter = GameObject.Find("TileHighlighter");
            tileHighlighterOriginalPosition = tileHighlighter.transform.position;
        }

        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        meshRendererOriginalColour = meshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if(tileHighlighter && canMoveOn)
        {
            tileHighlighter.transform.position = gameObject.transform.position;
        }
    }

    private void OnMouseExit()
    {
        if(tileHighlighter && canMoveOn)
        {
            tileHighlighter.transform.position = tileHighlighterOriginalPosition;
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Tile " + gameObject.name + " has been clicked on!");
            TileClickedOnEvent.Get().Invoke(this);
        }
    }

    [SerializeField]
    private int id;
    [SerializeField]
    private bool canMoveOn;
    [SerializeField]
    private int movementCost;

    private static GameObject tileHighlighter;
    private static Vector3 tileHighlighterOriginalPosition;

    MeshRenderer meshRenderer;
    Color meshRendererOriginalColour;
}
