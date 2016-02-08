using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

<<<<<<< HEAD
    public CameraControl cam;
=======
    public CameraControl control;
>>>>>>> bd21dd84efdca799f5d5a439b482adf80bb94f18
    public GameObject boardParent;
    public BoardManager Board;
    public Player player;
    public Player[] AI;

    public int BoardWidth;
    public int BoardHeight;

    public int currentTurn;     //this will keep track of which turn it is...
    public bool playerFirst;    //this will determine with currentTurn whos turn it is... if true  player = currentTurn %= 0 else player = currentTurn %= 1

    public UIPlayer currentUIPlayer;

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
        currentUIPlayer.setUI(player);
    }

    public void ShowPlayerMovement()
    {
<<<<<<< HEAD
        if (cam.mode == CameraControl.SelectedMode.None)
        {
            Player p = getCurrentPlayer();
            Board.selectMoveTiles(p.tileID, p.MoveLeft);
            cam.mode = CameraControl.SelectedMode.Move;
            cam.CenterCamera(p.tileID);
            currentUIPlayer.setMoveBar(p);
        }
        else
        {
            cam.mode = CameraControl.SelectedMode.None;
            Board.DeselectTiles(true, true);
        }
=======
        Player p = getCurrentPlayer();
        Board.selectMoveTiles(p.tileID, p.MoveLeft);
        control.mode = CameraControl.SelectedMode.Move;
>>>>>>> bd21dd84efdca799f5d5a439b482adf80bb94f18
    }

    public void MovePlayer(Tile moveTo)
    {
        Tile curr = Board.getTileAtID(getCurrentPlayer().tileID).GetComponent<Tile>();
        float d = Board.TilePosDistanceBetween(moveTo.ID, curr.ID);
        Debug.Log(d);
        player.PlacePlayer(moveTo.gameObject, (int)d);
        currentUIPlayer.setMoveBar(getCurrentPlayer());
    }

    public void ShowPlayerAttack(Player p)
    {
        
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
