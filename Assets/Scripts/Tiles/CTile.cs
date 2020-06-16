﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTile : MonoBehaviour
{
    public int GetId() { return id; }

    // Start is called before the first frame update
    void Start()
    {
        if(!tileHighlighter)
        {
            tileHighlighter = GameObject.Find("TileHighlighter");
            tileHighlighterOriginalPosition = tileHighlighter.transform.position;
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

    [SerializeField]
    private int id;
    [SerializeField]
    private bool canMoveOn;
    [SerializeField]
    private int movementCost;

    private static GameObject tileHighlighter;
    private static Vector3 tileHighlighterOriginalPosition;
}
