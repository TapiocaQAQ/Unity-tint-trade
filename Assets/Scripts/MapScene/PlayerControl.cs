using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public static int X, Y;
    private static float initialDelay = 0.1f;
    private float delay = 0;
    private MapUnitControl.UnitGroup playerGroup;
    // Start is called before the first frame update
    void Start()
    {
        X = (int)transform.position.x;
        Y = (int)transform.position.y;
        delay = initialDelay;
        Camera.main.transform.position = new Vector3(X, Y, -10);
        playerGroup = MapUnitControl.GetPlayerGroup();
    }

    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }
        if (!AnimationControl.isBusy())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BtnOnClick.NextTurn();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Move(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Move(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(1, 0);
            }
        }
    }

    private void Move(int Xbias, int Ybias)
    {
        MapUnitControl.addMovement(new MapUnitControl.Movement(playerGroup, X, Y, X + Xbias, Y + Ybias));
        BtnOnClick.NextTurn();
    }

    public static void NextTurn()
    {

    }
}
