using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //Board and Manager
    public GameObject boardParent;
    public BoardManager Board;

    //Camera controller
    public CameraControl cam;

    //Players on board
    public Player player;
    public EnemyManager AI;
    public int Enemies;

    public int BoardWidth;
    public int BoardHeight;

    public int currentTurn = -1;     //this will keep track of which turn it is...
    public bool playerFirst;    //this will determine with currentTurn whos turn it is... if true  player = currentTurn %= 0 else player = currentTurn %= 1

    //UI Manager
    public GameObject[] UIPlayerOnly;
    public UIManager UIManager;

    public CombatManager combatManager;

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

    #region UI Controls

    public void UI_SelectTile(Tile id)
    {
        Board.DeselectTiles(true, true);            //reset the tiles on the board
        if (id.State != Tile.TileStates.Border)     //make sure selected tile is not a border
        {
            id.TouchedTile();                       //activate the select hex layer
            cam.selectedTile = id.ID;

            GameObject e = AI.EnemyOnTile(id.ID);
            if (e != null)
                UIManager.setEnemyUI(e.GetComponent<Enemy>());
            else
                UIManager.hideEnemyUI();
        }
    }

    public void UI_SelectMove(Tile id)
    {
        if (id.Selected())
        {
            Board.buildAccept(id.ID);
            cam.mode = CameraControl.SelectedMode.AcceptMove;
            cam.selectedTile = id.ID;
        }
    }

    public void UI_AcceptMove(Tile id)
    {
        MovePlayer(cam.selectedTile);
        if (player.MoveLeft == 0)
            UIManager.SetMoveUI(false);
        Board.DeselectTiles(true, true);
        cam.mode = CameraControl.SelectedMode.None;
    }

    public void UI_CancelMove(Tile id)
    {
        Board.DeselectTiles(false, true);
    }

    public void UI_ReselectMove(Tile id)
    {
        Board.DeselectTiles(false, true);
        Board.buildAccept(id.ID);
    }

    public void UI_SelectFire(Tile id)
    {
        if (id.Target())
        {
            UIManager.setEnemyUI(AI.EnemyOnTile(id.ID).GetComponent<Enemy>());
            Board.buildAccept(id.ID);
            cam.mode = CameraControl.SelectedMode.AcceptFire;
            cam.selectedTile = id.ID;
        }
    }

    public void UI_AcceptFire(Tile id)
    {
        combatManager.Fight(this, player, AI.EnemyOnTile(cam.selectedTile).GetComponent<Enemy>());
        UIManager.setEnemyUI(AI.EnemyOnTile(cam.selectedTile).GetComponent<Enemy>());
        Board.DeselectTiles(true, true);
        cam.mode = CameraControl.SelectedMode.None;
        UIManager.SetCombatUI(false);
        CheckEnemies();
    }

    public void UI_CancelFire(Tile id)
    {
        Board.DeselectTiles(false, true);
    }

    public void UI_ReselectFire(Tile id)
    {
        Board.DeselectTiles(false, true);
        Board.buildAccept(id.ID);
    }

    #endregion

    #region Player Movement

    public void ShowPlayerMovement()
    {
        if (cam.mode == CameraControl.SelectedMode.None || cam.mode == CameraControl.SelectedMode.Fire)
        {
            Board.DeselectTiles(true, true);

            Player p = getCurrentPlayer();
            Board.selectMoveTiles(p.tileID, p.MoveLeft);
            cam.mode = CameraControl.SelectedMode.Move;
            //cam.CenterCamera(p.tileID);
            UIManager.PlayerMove(p);
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
        UIManager.PlayerMove(getCurrentPlayer());
    }

    #endregion

    #region Enemy Movement

    public void MoveEnemy(GameObject obj, int moveTo)
    {
        Tile curr = Board.getTileAtID(obj.GetComponent<Enemy>().tileID).GetComponent<Tile>();
        Tile target = Board.getTileAtID(moveTo).GetComponent<Tile>();
        float d = Board.TilePosDistanceBetween(target.ID, curr.ID);
        obj.GetComponent<Enemy>().PlaceEntity(target.gameObject, (int)d);
    }

    #endregion

    public void AIStartTurn()
    {
        AI.Playing = true;
        AI.curAI = 0;
    }
    public void AICompletedTurn()
    {
        NextTurn();
    }

    #region Player Combat

    public void ShowPlayerAttack()
    {
        if (cam.mode == CameraControl.SelectedMode.None || cam.mode == CameraControl.SelectedMode.Move)
        {
            Board.DeselectTiles(true, true);

            for (int i = 0; i < AI.Enemies.Count; i++)
            {
                if (Board.TilePosDistanceBetween(getCurrentPlayer().tileID, AI.Enemies[i].GetComponent<Enemy>().tileID) <= getCurrentPlayer().weapon.RangeMax)
                {
                    Board.getTileAtID(AI.Enemies[i].GetComponent<Enemy>().tileID).GetComponent<Tile>().Crosshair(true);
                }
            }

            cam.mode = CameraControl.SelectedMode.Fire;
        }
        else
        {
            Board.DeselectTiles(true, false);
            cam.mode = CameraControl.SelectedMode.None;
        }
    }

    public void ShowAttackDistance()
    {
        if (cam.mode == CameraControl.SelectedMode.None || cam.mode == CameraControl.SelectedMode.Move)
        {
            Board.DeselectTiles(true, true);
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

    public void DebugAttack()
    {
        //Combat.Fight(this, player, AI.Enemies[0].GetComponent<Enemy>());
    }

    public void AttackPlayer(Player pAtk, Enemy pDef)
    {
        //Combat.Fight(this, pAtk, pDef);
    }

    #endregion

    #region Enemy Attack

    public bool CheckEnemies()
    {
        for (int i = AI.Enemies.Count - 1; i > -1; i--)
        {
            if (AI.Enemies[i].GetComponent<Enemy>().Health <= 0)
            {
                Spawner.SpawnExplosion(AI.Enemies[i].transform.position);
                GameObject.Destroy(AI.Enemies[i]);
                AI.Enemies.RemoveAt(i);
                UIManager.hideEnemyUI();
                return true;
            }
        }
        return false;
    }

    #endregion

    #region Loot Drop

    public void PickupLoot(Player player, object item)
    {

    }

    public void DropLoot(GameObject Tile, object item)
    {

    }

    #endregion

    #region Game States

    public void FinishGame()
    {

    }

    public void NextTurn()
    {
        Board.DeselectTiles(true, true);
        currentTurn++;
        UIManager.setTurnCounter();
        if (currentTurn % 2 == 0 && playerFirst || currentTurn % 2 == 1 && !playerFirst)
        {
            player.ResetTurn();
            UIManager.PlayerMove(player);
            //Debug.Log("Player Turn");
            UIManager.PlayerUI(true);
            UIManager.NextTurn();
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

    #endregion

    // Use this for initialization
	void Start () {
        SetupGame();
	}


    
}
