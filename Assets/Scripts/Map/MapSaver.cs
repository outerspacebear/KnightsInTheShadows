using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using MapProperties = MapOperationsManager.MapProperties;

public class MapSaver
{
    public MapSaver(MapProperties properties)
    {
        mapProperties = properties;
    }

    public bool SaveMap(string fileName)
    {
        XElement xRoot = new XElement(MapOperationsManager.XMLFields.ROOT);

        bool hasSavedSomething = false;
        float rowZ = mapProperties.startingPosition.z;

        while(SaveRow(rowZ, xRoot))
        {
            rowZ += mapProperties.tileWidth;
            hasSavedSomething = true;
        }

        if(!hasSavedSomething)
        {
            Debug.LogError("Failed to save map!");
            return false;
        }

        XDocument xMap = new XDocument(xRoot);
        xMap.Save(fileName + ".xml");
        Debug.Log("Level saved successfully!");
        return true;
    }

    bool SaveRow(float rowZ, XElement XMLRoot)
    {
        XElement xRow = new XElement(MapOperationsManager.XMLFields.ROW);

        bool hasSavedSomething = false;
        float cellX = mapProperties.startingPosition.x;

        while(SaveCell(cellX, rowZ, xRow))
        {
            cellX += mapProperties.tileWidth;
            hasSavedSomething = true;
        }

        if(!hasSavedSomething)
        {
            Debug.Log("Coudln't find row at z = " + rowZ.ToString());
            return false;
        }

        XMLRoot.Add(xRow);
        return true;
    }

    bool SaveCell(float cellX, float cellZ, XElement XMLRow)
    {
        XElement xCell = new XElement(MapOperationsManager.XMLFields.CELL);

        bool hasSavedSomething = false;
        Vector3 tilePosition = new Vector3(cellX, mapProperties.startingPosition.y, cellZ);

        while(SaveTile(tilePosition, xCell))
        {
            tilePosition.y += mapProperties.tileHeight;
            hasSavedSomething = true;
        }

        if (!hasSavedSomething)
        {
            Debug.Log("Couldn't find cell at " + cellX.ToString() + ", " + cellZ.ToString());
            return false;
        }

        XMLRow.Add(xCell);
        return true;
    }

    bool SaveTile(Vector3 position, XElement XMLCell)
    {
        if(allTileObjects == null)
        {
            allTileObjects = GameObject.FindGameObjectsWithTag("Tile");
        }

        GameObject currentTileObject = TryGetGameObjectAtPosition(allTileObjects, position);
        if(!currentTileObject)
        {
            Debug.Log("No tile found at " + position.ToString());
            return false;
        }

        XElement xTile = new XElement(MapOperationsManager.XMLFields.TILE
            , new XElement(MapOperationsManager.XMLFields.ID, currentTileObject.GetComponent<CTile>().GetId()));
        XMLCell.Add(xTile);

        return true;
    }

    GameObject TryGetGameObjectAtPosition(GameObject[] objects, Vector3 position)
    {
        foreach(GameObject gameObject in objects)
        {
            if(gameObject.transform.position == position)
            {
                return gameObject;
            }
        }
        return null;
    }

    MapProperties mapProperties;

    private static GameObject[] allTileObjects = null;
}
