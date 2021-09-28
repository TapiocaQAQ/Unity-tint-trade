using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    static bool[,] mapDirty;
    public GameObject[,] terrainObject;
    private readonly int MAP_WIDTH = Setting.MAP_WIDTH;
    private readonly int MAP_HEIGHT = Setting.MAP_HEIGHT;

    public enum Terrain 
    {
        plain,
        sea,
        mountain,
        forest
    }

    public static Terrain[,] MapTerrain = new Terrain[Setting.MAP_WIDTH, Setting.MAP_HEIGHT];

    // Start is called before the first frame update
    void Start()
    {
        terrainObject = new GameObject[MAP_WIDTH, MAP_HEIGHT];
        mapDirty = new bool[MAP_WIDTH, MAP_HEIGHT];

        if (DataTransport.hasNoMap)
        {
            MapTerrain = GenerateNewMap();
            MapObjectControl.SetupBuildings();
            DataTransport.hasNoMap = false;
        }

        for (int x = 0; x < MAP_WIDTH; x++) for (int y = 0; y < MAP_HEIGHT; y++)
            {
                switch (MapTerrain[x,y])
                {
                    case Terrain.sea:
                        terrainObject[x, y] = Instantiate(AssetLoader.tile_sea, new Vector3(x, y), new Quaternion());
                        break;
                    case Terrain.plain:
                        terrainObject[x, y] = Instantiate(AssetLoader.tile_plane, new Vector3(x, y), new Quaternion());
                        break;
                    case Terrain.forest:
                        terrainObject[x, y] = Instantiate(AssetLoader.tile_forest, new Vector3(x, y), new Quaternion());
                        break;
                    case Terrain.mountain:
                        terrainObject[x, y] = Instantiate(AssetLoader.tile_mountain, new Vector3(x, y), new Quaternion());
                        break;
                }
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Terrain[,] GenerateNewMap()
    {
        int[,] heightArray = new int[MAP_WIDTH, MAP_HEIGHT];
        Terrain[,] newMap = new Terrain[MAP_WIDTH, MAP_HEIGHT];
        float mapRng = Random.Range(0.0f, 1.0f);

        for (int x = 0; x < MAP_WIDTH; x++) for (int y = 0; y < MAP_HEIGHT; y++)
            {
                heightArray[x, y] = Mathf.FloorToInt(Mathf.PerlinNoise(x * 0.1f + mapRng, y * 0.1f + mapRng) * 100);
                if (x < 3 || x > MAP_WIDTH - 3 || y < 3 || y > MAP_HEIGHT - 3) heightArray[x, y] /= 2;
            }

        for (int x = 0; x < MAP_WIDTH; x++) for (int y = 0; y < MAP_HEIGHT; y++)
            {
                if (heightArray[x, y] < 30)
                {
                    newMap[x, y] = Terrain.sea;
                }
                else if (heightArray[x, y] < 60)
                {
                    newMap[x, y] = Terrain.plain;
                }
                else if (heightArray[x, y] < 75)
                {
                    newMap[x, y] = Terrain.forest;
                }
                else
                {
                    newMap[x, y] = Terrain.mountain;
                }
                mapDirty[x, y] = false;
            }

        return newMap;
    }
}
