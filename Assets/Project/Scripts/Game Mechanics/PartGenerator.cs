using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int imageDim;

    private int regionAmount;

    private List<Sprite> parts = new List<Sprite>();

    private List<Color[]> regionPixels = new List<Color[]>();


    private void Start()
    {
        regionAmount = GameManager.Instance.piceAmount;
        SpawnTheParts();
    }

    private void SpawnTheParts()
    {
        GenerateParts();

        for (int i = 0; i < regionAmount; i++)
        {
            GameObject newObject = new GameObject();
            newObject.name = "Pice " + i.ToString();
            SpriteRenderer sr = newObject.AddComponent<SpriteRenderer>();
            sr.sprite = parts[i];
            sr.sortingOrder = 1;
            newObject.transform.position = GameManager.Instance.spawnPositionOnBoard;
            newObject.transform.localScale = GetScaleAccordingToBoardSize(GameManager.Instance.gridAmount);
            GameManager.Instance.generatedPices.Add(newObject);
        }
    }
    private void GenerateParts()
    {
        parts.AddRange(GetParts());
    }
    List<Sprite> GetParts()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];
        Color[] regions = new Color[regionAmount];

        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
            regions[i] = GameManager.Instance.GetColorFromColorArray(i, true);
            regionPixels.Add(new Color[imageDim.x * imageDim.y]);
        }
        for (int x = 0; x < imageDim.x; x++)
        {
            for (int y = 0; y < imageDim.y; y++)
            {
                int index = x * imageDim.x + y;
                int selectedRegionIndex = GetClosestCentroidIndex(new Vector2Int(x, y), centroids);
                Color selectedColorForRegion = regions[selectedRegionIndex];

                regionPixels[selectedRegionIndex][index] = selectedColorForRegion;
            }
        }
        return SliceTheParts(GetImageFromColorArray(regionPixels));
    }
    int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
    {
        float smallestDst = float.MaxValue;
        int index = 0;
        for (int i = 0; i < centroids.Length; i++)
        {
            if (Vector2.Distance(pixelPos, centroids[i]) < smallestDst)
            {
                smallestDst = Vector2.Distance(pixelPos, centroids[i]);
                index = i;
            }
        }
        return index;
    }
    List<Texture2D> GetImageFromColorArray(List<Color[]> pixelColors)
    {
        List<Texture2D> piceTextures = new List<Texture2D>();

        for (int i = 0; i < regionAmount; i++)
        {
            Texture2D tex = new Texture2D(imageDim.x, imageDim.y);
            tex.filterMode = FilterMode.Bilinear;
            tex.SetPixels(pixelColors[i]);
            tex.Apply();
            piceTextures.Add(tex);
        }

        return piceTextures;
    }
    private List<Sprite> SliceTheParts(List<Texture2D> textures)
    {
        List<Sprite> parts = new List<Sprite>();

        for (int i = 0; i < textures.Count; i++)
        {
            Rect rect = new Rect(0f, 0f, textures[i].width, textures[i].height);
            parts.Add(Sprite.Create(textures[i], rect, Vector2.one * 0.5f));
        }

        return parts;
    }

    private Vector3 GetScaleAccordingToBoardSize(int gridAmount)
    {
        return new Vector3(0.85f, 0.85f, 0f);
    }
}
