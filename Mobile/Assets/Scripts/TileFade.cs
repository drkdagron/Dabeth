using UnityEngine;
using System.Collections;

public class TileFade : MonoBehaviour {

    public float fadeRate = 1.0f;
    public SpriteRenderer sprite;
	
	// Update is called once per frame
	void FixedUpdate () {
       
        //float f = Mathf.Sin(Time.time);
       // if (f < 0)
        ////    f *= -1;
        ///f *= 255;
        //Debug.Log(f);

        //sprite.color = new Color(255,255,255, f);
	}
}
