using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapUnitControl : MonoBehaviour
{
    public static UnitTile[,] TileMap;
    private static List<Movement> movementsList;
    public enum UnitType
    {
        none,
        player,
        guard,
        slime
    }

    // Start is called before the first frame update
    void Start()
    {

        if (TileMap == null)
        {
            TileMap = new UnitTile[Setting.MAP_WIDTH, Setting.MAP_HEIGHT];
        }

        if (DataTransport.hasUnit) //load unit
        {
            //BgmContral.MapBGM.UnPause();
            for (int x = 0; x < Setting.MAP_WIDTH; x++) for (int y = 0; y < Setting.MAP_HEIGHT; y++)
                {
                    if (TileMap[x, y] != null && TileMap[x, y].groupList.Count > 0)
                    {
                        for (int i = 0; i < TileMap[x, y].groupList.Count; i++)
                        {
                            UnitGroup group = TileMap[x, y].groupList[i];
                            if (group.unitsList.Count > 0)
                            {
                                switch (group.type)
                                {
                                    case UnitType.player:
                                        group.avatar = Instantiate(AssetLoader.CookieMan, new Vector3(x, y), new Quaternion());
                                        CamMove.MoveCam(x, y);
                                        break;
                                    case UnitType.slime:
                                        group.avatar = Instantiate(AssetLoader.Slime, new Vector3(x, y), new Quaternion());
                                        break;
                                    case UnitType.guard:
                                        group.avatar = Instantiate(AssetLoader.Guard, new Vector3(x, y), new Quaternion());
                                        break;
                                }
                            }
                            else
                            {
                                throw new Exception("no unit in group");
                            }
                        }
                    }
                }
        }
        DataTransport.hasUnit = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class UnitTile
    {
        public List<UnitGroup> groupList;

        public UnitTile()
        {
            this.groupList = new List<UnitGroup>();
        }

        public int hasType(UnitType type)
        {
            int count = 0;
            foreach(UnitGroup group in groupList)
            {
                if (group.type == type) count++;
            }
            return count;
        }
    }

    public class UnitGroup
    {
        public int x, y;
        public List<Units> unitsList;
        public GameObject avatar;
        public UnitType type;
        public readonly Faction faction;
        public MapObjectControl.Building home;
        public List<Item> inventory = new List<Item>();

        public UnitGroup(int x, int y, Faction faction, MapObjectControl.Building home, UnitType avatarType, GameObject avatar, int count = 1)
        {
            this.x = x;
            this.y = y;
            this.unitsList = new List<Units>();
            while(count-- > 0)
            {
                unitsList.Add(new Units(avatarType));
            }
            this.faction = faction;
            faction.GroupList.Add(this);
            this.type = avatarType;
            this.avatar = avatar;
            this.home = home;
            if (home != null) home.groupList.Add(this);
        }

        public void Remove()
        {
            faction.GroupList.Remove(this);
            if (TileMap[x, y] == null) throw new Exception("TileMap doesn't exist");
            TileMap[x, y].groupList.Remove(this);
            if (home != null) home.groupList.Remove(this);
        }

        public bool Have(UnitType type)
        {
            foreach(Units unit in unitsList)
            {
                if (unit.type == type) return true;
            }
            return false;
        }

        public int CountOf(UnitType type)
        {
            foreach (Units unit in unitsList)
            {
                if (unit.type == type) return unit.count;
            }
            return 0;
        }


        public int getTotalCount()
        {
            int count = 0;
            foreach (Units unit in unitsList)
            {
                count += unit.count;
            }
            return count;
        }

        public Units GetUnit(UnitType type)
        {
            foreach (Units unit in unitsList) if (unit.type == type) return unit;
            return null;
        }

        public void modifyUnit(Units newUnits)
        {
            Units units = GetUnit(newUnits.type);

            if (units == null)
            {
                units = new Units(newUnits.type, newUnits.count);
                unitsList.Add(units);
            }
            else
            {
                units.count += newUnits.count;
            }

            if (units.count == 0)
            {
                unitsList.Remove(units);
            }
            else if (units.count < 0) throw new IndexOutOfRangeException();

            if (unitsList.Count == 0) Remove();
        }

        public Item GetItem(Item.Type type)
        {
            foreach (Item item in inventory)
            {
                if (item.type == type) return item;
            }
            return null;
        }

        public int CountOf(Item.Type type)
        {
            foreach(Item item in inventory)
            {
                if (item.type == type) return item.count;
            }
            return 0;
        }

        public void ModifyItem(Item item)
        {
            for(int i=0; i<inventory.Count; i++)
            {
                if (inventory[i].type == item.type)
                {
                    inventory[i].count += item.count;
                    return;
                }
            }
            inventory.Add(item);
        }

        public Vector2Int findClosest(Vector2Int selfPos, UnitType type, int range)
        {
            int x = selfPos.x, y = selfPos.y;
            for(int i=0; i<=range; i++) 
            { 
                for (int j = -i; j < i; j++)
                {
                    if (x - i >= 0 && y + j >= 0 && y + j < Setting.MAP_HEIGHT)
                    {
                        if (TileMap[x - i, y + j] != null && TileMap[x - i, y + j].hasType(type) > 0) 
                            return new Vector2Int(x - i, y + j);
                    }
                    if (x + i < Setting.MAP_WIDTH && y - j >= 0 && y - j < Setting.MAP_HEIGHT)
                    {
                        if (TileMap[x + i, y - j] != null && TileMap[x + i, y - j].hasType(type) > 0)
                            return new Vector2Int(x + i, y - j);
                    }
                    if (y + i < Setting.MAP_HEIGHT && x + j >= 0 && x + j < Setting.MAP_WIDTH)
                    {
                        if (TileMap[x + j, y + i] != null && TileMap[x + j, y + i].hasType(type) > 0)
                            return new Vector2Int(x + j, y + i);
                    }
                    if (y - i >= 0 && x - j >= 0 && x - j < Setting.MAP_WIDTH)
                    {
                        if (TileMap[x - j, y - i] != null && TileMap[x - j, y - i].hasType(type) > 0)
                            return new Vector2Int(x - j, y - i);
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }
    }

    public class Units
    {
        public UnitType type;
        public int count;

        public Units(UnitType type, int count = 1)
        {
            this.type = type;
            this.count = count;
        }

    }


    public class Movement
    {
        public UnitGroup group;
        public int oldX, oldY, newX, newY;

        public Movement(UnitGroup group, int oldX, int oldY, int newX, int newY)
        {
            this.group = group;
            this.oldX = oldX;
            this.oldY = oldY;
            this.newX = newX;
            this.newY = newY;
            if (newX < 0 || newX >= Setting.MAP_WIDTH) this.newX = oldX;
            if (newY < 0 || newY >= Setting.MAP_WIDTH) this.newY = oldY;
        }
    }

    private static void addGroup(UnitGroup group, int new_x, int new_y)
    {
        if (TileMap[new_x, new_y] == null) TileMap[new_x, new_y] = new UnitTile();
        TileMap[new_x, new_y].groupList.Add(group);
    }

    public static void deleteGroup(UnitGroup group, int x, int y)
    {
        TileMap[x, y].groupList.Remove(group);
        if (TileMap[x, y].groupList.Count == 0) TileMap[x, y] = null;
    }

    public static void MoveGroup(UnitGroup group, int old_x, int old_y, int new_x, int new_y)
    {
        if (TileMap[old_x, old_y] == null) throw new NullReferenceException();
        addGroup(group, new_x, new_y);
        deleteGroup(group, old_x, old_y);
        group.x = new_x;
        group.y = new_y;
    }

    public static void SpawnNewGroup(Faction faction, MapObjectControl.Building building, UnitType type, int x, int y, int count=1)
    {
        if(TileMap == null)
        {
            TileMap = new UnitTile[Setting.MAP_WIDTH, Setting.MAP_HEIGHT];
        }
        if(TileMap[x, y] == null)
        {
            TileMap[x, y] = new UnitTile();
        }
        switch (type)
        {
            case UnitType.player:
                GameObject avatar;
                avatar = Instantiate(AssetLoader.CookieMan, new Vector3(x,y), new Quaternion());
                addGroup(new UnitGroup(x, y, faction, building, type, avatar, count), x, y);
                break;
            case UnitType.slime:
                avatar = Instantiate(AssetLoader.Slime, new Vector3(x, y), new Quaternion());
                addGroup(new UnitGroup(x, y, faction, building, type, avatar, count), x, y);
                break;
            case UnitType.guard:
                avatar = Instantiate(AssetLoader.Guard, new Vector3(x, y), new Quaternion());
                addGroup(new UnitGroup(x, y, faction, building, type, avatar, count), x, y);
                break;
        }
    }

    public static UnitGroup GetPlayerGroup()
    {
        foreach(UnitTile tile in TileMap)
        {
            if(tile != null) foreach (UnitGroup group in tile.groupList)
                {
                    if (group.Have(UnitType.player)) return group;
                }
        }
        return null;
    }

    public static void addMovement(Movement newMovement)
    {
        if (movementsList == null) movementsList = new List<Movement>();
        movementsList.Add(newMovement);
    }

    public static bool CheckBattle(Faction faction, int x, int y)
    {
        if (TileMap[x, y] == null || TileMap[x, y].groupList == null) return false;
        bool factionPresent = false;
        List<Faction> hostileFactionList = new List<Faction>();

        foreach (UnitGroup group in TileMap[x, y].groupList)
        {
            if (group.faction == faction) factionPresent = true;
            else if (faction.hostileFactionList.Contains(group.faction) && !hostileFactionList.Contains(group.faction)) hostileFactionList.Add(group.faction);
        }

        if (factionPresent && hostileFactionList.Count>0)
        {
            foreach(Faction hostileFaction in hostileFactionList) DataTransport.battleList.Add(new Battle(faction, hostileFaction, x, y));
            return true;
        }
        return false;
    }

    public static void ProcessMovement(Faction faction)
    {
        if (movementsList != null)
        {
            foreach (Movement movement in movementsList) //move all group
            {
                //human can't go on water
                if(movement.group.type == UnitType.slime || MapControl.MapTerrain[movement.newX, movement.newY] != MapControl.Terrain.sea)
                {
                    MoveGroup(movement.group, movement.oldX, movement.oldY, movement.newX, movement.newY);
                    AnimationControl.addMapMove(movement.group.avatar, movement.oldX, movement.oldY, movement.newX, movement.newY);
                    if(movement.group.type == UnitType.player)
                    {
                        PlayerControl.X = movement.newX;
                        PlayerControl.Y = movement.newY;
                    }
                }
            }
            foreach (Movement movement in movementsList) //check all the moved tile for battle
            {
                CheckBattle(faction, movement.newX, movement.newY);
            }
        }
        movementsList = null;
    }
}
