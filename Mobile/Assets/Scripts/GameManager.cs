using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public CameraControl cam;

    public GameObject boardParent;
    public BoardManager Board;
    public Player player;
    public EnemyManager AI;

    public int BoardWidth;
    public int BoardHeight;

    public int currentTurn = -1;     //this will keep track of which turn it is...
    public bool playerFirst;    //this will determine with currentTurn whos turn it is... if true  player = currentTurn %= 0 else player = currentTurn %= 1

    public GameObject[] UIPlayerOnly;
    public UIManager UIManager;

    public int Enemies;

    public int TestTile1;
    public int TestTile2;

    public void TestDistance()
    {
        Debug.Log("Distance: " + Vector3.Distance(Board.getTileAtID(TestTile1).transform.position, Board.getTileAtID(TestTile2).transform.position));
    }

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
        AI.Init(Enemies);

        //Setup Game

        //Start Turn
        NextTurn();
        UIManager.setUI(player);
    }

    public void ShowPlayerMovement()
    {
        if (cam.mode == CameraControl.SelectedMode.None)
        {
            Player p = getCurrentPlayer();
            Board.selectMoveTiles(p.tileID, p.MoveLeft);
            cam.mode = CameraControl.SelectedMode.Move;
            //cam.CenterCamera(p.tileID);
            UIManager.setMoveBar(p);
        }
        else
        {
            cam.mode = CameraControl.SelectedMode.None;
            Board.DeselectTiles(true, true);
        }
    }

    public void MovePlayer(int moveTo)
    {
        Tile curr = Board.getTileAtID(getCurrentPlayer().tileID).GetComponent<Tile>();
        Tile target = Board.getTileAtID(moveTo).GetComponent<Tile>();
        float d = Board.TilePosDistanceBetween(target.ID, curr.ID);
        player.PlaceEntity(target.gameObject, (int)d);
        UIManager.setMoveBar(getCurrentPlayer());
    }
    public void MoveEnemy(GameObject obj, int moveTo)
    {
        Tile curr = Board.getTileAtID(obj.GetComponent<Enemy>().tileID).GetComponent<Tile>();
        Tile target = Board.getTileAtID(moveTo).GetComponent<Tile>();
        float d = Board.TilePosDistanceBetween(target.ID, curr.ID);
        obj.GetComponent<Enemy>().PlaceEntity(target.gameObject, (int)d);
    }

    public void AIStartTurn()
    {
        AI.Playing = true;
        AI.curAI = 0;
    }

    public void AICompletedTurn()
    {
        NextTurn();
    }

    public void ShowPlayerAttack(Player p)
    {
        
    }

    public void ShowAttackDistance()
    {
        if (cam.mode == CameraControl.SelectedMode.None)
        {
            int range = Debug_Weapon.Range(getCurrentPlayer().Weapon);
            Board.selectRangeTiles(getCurrentPlayer().tileID, range);
            cam.mode = CameraControl.SelectedMode.Fire;
        }
        else
        {
            cam.mode = CameraControl.SelectedMode.None;
            Board.DeselectTiles(true, true);
        }
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
        currentTurn++;
        UIManager.setTurnCounter();
        if (currentTurn % 2 == 0 && playerFirst || currentTurn % 2 == 1 && !playerFirst)
        {
            player.ResetTurn();
            UIManager.setMoveBar(player);
            //Debug.Log("Player Turn");
            UIManager.PlayerUI(true);
        }
        else
        {
            AIStartTurn();
            //Debug.Log("AI Turn");
            UIManager.PlayerUI(false);
        }        
    }

    public void QuitGame()
    {

    }

	// Use this for initialization
	void Start () {
        SetupGame();
	}
}
