using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardCreator : MonoBehaviour {

    public Sprite[] TileArt;
    public enum Tiles
    {
        grass,
        wall,
        water,
        sand,
        forest,
        stone
    };

	// Use this for initialization

	public GameObject[] BuildBoard (GameObject parent, int width, int height) {

        List<GameObject> tiles = new List<GameObject>();
        GameObject tileObj = Resources.Load("Assets/Prefabs/Tile") as GameObject;

        float xOrg = Random.Range(0, 2500000);
        float yOrg = Random.Range(0, 2500000);
        float[,] perlin = new float[width, height];
        for (int i= 0; i < perlin.GetLength(0); i++)
        {
            for (int j= 0; j < perlin.GetLength(1); j++)
            {
                float xCor = xOrg + (float)i / width * 10f;
                float yCor = yOrg + (float)j / height * 10f;
                perlin[i, j] = Mathf.PerlinNoise(xCor, yCor);
            }
        }

        //grass and wall tile creation
        int count = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject obj;
                if (i % 2 == 0)
                    obj = (GameObject)Instantiate(tileObj, new Vector3(3 + j * 1.2f, i * 1.039230484541326f + 3, 0), Quaternion.identity);
                else
                    obj = (GameObject)Instantiate(tileObj, new Vector3(j * 1.2f + 3.6f, i * 1.039230484541326f + 3, 0), Quaternion.identity);

                if (i == 0 || j == 0 || i == height - 1 || j == width - 1)
                {
                    obj.GetComponent<Tile>().State = Tile.TileStates.Border;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.wall];
                }
                else if (perlin[j,i] > 0.775f)
                {
                    obj.GetComponent<Tile>().State = Tile.TileStates.Closed;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.stone];
                }
                else if (perlin[j,i] > 0.625f)
                {
                    obj.GetComponent<Tile>().State = Tile.TileStates.Open;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.forest];
                }
                else if (perlin[j,i] < 0.25f)
                {
                    obj.GetComponent<Tile>().State = Tile.TileStates.Closed;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.water];
                }
                else if (perlin[j,i] > 0.25f && perlin[j,i] < 0.325f)
                {
                    obj.GetComponent<Tile>().State = Tile.TileStates.Open;
                    obj.GetComponent<SpriteRenderer>().sprite = TileArt[(int)Tiles.sand];
                }
                else
                {
                    obj.GetComponent<Tile>().State = global::Tile.TileStates.Open;
                }
                obj.transform.parent = parent.transform;
                obj.GetComponent<Tile>().ID = count++;
                Debug.Log(i % 2);
                float x = j - ((i - (i % 2)) / 2);
                Debug.Log(x);
                obj.GetComponent<Tile>().Cube = new Vector3(x, (x * -1) - i, i);
                tiles.Add(obj);
            }
        }

        /*
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                if (i != j)
                {
                    float dist = Vector3.Distance(tiles[i].transform.position, tiles[j].transform.position);
                    if (dist < 1.5f)
                    {
                        bool space = true;
                        for (int k = 0; k < tiles[i].GetComponent<Tile>().Neighbours.Length; k++)
                        {
                            if (tiles[i].GetComponent<Tile>().Neighbours[k] == -1 && space == true)
                            {
                                tiles[i].GetComponent<Tile>().Neighbours[k] = tiles[j].GetComponent<Tile>().ID;
                                space = false;
                            }
                        }
                    }
                }
            }
        }
        */
            
        return tiles.ToArray();
	}
}
