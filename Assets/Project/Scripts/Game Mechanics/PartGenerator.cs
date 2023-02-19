using System;
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
            GameObject parentObject = new GameObject();
            GameObject newpice = new GameObject();
            newpice.name = "Pice " + i.ToString();
            parentObject.name = "Parent " + i.ToString();
            SpriteRenderer sr = newpice.AddComponent<SpriteRenderer>();
            sr.sprite = parts[i];
            sr.sortingOrder = 1;
            newpice.transform.position = GameManager.Instance.spawnPositionOnBoard;
            newpice.transform.localScale = GetScaleAccordingToBoardSize(GameManager.Instance.gridAmount);
            newpice.AddComponent<PolygonCollider2D>();
            newpice.AddComponent<GamePice>();
            parentObject.transform.position = newpice.GetComponent<PolygonCollider2D>().bounds.center;
            newpice.transform.SetParent(parentObject.transform);
            GameManager.Instance.generatedPices.Add(newpice);
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
            centroids[i] = new Vector2Int(UnityEngine.Random.Range(0, imageDim.x), UnityEngine.Random.Range(0, imageDim.y));
            regions[i] = GameManager.Instance.GetColorFromColorArray(i);
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
            tex.filterMode = FilterMode.Point;
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
        switch (gridAmount)
        {
            case 4:
                return new Vector3(0.6f, 0.6f, 0.6f);
            case 5:
                return new Vector3(0.71f, 0.71f, 0.71f);
            case 6:
                return new Vector3(0.85f, 0.85f, 0.85f);
            default:
                throw new NullReferenceException("Check the grid amount");
        }
    }
}
