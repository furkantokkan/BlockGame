using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Sprite[] grids;

    private SpriteRenderer boardRenderer;

    private readonly int firstGridAmount = 4;

    private void Awake()
    {
        boardRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        boardRenderer.sprite = grids[GameManager.Instance.gridAmount - firstGridAmount];
        boardRenderer.sortingOrder = 0;
        boardRenderer.transform.position = GameManager.Instance.spawnPositionOnBoard;
        GameManager.Instance.gridTrnasform = boardRenderer.transform;
    }
}
