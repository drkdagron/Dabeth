using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameManager game;

    public int Health;
    public int Armor;

    public int tileID;

    public int MoveLeft;
    public int Move;

    public void ResetTurn()
    {
        Move = 3;
        MoveLeft = Move;
    }

    public void PlacePlayer(GameObject tile, int moves)
    {
        this.transform.position = tile.transform.position;
        tileID = tile.GetComponent<Tile>().ID;
        MoveLeft -= moves;
    }

	// Use this for initialization
	void Start () {
        MoveLeft = Move;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
