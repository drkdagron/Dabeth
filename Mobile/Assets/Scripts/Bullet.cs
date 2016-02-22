using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float Speed = 5;
    public Vector3 Dir;

    public void RotateDirection(Vector3 Direction)
    {
        Dir = Direction;
        Dir.z = 0;
        Dir = Vector3.Normalize(Dir);
        //float f = Mathf.Atan2(Dir.y, Dir.x);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * f));
    }

	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Dir * Speed * Time.deltaTime);
	}
}
