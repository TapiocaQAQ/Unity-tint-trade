using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    private List<BattleAI> soldierList = new List<BattleAI>();
    public int soldierNum = 8;
    public Formation formation;
    public float time = 3;

    public enum Formation
    {
        skirmish,
        doubleLine,
        tripleLine
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    float lapsedTime;
    // Update is called once per frame
    void Update()
    {
        lapsedTime += Time.deltaTime;
        if (lapsedTime > 0.015)
        {
            for (int i = 0; i < soldierList.Count; i++)
            {
                switch (formation)
                {
                    case Formation.skirmish:
                        if (i < soldierList.Count / 3) soldierList[i].newTargetPos(transform.position + new Vector3(0, i * 2));
                        else if (i < soldierList.Count * 2 / 3) soldierList[i].newTargetPos(transform.position + new Vector3(2f, (i - soldierList.Count / 3) * 2));
                        else soldierList[i].newTargetPos(transform.position + new Vector3(4f, (i - soldierList.Count * 2 / 3) * 2));
                        break;
                    case Formation.doubleLine:
                        if (i < soldierList.Count / 2) soldierList[i].newTargetPos(transform.position + new Vector3(0, i));
                        else soldierList[i].newTargetPos(transform.position + new Vector3(1.6f, i - soldierList.Count / 2));
                        break;
                    case Formation.tripleLine:
                        if (i < soldierList.Count / 3) soldierList[i].newTargetPos(transform.position + new Vector3(0, i));
                        else if (i < soldierList.Count * 2 / 3) soldierList[i].newTargetPos(transform.position + new Vector3(1, i - soldierList.Count / 3));
                        else soldierList[i].newTargetPos(transform.position + new Vector3(2, i - soldierList.Count * 2 / 3));
                        break;
                }
                if (soldierList[i].HP <= 0) soldierList.RemoveAt(i);
            }

            lapsedTime = 0;
        }
    }

    public bool JoinRank(BattleAI newSoldier)
    {
        if (soldierList.Count < soldierNum)
        {
            soldierList.Add(newSoldier);
            return true;
        }
        return false;
    }
}
