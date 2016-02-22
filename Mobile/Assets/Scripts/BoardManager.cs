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
    public void setTileUI(bool act, Vector3 p, UISprite ui)
    {
        GameObject t = getTileAtCube(p);
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
    public GameObject getUnoccupiedTile()
    {
        bool open = false;
        while (open == false)
        {
            int i = getRandom();
            Tile t = getTileAtID(i).GetComponent<Tile>();
            if (t.State != Tile.TileStates.Closed)
            {
                if (t.Occupied == false)
                {
                    return getTileAtID(i);
                }
            }
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
    public GameObject getTileAtID(int id)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<Tile>().ID == id)
                return Tiles[i];
        }

        return null;
    }
    public GameObject getTileAtCube(int x, int y, int z)
    {
        for (int i= 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<Tile>().Cube == new Vector3(x, y, z))
                return Tiles[i];
        }
        return null;
    }
    public GameObject getTileAtCube(Vector3 cube)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<Tile>().Cube == cube)
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

    public void selectMoveTiles(int id, int stage) //id is the id of the tile of which you want to select, stage is the number of tiles around you want selected;
    {
        List<int> tiles = selectTilesAround(id, stage);
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile t = getTileAtID(tiles[i]).GetComponent<Tile>();
            if (id != t.ID)
            {
                if (t.State == Tile.TileStates.Open)
                    getTileAtID(tiles[i]).transform.FindChild("Selected").gameObject.SetActive(true);
            }
        }
    }

    public void selectRangeTiles(int id, int stage)
    {
        List<int> tiles = selectTilesAround(id, stage);
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile t = getTileAtID(tiles[i]).GetComponent<Tile>();
            //if (id != t.ID)
            //{
            //    getTileAtID(tiles[i]).transform.FindChild("Range").gameObject.SetActive(true);
            //}
        }
        Debug.Log("Done");
    }
    public void selectTiles(Vector2 p, int stage)
    {
        //selectTiles(getTileAtPos(p).GetComponent<Tile>().ID, stage);
    }

    public List<int> selectTilesAround(int id, int range)
    {
        List<int> tiles = new List<int>();      
        List<int> tRange = new List<int>();
        tiles.Add(getTileAtID(id).GetComponent<Tile>().ID);
        for (int i= 0; i < range; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                List<int> tmp = tilesAround(tiles[j]);

                for (int k = tmp.Count - 1; k > -1; k--)
                {
                    if (tiles.Contains(tmp[k]))
                        tmp.RemoveAt(k);
                }

                tRange.AddRange(tmp);
            }

            tiles.AddRange(tRange);
        }
        tiles.Remove(0);
        Debug.Log(tiles.Count);
        return tiles;
    }
    public List<int> selectTileRingAt(int id, int range)
    {
        List<int> t = new List<int>();      //this the list of tiles we dont want
        List<int> tiles = new List<int>();  //this is the list of tiles we want to return
        t.AddRange(tilesAround(id));
        for (int i= 1; i < range; i++)
        {
            Debug.Log("Ring: i = " + i + ",  range = " + range);
            for (int j = t.Count-1; j > -1; j--)
            {
                List<int> tmp = tilesAround(t[j]);
                for (int k = 0; k < tmp.Count; k++)
                {
                    if (!t.Contains(tmp[k]))
                    {
                        if (i + 1 == range)
                        {
                            tiles.Add(tmp[k]);
                        }

                        t.Add(tmp[k]);
                    }

                }

            }
            Debug.Log("Tile Count: " + t.Count);
        }

        Debug.Log("ENDING TILE COUNT: " + tiles.Count);
        return tiles;
    }

    public float TilePosDistanceBetween(int from, int to)
    {
        Tile t1 = getTileAtID(to).GetComponent<Tile>();
        Tile t2 = getTileAtID(from).GetComponent<Tile>();

        float x = Math.Abs(t1.Cube.x - t2.Cube.x);
        float y = Math.Abs(t1.Cube.y - t2.Cube.y);
        float z = Math.Abs(t1.Cube.z - t2.Cube.z);

        if (x >= y && x >= z)
            return x;
        if (y >= x && y >= z)
            return y;
        if (z >= x && z >= y)
            return z;

        return 0;
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
        Vector3[] neigh = new Vector3[6] {new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1),
        new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1)};

        Tile t = getTileAtID(id).GetComponent<Tile>();
        for (int i= 0; i < neigh.Length; i++)
        {
            GameObject tObj = getTileAtCube(t.Cube + neigh[i]);
            if (tObj != null)
                tiles.Add(tObj.GetComponent<Tile>().ID);
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
        GameObject t2 = getTileAtCube(t.GetComponent<Tile>().Cube + new Vector3(-1, 1, 0));
        setTileUI(true, t2.GetComponent<Tile>().ID, UISprite.No);
        GameObject t3 = getTileAtCube(t.GetComponent<Tile>().Cube + new Vector3(1,-1, 0));
        setTileUI(true, t3.GetComponent<Tile>().ID, UISprite.Yes);
    }
}
