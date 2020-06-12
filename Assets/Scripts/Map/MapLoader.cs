using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using MapProperties = MapOperationsManager.MapProperties;
using XMLFields = MapOperationsManager.XMLFields;
using UnityEngine.Assertions;
using System;

public class MapLoader
{
    public MapLoader(MapProperties properties, GameObject[] allTilePrefabs, Transform parentTransformForTiles = null)
    {
        mapProperties = properties;
        tilePrefabs = allTilePrefabs;
        parentTransform = parentTransformForTiles;
    }

    public bool LoadMap(string sourceFile)
    {
        XDocument xFile = XDocument.Load(sourceFile);
        if(xFile == null)
        {
            Debug.LogError("No map file found at path " + sourceFile + "; Loading failed!");
            return false;
        }

        XElement xMap = xFile.Element(XMLFields.ROOT);
        var xRows = xMap.Elements(XMLFields.ROW);

        float rowZ = mapProperties.startingPosition.z;

        foreach(var xRow in xRows)
        {
            if(!LoadRow(rowZ, xRow))
            {
                Debug.LogError("Failed to load map! Couldn't load row with z = " + rowZ.ToString());
                return false;
            }
            //If successfully loaded row
            rowZ += mapProperties.tileWidth;
        }

        return true;
    }

    bool LoadRow(float rowZ, XElement xRow)
    {
        var xCells = xRow.Elements(XMLFields.CELL);
        float cellX = mapProperties.startingPosition.x;

        foreach(var xCell in xCells)
        {
            if(!LoadCell(rowZ, cellX, xCell))
            {
                Debug.Log("Couldn't load cell with z = " + rowZ.ToString() + ", x = " + cellX.ToString());
                return false;
            }

            cellX += mapProperties.tileWidth;
        }

        return true;
    }

    bool LoadCell(float cellZ, float cellX, XElement xCell)
    {
        var xTiles = xCell.Elements(XMLFields.TILE);

        float tileY = mapProperties.startingPosition.y;
        Vector3 tilePosition = new Vector3(cellX, tileY, cellZ);

        foreach(var xTile in xTiles)
        {
            if(!LoadTile(tilePosition, xTile))
            {
                Debug.Log("Couldn't load tile at " + tilePosition.ToString());
                return false;
            }

            tilePosition.y += mapProperties.tileHeight;
        }

        return true;
    }

    bool LoadTile(Vector3 position, XElement xTile)
    {
        string tileIDStr = xTile.Element(XMLFields.ID).Value;
        int tileID;
        if(!int.TryParse(tileIDStr, out tileID))
        {
            Debug.LogError("Tile ID \"" + tileIDStr + "\" is not an int!");
            return false;
        }

        GameObject tileObject = GetTilePrefabForID(tileID);
        if(tileObject == null)
        {
            Debug.Log("Couldn't find tile prefab with ID " + tileIDStr);
            return false;
        }

        if (parentTransform != null)
        {
            UnityEngine.Object.Instantiate(tileObject, position, Quaternion.identity, parentTransform);
        }
        else
        {
            UnityEngine.Object.Instantiate(tileObject, position, Quaternion.identity);
        }

        return true;
    }

    GameObject GetTilePrefabForID(int tileID)
    {
        foreach(var tileObject in tilePrefabs)
        {
            Tile tileComponent = tileObject.GetComponent<Tile>();
            if(tileComponent == null)
            {
                Debug.LogError("No Tile component attached to prefab " + tileObject.name);
                return null;
            }

            if(tileComponent.GetId() == tileID)
            {
                return tileObject;
            }
        }

        Debug.LogError("No tile prefab with ID " + tileID.ToString() + " found!");
        return null;
    }

    MapProperties mapProperties;
    GameObject[] tilePrefabs;
    Transform parentTransform;
}
