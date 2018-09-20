using System;
using Unity.Mathematics;
using UnityEngine;

public class NoiseTestFunction : MonoBehaviour
{
    internal Func<float2, float> genNoiseAction;
    internal int res;
    internal float scale;

    private Texture2D tex;

    private void Start()
    {
        tex = new Texture2D(res, res, TextureFormat.ARGB32, false);

        Renderer render = GetComponent<Renderer>();
        if (render == null)
        {
            Debug.LogWarning("Missing renderer. add quad and attach this script");
            return;
        }
        render.material.mainTexture = tex;

        UpdateTexture();
    }

    private void UpdateTexture()
    {
        float scaleAdjust = scale / res;

        Color[] pixels = new Color[res * res];

        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                float2 pos = new float2(x, y) * scaleAdjust;
                float val = genNoiseAction(pos);

                int index = x + (y * res);
                pixels[index] = new Color(val, val, val, 1);
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
    }
}

public class NoiseTester : MonoBehaviour
{
    [SerializeField]
    private int resolution = 512;

    [SerializeField]
    private float scale = 8;

    private float oldScale;

    // Use this for initialization
    private void Start()
    {
        CreateTestQuad(noise.srnoise, new Vector3(-2, 1, 0));
        CreateTestQuad(noise.cnoise, new Vector3(0, 1, 0));
        CreateTestQuad(noise.snoise, new Vector3(2, 1, 0));
    }

    private void CreateTestQuad(Func<float2, float> testSrNoise, Vector3 vector3)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.transform.localScale = new Vector3(2, 2, 2);
        NoiseTestFunction noiseTest = go.AddComponent<NoiseTestFunction>();
        noiseTest.genNoiseAction = testSrNoise;
        noiseTest.res = resolution;
        noiseTest.scale = scale;
        go.transform.position = vector3;
    }
}