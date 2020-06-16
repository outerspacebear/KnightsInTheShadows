using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public TileMap(Map map) 
    {
        this.map = map;
    }

    Map map;
}
