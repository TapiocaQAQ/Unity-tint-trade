using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MapAI
{
    public static int searchRange = 1;

    public override void NextTurn(MapUnitControl.UnitGroup group,int oldX, int oldY)
    {
        if (group.type == MapUnitControl.UnitType.player) return;

        bool nearEnemy = false;
        int NewX = oldX, NewY = oldY;
        int nearestEnemyX = oldX + searchRange + 1, nearestEnemyY = oldY + searchRange + 1;
        for (int x = oldX - searchRange; x <= oldX + searchRange; x++) for(int y = oldY - searchRange; y <= oldY + searchRange; y++) if(x>0 && x<Setting.MAP_WIDTH && y>0 && y<Setting.MAP_HEIGHT)
        {           
                    int distanceX = Mathf.Abs(x - oldX), distanceY = Mathf.Abs(y - oldY);
                    
                    if (MapUnitControl.TileMap[x, y] != null && MapUnitControl.TileMap[x, y].hasType(MapUnitControl.UnitType.slime)>0) 
                    {
                        if (distanceX < nearestEnemyX && distanceY < nearestEnemyY) 
                        {
                            nearestEnemyX = x;
                            nearestEnemyY = y;
                        }
                        nearEnemy = true;
                    } 
        }

        if (group.CountOf(Item.Type.money) > 0 && oldX == group.home.x && oldY == group.home.y)
        {
            //give money to village
            group.home.ModifyItem(group.GetItem(Item.Type.money));
            group.ModifyItem(new Item(Item.Type.money, -group.CountOf(Item.Type.money)));
        }

        if (Mathf.Abs(oldX - group.home.x) + Mathf.Abs(oldY - group.home.y) > 3 || group.CountOf(Item.Type.money) > 0)
        {
            //go back home
            if (group.home.x < oldX) NewX = oldX - 1;
            else if (group.home.x > oldX) NewX = oldX + 1;
            else NewX = oldX;
            if (group.home.y < oldY) NewY = oldY - 1;
            else if (group.home.y > oldY) NewY = oldY + 1;
            else NewY = oldY;
        }
        else if (nearEnemy)
        {
            if (nearestEnemyX < oldX) NewX = oldX - 1;
            else if (nearestEnemyX == oldX) NewX = oldX;
            else NewX = oldX + 1;
            if (nearestEnemyY < oldY) NewY = oldY - 1;
            else if (nearestEnemyY == oldY) NewY = oldY;
            else NewY = oldY + 1;
        }
        else
        {
            NewX = oldX + Random.Range(-1, 2);
            NewY = oldY + Random.Range(-1, 2);
        }

        //Debug.Log("oldX: " + oldX + " oldY: " + oldY + " NewX: " + NewX + "NewY: " + NewY);
        MapUnitControl.addMovement(new MapUnitControl.Movement(group, oldX, oldY, NewX, NewY));
    }
}
