using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectControl : MonoBehaviour
{
    private static GameObject[,] mapObject;
    public static Building[,] mapBuilding;
    //public static 

    public enum BuildingType
    {
        none,
        village,
        farm,
        sawmill,
        mine,
        tint
    }

    public class Building
    {
        public BuildingType type;
        public Faction faction;
        public int x, y;
        public List<Item> itemList = new List<Item>();
        public List<MapUnitControl.UnitGroup> groupList = new List<MapUnitControl.UnitGroup>();
        public int development = 0;

        public Building(Faction faction, BuildingType type, int x, int y)
        {
            this.type = type;
            this.faction = faction;
            this.x = x;
            this.y = y;
            faction.buildingList.Add(this);
        }

        public void Process()
        {
            int Rng = Random.Range(0, 100) + 1;
            switch (type)
            {
                case BuildingType.village:
                    ModifyItem(new Item(Item.Type.money, development/5 + 1));
                    if(GetItem(Item.Type.money) != null)
                    {
                        if(GetItem(Item.Type.money).count >= development)
                        {
                            ModifyItem(new Item(Item.Type.money, -development));
                            development++;
                        }
                        if (CountOf(Item.Type.money) >= Rng*2)
                        {
                            ModifyItem(new Item(Item.Type.money, -Rng*2));
                            MapUnitControl.SpawnNewGroup(FactionControl.neutralFaction, this, MapUnitControl.UnitType.guard, x, y, Rng);
                        }
                    }
                    break;
                case BuildingType.tint:
                    ModifyItem(new Item(Item.Type.nutrient, development/5 + 1));
                    if(GetItem(Item.Type.nutrient) != null)
                    {
                        if (GetItem(Item.Type.nutrient).count >= development)
                        {
                            ModifyItem(new Item(Item.Type.nutrient, -development));
                            development++;
                        }
                        if (CountOf(Item.Type.nutrient) >= Rng)
                        {
                            ModifyItem(new Item(Item.Type.nutrient, -Rng));
                            MapUnitControl.SpawnNewGroup(FactionControl.monsterFaction, this, MapUnitControl.UnitType.slime, x, y, Rng);
                        }
                    }
                    break;
            }
        }

        public Item GetItem(Item.Type itemType)
        {
            foreach (Item item in itemList) if (item.type == itemType) return item;
            return null;
        }

        public int CountOf(Item.Type itemType)
        {
            foreach (Item item in itemList) if (item.type == itemType) return item.count;
            return 0;
        }

        public void ModifyItem(Item newItem)
        {
            Item item = GetItem(newItem.type);

            if(item == null)
            {
                item = new Item(newItem.type, newItem.count);
                itemList.Add(item);
            }
            else
            {
                item.count += newItem.count;
            }

            if (item.count == 0)
            {
                itemList.Remove(item);
            }
        }

        public void Trade(MapUnitControl.UnitGroup group, Item fromMarket, Item toMarket)
        {
            ModifyItem(new Item(fromMarket.type, -fromMarket.count));
            group.ModifyItem(fromMarket);
            group.ModifyItem(new Item(toMarket.type, -toMarket.count));
            ModifyItem(toMarket);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (mapBuilding != null)
        {
            for (int x = 0; x < Setting.MAP_WIDTH; x++) for (int y = 0; y < Setting.MAP_HEIGHT; y++)
                {
                    if(mapBuilding[x, y]!=null)
                    switch (mapBuilding[x, y].type)
                    {
                        case BuildingType.village:
                            mapObject[x,y] = Instantiate(AssetLoader.village, new Vector3(x, y), new Quaternion());
                            break;
                        case BuildingType.tint:
                            mapObject[x, y] = Instantiate(AssetLoader.tint, new Vector3(x, y), new Quaternion());
                            break;
                    }
                }
            
            //ToDo save cookie man
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetupBuildings()
    {
        bool hasFirstVillage = false;
        for(float bias = 0; bias <= 0.5; bias += 0.05f) //build first village from center
        {
            int x= (int)(Setting.MAP_WIDTH * (0.5 + Random.Range(-bias, bias))), y= (int)(Setting.MAP_HEIGHT * (0.5 + Random.Range(-bias, bias)));
            if (MapControl.MapTerrain[x, y] == MapControl.Terrain.plain)
            {
                FirstVillage(x, y);
                hasFirstVillage = true;
                break;
            }

        }

        while (!hasFirstVillage) //build first village from in random
        {
            int x = Random.Range(0, Setting.MAP_WIDTH), y = Random.Range(0, Setting.MAP_HEIGHT);
            if (MapControl.MapTerrain[x, y] == MapControl.Terrain.plain)
            {
                FirstVillage(x, y);
                hasFirstVillage = true;
            }
            
        }

        int village = 0;
        while(village<Setting.START_VILLAGE_NUM) //build other villages
        {
            int x = Random.Range(0, Setting.MAP_WIDTH); int y = Random.Range(0, Setting.MAP_HEIGHT);
            if (MapControl.MapTerrain[x, y] == MapControl.Terrain.plain && mapObject[x,y] == null)
            {
                NewBuilding(FactionControl.neutralFaction, BuildingType.village, x, y);
                village++;
            }
        }

        int tint = 0;
        while (tint < Setting.START_TINT_NUM) //make random tint
        {
            int x = Random.Range(0, Setting.MAP_WIDTH); int y = Random.Range(0, Setting.MAP_HEIGHT);
            if (mapObject[x, y] == null && MapControl.MapTerrain[x,y] != MapControl.Terrain.sea)
            {
                NewBuilding(FactionControl.monsterFaction, BuildingType.tint, x, y);
                tint++;
            }
        }
    }
    

    private static void FirstVillage(int x, int y)
    {
        NewBuildingMap();
        NewBuilding(FactionControl.neutralFaction, BuildingType.village, x, y);
        DataTransport.Respawn_X = x;
        DataTransport.Respawn_Y = y;
        //spawn player
        MapUnitControl.SpawnNewGroup(FactionControl.playerFaction, null, MapUnitControl.UnitType.player, x, y);
        ////spawn test unit
        //MapUnitControl.GetPlayerGroup().modifyUnit(new MapUnitControl.Units(MapUnitControl.UnitType.guard, 5));
        ////MapUnitControl.SpawnNewGroup(FactionControl.monsterFaction, null, MapUnitControl.UnitType.slime, x+1, y, 8);
        //MapUnitControl.SpawnNewGroup(FactionControl.neutralFaction, mapBuilding[x, y], MapUnitControl.UnitType.guard, x, y, 1);
        GameObject.Find("Main Camera").transform.position = new Vector3(x, y, -10);
    }

    private static void NewBuilding(Faction faction, BuildingType type, int x, int y)
    {
        switch (type)
        {
            case BuildingType.village:
                mapObject[x, y] = Instantiate(AssetLoader.village, new Vector3(x, y), new Quaternion());
                break;
            case BuildingType.tint:
                mapObject[x, y] = Instantiate(AssetLoader.tint, new Vector3(x, y), new Quaternion());
                break;
        }
        mapBuilding[x, y] = new Building(faction, type, x, y);
    }

    private static void NewBuildingMap()
    {
        if (mapObject == null)
        {
            mapObject = new GameObject[Setting.MAP_WIDTH, Setting.MAP_HEIGHT];
            mapBuilding = new Building[Setting.MAP_WIDTH, Setting.MAP_HEIGHT];
        }
    }
}
