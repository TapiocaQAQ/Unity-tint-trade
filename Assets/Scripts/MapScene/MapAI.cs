using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapAI
{
    public abstract void NextTurn(MapUnitControl.UnitGroup group, int X, int Y);
}
