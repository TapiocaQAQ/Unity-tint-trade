using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleAI : BattleAI
{
    public float moveSpeed = 1;
    public float attackCD = 0.5f;
    private float CDTime = 0;
    public int hp = 2;
    public float staggerTime = 0.5f;
    private float stagger = 0;
    public float colorChangeTime = 0.1f;
    private float colorTime = 0;
    private List<int> damageList = new List<int>();
    public override int HP { get => hp; protected set => hp = value; }
    // Start is called before the first frame update
    void Start()
    {

    }

    float lapsedTime;
    // Update is called once per frame
    void Update()
    {
        if (BattleControl.battleStarted)
        {
            lapsedTime += Time.deltaTime;
            if (lapsedTime > 0.015)
            {
                if (stagger <= 0)
                {
                    if (BattleControl.ClosestEnemyMap.ContainsKey(gameObject))
                    {
                        Vector2 enemyPos = BattleControl.ClosestEnemyMap[gameObject].transform.position;
                        //move
                        enemyPos -= (Vector2)transform.position;
                        transform.position += (Vector3)enemyPos.normalized * moveSpeed * lapsedTime;
                        //turn left,right
                        if (enemyPos.x < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);
                        else transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                }

                if (colorTime > 0)
                {
                    colorTime -= lapsedTime;
                }
                else if (colorTime < 0)
                {
                    Renderer cookieManRenderer = gameObject.GetComponent<Renderer>();
                    cookieManRenderer.material.SetColor("_Color", Color.white);
                    colorTime = 0;
                }

                if (stagger > 0) stagger -= lapsedTime;

                //process damage taken
                if (damageList.Count > 0)
                {
                    Renderer cookieManRenderer = gameObject.GetComponent<Renderer>();
                    cookieManRenderer.material.SetColor("_Color", Color.red);
                    colorTime = colorChangeTime;
                    stagger = staggerTime;
                    foreach (int damage in damageList)
                    {
                        HP -= damage;
                    }
                    if (HP <= 0)
                    {
                        MapUnitControl.UnitGroup group = BattleControl.UnitIdToGroupMap[gameObject.GetInstanceID()];
                        group.modifyUnit(new MapUnitControl.Units(MapUnitControl.UnitType.slime, -1));
                        BattleControl.RemoveUnit(gameObject);
                        Destroy(gameObject);
                    }
                    damageList.Clear();
                }
                lapsedTime = 0;
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (CDTime > 0)
        {
            CDTime -= Time.deltaTime;
        }
        else if (stagger <= 0 && other.gameObject.tag != gameObject.tag)
        {
            other.gameObject.SendMessage("TakeDamage", 1);
            CDTime = attackCD;
            stagger = attackCD;
        }
    }

    public override void TakeDamage(int damage = 1)
    {
        damageList.Add(damage);
    }

    public override void newTargetPos(Vector2 pos)
    {

    }
}
