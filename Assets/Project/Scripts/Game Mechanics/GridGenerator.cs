using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour, IInitializeable
{
    [Header("Referances")]
    [SerializeField] private Sprite[] grids;
    [SerializeField] private Transform cornerX;
    [SerializeField] private Transform cornerY;

    private SpriteRenderer boardRenderer;

    private readonly int firstGridAmount = 4;
    public int Priority()
    {
        return 0;
    }
    public void Initialize(int piceAmount, int gridSize, Vector3 spawnPos)
    {
        boardRenderer = GetComponentInChildren<SpriteRenderer>();
        boardRenderer.sprite = grids[gridSize - firstGridAmount];
        boardRenderer.sortingOrder = 0;
        boardRenderer.transform.position = spawnPos;
        GenerateDots(gridSize);
        boardRenderer.transform.localScale = GetBoardSizeAcordingToGridSize(gridSize);
        GameManager.Instance.gridTrnasform = boardRenderer.transform;
        GameManager.Instance.minX = cornerX.position.x;
        GameManager.Instance.maxX = -cornerX.position.x;
        GameManager.Instance.minY = cornerX.position.y;
        GameManager.Instance.maxY = cornerY.position.y;
    }
    private void GenerateDots(int gridAmount)
    {
        Vector2 startPos = GetDotStartPointAcordingToGridAmount(gridAmount);
        Vector2 offset = new Vector2(startPos.x * 2 / (gridAmount - 1), startPos.y * 2 / (gridAmount - 1));
        GameManager.Instance.dots = new Transform[gridAmount * gridAmount, gridAmount * gridAmount];
        for (int y = 0; y < gridAmount; y++)
        {
            for (int x = 0; x < gridAmount; x++)
            {
                GameObject newDOts = new GameObject();
                newDOts.transform.SetParent(boardRenderer.transform);
                newDOts.transform.localPosition = new Vector2(startPos.x - offset.x * x, startPos.y - offset.y * y);
                newDOts.gameObject.name = "X: " + x.ToString() + " Y: " + y.ToString();
                GameManager.Instance.dots[x, y] = newDOts.transform;
            }
        }
    }
    private Vector2 GetDotStartPointAcordingToGridAmount(int gridAmount)
    {
        switch (gridAmount)
        {
            case 4:
                return new Vector3(-3.607f, -3.606f);
            case 5:
                return new Vector3(-3.8438f, -3.8447f);
            case 6:
                return new Vector2(-4.008f, -4.005f);
            default:
                throw new NullReferenceException("Check the grid amount");
        }
    }
    private Vector2 GetBoardSizeAcordingToGridSize(int gridAmount)
    {
        switch (gridAmount)
        {
            case 4:
                return new Vector2(0.32f, 0.32f);
            case 5:
                return new Vector2(0.38f, 0.38f);
            case 6:
                return new Vector2(0.45f, 0.45f);
            default:
                throw new NullReferenceException("Check the grid amount");
        }
    }
}
