using UnityEngine;
using System.Collections.Generic;
public class EnemyManager : MonoBehaviour {

    public bool Playing;
    public GameManager Game;
    public List<GameObject> Enemies;

    public int curAI = 0;
    public float TimePerTurn;
    public float Timer;

    public GameObject EnemyOnTile(int id)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].GetComponent<Enemy>().tileID == id)
            {
                return Enemies[i];
            }
        }

        return null;
    }

    public void Init(int count)
    {
        Enemies = new List<GameObject>();
        for (int i= 0; i < count; i++)
        {
            GameObject tile = Game.Board.getUnoccupiedTile();
            tile.GetComponent<Tile>().Occupied = true;
            Enemies.Add(Instantiate(Resources.Load("Assets/Prefabs/Enemy"), tile.transform.position, Quaternion.identity) as GameObject);
            Enemies[i].GetComponent<Entity>().game = Game;
            Enemies[i].GetComponent<Entity>().Weapon = Debug_Weapon.WeaponType.Pistol;
            Enemies[i].GetComponent<Entity>().tileID = tile.GetComponent<Tile>().ID;
            Enemies[i].GetComponent<Entity>().Name = "Drone " + i;
            Enemies[i].GetComponent<Enemy>().manager = this;
        }
    }

    public void PlayTurn()
    {
        for (int i= 0; i < Enemies.Count; i++)
        {
            Enemies[i].GetComponent<Enemy>().PlayTurn();
        }

        Game.NextTurn();
    }

    public void PlayerFound()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].GetComponent<Enemy>().AIState = Enemy.EnemyState.CHASE;
        }
    }

    public void PlayerLost()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].GetComponent<Enemy>().AIState = Enemy.EnemyState.PATROL;
        }
    }

    void Update()
    {
        if (Playing)
        {
            Timer += Time.deltaTime;
            if (Timer > TimePerTurn)
            {
                if (curAI < Enemies.Count)
                {
                    Enemies[curAI].GetComponent<Enemy>().PlayTurn();
                    curAI++;
                    Timer = 0;
                }
                else
                {
                    Playing = false;
                    Game.NextTurn();
                }
            }
        }
    }
}
