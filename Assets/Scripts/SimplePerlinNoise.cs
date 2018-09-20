using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SimplePerlinNoise : MonoBehaviour
{
    [SerializeField]
    private int resolution = 512;

    private Texture2D tex;

    // Use this for initialization
    private void Start()
    {
        tex = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);

        Renderer render = GetComponent<Renderer>();
        if (render == null)
        {
            Debug.LogWarning("Missing renderer. add quad and attach this script");
            return;
        }
        render.material.mainTexture = tex;

        float scaleAdjust = 1f / resolution;

        Color[] pixels = new Color[resolution * resolution];
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float2 rc = noise.cellular(new float2(x, y) * scaleAdjust);

                int index = x + (y * resolution);
                pixels[index] = new Color(rc.x, rc.y, 1, 1);
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
    }
}