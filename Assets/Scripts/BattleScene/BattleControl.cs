using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleControl : MonoBehaviour
{
    public static Battle currentBattle;
    public static Dictionary<int, MapUnitControl.UnitGroup> UnitIdToGroupMap = new Dictionary<int, MapUnitControl.UnitGroup>();
    public static GameObject Player;
    public static int defenderCount = 0, attackerCount = 0, defenderTotal, attackerTotal;
    public static List<Commander> defendCommenderList = new List<Commander>();
    public static List<Commander> attackCommenderList = new List<Commander>();
    public static List<GameObject> defenderList = new List<GameObject>(), attackerList = new List<GameObject>();
    public static Dictionary<GameObject, GameObject> ClosestEnemyMap = new Dictionary<GameObject, GameObject>();
    public static bool newCommender = false;
    public static bool isCheckingBattleEnd = false;
    public static bool battleStarted = false;

    public GameObject seaImage, planeImage, forestImage, mountainImage;
    public static GameObject sea, plane, forest, mountain;
    public Text battleText;
    public static Text text;
    // Start is called before the first frame update
    void Start()
    {
        defendCommenderList.Clear();
        attackCommenderList.Clear();
        defenderList.Clear();
        attackerList.Clear();
        UnitIdToGroupMap.Clear();
        ClosestEnemyMap.Clear();
        sea = seaImage;
        plane = planeImage;
        forest = forestImage;
        mountain = mountainImage;
        text = battleText;
        LoadBattle(DataTransport.battleList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckingBattleEnd)
        {
            if (defenderCount == 0 || attackerCount == 0)
            {
                //give battle reward
                Faction winnerFaction = null;
                int killCount = 0;
                if(defenderCount == 0)
                {
                    winnerFaction = currentBattle.attackFaction;
                    killCount = defenderTotal;
                }
                else if(attackerCount == 0)
                {
                    winnerFaction = currentBattle.defenceFaction;
                    killCount = attackerTotal;
                }
                foreach (MapUnitControl.UnitGroup group in currentBattle.BattleGroupList)
                {
                    if(group.getTotalCount() > 0 && group.faction == winnerFaction)
                    {
                        if(winnerFaction == FactionControl.monsterFaction)
                        {
                            group.ModifyItem(new Item(Item.Type.nutrient, killCount));
                        }
                        else
                        {
                            group.ModifyItem(new Item(Item.Type.money, killCount));
                        }
                    }
                }

                DataTransport.battleList.RemoveAt(0);
                if (DataTransport.battleList.Count > 0)
                {
                    Debug.Log("load next battle, Remain battle count: " + DataTransport.battleList.Count);
                    SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
                }
                else 
                {
                    SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
                }

            }
            else if (defenderCount < 0 || attackerCount < 0)
            {
                throw new System.Exception("negative count of unit");
            }
            isCheckingBattleEnd = false;
        }
        newCommender = false;

        //calculate all the distant between attackers and defenders
        ClosestEnemyMap.Clear();
        foreach (GameObject defender in defenderList) foreach (GameObject attacker in attackerList)
            {
                if (!ClosestEnemyMap.ContainsKey(defender))
                {
                    ClosestEnemyMap[defender] = attacker;
                }
                else if (Vector2.Distance(defender.transform.position, attacker.transform.position)
                      < Vector2.Distance(defender.transform.position, ClosestEnemyMap[defender].transform.position))
                {
                    ClosestEnemyMap[defender] = attacker;
                }
            }
        foreach (GameObject attacker in attackerList) foreach (GameObject defender in defenderList)
            {
                if (!ClosestEnemyMap.ContainsKey(attacker))
                {
                    ClosestEnemyMap[attacker] = defender;
                }
                else if (Vector2.Distance(attacker.transform.position, defender.transform.position)
                      < Vector2.Distance(attacker.transform.position, ClosestEnemyMap[attacker].transform.position))
                {
                    ClosestEnemyMap[attacker] = defender;
                }
            }

    }

    private static void LoadBattle(Battle battle)
    {
        text.text = battle.BattleGroupList.Count + " groups in battle, tile: " + battle.Battle_X + "," + battle.Battle_Y;
        currentBattle = battle;
        defenderCount = 0;
        attackerCount = 0;
        //place the background
        switch (battle.BattleTerrain)
        {
            case MapControl.Terrain.plain:
                for (int x = 0; x < Setting.BATTLE_MAP_WIDTH; x++) for (int y = 0; y < Setting.BATTLE_MAP_HEIGHT; y++)
                    {
                        Instantiate(plane, new Vector3(x, y), new Quaternion());
                    }
                break;
            case MapControl.Terrain.forest:
                for (int x = 0; x < Setting.BATTLE_MAP_WIDTH; x++) for (int y = 0; y < Setting.BATTLE_MAP_HEIGHT; y++)
                    {
                        Instantiate(forest, new Vector3(x, y), new Quaternion());
                    }
                break;
            case MapControl.Terrain.mountain:
                for (int x = 0; x < Setting.BATTLE_MAP_WIDTH; x++) for (int y = 0; y < Setting.BATTLE_MAP_HEIGHT; y++)
                    {
                        Instantiate(mountain, new Vector3(x, y), new Quaternion());
                    }
                break;
            case MapControl.Terrain.sea:
                for (int x = 0; x < Setting.BATTLE_MAP_WIDTH; x++) for (int y = 0; y < Setting.BATTLE_MAP_HEIGHT; y++)
                    {
                        Instantiate(sea, new Vector3(x, y), new Quaternion());
                    }
                break;
        }

        //place all the unit
        foreach (MapUnitControl.UnitGroup group in battle.BattleGroupList)
        {
            GameObject gameObject = null;
            foreach (MapUnitControl.Units unit in group.unitsList) for (int i = 0; i < unit.count; i++)
                {
                    Vector2 startPos = new Vector2();
                    if (group.faction == battle.attackFaction)
                    {
                        startPos = new Vector2(Random.Range((float)Setting.BATTLE_MAP_WIDTH - 5, Setting.BATTLE_MAP_WIDTH-1), Random.Range(1f, 19f));
                    }
                    else if (group.faction == battle.defenceFaction)
                    {
                        startPos = new Vector2(Random.Range(1f, 5f), Random.Range(1f, 19f));
                    }

                    switch (unit.type)
                    {
                        case MapUnitControl.UnitType.slime:
                            gameObject = Instantiate(AssetLoader.SlimeBattle, startPos, new Quaternion());
                            UnitIdToGroupMap.Add(gameObject.GetInstanceID(), group);
                            break;
                        case MapUnitControl.UnitType.player:
                            gameObject = Instantiate(AssetLoader.CookieManBattle, startPos, new Quaternion());
                            UnitIdToGroupMap.Add(gameObject.GetInstanceID(), group);
                            Player = gameObject;
                            break;
                        case MapUnitControl.UnitType.guard:
                            gameObject = Instantiate(AssetLoader.GuardBattle, startPos, new Quaternion());
                            UnitIdToGroupMap.Add(gameObject.GetInstanceID(), group);
                            break;
                    }
                    if (gameObject == null) throw new System.Exception("unknown unit type");

                    if(group.faction == battle.attackFaction)
                    {
                        gameObject.tag = "attacker";
                        attackerList.Add(gameObject);
                        attackerCount++;
                    }
                    else if(group.faction == battle.defenceFaction)
                    {
                        gameObject.tag = "defender";
                        defenderList.Add(gameObject);
                        defenderCount++;
                    }
                }
        }
        CheckBattleEnd();

        if (battle.attackFaction == FactionControl.playerFaction || battle.defenceFaction == FactionControl.playerFaction)
        {
            battleStarted = false;
        }
        else
        {
            battleStarted = true;
        }

        defenderTotal = defenderCount;
        attackerTotal = attackerCount;
    }

    public static void CheckBattleEnd()
    {
        isCheckingBattleEnd = true;
    }

    public static bool FindingRank(GameObject gameObject, BattleAI AI)
    {
        bool foundRank = false;
        bool foundUnit = false;
        if (attackerList.Contains(gameObject))
        {
            foundUnit = true;
            foreach (Commander commder in attackCommenderList)
            {
                foundRank = commder.JoinRank(AI);
                if (foundRank) break;
            }
        }
        else if (defenderList.Contains(gameObject))
        {
            foundUnit = true;
            foreach (Commander commder in defendCommenderList)
            {
                foundRank = commder.JoinRank(AI);
                if (foundRank) break;
            }
        }
        if (!foundUnit) throw new System.Exception("Unit not found!");
        return foundRank;
    }

    public static void SpawnCommander(GameObject gameObject)
    {
        if (newCommender) return;
        Debug.Log("new commander");
        GameObject flag = Instantiate(AssetLoader.CommandFlag, gameObject.transform.position, new Quaternion()); ;
        if (attackerList.Contains(gameObject))
        {
            attackCommenderList.Add((Commander)flag.GetComponent("Commander"));
            BattleControl.newCommender = true;
        }
        else if (defenderList.Contains(gameObject))
        {
            defendCommenderList.Add((Commander)flag.GetComponent("Commander"));
            BattleControl.newCommender = true;
        }
    }

    public static void RemoveUnit(GameObject gameObject)
    {
        if (attackerList.Contains(gameObject))
        {
            attackerList.Remove(gameObject);
            attackerCount--;
        }
        else if (defenderList.Contains(gameObject))
        {
            defenderList.Remove(gameObject);
            defenderCount--;
        }
        CheckBattleEnd();
    }
}
