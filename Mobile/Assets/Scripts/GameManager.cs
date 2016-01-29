using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public BoardManager Board;
    public Player Player;

    public GameObject Tile;

    public Player getCurrentPlayer()
    {
        return Player;
    }

    void SetupGame()
    {
        Board.gameObject.GetComponent<BoardCreator>().BuildBoard();

        
    }

    public void PlayerSetup()
    {
        Player.PlacePlayer(Board.getRandomTile(), 0);
        Camera.main.GetComponent<CameraControl>().CenterCamera(Player.tileID);
    }

    public void MovePlayer(Tile moveTo)
    {
        Tile curr = Board.getTileAtID(getCurrentPlayer().tileID).GetComponent<Tile>();
        float d = Board.selectStageFrom(moveTo.ID, curr.ID);
        Debug.Log(d);
        Player.PlacePlayer(moveTo.gameObject, (int)d);
    }

	// Use this for initialization
	void Start () {
        SetupGame();
	}
}
