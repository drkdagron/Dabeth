using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum TileUIState
    {
        Cancel,
        Accept,
        None
    };
    public TileUIState uiState;

    public enum TileStates
    {
        Open,
        Closed,
        Accept,
        Cancel,
        Fog,
    }

    public GameObject selected;
    public bool Selected()
    {
        return transform.FindChild("Selected").gameObject.activeSelf;
    }

    public TileStates State;
    public Vector2 BoardPosition;
    public int ID;

    public bool Occupied = false;
    public bool Chest;

    public int[] Neighbours = {-1,-1,-1,-1,-1,-1};

    public void DropChest()
    {
        Chest = true; 
        transform.FindChild("ChestDrop").gameObject.SetActive(true);
    }

    public void Reset()
    {
        transform.FindChild("Selected").gameObject.SetActive(false);
        transform.FindChild("UILayer").gameObject.SetActive(false);
        transform.FindChild("Range").gameObject.SetActive(false);
        uiState = TileUIState.None;
    }

    public void ResetUI()
    {
        transform.FindChild("UILayer").gameObject.SetActive(false);
        uiState = TileUIState.None;
    }
}
