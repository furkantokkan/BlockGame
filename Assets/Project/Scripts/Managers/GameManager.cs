using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Level Settins")]
    public List<ServerData.Data> defaultLevelData = new List<ServerData.Data>();
    public bool overrideDefaultLevelData = false;
    [Header("Settings")]
    [Range(4, 6)] public int gridAmount = 4;
    [Range(5, 12)] public int piceAmount = 5;
    [SerializeField] private bool useRandomColors = true;
    [SerializeField] private Color[] colors;
    public Vector3 spawnPositionOnBoard = new Vector3(0f, 1.450012f, 0f);
    public bool gameHasWon = false;
    [Header("Referances")]
    public List<GameObject> generatedPieces = new List<GameObject>();
    [SerializeField] private Transform piceSpawnArea;
    public Transform gridTrnasform = null;
    public Transform[,] dots;
    private List<int> selectedIndex = new List<int>();
    private List<IInitializeable> ýnitializeables = new List<IInitializeable>();

    private int levelIndex = 0;
    private readonly string LevelKey = "LEVEL";

    public Action onCheckGameHadDone;

    [HideInInspector] public float minX, maxX, minY, maxY;
    private void Awake()
    {
        ýnitializeables.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IInitializeable>());
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ServerData.serverIsResponded);

        levelIndex = PlayerPrefs.GetInt(LevelKey, 0);
        SetLevelData();

        onCheckGameHadDone += OnCheckGameHasDone;
        ýnitializeables = ýnitializeables.OrderBy(x => x.Priority()).ToList();

        for (int i = 0; i < ýnitializeables.Count; i++)
        {
            ýnitializeables[i].Initialize(piceAmount, gridAmount, spawnPositionOnBoard);
            yield return new WaitForEndOfFrame();
        }

        ShufflePice();
    }
    private void OnDisable()
    {
        onCheckGameHadDone -= OnCheckGameHasDone;
    }
    private void SetLevelData()
    {
        if (ServerData.serverListData.Count > 0)
        {
            if (levelIndex > ServerData.serverListData.Count)
            {
                ResetTheLevelSave();
            }

            gridAmount = ServerData.serverListData[levelIndex].gridAmount;
            piceAmount = ServerData.serverListData[levelIndex].piceAmount;
        }
        else
        {
            if (defaultLevelData.Count > 0)
            {

                if (levelIndex > defaultLevelData.Count)
                {
                    ResetTheLevelSave();
                }

                gridAmount = defaultLevelData[levelIndex].gridAmount;
                piceAmount = defaultLevelData[levelIndex].piceAmount;
                Debug.Log("Grid Amount: " + gridAmount);
            }
        }
    }
    private void ShufflePice()
    {
        BoxCollider2D col = piceSpawnArea.GetComponent<BoxCollider2D>();

        for (int i = 0; i < piceAmount; i++)
        {
            float screenX = UnityEngine.Random.Range(col.bounds.min.x, col.bounds.max.x);
            float screenY = UnityEngine.Random.Range(col.bounds.min.y, col.bounds.max.y);
            generatedPieces[i].transform.parent.DOMove(new Vector2(screenX, screenY), 1f, false).SetEase(Ease.Linear);
        }
    }
    private void OnCheckGameHasDone()
    {
        gameHasWon = CheckForGameHasDone();
        if (gameHasWon)
        {
            UIManager.Instance.UpdateUI();
        }
    }
    private bool CheckForGameHasDone()
    {
        foreach (GameObject item in generatedPieces)
        {
            if (!item.GetComponent<GamePice>().waitingOnRightPlace)
            {
                return false;
            }
        }

        return true;
    }
    public Color GetColorFromColorArray(int index)
    {
        if (useRandomColors && colors.Length > 0 && index < colors.Length)
        {
            int randomIndex = UnityEngine.Random.Range(0, colors.Length);

            if (selectedIndex.Contains(randomIndex))
            {
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, colors.Length);
                    if (!selectedIndex.Contains(randomIndex))
                    {
                        selectedIndex.Add(randomIndex);
                        return colors[randomIndex];
                    }
                } while (selectedIndex.Contains(randomIndex));
            }
            else
            {
                selectedIndex.Add(randomIndex);
                return colors[randomIndex];
            }
        }
        else if (!useRandomColors && colors.Length > 0 && index < colors.Length)
        {
            return colors[index];
        }


        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
    }
    public void SaveTheNextLevel()
    {
        levelIndex++;
        PlayerPrefs.SetInt(LevelKey, levelIndex);
    }
    [ContextMenu("Reset Level")]
    public void ResetTheLevelSave()
    {
        levelIndex = 0;
        PlayerPrefs.SetInt(LevelKey, levelIndex);
    }
}
