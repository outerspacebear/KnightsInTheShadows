using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using MapProperties = MapOperationsManager.MapProperties;
using XMLFields = MapOperationsManager.XMLFields;
using UnityEngine.Assertions;
using System;
using UnityEngine.Tilemaps;

public class MapLoader
{
    public MapLoader(MapProperties properties, GameObject[] allTilePrefabs, Transform parentTransformForTiles = null, GameObject baseGround = null)
    {
        mapProperties = properties;
        tilePrefabs = allTilePrefabs;
        parentTransform = parentTransformForTiles;
        baseGroundPrefab = baseGround;
    }

    public VirtualMap LoadMap(string sourceFile)
    {
        VirtualMap.Map loadedMap = new VirtualMap.Map();
        loadedMap.rows = new List<VirtualMap.Row>();

        XDocument xFile = XDocument.Load(sourceFile);
        if(xFile == null)
        {
            Debug.LogError("No map file found at path " + sourceFile + "; Loading failed!");
            return null;
        }

        XElement xMap = xFile.Element(XMLFields.ROOT);
        var xRows = xMap.Elements(XMLFields.ROW);

        float rowZ = mapProperties.startingPosition.z;

        foreach(var xRow in xRows)
        {
            if(!LoadRow(rowZ, xRow, loadedMap))
            {
                Debug.LogError("Failed to load map! Couldn't load row with z = " + rowZ.ToString());
                return null;
            }
            //If successfully loaded row
            rowZ += mapProperties.tileWidth;
        }

        return new VirtualMap(loadedMap);
    }

    bool LoadRow(float rowZ, XElement xRow, VirtualMap.Map virtualMap)
    {
        VirtualMap.Row loadedRow = new VirtualMap.Row();
        loadedRow.cells = new List<VirtualMap.Cell>();

        var xCells = xRow.Elements(XMLFields.CELL);
        float cellX = mapProperties.startingPosition.x;

        foreach(var xCell in xCells)
        {
            if(!LoadCell(rowZ, cellX, xCell, loadedRow))
            {
                Debug.Log("Couldn't load cell with z = " + rowZ.ToString() + ", x = " + cellX.ToString());
                return false;
            }

            cellX += mapProperties.tileWidth;
        }

        virtualMap.rows.Add(loadedRow);
        return true;
    }

    bool LoadCell(float cellZ, float cellX, XElement xCell, VirtualMap.Row virtualRow)
    {
        VirtualMap.Cell loadedCell = new VirtualMap.Cell();
        loadedCell.tiles = new List<VirtualMap.Tile>();

        var xTiles = xCell.Elements(XMLFields.TILE);

        //Generate base ground below each cell, if enabled
        if (baseGroundPrefab)
        {
            UnityEngine.Object.Instantiate(baseGroundPrefab, new Vector3(cellX, mapProperties.startingPosition.y - mapProperties.tileHeight, cellZ)
                , Quaternion.identity);
        }

        float tileY = mapProperties.startingPosition.y;
        Vector3 tilePosition = new Vector3(cellX, tileY, cellZ);

        foreach (var xTile in xTiles)
        {
            if(!LoadTile(tilePosition, xTile, loadedCell))
            {
                Debug.Log("Couldn't load tile at " + tilePosition.ToString());
                return false;
            }

            tilePosition.y += mapProperties.tileHeight;
        }

        virtualRow.cells.Add(loadedCell);
        return true;
    }

    bool LoadTile(Vector3 position, XElement xTile, VirtualMap.Cell virtualCell)
    {
        VirtualMap.Tile loadedTile = new VirtualMap.Tile();

        string tileIDStr = xTile.Element(XMLFields.ID).Value;
        int tileID;
        if(!int.TryParse(tileIDStr, out tileID))
        {
            Debug.LogError("Tile ID \"" + tileIDStr + "\" is not an int!");
            return false;
        }
        loadedTile.ID = tileID;

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

        virtualCell.tiles.Add(loadedTile);
        return true;
    }

    GameObject GetTilePrefabForID(int tileID)
    {
        foreach(var tileObject in tilePrefabs)
        {
            TileProperties tileComponent = tileObject.GetComponent<TileProperties>();
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
    GameObject baseGroundPrefab;
}
