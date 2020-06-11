using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

//Attach this script to the bottom-left corner tile on the map, play, and press Enter to save the map
public class MapSaver : MonoBehaviour
{
    private static class XMLFields
    {
        public const string ROOT = "map";
        public const string ROW = "row";
        public const string CELL = "cell";
        public const string TILE = "tile";
        public const string ID = "id";
        public const string FLOOR = "floor";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SaveMap();
        }
    }

    bool SaveMap()
    {
        XElement xRoot = new XElement(XMLFields.ROOT);

        bool hasSavedSomething = false;
        float rowZ = gameObject.transform.position.z;

        while(SaveRow(rowZ, xRoot))
        {
            rowZ += tileWidth;
            hasSavedSomething = true;
        }

        if(!hasSavedSomething)
        {
            Debug.LogError("Failed to save map!");
            return false;
        }

        XDocument xMap = new XDocument(xRoot);
        xMap.Save("level.xml");
        Debug.Log("Level saved successfully!");
        return true;
    }

    bool SaveRow(float rowZ, XElement XMLRoot)
    {
        XElement xRow = new XElement(XMLFields.ROW);

        bool hasSavedSomething = false;
        float cellX = gameObject.transform.position.x;

        while(SaveCell(cellX, rowZ, xRow))
        {
            cellX += tileWidth;
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
        XElement xCell = new XElement(XMLFields.CELL);

        bool hasSavedSomething = false;
        Vector3 tilePosition = new Vector3(cellX, gameObject.transform.position.y, cellZ);
        int currentFloor = 0;

        while(SaveTile(tilePosition, currentFloor, xCell))
        {
            tilePosition.y += tileHeight;
            ++currentFloor;
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

    bool SaveTile(Vector3 position, int currentFloor, XElement XMLCell)
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

        XElement xTile = new XElement(XMLFields.TILE
            , new XElement(XMLFields.ID, currentTileObject.GetComponent<Tile>().GetId())
            , new XElement(XMLFields.FLOOR, currentFloor));
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

    [SerializeField]
    private float tileWidth = 1.0f;
    [SerializeField]
    private float tileHeight = 2.0f;

    private static GameObject[] allTileObjects = null;
}
