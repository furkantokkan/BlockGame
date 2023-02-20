using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGenerator : MonoBehaviour, IInitializeable
{
    [SerializeField] private Vector2Int imageDim;
    private int piceAmount;
    private int gridAmount;

    private List<Color[]> regionPixels = new List<Color[]>();


    public void Initialize(int newPiceAmount, int newGridSize, Vector3 spawnPos)
    {
        piceAmount = newPiceAmount;
        gridAmount = newGridSize;
        List<Sprite> parts = GetParts();

        for (int i = 0; i < piceAmount; i++)
        {
            GameObject newObject = new GameObject();
            GameObject newOrgin = new GameObject();
            newOrgin.name = "Pice Orgin " + i.ToString();
            newObject.name = "Pice " + i.ToString();
            newObject.layer = LayerMask.NameToLayer("Pice"); 
            SpriteRenderer sr = newObject.AddComponent<SpriteRenderer>();
            sr.sprite = parts[i];
            sr.sortingOrder = 1;
            newObject.transform.position = spawnPos;
            newObject.transform.localScale = GetScaleAccordingToBoardSize(gridAmount);
            PolygonCollider2D col = newObject.AddComponent<PolygonCollider2D>();
            GamePice pice = newObject.AddComponent<GamePice>();
            Transform closeDot = FindCloseDot(new Vector2(col.bounds.center.x, col.bounds.center.y), gridAmount);
            newOrgin.transform.position = closeDot.position;
            pice.transform.SetParent(newOrgin.transform);
            pice.Inithialize();
            GameManager.Instance.generatedPieces.Add(newObject);
        }
    }
    public int Priority()
    {
        return 1;
    }
    List<Sprite> GetParts()
    {
        Vector2Int[] centroids = new Vector2Int[piceAmount];
        Color[] regions = new Color[piceAmount];

        for (int i = 0; i < piceAmount; i++)
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

        for (int i = 0; i < piceAmount; i++)
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
    private Transform FindCloseDot(Vector2 orgin, int gridAmount)
    {
        float oldDistance = float.MaxValue;
        Transform result = null;

        for (int y = 0; y < gridAmount; y++)
        {
            for (int x = 0; x < gridAmount; x++)
            {
                Transform newDot = GameManager.Instance.dots[x, y].transform;
                float dist = Vector2.Distance(orgin, newDot.position);
                if (dist < oldDistance)
                {
                    result = newDot;
                    oldDistance = dist;
                }
            }
        }

        return result;
    }
}
