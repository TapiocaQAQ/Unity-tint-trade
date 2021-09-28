using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAI : MonoBehaviour
{
    public abstract int HP { get; protected set; }
    public abstract void TakeDamage(int damage = 1);
    public abstract void newTargetPos(Vector2 pos);
}
