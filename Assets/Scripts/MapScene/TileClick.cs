using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMouseUp()
    {
        if(!EventSystem.current.IsPointerOverGameObject()) PanelControl.ShowPanel((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);
    }
}
