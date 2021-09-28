using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnOnClick : MonoBehaviour
{
    public void OnNextTurnClick()
    {
        if(!AnimationControl.isBusy()) NextTurn();
    }

    public static void NextTurn()
    {
        PlayerControl.NextTurn();
        FactionControl.NextTurn();
        PanelControl.UpdatePanel();
    }

    public void OnStartBattleClick()
    {
        BattleControl.battleStarted = true;
    }

    public void OnClosePanelClick()
    {
        PanelControl.HidePanel();
    }

    public void OnHireGuardClick()
    {
        MapUnitControl.UnitGroup playerGroup = MapUnitControl.GetPlayerGroup();
        if (playerGroup.CountOf(Item.Type.money) >= 2)
        {
            PanelControl.building.ModifyItem(new Item(Item.Type.money, 2));
            playerGroup.ModifyItem(new Item(Item.Type.money, -2));
            playerGroup.modifyUnit(new MapUnitControl.Units(MapUnitControl.UnitType.guard));
            PanelControl.UpdatePanel();
        }
    }
}
