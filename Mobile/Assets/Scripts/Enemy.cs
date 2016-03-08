using UnityEngine;
using System.Collections.Generic;

public class Enemy : Entity {

    public Player Target;
    public EnemyManager manager;

    public enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    };
    public EnemyState AIState;


    public bool PlayTurn()
    {
        //check Chase
        CheckState();

        //play turn
        switch (AIState)
        {
            case EnemyState.PATROL:
                AIPatrol();
                break;
            case EnemyState.CHASE:
                AIChase();
                break;
            case EnemyState.ATTACK:
                AIAttack();
                break;
        }

        CheckState();

        return true;
    }

    void CheckState()
    {
        float dist = game.Board.TilePosDistanceBetween(tileID, game.player.tileID);
        //if in range, make target change state to chase
        if (dist <= Debug_Weapon.Range(Weapon))
        {
            manager.PlayerFound();
            AIState = EnemyState.ATTACK;
            game.combatManager.Fight(game, this, game.player);
            game.UIManager.setHealthBar(game.player);
        }
        else
        {
            manager.PlayerLost();
            AIState = EnemyState.PATROL;
        }
    }

    void AIPatrol()
    {
        
        bool done = false;
        int t = 0;
        List<int> tries = new List<int>();
        int size = 3;
        List<int> tiles = game.Board.selectTileRingAt(tileID, size);
        while (done == false)
        {
            t = Random.Range(0, tiles.Count);

                Tile obj = game.Board.getTileAtID(t).GetComponent<Tile>();
                if (obj.Occupied == false || obj.State != Tile.TileStates.Closed)
                    done = true;

                if (tries.Count == tiles.Count)
                {
                    tiles.Clear(); tries.Clear();
                    size--;
                    Debug.Log("Size getting smaller: " + size);
                    if (size == 0)
                    {
                        t = -1;
                        done = true;
                    }
                    tiles = game.Board.selectTileRingAt(tileID, size);
                }
        }
        if (t != -1)
        {
            game.MoveEnemy(gameObject, tiles[t]);
            CheckState();
        }
    }
    void AIChase()
    {
        List<int> tiles = game.Board.selectTileRingAt(tileID, 3);
        bool checkRange = false;
        for (int i= 0; i < tiles.Count; i++)
        {
            if (tiles[i] == game.player.GetComponent<Entity>().tileID)
            {
                checkRange = true;
            }
        }
        if (checkRange == true)
        {
            tiles.Clear();
            tiles = game.Board.selectTileRingAt(tileID, 2);
        }
        int tileMove = -1;
        float distance = float.MaxValue;
        for (int i = 0; i < tiles.Count; i++)
        {
            float d = Vector3.Distance(game.Board.getTileAtID(game.player.tileID).transform.position, game.Board.getTileAtID(tiles[i]).transform.position);
            if (d < distance)
            {
                tileMove = tiles[i];
                distance = d;
            }
        }
         
        game.MoveEnemy(gameObject, tileMove);
        CheckState();
    }
    void AIAttack()
    {

    }

}
