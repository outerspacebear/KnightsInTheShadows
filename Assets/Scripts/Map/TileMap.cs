using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapProperties = MapOperationsManager.MapProperties;

public class TileMap
{
    public struct Cell
    {
        public List<CTile> tiles;
    }
    public struct Row
    {
        public List<Cell> cells;
    }
    public struct Map
    {
        public List<Row> rows;
    }

    public TileMap(Map map, MapProperties properties) 
    {
        this.map = map;
        mapProperties = properties;
    }

    public bool IsTileInMap(CTile tile)
    {
        foreach(Row row in map.rows)
        {
            foreach(Cell cell in row.cells)
            {
                if(cell.tiles.Contains(tile))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public Vector3Int GetTileIndices(CTile tile)
    {
        //Assumes the map always spawns from 0,0
        int rowZ = (int)(tile.transform.position.z / mapProperties.tileWidth);
        int cellX = (int)(tile.transform.position.x / mapProperties.tileWidth);
        int tileY = (int)(tile.transform.position.y / mapProperties.tileHeight);

        return new Vector3Int(cellX, tileY, rowZ);
    }

    public CTile TryGetTileAtIndices(Vector3 indices)
    {
        
        int zIndex = Mathf.RoundToInt(indices.z);
        int xIndex = Mathf.RoundToInt(indices.x);
        int yIndex = Mathf.RoundToInt(indices.y);

        if (indices.x < 0 || indices.y < 0 || indices.z < 0
            || indices.z >= map.rows.Count || indices.x >= map.rows[zIndex].cells.Count
            || indices.y >= map.rows[zIndex].cells[xIndex].tiles.Count)
        {
            return null;
        }

        return map.rows[zIndex].cells[xIndex].tiles[yIndex];
    }

    public CTile TryGetTileAt(Vector3 worldPosition)
    {
        foreach (Row row in map.rows)
        {
            foreach (Cell cell in row.cells)
            {
                foreach(CTile tile in cell.tiles)
                {
                    if(tile.transform.position == worldPosition)
                    {
                        return tile;
                    }
                }
            }
        }

        return null;
    }

    Map map;
    public MapProperties mapProperties { get; }
}
