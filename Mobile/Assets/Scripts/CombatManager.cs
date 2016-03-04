using UnityEngine;
using System;
using System.Collections;

public class CombatManager : MonoBehaviour{

    public GameManager game;

    public bool CombatWait;

    //last combat info
    Entity atk;
    Entity def;
    int rolls;
    int currShot = 0;
    float fireTime = 0;
    int[] atkR;
    int[] defR;

    public void Fight(GameManager g, Entity p, Entity e)
    {
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
                if (Hit(currShot))
                {
                    Debug.Log("Hit");
                    Spawner.SpawnSmoke(def.transform.position, 0.5f);
                    def.Health--;
                    if (def.Name.Contains("Drone"))
                    {
                        game.UIManager.setEnemyUI((Enemy)def);
                        if (game.CheckEnemies())
                            CombatWait = false;
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
                    CombatWait = false;
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
