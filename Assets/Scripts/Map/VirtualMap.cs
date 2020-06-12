using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMap
{
    public struct Tile
    {
        public int ID;
    }
    public struct Cell
    {
        public List<Tile> tiles;
    }
    public struct Row
    {
        public List<Cell> cells;
    }
    public struct Map
    {
        public List<Row> rows;
    }

    public VirtualMap(Map map) 
    {
        this.map = map;
    }

    Map map;
}
