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
        Border,
    }

    public GameObject selected;
    public bool Selected()
    {
        return transform.FindChild("Selected").gameObject.activeSelf;
    }
    public bool Target()
    {
        return transform.FindChild("XHair").gameObject.activeSelf;
    }

    public TileStates State;
    public int ID;

    public Vector3 Cube;

    public bool Occupied = false;
    public bool Chest;

    public void DropChest()
    {
        Chest = true; 
        transform.FindChild("ChestDrop").gameObject.SetActive(true);
    }

    public void TouchedTile()
    {
        transform.FindChild("SelectHex").gameObject.SetActive(true);
    }
    public void Crosshair(bool setup)
    {
        transform.FindChild("XHair").gameObject.SetActive(setup);
    }

    public void Reset()
    {
        transform.FindChild("Selected").gameObject.SetActive(false);
        transform.FindChild("UILayer").gameObject.SetActive(false);
        transform.FindChild("Range").gameObject.SetActive(false);
        transform.FindChild("SelectHex").gameObject.SetActive(false);
        Crosshair(false);
        uiState = TileUIState.None;
    }

    public void ResetUI()
    {
        transform.FindChild("UILayer").gameObject.SetActive(false);
        uiState = TileUIState.None;
    }
}
