using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour
{
    public CanvasGroup panel;
    public GameObject content;
    public GameObject GroupInfoView;
    public GameObject BuildingInfoView;
    public Text panelText;
    private static bool showPanel = false;
    private static bool hidePanel = false;
    private static bool isShowing = false;
    public static int X, Y;
    public static MapUnitControl.UnitTile tile;
    public static MapObjectControl.Building building;
    // Start is called before the first frame update
    void Start()
    {
        panel.alpha = 0;
        panel.blocksRaycasts = false;
        isShowing = false;
        showPanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(showPanel == true && !AnimationControl.isBusy() && !FactionControl.isBusy)
        {
            isShowing = true;
            tile = MapUnitControl.TileMap[X, Y];
            building = MapObjectControl.mapBuilding[X, Y];
            panel.alpha = 1;
            panel.blocksRaycasts = true;
            showPanel = false;
            panelText.text = X + "," + Y;
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
            if (building != null)
            {
                GameObject view = Instantiate(BuildingInfoView);
                view.transform.SetParent(content.transform);
                view.GetComponent<BuildingInfoViewControl>().SetBuilding(building);
            }
            if (tile != null) 
            {
                foreach (MapUnitControl.UnitGroup group in tile.groupList)
                {
                    GameObject view = Instantiate(GroupInfoView);
                    view.transform.SetParent(content.transform);
                    view.GetComponent<GroupInfoViewControl>().SetGroup(group);
                }
            } 
        }
        if (hidePanel)
        {
            isShowing = false;
            panel.alpha = 0;
            panel.blocksRaycasts = false;
            hidePanel = false;
        }
    }

    public static void ShowPanel(int x, int y)
    {
        showPanel = true;
        X = x;
        Y = y;
    }

    public static void HidePanel()
    {
        hidePanel = true;
        showPanel = false;
    }

    public static void UpdatePanel()
    {
        if(isShowing) showPanel = true;
    }
}
