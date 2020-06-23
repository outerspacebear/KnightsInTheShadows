using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileMapTools
{ 
    public static List<CTile> GetTilesWithinMovementRange(TileMap map, CTile origin, int availableMovementPoints)
    {
        if (tilesCheckedForMovementCost == null)
        {
            tilesCheckedForMovementCost = new Dictionary<CTile, int>();
        }
        else
        {
            tilesCheckedForMovementCost.Clear();
        }

        var tiles = GetTilesInRange(map, origin, availableMovementPoints);
        return tiles;
    }

    static List<CTile> GetTilesInRange(TileMap map, CTile origin, int availableMovementPoints)
    {
        List<CTile> outputTiles = new List<CTile>();
        if(!outputTiles.Contains(origin))
        {
            outputTiles.Add(origin);
        }

        if (tilesCheckedForMovementCost.ContainsKey(origin))
        {
            if (tilesCheckedForMovementCost[origin] > availableMovementPoints)
            {
                Debug.LogWarning("Iterating over already visited tile with less movement points!");
                return outputTiles;
            }

            tilesCheckedForMovementCost[origin] = availableMovementPoints;
        }
        else
        {
            tilesCheckedForMovementCost.Add(origin, availableMovementPoints);
        }

        if (availableMovementPoints <= 0)
        {
            return outputTiles;
        }

        if (!map.IsTileInMap(origin))
        {
            Debug.LogError("Origin tile is not in map; cannot calculate tiles within movement range!");
            return outputTiles;
        }

        Vector3Int originTileIndices = map.GetTileIndices(origin);
        CTile adjacentTile = null;

        List<Vector3Int> adjacentTileIndices = new List<Vector3Int>();
        //Front, back, left, right
        adjacentTileIndices.Add(new Vector3Int(originTileIndices.x, originTileIndices.y, originTileIndices.z + 1));
        adjacentTileIndices.Add(new Vector3Int(originTileIndices.x, originTileIndices.y, originTileIndices.z - 1));
        adjacentTileIndices.Add(new Vector3Int(originTileIndices.x - 1, originTileIndices.y, originTileIndices.z));
        adjacentTileIndices.Add(new Vector3Int(originTileIndices.x + 1, originTileIndices.y, originTileIndices.z));

        foreach (var tileIndices in adjacentTileIndices)
        {
            if (adjacentTile = map.TryGetTileAt(new Vector3Int(tileIndices.x, tileIndices.y, tileIndices.z)))
            {
                bool canMoveTo = adjacentTile.CanMoveOn() && adjacentTile.GetMovementCost() <= availableMovementPoints;
                if (canMoveTo)
                {
                    int remainingMovementCost = availableMovementPoints - adjacentTile.GetMovementCost();

                    //The tile has already been checked
                    if (tilesCheckedForMovementCost.ContainsKey(adjacentTile))
                    {
                        if (remainingMovementCost > tilesCheckedForMovementCost[adjacentTile])
                        {
                            outputTiles.AddRange(GetTilesInRange(map, adjacentTile, remainingMovementCost));
                        }
                    }
                    else
                    {
                        outputTiles.AddRange(GetTilesInRange(map, adjacentTile, remainingMovementCost));
                    }
                }
            }
        }

        return outputTiles;
    }

    static Dictionary<CTile, int> tilesCheckedForMovementCost;
}
