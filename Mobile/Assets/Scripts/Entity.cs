using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public GameManager game;
    public string Name;

    public int HealthTotal = 100;
    public int Health;

    public int Armor;

    public int tileID;

    public int MoveLeft;
    public int Move;

    bool Alive = true;

    public Debug_Weapon.WeaponType Weapon;
    public Weapon weapon;
    public Armor armor;

    public bool Fired = false;

    public void Heal(int val)
    {
        Health += val;
        if (Health > HealthTotal)
            Health = HealthTotal;
    }

    public void Init(GameManager game, GameObject tile, string name = "")
    {
        this.game = game;
        tile.GetComponent<Tile>().Occupied = true;
        this.transform.position = tile.transform.position + new Vector3(0, 0, -0.5f);
        this.tileID = tile.GetComponent<Tile>().ID;
        transform.rotation = Quaternion.Euler(0, 0, 90);
        Health = HealthTotal;
        this.Name = name;
    }

    public void PlaceEntity(GameObject tile, int moves)
    {
        GameObject prev = game.Board.getTileAtID(tileID);
        prev.GetComponent<Tile>().Occupied = false;
        Vector3 edge = tile.transform.position - prev.transform.position;
        float f = Mathf.Atan2(edge.y, edge.x);

        this.transform.position = tile.transform.position + new Vector3(0, 0, -0.5f);
        tileID = tile.GetComponent<Tile>().ID;
        MoveLeft -= moves;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * f));

        tile.GetComponent<Tile>().Occupied = true;
    }

    public void ResetTurn()
    {
        Move = 3;
        MoveLeft = Move;
        Fired = false;
    }
}