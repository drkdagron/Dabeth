using UnityEngine;
using System;
using System.Collections;

public class CombatManager : MonoBehaviour{

    public GameManager game;

    public bool CombatWait;
    public bool buffer;
    public float bufTimer = 0f;
    public float bufTime = 1f;

    public Camera mainCam;
    public CombatView combatView;

    //last combat info
    Entity atk;
    Entity def;
    int rolls;
    int currShot = 0;
    float fireTime = 0;
    int[] atkR;
    int[] defR;

    public void StartCombatEvent()
    {
        game.UIManager.hideEnemyUI();
        game.UIManager.PlayerUI(false);
        game.UIManager.UIPlayerInfo.SetActive(false);
        mainCam.gameObject.SetActive(false);
        combatView.gameObject.SetActive(true);
    }

    public void EndCombatEvent()
    {
        game.UIManager.PlayerUI(true);
        game.UIManager.UIPlayerInfo.SetActive(true);
        mainCam.gameObject.SetActive(true);
        combatView.gameObject.SetActive(false);

        atk.PutEntity();
        def.PutEntity();
    }

    public void Fight(GameManager g, Entity p, Entity e)
    {
        StartCombatEvent();
        p.transform.position = combatView.AttPosition.transform.position;
        e.transform.position = combatView.DefPosition.transform.position;
        Vector3 edge = e.transform.position - p.transform.position;
        float f = Mathf.Atan2(edge.y, edge.x);
        p.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * f));

        int rolls = p.weapon.Rolls;
        int[] AtkRolls = new int[rolls];
        int[] DefRolls = new int[rolls];

        for (int i= 0; i < rolls; i++)
        {
            AtkRolls[i] = UnityEngine.Random.Range(1, p.weapon.DSize+1);
            DefRolls[i] = UnityEngine.Random.Range(1, (p.weapon.DSize + 1) + (2 * (int)e.armor.ArmorClass));
        }
        Array.Sort(AtkRolls);
        Array.Sort(DefRolls);
        Array.Reverse(AtkRolls);
        Array.Reverse(DefRolls);

        SetCombat(AtkRolls, DefRolls, p, e, rolls);
    }

    void SetCombat(int[] atk, int[] def, Entity p, Entity e, int rolls)
    {
        this.atkR = atk;
        this.defR = def;
        this.atk = p;
        this.def = e;
        this.rolls = rolls;
        currShot = 0;
        
        CombatWait = true;
        p.Fired = true;
    }

    void Update()
    {
        if (CombatWait == true)
        {
            fireTime += Time.deltaTime;
            if (fireTime > Debug_Weapon.FPS(atk.Weapon))
            {
                Spawner.SpawnMuzzleFlash(atk.weapon.Barrel.transform.position);
                if (Hit(currShot))
                {
                    Debug.Log("Hit");
                    Spawner.SpawnSmoke(def.transform.position, 0.5f);
                    def.Health--;
                    if (def.Name.Contains("Drone"))
                    {
                        game.UIManager.setEnemyUI((Enemy)def);
                        if (game.CheckEnemies())
                        {
                            buffer = true;
                        }
                    }
                    else
                        game.UIManager.setHealthBar((Player)def);
                }
                else
                {
                    Debug.Log("Miss");
                }

                currShot++;
                fireTime = 0;
                if (currShot == atkR.Length)
                {
                    buffer = true;
                }
            }
        }
        if (buffer)
        {
            CombatWait = false;
            bufTimer += Time.deltaTime;
            if (bufTimer > bufTime)
            {
                buffer = false;
                bufTimer = 0;
                EndCombatEvent();
                
            }
        }
    }

    bool Hit(int shot)
    {
        if (atkR[shot] > defR[shot])
            return true;

        return false;
    }





    /*
     * static int rolls, hits;
    static Entity atk, def;
    static float fireTimer;
    static float fireCount = 0;
    public static void StartCombat(int roll, int hit, Entity at, Entity de)
    {
        rolls = roll;
        hits = hit;
        atk = at;
        def = de;
        //Combat.CombatWait = true;
        fireTimer = Debug_Weapon.FPS(atk.Weapon);
    }

    void Update()
    {
        /*
        if (Combat.CombatWait == true)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer > Debug_Weapon.FPS(atk.Weapon))
            {
                GameObject bul = GameObject.Instantiate(Resources.Load("Assets/Prefabs/Bullet"),
                    atk.weapon.Barrel.transform.position, Quaternion.identity) as GameObject;
                bul.GetComponent<Bullet>().RotateDirection(-(atk.weapon.transform.position - def.transform.position));
                
                fireCount++;
                fireTimer = 0;
                if (fireCount == atk.weapon.Rolls)
                {
                    Combat.CombatWait = false;
                }
            }
        }

    }

     */
}
