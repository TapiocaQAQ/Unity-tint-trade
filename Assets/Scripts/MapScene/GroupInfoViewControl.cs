using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupInfoViewControl : MonoBehaviour
{
    public Image image;
    public GameObject coinImage;
    public GameObject nutrientImage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGroup(MapUnitControl.UnitGroup group)
    {
        foreach(MapUnitControl.Units unit in group.unitsList)
        {
            for(int i=0; i<unit.count; i++) switch (unit.type)
                {
                    case MapUnitControl.UnitType.guard:
                        image.sprite = AssetLoader.Guard.GetComponent<SpriteRenderer>().sprite;
                        Instantiate(image).transform.SetParent(transform);
                        break;
                    case MapUnitControl.UnitType.slime:
                        image.sprite = AssetLoader.Slime.GetComponent<SpriteRenderer>().sprite;
                        Instantiate(image).transform.SetParent(transform);
                        break;
                    case MapUnitControl.UnitType.player:
                        image.sprite = AssetLoader.CookieMan.GetComponent<SpriteRenderer>().sprite;
                        Instantiate(image).transform.SetParent(transform);
                        break;
                    default:
                        throw new System.Exception("unknown unit type");
                }
        }
        image.sprite = AssetLoader.CommandFlag.GetComponent<SpriteRenderer>().sprite;
        if(group.faction == FactionControl.monsterFaction)
        {
            GameObject nutrient = Instantiate(nutrientImage);
            nutrient.transform.SetParent(transform);
            nutrient.transform.GetChild(0).GetComponent<Text>().text = "" + group.CountOf(Item.Type.nutrient);
        }
        else
        {
            GameObject coin = Instantiate(coinImage);
            coin.transform.SetParent(transform);
            coin.transform.GetChild(0).GetComponent<Text>().text = "" + group.CountOf(Item.Type.money);
        }
    }

}
