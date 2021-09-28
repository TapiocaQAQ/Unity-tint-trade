using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBattleAI : BattleAI
{
    public float moveSpeed = 3;
    public GameObject attack;
    public float attackRange = 1, staggerTime = 0.5f, attackScale = 1;
    private float stagger = 0;
    private int hp = 2;
    public float colorChangeTime = 0.1f;
    private float colorTime = 0;
    private List<int> damageList = new List<int>();
    private bool inRank = false;
    private Vector2 TargetPos = new Vector2();
    public override int HP { get => hp; protected set => hp = value; }
    // Start is called before the first frame update
    void Start()
    {
        attack = AssetLoader.Slash;
    }

    float lapsedTime;
    // Update is called once per frame
    void Update()
    {
        //get into formation
        if (inRank == false)
        {
            inRank = BattleControl.FindingRank(gameObject, this);
            //if (!inRank) BattleControl.SpawnCommander(gameObject);
        }

        if (BattleControl.battleStarted)
        {
            lapsedTime += Time.deltaTime;
            if (lapsedTime > 0.015)
            {
                if (stagger > 0)
                {
                    stagger -= lapsedTime;
                }
                else if(BattleControl.ClosestEnemyMap.ContainsKey(gameObject))
                {
                    //move toward nearest enemy
                    Vector2 enemyPos = BattleControl.ClosestEnemyMap[gameObject].transform.position;
                    enemyPos -= (Vector2)transform.position;

                    //attack enemy
                    if (enemyPos.magnitude <= 2.2) Attack(enemyPos);

                    if ((inRank && enemyPos.magnitude > 2 && enemyPos.magnitude < 2.5)
                        || !inRank && enemyPos.magnitude > 2)
                    {
                        //move towards enemy
                        transform.position += (Vector3)enemyPos.normalized * moveSpeed * lapsedTime;
                    }
                    else if (enemyPos.magnitude < 2)
                    {
                        //move away from enemy
                        transform.position -= (Vector3)enemyPos.normalized * moveSpeed * lapsedTime;
                    }
                    else
                    {
                        //form ranks
                        if(Vector2.Distance(transform.position, TargetPos) > 0.1)
                        transform.position += (Vector3)(TargetPos - (Vector2)transform.position).normalized * moveSpeed * lapsedTime;
                    }
                    //turn left,right
                    if (enemyPos.x < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);
                    else if (enemyPos.x > 0) transform.localRotation = Quaternion.Euler(0, 0, 0);
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

                //process damage taken
                if (damageList.Count > 0)
                {
                    Renderer cookieManRenderer = gameObject.GetComponent<Renderer>();
                    cookieManRenderer.material.SetColor("_Color", Color.red);
                    colorTime = colorChangeTime;
                    foreach (int damage in damageList)
                    {
                        HP -= damage;
                    }
                    if (HP <= 0)
                    {
                        MapUnitControl.UnitGroup group = BattleControl.UnitIdToGroupMap[gameObject.GetInstanceID()];
                        group.modifyUnit(new MapUnitControl.Units(MapUnitControl.UnitType.guard, -1));
                        BattleControl.RemoveUnit(gameObject);
                        Destroy(gameObject);
                    }
                    damageList.Clear();
                }
                lapsedTime = 0;
            }
        }
        else if (inRank)
        {
            gameObject.transform.position = TargetPos;
        }
    }

    public override void TakeDamage(int damage = 1)
    {
        damageList.Add(damage);
    }

    public void Attack(Vector2 enemyPos)
    {
        GameObject attackObject;
        if (stagger <= 0)
        {
            Vector2 attackDirection = (Vector2)transform.position + enemyPos.normalized * attackRange;
            attackObject = Instantiate(attack, attackDirection, Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(0, 1), enemyPos)));
            attackObject.tag = gameObject.tag;
            attackObject.transform.localScale = new Vector3(0.5f, 6) * attackScale;
            if (enemyPos.x < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);//turn left
            else transform.localRotation = Quaternion.Euler(0, 0, 0);//turn right
            stagger = staggerTime;
        }
    }

    public override void newTargetPos(Vector2 pos)
    {
        TargetPos = pos;
    }
}
