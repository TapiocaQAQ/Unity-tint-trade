using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FactionControl : MonoBehaviour
{
    public static List<Faction> factionsList;
    public static Faction currentFaction;
    private static int currentFactionIndex = 0;
    public static Faction playerFaction = new Faction("玩家", new PlayerAI());
    public static Faction neutralFaction = new Faction("中立", new GuardAI());
    public static Faction monsterFaction = new Faction("怪物", new EnemyAI());
    public static bool isBusy = false;

    // Start is called before the first frame update
    void Start()
    {
        if(factionsList == null)
        {
            factionsList = new List<Faction>();
            MakeHostile(playerFaction, monsterFaction);
            MakeHostile(neutralFaction, monsterFaction);
            factionsList.Add(playerFaction);
            factionsList.Add(neutralFaction);
            factionsList.Add(monsterFaction);
            currentFaction = factionsList[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!AnimationControl.isBusy())
        {
            AssetLoader.APText.text = "Turn: " + factionsList[currentFactionIndex].name;
            if (DataTransport.battleList.Count > 0)
            {
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
            else if (currentFactionIndex != 0)
            {
                NextTurn();
            }
        }
    }

    public static Faction GetFaction(string factionName)
    {
        foreach(Faction faction in factionsList)
        {
            if (faction.name.Equals(factionName)) return faction;
        }
        throw new System.Exception("faction not found");
    }

    public static void MakeHostile(Faction faction, Faction otherFaction)
    {
        if(!faction.hostileFactionList.Contains(otherFaction)) faction.hostileFactionList.Add(otherFaction);
        if(!otherFaction.hostileFactionList.Contains(faction)) otherFaction.hostileFactionList.Add(faction);
    }

    public static void NextTurn()
    {
        isBusy = true;
        AssetLoader.APText.text = "Turn: " + factionsList[currentFactionIndex].name;
        if (currentFactionIndex < factionsList.Count)
        {
            factionsList[currentFactionIndex].NextTurn();
            MapUnitControl.ProcessMovement(factionsList[currentFactionIndex]);
            currentFactionIndex++;
            if (currentFactionIndex == factionsList.Count)
            {
                currentFactionIndex = 0;
                isBusy = false;
            }
        }
    }
}
