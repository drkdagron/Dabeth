using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

    public enum SelectedMode
    {
        Move,
        None,
        AcceptMove
    };
    public SelectedMode mode = SelectedMode.None;

    public float moveSpeed;
    public float zoomRate;

    public Transform camTran;
    public Camera cam;
    public BoardManager board;

    public GameManager game;

    public float XMin = 0;
    public float XMax = 0;
    public float YMin;
    public float YMax;

    public float ZMin = 2;

    public bool Control;

    public int selectedTile = -1;
    public bool Selected
    {
        get { if (selectedTile > 0) return true; else return false; }
    }

	// Use this for initialization
	void Start () {
        camTran = GetComponent<Transform>();
        cam = GetComponent<Camera>();
        Move(0, 0, 0);

        mode = SelectedMode.None;
	}
	
    void Move(Vector3 dir)
    {
        Vector3 prev = camTran.position;
        camTran.position = Vector3.Lerp(prev, prev + (dir * moveSpeed), 0.5f);
        //camTran.Translate(dir * moveSpeed);
        Vector3 p = camTran.position;
        if (p.x - Extents.x < 0)
            p.x = Extents.x;
        if (p.y - Extents.y < 0)
            p.y = Extents.y;
        if (p.x + Extents.x > XMax)
            p.x = XMax - Extents.x;
        if (p.y + Extents.y > YMax)
            p.y = YMax - Extents.y;
        camTran.position = p;
    }
    void Move(float x, float y, float z)
    {
        Move(new Vector3(x, y, z));
    }

    void Zoom(float v)
    {
        if (Extents.x * 2 < XMax && v > 0 || v < 0)
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize += (v * 0.25f) * zoomRate, ZMin, float.MaxValue);
        Move(0, 0, 0);
    }

	// Update is called once per frame
	void FixedUpdate () {
	    if (Input.GetKey(KeyCode.A))
        {
            Move(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Move(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(0, -1, 0);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            Zoom(1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Zoom(-1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction);
            if (hit.collider.tag == "Tile")
            {
                Tile t = hit.collider.GetComponent<Tile>();
                if (mode == SelectedMode.None)
                {
                    Debug.Log(t.ID);
                    if (game.getCurrentPlayer().tileID == t.ID)
                    {
                        board.selectTiles(hit.collider.GetComponent<Tile>().ID, game.getCurrentPlayer().MoveLeft);
                        mode = SelectedMode.Move;
                    }
                    selectedTile = hit.collider.GetComponent<Tile>().ID;
                }
                else if (mode == SelectedMode.Move)
                {
                    if (t.Selected())
                    {
                        board.buildAccept(t.ID);
                        mode = SelectedMode.AcceptMove;
                        selectedTile = t.ID;
                    }
                }
                else if (mode == SelectedMode.AcceptMove)
                {
                    if (t.uiState == Tile.TileUIState.Accept)
                    {
                        game.MovePlayer(board.getTileAtID(selectedTile).GetComponent<Tile>());
                        board.DeselectTiles(true, true);
                        mode = SelectedMode.None;
                    }
                    else if (t.uiState == Tile.TileUIState.Cancel)
                    {
                        Debug.Log("Cancel Hit");
                        board.DeselectTiles(false, true);
                    }
                    else if (t.Selected())
                    {
                        board.DeselectTiles(false, true);
                        board.buildAccept(t.ID);
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            board.DeselectTiles(true, false);
            selectedTile = -1;
            mode = SelectedMode.None;
        }
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            Move(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
        }
        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            Zoom(-Input.GetAxis("Mouse Y"));
        }

        //stage tests
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(board.selectStageFrom(40, 35));
        }

        if (Control)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        Touch t = Input.GetTouch(0);
                        Vector3 p = Input.GetTouch(0).deltaPosition.normalized * 3;
                        Move(-p.x, -p.y, 0);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        Ray r = cam.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit2D tHit = Physics2D.Raycast(r.origin, r.direction);
                        if (tHit.collider.tag == "Tile")
                        {
                            //center camera on tile
                            CenterCamera(tHit.collider.GetComponent<Tile>().ID);
                            Debug.Log("Hit Tile: " + tHit.collider.GetComponent<Tile>().ID);
                        }
                    }
                }
                else if (Input.touchCount == 2)
                {
                    Vector2 t1 = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
                    Vector2 t2 = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

                    float prevTouch = (t1 - t2).magnitude ;
                    float touch = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                    float deltaDiff = prevTouch - touch;

                    Zoom(deltaDiff);
                }
            }
        }
  
	}

    public void CenterCamera(int id)
    {
        cam.transform.position = board.getTileAtID(id).transform.position + new Vector3(0,0,-10);
        Debug.Log("Camera Moveto: " + board.getTileAtID(id).transform.position);
        Move(0, 0, 0);
    }

    void OnDrawGizmos()
    {
        Ray r = new Ray(GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, 0, 0)), Vector3.forward);
        Gizmos.DrawRay(r);

        Ray r2 = new Ray(GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, GetComponent<Camera>().pixelHeight, 0)), Vector3.forward);
        Gizmos.DrawRay(r2);
    }

    Vector3 MinBounds
    {
        get { return cam.transform.position - Extents; }
    }
    Vector3 MaxBounds
    {
        get { return cam.transform.position + Extents; }
    }

    Vector3 Extents
    {
        get { return new Vector2(cam.orthographicSize * Screen.width / Screen.height, cam.orthographicSize); }
    }

}
