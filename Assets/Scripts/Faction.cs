using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public string name;
    public MapAI mapAI;
    public List<Faction> hostileFactionList;
    public List<MapObjectControl.Building> buildingList;
    public List<MapUnitControl.UnitGroup> GroupList;
    public Faction(string name, MapAI mapAI)
    {
        this.name = name;
        this.mapAI = mapAI;
        hostileFactionList = new List<Faction>();
        buildingList = new List<MapObjectControl.Building>();
        GroupList = new List<MapUnitControl.UnitGroup>();
    }

    public void NextTurn()
    {
        for (int x = 0; x < Setting.MAP_WIDTH; x++) for (int y = 0; y < Setting.MAP_HEIGHT; y++)
                if (MapUnitControl.TileMap[x, y] != null)
                {
                    //make all the group in this faction move
                    foreach (MapUnitControl.UnitGroup group in MapUnitControl.TileMap[x, y].groupList) 
                    {
                        if (group.faction == this) mapAI.NextTurn(group, x, y);
                    }
                }

        foreach (MapObjectControl.Building building in buildingList)
        {
            building.Process();
        }
    }
}
