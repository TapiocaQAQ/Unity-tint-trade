using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleControl : MonoBehaviour
{
    public float moveSpeed = 10;
    private GameObject attack;
    public float attackRange = 1, staggerTime = 0.5f, attackScale = 1;
    private float stagger = 0;
    public float colorChangeTime = 0.1f;
    private float colorTime = 0;
    public int HP = 3;
    public Sprite normal, raiseSword;
    private Camera cam;
    private MapUnitControl.UnitGroup playerGroup;
    private bool fleed = false;
    // Start is called before the first frame update
    void Start()
    {
        attack = AssetLoader.Slash;
        playerGroup = MapUnitControl.GetPlayerGroup();
        if (cam == null) cam = Camera.main;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        if(playerGroup.getTotalCount() > 1) BattleControl.SpawnCommander(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleControl.battleStarted)
        {
            if (cam == null) cam = Camera.main;
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

            if (colorTime > 0)
            {
                colorTime -= Time.deltaTime;
            }
            else if (colorTime < 0)
            {
                Renderer cookieManRenderer = gameObject.GetComponent<Renderer>();
                cookieManRenderer.material.SetColor("_Color", Color.white);
                colorTime = 0;
            }

            if (stagger > 0)
            {
                stagger -= Time.deltaTime;
                if (stagger <= 0) gameObject.GetComponent<SpriteRenderer>().sprite = normal;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject attackObject;
                Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                Vector2 attackDirection = (Vector2)transform.position + mousePos.normalized * attackRange;
                attackObject = Instantiate(attack, attackDirection, Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(0, 1), mousePos)));
                attackObject.tag = gameObject.tag;
                attackObject.transform.localScale *= attackScale;
                gameObject.GetComponent<SpriteRenderer>().sprite = raiseSword;
                if (mousePos.x < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);//turn left
                else transform.localRotation = Quaternion.Euler(0, 0, 0);//turn right
                stagger = staggerTime;
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position += new Vector3(-moveSpeed, 0) * Time.deltaTime;
                    transform.localRotation = Quaternion.Euler(0, 180, 0);//turn left
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position += new Vector3(0, -moveSpeed) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(moveSpeed, 0) * Time.deltaTime;
                    transform.localRotation = Quaternion.Euler(0, 0, 0);//turn right
                }
            }

            MapUnitControl.Units player = new MapUnitControl.Units(MapUnitControl.UnitType.player);
            if (transform.position.x < 0)
            {
                movePlayerOnMap(PlayerControl.X - 1, PlayerControl.Y);
            }
            else if (transform.position.x > Setting.BATTLE_MAP_WIDTH - 1)
            {
                movePlayerOnMap(PlayerControl.X + 1, PlayerControl.Y);
            }
            else if (transform.position.y < 0)
            {
                movePlayerOnMap(PlayerControl.X, PlayerControl.Y - 1);
            }
            else if (transform.position.y > Setting.BATTLE_MAP_HEIGHT - 1)
            {
                movePlayerOnMap(PlayerControl.X, PlayerControl.Y + 1);
            }
        }
    }

    public void TakeDamage()
    {
        HP--;
        if(colorTime == 0)
        {
            Renderer cookieManRenderer = gameObject.GetComponent<Renderer>();
            cookieManRenderer.material.SetColor("_Color", Color.red);
            colorTime = colorChangeTime;
        }
        if (HP <= 0)
        {
            BattleControl.RemoveUnit(gameObject);
            movePlayerOnMap(DataTransport.Respawn_X, DataTransport.Respawn_Y);
            Destroy(gameObject);
        }
    }

    public void movePlayerOnMap(int x, int y)
    {
        if (!fleed)
        {
            MapUnitControl.MoveGroup(playerGroup, PlayerControl.X, PlayerControl.Y, x, y);
            PlayerControl.X = x;
            PlayerControl.Y = y;
            playerGroup.ModifyItem(new Item(Item.Type.money, -playerGroup.CountOf(Item.Type.money)));
            BattleControl.RemoveUnit(gameObject);
            Destroy(gameObject);
            fleed = true;
        }
    }
}
