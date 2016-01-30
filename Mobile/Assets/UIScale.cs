using UnityEngine;
using System.Collections;

public class UIScale : MonoBehaviour {

    public int DefaultWidth = 800;
    public int DefaultHeight = 480;

    public float scaleX;
    public float scaleY;

	// Use this for initialization
	void Start () {
        scaleX = Screen.width / DefaultWidth;
        scaleY = Screen.height / DefaultHeight;
	}
}
