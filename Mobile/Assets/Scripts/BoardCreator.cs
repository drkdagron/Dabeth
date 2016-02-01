using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardCreator : MonoBehaviour {

    public Sprite[] TileArt;
    public enum Tiles
    {
        grass,
        wall,
        water
    };

	// Use this for initialization

	public GameObject[] BuildBoard (GameObject parent, int width, int height) {

        List<GameObject> tiles = new List<GameObject>();
        GameObject tileObj = Resources.Load("Assets/Prefabs/Tile") as GameObject;
        //grass and wall tile creation
        int count = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject obj;
                if (i % 2 == 0)
                    obj = (GameObject)Instantiate(tileObj, new Vector3(3 + j * 1.2f, i * 1.03f + 3, 0), Quaternion.identity);
                else
                    obj = (GameObject)Instantiate(tileObj, new Vector3(j * 1.2f + 3.6f, i * 1.03f + 3, 0), Quaternion.identity);

                if (i == 0 || j == 0 || i == height - 1 || j == width - 1)
                {
                    obj.GetComponent<Tile>().State = global::Tile.TileStates.Closed;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.wall];
                }
                else
                {
                    obj.GetComponent<Tile>().State = global::Tile.TileStates.Open;
                }
                obj.transform.parent = parent.transform;
                obj.GetComponent<Tile>().BoardPosition = new Vector2(i, j);
                obj.GetComponent<Tile>().ID = count++;
                tiles.Add(obj);
            }
        }
        
        
        //spawn ponds
        int water = Random.Range(0, 3);
        Debug.Log("Number of Ponds: " + water);
        int size = Random.Range(1, 6);
        Debug.Log("Size of Ponds: " + size);
        for (int i = 0; i < water; i++)
        {

        }
         

        return tiles.ToArray();
	}
}
