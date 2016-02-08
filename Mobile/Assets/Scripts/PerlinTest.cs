using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class PerlinTest : MonoBehaviour
    {
        public int pixWidth = 200;
        public int pixHeight = 200;
        public float xOrg;
        public float yOrg;
        public float scale = 1.0f;
        private Texture2D noise;
        private Color[] pix;
        private Image rend;
        private Sprite test;

        public bool createNoise = false;

        void Start()
        {
            rend = GetComponent<Image>();
            noise = new Texture2D(pixWidth, pixHeight);
            pix = new Color[noise.width * noise.height];
            test = new Sprite();
            rend.sprite = test;
        }

        void CalcNoise()
        {
            Sprite newSprite = new Sprite();
            float y = 0.0f;
            while (y < noise.height)
            {
                float x = 0.0f;
                while (x < noise.width)
                {
                    float xCor = xOrg + x / noise.width * scale;
                    float yCor = yOrg + y / noise.height * scale;
                    float sample = Mathf.PerlinNoise(xCor, yCor);
                    pix[(int)(y * noise.width + x)] = new Color(sample, sample, sample);
                    Debug.Log("Perlin Noise (" + x + ", " + y + "): " + sample);
                    x++;
                }
                y++;
            }
            newSprite.texture.SetPixels(pix);
            newSprite.texture.Apply();
            rend.sprite = newSprite;
            createNoise = false;
        }

        void Update()
        {
            if (createNoise == true)
            {
                CalcNoise();
            }
        }
    }
}
