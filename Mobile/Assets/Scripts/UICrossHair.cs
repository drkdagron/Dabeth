using UnityEngine;
using System.Collections;

public class UICrossHair : MonoBehaviour {

    public float XHairRate = 10;

	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(new Vector3(0, 0, 1), XHairRate * Time.deltaTime);
	}
}
