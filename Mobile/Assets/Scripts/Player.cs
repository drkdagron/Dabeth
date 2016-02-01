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
        GameObject prev = game.Board.getTileAtID(tileID);
        Vector3 edge = tile.transform.position - prev.transform.position;
        float f = Mathf.Atan2(edge.y, edge.x);

        this.transform.position = tile.transform.position + new Vector3(0,0,-0.5f);
        tileID = tile.GetComponent<Tile>().ID;
        MoveLeft -= moves;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * f));
    }

    public void Init(GameManager game, GameObject tile)
    {
        this.game = game;
        this.transform.position = tile.transform.position + new Vector3(0, 0, -0.5f);
        this.tileID = tile.GetComponent<Tile>().ID;
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
