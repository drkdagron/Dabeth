using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardCreator : MonoBehaviour {

    public int XTiles;
    public int YTiles;

    public GameObject Tile;
    public Transform parent;
    public GameManager game;

    public BoardManager board;

    public Sprite[] TileArt;
    public enum Tiles
    {
        grass,
        wall,
        water
    };

	// Use this for initialization

    public void BuildBoard()
    {
        Debug.Log("Building Board");
    }
	void Start () {

        //grass and wall tile creation
        int count = 0;
	    for (int i= 0; i < YTiles; i++)
        {
            for (int j = 0; j < XTiles; j++)
            {
                GameObject obj;
                if (i % 2 == 0)
                    obj = (GameObject)Instantiate(Tile, new Vector3(3 + j * 1.2f, i * 1.03f + 3, 0), Quaternion.identity);
                else
                    obj = (GameObject)Instantiate(Tile, new Vector3(j * 1.2f + 3.6f, i * 1.03f + 3, 0), Quaternion.identity);

                if (i == 0 || j == 0 || i == YTiles - 1 || j == XTiles - 1)
                {
                    obj.GetComponent<Tile>().State = global::Tile.TileStates.Closed;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.wall];
                }
                else
                {
                    obj.GetComponent<Tile>().State = global::Tile.TileStates.Open;
                }
                obj.transform.parent = parent;
                obj.GetComponent<Tile>().BoardPosition = new Vector2(i, j);
                obj.GetComponent<Tile>().ID = count++;
            }
        }
        CameraControl cc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        cc.XMax = (XTiles - 1) * 1.2f + 0.6f + 6;
        cc.YMax = (YTiles - 1) * 1.03f + 6;

        GetComponent<BoardManager>().Setup(XTiles, YTiles, GameObject.FindGameObjectsWithTag("Tile"));
        
        //spawn ponds
        int water = Random.Range(0, 3);
        Debug.Log("Number of Ponds: " + water);
        int size = Random.Range(1, 6);
        Debug.Log("Size of Ponds: " + size);
        for (int i = 0; i < water; i++)
        {
            GameObject t = GetComponent<BoardManager>().getRandomTile();
            List<int> tile = GetComponent<BoardManager>().selectTilesAround(t.GetComponent<Tile>().ID, size);
            for (int j = 0; j < tile.Count; j++)
            {
                board.getTileAtID(tile[i]).GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.water];
                board.getTileAtID(tile[i]).GetComponent<Tile>().State = global::Tile.TileStates.Closed;
            }
        }
        game.PlayerSetup();
        Destroy(this);
	}
}
