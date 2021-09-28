using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MapAI
{
    public override void NextTurn(MapUnitControl.UnitGroup group, int X, int Y)
    {
        int newX, newY;
        Vector2Int selfPos = new Vector2Int(X, Y);
        Vector2Int guardPos = group.findClosest(selfPos, MapUnitControl.UnitType.guard, 2);
        Vector2Int playerPos = group.findClosest(selfPos, MapUnitControl.UnitType.player, 2);

        if (group.CountOf(Item.Type.nutrient) > 0 && X == group.home.x && Y == group.home.y)
        {
            //give nutrient to tint
            group.home.ModifyItem(group.GetItem(Item.Type.nutrient));
            group.ModifyItem(new Item(Item.Type.nutrient, -group.CountOf(Item.Type.nutrient)));
        }

        if (group.CountOf(Item.Type.nutrient) > 0)
        {
            //go back home
            if (group.home.x < X) newX = X - 1;
            else if (group.home.x > X) newX = X + 1;
            else newX = X;
            if (group.home.y < Y) newY = Y - 1;
            else if (group.home.y > Y) newY = Y + 1;
            else newY = Y;
        }
        else if (playerPos != new Vector2Int(-1, -1))
        {
            newX = X;
            if (playerPos.x < selfPos.x) newX -= 1;
            else if (playerPos.x > selfPos.x) newX += 1;
            newY = Y;
            if (playerPos.y < selfPos.y) newY -= 1;
            else if (playerPos.y > selfPos.y) newY += 1;
        }
        else if(guardPos != new Vector2Int(-1, -1))
        {
            newX = X;
            if (guardPos.x < selfPos.x) newX -= 1;
            else if (guardPos.x > selfPos.x) newX += 1;
            newY = Y;
            if (guardPos.y < selfPos.y) newY -= 1;
            else if (guardPos.y > selfPos.y) newY += 1;
        }
        else
        {
            newX = X + Random.Range(-1, 2);
            newY = Y + Random.Range(-1, 2);
        }

        MapUnitControl.addMovement(new MapUnitControl.Movement(group, X, Y, newX, newY));
    }

}
