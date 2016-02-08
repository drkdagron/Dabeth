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
                    obj = (GameObject)Instantiate(tileObj, new Vector3(3 + j * 1.2f, i * 1.03f + 3, 0), Quaternion.identity);
                else
                    obj = (GameObject)Instantiate(tileObj, new Vector3(j * 1.2f + 3.6f, i * 1.03f + 3, 0), Quaternion.identity);

                if (i == 0 || j == 0 || i == height - 1 || j == width - 1)
                {
                    obj.GetComponent<Tile>().State = Tile.TileStates.Closed;
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
                obj.GetComponent<Tile>().BoardPosition = new Vector2(i, j);
                obj.GetComponent<Tile>().ID = count++;
                tiles.Add(obj);
            }
        }

            return tiles.ToArray();
	}
}
