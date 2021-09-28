using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoViewControl : MonoBehaviour
{
    public Image image;
    public Text infoText;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuilding(MapObjectControl.Building building)
    {
        switch (building.type)
        {
            case MapObjectControl.BuildingType.village:
                image.sprite = AssetLoader.village.GetComponent<SpriteRenderer>().sprite;
                break;
            case MapObjectControl.BuildingType.tint:
                image.sprite = AssetLoader.tint.GetComponent<SpriteRenderer>().sprite;
                break;
            default:
                throw new System.Exception("unknown building type");
        }

        infoText.text = building.type + "\n";
        infoText.text += "發展度: " + building.development + "\n";
        foreach (Item item in building.itemList)
        {
            infoText.text += item.type + ": " + item.count + "\n";
        }

        if(building.type == MapObjectControl.BuildingType.village &&
            building.x == PlayerControl.X && building.y == PlayerControl.Y)
        {

        }
        else
        {
            Destroy(button.gameObject);
        }
    }
}
