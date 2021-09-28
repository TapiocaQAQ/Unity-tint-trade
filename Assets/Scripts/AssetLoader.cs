using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetLoader : MonoBehaviour
{
    public GameObject tile_planePrefab;
    public static GameObject tile_plane;
    public GameObject tile_seaPrefab;
    public static GameObject tile_sea;
    public GameObject tile_forestPrefab;
    public static GameObject tile_forest;
    public GameObject tile_mountainPrefab;
    public static GameObject tile_mountain;
    public GameObject villagePrefab;
    public static GameObject village;
    public GameObject tintPrefab;
    public static GameObject tint;
    public GameObject CookieManPrefab;
    public static GameObject CookieMan;
    public GameObject CookieManBattlePrefab;
    public static GameObject CookieManBattle;
    public GameObject GuardPrefab;
    public static GameObject Guard;
    public GameObject GuardBattlePrefab;
    public static GameObject GuardBattle;
    public GameObject SlimePrefab;
    public static GameObject Slime;
    public GameObject SlimeBattlePrefab;
    public static GameObject SlimeBattle;
    public GameObject SlashPrefab;
    public static GameObject Slash;
    public GameObject RedBoxPrefab;
    public static GameObject RedBox;
    public GameObject CommandFlagPrefab;
    public static GameObject CommandFlag;
    public Text APTextLoader;
    public static Text APText;
    // Start is called before the first frame update
    void Start()
    {
        tile_forest = tile_forestPrefab;
        tile_mountain = tile_mountainPrefab;
        tile_plane = tile_planePrefab;
        tile_sea = tile_seaPrefab;
        village = villagePrefab;
        tint = tintPrefab;
        CookieMan = CookieManPrefab;
        Guard = GuardPrefab;
        GuardBattle = GuardBattlePrefab;
        Slime = SlimePrefab;
        CookieManBattle = CookieManBattlePrefab;
        SlimeBattle = SlimeBattlePrefab;
        Slash = SlashPrefab;
        RedBox = RedBoxPrefab;
        CommandFlag = CommandFlagPrefab;
        APText = APTextLoader;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
