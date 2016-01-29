using UnityEngine;
using System.Collections;

public class BoardCreator : MonoBehaviour {

    public int XTiles;
    public int YTiles;

    public GameObject Tile;
    public Transform parent;
    public GameManager game;

	// Use this for initialization

    public void BuildBoard()
    {
        Debug.Log("Building Board");
    }
	void Start () {

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
                    obj.GetComponent<SpriteRenderer>().enabled = false;
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

        game.PlayerSetup();
        Destroy(this);
	}
}
