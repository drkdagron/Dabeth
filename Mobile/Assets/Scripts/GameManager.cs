using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public CameraControl control;
    public GameObject boardParent;
    public BoardManager Board;
    public Player player;
    public Player[] AI;

    public int BoardWidth;
    public int BoardHeight;

    public int currentTurn;     //this will keep track of which turn it is...
    public bool playerFirst;    //this will determine with currentTurn whos turn it is... if true  player = currentTurn %= 0 else player = currentTurn %= 1

    public Player getCurrentPlayer()
    {
        return player;
    }

    void SetupGame()
    {
        //Setup Board
        Board.Tiles = Board.gameObject.GetComponent<BoardCreator>().BuildBoard(boardParent, BoardWidth, BoardHeight);

        //Setup Camera
        Camera.main.GetComponent<CameraControl>().XMax = (BoardWidth - 1) * 1.2f + 0.6f + 6;
        Camera.main.GetComponent<CameraControl>().YMax = (BoardHeight - 1) * 1.03f + 6;

        //Setup Player
        GameObject pTmp = (GameObject)GameObject.Instantiate(Resources.Load("Assets/Prefabs/Player") as GameObject, Vector3.zero, Quaternion.identity);
        player = pTmp.GetComponent<Player>();
        player.Init(this, Board.getRandomTile());
        Camera.main.GetComponent<CameraControl>().CenterCamera(player.tileID);

        //Setup AI

        //Setup Game

        //Start Turn
        player.ResetTurn();
    }

    public void ShowPlayerMovement()
    {
        Player p = getCurrentPlayer();
        Board.selectMoveTiles(p.tileID, p.MoveLeft);
        control.mode = CameraControl.SelectedMode.Move;
    }

    public void MovePlayer(Tile moveTo)
    {
        Tile curr = Board.getTileAtID(getCurrentPlayer().tileID).GetComponent<Tile>();
        float d = Board.TilePosDistanceBetween(moveTo.ID, curr.ID);
        Debug.Log(d);
        player.PlacePlayer(moveTo.gameObject, (int)d);
    }

    public void ShowAttackDistance()
    {
        Weapon p = getCurrentPlayer().Weapon;
        
    }

    public void AttackPlayer(Player pAtk, Player pDef)
    {

    }

    public void PickupLoot(Player player, object item)
    {

    }

    public void DropLoot(GameObject Tile, object item)
    {

    }

    public void FinishGame()
    {

    }

    public void NextTurn()
    {
        player.ResetTurn();
    }

    public void QuitGame()
    {

    }

	// Use this for initialization
	void Start () {
        SetupGame();
	}
}
