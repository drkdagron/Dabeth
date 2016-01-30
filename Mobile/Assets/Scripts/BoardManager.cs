using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardManager : MonoBehaviour
{
    public GameObject[] Tiles;
    public int xTiles;
    public int yTiles;

    public enum UISprite
    {
        No = 0,
        Yes = 1,
    };
    public Sprite[] UISpriteTiles;

    public void setTileUI(bool act, int id, UISprite ui)
    {
        GameObject t = getTileAtID(id);
        t.transform.FindChild("UILayer").gameObject.SetActive(act);
        t.GetComponent<Tile>().uiState = (Tile.TileUIState)ui;
        t.transform.FindChild("UILayer").GetComponent<SpriteRenderer>().sprite = UISpriteTiles[(int)ui];
    }
    public void setTileUI(bool act, Vector2 p, UISprite ui)
    {
        GameObject t = getTileAtPos(p);
        t.transform.FindChild("UILayer").gameObject.SetActive(act);
        t.GetComponent<Tile>().uiState = (Tile.TileUIState)ui;
        t.transform.FindChild("UILayer").GetComponent<SpriteRenderer>().sprite = UISpriteTiles[(int)ui];
    }

    public int getRandom()
    {
        int tmp = UnityEngine.Random.Range(0, Tiles.Length);
        return tmp;
    }
    public GameObject getRandomTile()
    {
        bool open = false;
        while (open == false)
        {
            int i = getRandom();
            if (getTileAtID(i).GetComponent<Tile>().State != Tile.TileStates.Closed)
                return getTileAtID(i);
        }
        return null;
    }

    public void Setup(int x, int y, GameObject[] list)
    {
        Tiles = new GameObject[x * y];
        Debug.Log("Tile Count: " + list.Length);
        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i] = list[i];
        }
    }

    #region Tile Getter
    public GameObject getTileAtPos(Vector2 p)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<Tile>().BoardPosition == p)
                return Tiles[i];
        }

        return null;
    }
    public GameObject getTileAtID(int id)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<Tile>().ID == id)
                return Tiles[i];
        }

        return null;
    }
    #endregion

    #region TileUI Checker
    public bool getUIActiveTile(int id)
    {
        return getTileAtID(id).transform.FindChild("UIStyle").gameObject.activeSelf;
    }
    public bool getUIActiveTile(Vector2 p)
    {
        return getTileAtPos(p).transform.FindChild("UIStyle").gameObject.activeSelf;
    }
    #endregion

    public void DeselectTiles(bool select, bool ui)
    {
        for (int i= 0; i < Tiles.Length; i++)
        {
            if (select)
                Tiles[i].GetComponent<Tile>().Reset();
            if (ui)
                Tiles[i].GetComponent<Tile>().ResetUI();
        }
    }

    public void selectTiles(int id, int stage) //id is the id of the tile of which you want to select, stage is the number of tiles around you want selected;
    {
        Debug.Log("SelectTiles Called");
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (id != i)
            {
                if (Tiles[i].GetComponent<Tile>().State == Tile.TileStates.Open)
                {
                    Debug.Log("Tile Open");
                    if (Vector2.Distance(getTileAtID(id).transform.position, getTileAtID(i).transform.position) < stage * 1.25f)
                    {
                        Debug.Log("Tile Close Enough");
                        Tiles[i].transform.FindChild("Selected").gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void selectTiles(Vector2 p, int stage)
    {
        selectTiles(getTileAtPos(p).GetComponent<Tile>().ID, stage);
    }

    public List<int> selectTilesAround(int id, int range)
    {
        List<int> tiles = new List<int>();
        List<int> tmp = new List<int>();
        tiles.Add(getTileAtID(id).GetComponent<Tile>().ID);
        for (int i= 0; i < range; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                List<int> t = tilesAround(tiles[i]);

                for (int k = 0; k < t.Count; k++)
                {
                    if (!tiles.Contains(t[k]))
                        tmp.Add(t[k]);
                }
            }

            tiles.AddRange(tmp);
        }
        Debug.Log(tiles.Count);
        return tiles;
    }

    public int selectStageFrom(int idTo, int idFrom)
    {
        int total = 0;
        int newStage = 1;
        List<int> ids = new List<int>();
        ids.Add(idFrom);
        List<int> newT = new List<int>();
        while (true)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                List<int> tmpT = tilesAround(ids[i]);

                for (int j = 0; j < tmpT.Count; j++)
                {
                    if (tmpT[j] != idTo)
                    {
                        if (!newT.Contains(tmpT[j]))
                            newT.Add(tmpT[j]);
                    }
                    else
                    {
                        Debug.Log(total);
                        return newStage;
                    }
                    
                }
            }
            ids.RemoveRange(0, ids.Count);
            ids.AddRange(newT);
            newStage++;
        }

        //selectStageFrom(newT, idTo, -1, newStage);
    }

    /*
    public int selectStageFrom(int idTo, int idFrom)
    {
        bool found = false;
        List<int> check = new List<int>();
        check.Add(idTo);
        int count = 1;
        List<int> checker = new List<int>();
        while (!found)
        {

            for (int i = 0; i < check.Count; i++)
            {
                List<int> tID = tilesAround(check[i]);
                for (int j = 0; j < tID.Count; j++)
                {
                    if (!checker.Contains(tID[j]))
                    {
                        Debug.Log("ADDING CHECKER #: " + tID[j]);
                        checker.Add(tID[j]);
                    }
                }
            }


            for (int i= 0; i < checker.Count; i++)
            {
                if (checker[i] == idTo)
                {
                    Debug.Log("CHECK SIZE: " + check.Count);
                    Debug.Log("RETURNING: " + count);
                    return count;
                }
                else
                {
                    //Debug.Log("Checking to CHECK: " + checker[i]);
                    if (!check.Contains(checker[i]))
                    {
                        Debug.Log("ADDING TO CHECK: " + checker[i]);
                        check.Add(checker[i]);
                    }
                }
            }
            checker.RemoveRange(0, checker.Count);
            count++;
        }
        
        return 0;
    }
    */
    public List<int> tilesAround(int id)
    {
        List<int> tiles = new List<int>();
        Tile center = getTileAtID(id).GetComponent<Tile>();

        if (center.BoardPosition.y > 1 || center.BoardPosition.y < yTiles -1)
        {
            if (getTileAtPos(center.BoardPosition + new Vector2(0, -1)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(new Vector2(center.BoardPosition.x, center.BoardPosition.y - 1)).GetComponent<Tile>().ID);
            if (getTileAtPos(center.BoardPosition + new Vector2(0, 1)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(new Vector2(center.BoardPosition.x, center.BoardPosition.y + 1)).GetComponent<Tile>().ID);
        }

        if (center.BoardPosition.x % 2 == 0)
        {
            if (getTileAtPos(center.BoardPosition + new Vector2(-1, -1)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(-1, -1)).GetComponent<Tile>().ID);    //down left
            if (getTileAtPos(center.BoardPosition + new Vector2(-1, 0)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(-1, 0)).GetComponent<Tile>().ID);    //down right
            if (getTileAtPos(center.BoardPosition + new Vector2(1, -1)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(1, -1)).GetComponent<Tile>().ID);          //up left
            if (getTileAtPos(center.BoardPosition + new Vector2(1, 0)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(1, 0)).GetComponent<Tile>().ID);          //down right
        }
        else
        {
            if (getTileAtPos(center.BoardPosition + new Vector2(-1, 0)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(-1, 0)).GetComponent<Tile>().ID);    //down left
            if (getTileAtPos(center.BoardPosition + new Vector2(-1, 1)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(-1, 1)).GetComponent<Tile>().ID);    //down right
            if (getTileAtPos(center.BoardPosition + new Vector2(1, 0)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(1, 0)).GetComponent<Tile>().ID);          //up left
            if (getTileAtPos(center.BoardPosition + new Vector2(1, 1)).GetComponent<Tile>().State == Tile.TileStates.Open)
                tiles.Add(getTileAtPos(center.BoardPosition + new Vector2(1, 1)).GetComponent<Tile>().ID);          //down right
        }


        return tiles;
    }
    

    bool isGoodPosition(float x, float y)
    {
        if (x < 1 || y < 1)
            return false;
        if (x >= xTiles || y >= yTiles)
            return false;

        return true;
    }

    public void buildAccept(int id)
    {
        GameObject t = getTileAtID(id);
        GameObject t2 = getTileAtPos(t.GetComponent<Tile>().BoardPosition + new Vector2(0, -1));
        setTileUI(true, t2.GetComponent<Tile>().ID, UISprite.No);
        GameObject t3 = getTileAtPos(t.GetComponent<Tile>().BoardPosition + new Vector2(0,1));
        setTileUI(true, t3.GetComponent<Tile>().ID, UISprite.Yes);
    }
}
