using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class MapOperationsManager : MonoBehaviour
{
    public static class XMLFields
    {
        public const string ROOT = "map";
        public const string ROW = "row";
        public const string CELL = "cell";
        public const string TILE = "tile";
        public const string ID = "id";
        public const string Y_ROTATION = "yRotation";
    }

    public struct MapProperties
    {
        public float tileWidth;
        public float tileHeight;
        public Vector3 startingPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        mapLoadProperties.tileWidth = mapSaveProperties.tileWidth = tileWidth;
        mapLoadProperties.tileHeight = mapSaveProperties.tileHeight = tileHeight;

        mapLoadProperties.startingPosition = mapLoadStartingPosition;

        mapSaveProperties.startingPosition = mapSaveBaseTileTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadMap(levelSaveName + ".xml");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SaveMap(levelSaveName);
        }
    }

    public void LoadMap(string mapFile)
    {
        MapLoader mapLoader = new MapLoader(mapLoadProperties, GetAllTilePrefabs(), tileContainerTransform, baseGround);
        var virtualMap = mapLoader.LoadMap(mapFile);
        if(virtualMap != null)
        {
            Debug.Log("Map loaded successfully!");
        }
    }

    void SaveMap(string fileName)
    {
        MapSaver mapSaver = new MapSaver(mapSaveProperties);
        mapSaver.SaveMap(fileName);
    }

    GameObject[] GetAllTilePrefabs()
    {
        var tilePrefabs = Resources.LoadAll("Prefabs/Tiles", typeof(GameObject)).Cast<GameObject>().ToArray();
        return tilePrefabs;
    }

    [SerializeField]
    float tileWidth = 1.0f;
    [SerializeField]
    float tileHeight = 1.0f;

    [SerializeField]
    Vector3 mapLoadStartingPosition = Vector3.zero;
    [SerializeField][Tooltip("Instantiated tiles will be parented to this transform when loading a map.")]
    Transform tileContainerTransform;
    [SerializeField][Tooltip("GameObject to be generated as the base ground below each cell. Leave blank if no base ground should be generated.")]
    GameObject baseGround = null;

    [SerializeField][Tooltip("Name used when saving the level")]
    string levelSaveName = "level";
    [SerializeField][Tooltip("Set to bottom-left-most tile when saving a map.")]
    Transform mapSaveBaseTileTransform;

    MapProperties mapLoadProperties;
    MapProperties mapSaveProperties;
}
