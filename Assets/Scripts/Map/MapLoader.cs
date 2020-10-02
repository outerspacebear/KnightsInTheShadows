using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using MapProperties = MapOperationsManager.MapProperties;
using XMLFields = MapOperationsManager.XMLFields;
using UnityEngine.Assertions;
using System;
using UnityEngine.Tilemaps;
using System.Runtime.InteropServices.WindowsRuntime;

public class MapLoader
{
    public MapLoader(MapProperties properties, GameObject[] allTilePrefabs, Transform parentTransformForTiles = null, GameObject baseGround = null)
    {
        mapProperties = properties;
        tilePrefabs = allTilePrefabs;
        parentTransform = parentTransformForTiles;
        baseGroundPrefab = baseGround;
    }

    public TileMap LoadMap(string sourceFile)
    {
        TileMap.Map loadedMap = new TileMap.Map();
        loadedMap.rows = new List<TileMap.Row>();

        var mapText = Resources.Load<TextAsset>(sourceFile);
        XDocument xFile = XDocument.Parse(mapText.text);
        if (xFile == null)
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

        TileMap tileMap = new TileMap(loadedMap, mapProperties);

        return tileMap;
    }

    bool LoadRow(float rowZ, XElement xRow, TileMap.Map virtualMap)
    {
        TileMap.Row loadedRow = new TileMap.Row();
        loadedRow.cells = new List<TileMap.Cell>();

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

    bool LoadCell(float cellZ, float cellX, XElement xCell, TileMap.Row virtualRow)
    {
        TileMap.Cell loadedCell = new TileMap.Cell();
        loadedCell.tiles = new List<CTile>();

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

    bool LoadTile(Vector3 position, XElement xTile, TileMap.Cell virtualCell)
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

        string tileYRotationStr = xTile.Element(XMLFields.Y_ROTATION).Value;
        float tileYRotation;
        if(!float.TryParse(tileYRotationStr, out tileYRotation))
        {
            Debug.LogError("Tile rotation " + tileYRotationStr + " isn't a float!");
            return false;
        }
        Quaternion tileSpawnRotation = Quaternion.identity;
        tileSpawnRotation.eulerAngles = new Vector3(0.0f, tileYRotation, 0.0f);

        GameObject loadedTile = null;
        if (parentTransform != null)
        {
            loadedTile = UnityEngine.Object.Instantiate(tileObject, position, tileSpawnRotation, parentTransform);
        }
        else
        {
            loadedTile = UnityEngine.Object.Instantiate(tileObject, position, tileSpawnRotation);
        }

        if(!loadedTile)
        {
            Debug.Log("Couldn't instantiate prefab " + tileObject.name);
            return false;
        }

        virtualCell.tiles.Add(loadedTile.GetComponent<CTile>());
        return true;
    }

    GameObject GetTilePrefabForID(int tileID)
    {
        foreach(var tileObject in tilePrefabs)
        {
            CTile tileComponent = tileObject.GetComponent<CTile>();
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
