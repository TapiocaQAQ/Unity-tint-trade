using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public Faction attackFaction, defenceFaction;
    public List<MapUnitControl.UnitGroup> BattleGroupList = new List<MapUnitControl.UnitGroup>();
    public MapUnitControl.UnitTile BattleTile;
    public MapControl.Terrain BattleTerrain;
    public int Battle_X;
    public int Battle_Y;

    public Battle(Faction attackFaction, Faction defenceFaction, int Battle_X, int Battle_Y)
    {
        this.attackFaction = attackFaction;
        this.defenceFaction = defenceFaction;
        this.BattleTile = MapUnitControl.TileMap[Battle_X, Battle_Y];
        foreach (MapUnitControl.UnitGroup group in BattleTile.groupList)
        {
            if (group.faction == attackFaction || group.faction == defenceFaction) BattleGroupList.Add(group);
        }
        this.BattleTerrain = MapControl.MapTerrain[Battle_X, Battle_Y];
        this.Battle_X = Battle_X;
        this.Battle_Y = Battle_Y;
    }
}
