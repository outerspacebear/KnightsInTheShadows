using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CTile : MonoBehaviour
{
    public enum EDirection { FRONT, BACK, LEFT, RIGHT};

    public bool CanMoveToFromTile(CTile tile)
    {
        //Doesn't have any walls
        if(wallDirections == null || wallDirections.Count == 0)
        {
            return true;
        }

        //Has one or more walls
        foreach(EDirection direction in wallDirections)
        {
            Vector3 directionVector = Vector3.zero;
            switch(direction)
            {
                case EDirection.FRONT:
                    directionVector = transform.forward;
                    break;
                case EDirection.BACK:
                    directionVector = -transform.forward;
                    break;
                case EDirection.RIGHT:
                    directionVector = transform.right;
                    break;
                case EDirection.LEFT:
                    directionVector = -transform.right;
                    break;
            }

            //The position of the tile on the other end of this wall
            Vector3 blockedTilePosition = transform.position + directionVector;
            if(tile.transform.position == blockedTilePosition)
            {
                return false;
            }
        }

        return true;
    }

    public int GetId() { return id; }

    public int GetMovementCost() { return movementCost; }

    public bool CanMoveOn() { return canMoveOn; }

    public void RemoveAllHighlights()
    {
        DisableMovementRangeHighlight();
    }

    public void EnableMovementRangeHighlight(Color color)
    {
        foreach(var renderer in meshRenderers)
        {
            renderer.Key.material.color = color;
        }
    }

    public void DisableMovementRangeHighlight()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.Key.material.color = renderer.Value;
        }
    }

    private void Awake()
    {
        if(!tileHighlighter)
        {
            tileHighlighter = GameObject.Find("TileHighlighter");
            tileHighlighterOriginalPosition = tileHighlighter.transform.position;
        }

        var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach(var renderer in renderers)
        {
            meshRenderers.Add(renderer, renderer.material.color);
        }
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
            TileEvents.tileClickedOnEvent.Invoke(this);
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

    Dictionary<MeshRenderer, Color> meshRenderers = new Dictionary<MeshRenderer, Color>();

    [SerializeField][Tooltip("Directions in which the tile has a wall (to block movement)")]
    List<EDirection> wallDirections = null;
}
