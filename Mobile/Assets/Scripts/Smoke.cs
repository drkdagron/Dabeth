using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour {

    public SpriteRenderer SpriteRender;

    public Sprite[] Sprites;
    public int CurrentSprite = 0;
    public float FPSTimer = 0.5f;
    public float Timer = 0.0f;

    public bool OneShot = false;
	
    void Start()
    {
        SpriteRender.sprite = Sprites[CurrentSprite];
    }

	// Update is called once per frame
	void FixedUpdate () {
        Timer += Time.deltaTime;
        if (Timer > FPSTimer)
        {
            CurrentSprite++;
            SpriteRender.sprite = Sprites[CurrentSprite];
            float r = (float)CurrentSprite / (float)Sprites.Length;
            Color c = new Color(255, 255, 255, 100);
            SpriteRender.color = new Color(255, 255, 255, 1 - r);
            Timer = 0;

            if (CurrentSprite >= Sprites.Length - 1)
                Destroy(this.gameObject);
            if (OneShot == false)
                CurrentSprite %= Sprites.Length;
        }
	}
}
