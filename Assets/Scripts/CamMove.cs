using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamMove : MonoBehaviour
{
    public static GameObject cam;
    public float wheelSpeed = 5f;
    private float camScale = 5;
    public float camMoveSpeed = 0.1f;
    private bool isDraggingMap = false;
    private Vector2 lastMouse;
    private static bool isFallowing = false;
    private static Vector3 fallowingPoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        if(SceneManager.GetActiveScene().name == "BattleScene")
        {
            MoveCam(Setting.BATTLE_MAP_WIDTH / 2, Setting.BATTLE_MAP_HEIGHT / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // move
        if (!isDraggingMap && Input.GetKey(KeyCode.Mouse1)) {
            lastMouse = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            isDraggingMap = true;
        }
        else if (isDraggingMap)
        {
            Vector2 mouseNow = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position += new Vector3(lastMouse.x - mouseNow.x, lastMouse.y - mouseNow.y);
            lastMouse = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            if (!Input.GetKey(KeyCode.Mouse1)) { isDraggingMap = false; }
        }

        // scale
        if ((Input.GetAxis("Mouse ScrollWheel")> 0 && camScale>1 ) || ( Input.GetAxis("Mouse ScrollWheel") < 0 && camScale < 8))
        {
            camScale -= wheelSpeed* Input.GetAxis("Mouse ScrollWheel");
            Camera.main.orthographicSize = camScale;
        }

        if (isFallowing)
        {
            transform.position = fallowingPoint;
            isFallowing = false;
        }
    }

    public static void MoveCam(float x, float y)
    {
        fallowingPoint = new Vector3(x,y,-10);
        isFallowing = true;
    }
}
