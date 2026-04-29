using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Settings")]
    public int cloudCount = 8;
    public float minSpeed = 20f;
    public float maxSpeed = 60f;
    public float minScale = 0.6f;
    public float maxScale = 2.2f;
    public float minAlpha = 0.4f;
    public float maxAlpha = 0.85f;

    [Header("References")]
    public Image backgroundImage;

    private RectTransform canvasRect;
    private List<RectTransform> clouds = new List<RectTransform>();
    private List<float> cloudSpeeds = new List<float>();

    private Color sunsetBottom = new Color(0.95f, 0.45f, 0.2f);
    private Color sunsetMid = new Color(0.98f, 0.65f, 0.3f);
    private Color sunsetTop = new Color(0.55f, 0.25f, 0.55f);

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        if (backgroundImage != null)
        {
            backgroundImage.color = sunsetMid;
        }

        SetCameraBackground();

        SpawnInitialClouds();
    }

    void SetCameraBackground()
    {
        Camera.main.backgroundColor = sunsetBottom;
    }

    void SpawnInitialClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            float startX = Random.Range(-canvasRect.rect.width * 0.5f, canvasRect.rect.width * 0.5f);
            
            SpawnCloud(startX);
        }
    }

    void SpawnCloud(float startX)
    {
        GameObject cloudObj = new GameObject("Cloud");

        cloudObj.transform.SetParent(transform, false);

        RectTransform rt = cloudObj.AddComponent<RectTransform>();

        Image img = cloudObj.AddComponent<Image>();

        img.sprite = CreateCloudSprite();

        img.raycastTarget = false;

        float scale = Random.Range(minScale, maxScale);

        rt.sizeDelta = new Vector2(200f * scale, 80f * scale);

        float canvasHeight = canvasRect.rect.height;

        float yPos = Random.Range(-canvasHeight * 0.1f, canvasHeight * 0.45f);

        rt.anchoredPosition = new Vector2(startX, yPos);

        float alpha = Random.Range(minAlpha, maxAlpha);

        float t = Mathf.InverseLerp(-canvasHeight * 0.1f, canvasHeight * 0.45f, yPos);
        
        Color cloudColor = Color.Lerp(new Color(1f, 0.85f, 0.7f, alpha), new Color(0.9f, 0.75f, 0.9f, alpha * 0.7f), t);
        
        img.color = cloudColor;

        float speed = Random.Range(minSpeed, maxSpeed);

        speed *= Mathf.Lerp(1f, 0.5f, t);

        clouds.Add(rt);

        cloudSpeeds.Add(speed);
    }

    Sprite CreateCloudSprite()
    {
        int w = 200;
        int h = 80;

        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);

        tex.filterMode = FilterMode.Bilinear;

        Color[] pixels = new Color[w * h];

        Vector2[] puffs = new Vector2[]
        {
            new Vector2(0.25f, 0.4f),
            new Vector2(0.45f, 0.6f),
            new Vector2(0.6f, 0.55f),
            new Vector2(0.75f, 0.4f),
            new Vector2(0.5f, 0.3f),
        };

        float[] puffRadii = new float[] { 0.18f, 0.22f, 0.2f, 0.16f, 0.15f };

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float u = (float)x / w;

                float v = (float)y / h;

                float maxDensity = 0f;

                for (int p = 0; p < puffs.Length; p++)
                {
                    float dx = (u - puffs[p].x) / (puffRadii[p] * 1.6f);

                    float dy = (v - puffs[p].y) / puffRadii[p];

                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    float density = Mathf.Clamp01(1f - dist);

                    density = density * density;

                    maxDensity = Mathf.Max(maxDensity, density);
                }

                pixels[y * w + x] = new Color(1f, 1f, 1f, maxDensity);
            }
        }

        tex.SetPixels(pixels);

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 100f);
    }

    void Update()
    {
        float canvasWidth = canvasRect.rect.width;

        float spawnX = -canvasWidth * 0.5f;

        float despawnX = canvasWidth * 0.5f + 300f;

        for (int i = 0; i < clouds.Count; i++)
        {
            if (clouds[i] == null) 
            {
                continue;
            }

            clouds[i].anchoredPosition += Vector2.right * cloudSpeeds[i] * Time.deltaTime;

            if (clouds[i].anchoredPosition.x > despawnX)
            {
                float canvasHeight = canvasRect.rect.height;

                float yPos = Random.Range(-canvasHeight * 0.1f, canvasHeight * 0.45f);

                clouds[i].anchoredPosition = new Vector2(spawnX - Random.Range(0f, 200f), yPos);

                float scale = Random.Range(minScale, maxScale);

                clouds[i].sizeDelta = new Vector2(200f * scale, 80f * scale);

                cloudSpeeds[i] = Random.Range(minSpeed, maxSpeed);

                float t = Mathf.InverseLerp(-canvasHeight * 0.1f, canvasHeight * 0.45f, yPos);

                cloudSpeeds[i] *= Mathf.Lerp(1f, 0.5f, t);

                float alpha = Random.Range(minAlpha, maxAlpha);

                Image img = clouds[i].GetComponent<Image>();

                if (img != null)
                {
                    img.color = Color.Lerp(new Color(1f, 0.85f, 0.7f, alpha), new Color(0.9f, 0.75f, 0.9f, alpha * 0.7f), t);
                }
            }
        }
    }
}