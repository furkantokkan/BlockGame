using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Settings")]
    [Range(4, 6)] public int gridAmount = 4;
    [Range(5, 12)] public int piceAmount = 5;
    [SerializeField] private bool useRandomColors = true;
    [SerializeField] private Color[] colors;
    public Vector3 spawnPositionOnBoard = new Vector3(0f, 1.450012f, 0f);
    [Header("Referances")]
    public List<GameObject> generatedPieces = new List<GameObject>();
    [SerializeField] private Transform piceSpawnArea;
    public Transform gridTrnasform = null;
    public Transform[,] dots;
    private List<int> selectedIndex = new List<int>();
    private List<IInitializeable> ýnitializeables = new List<IInitializeable>();

    [HideInInspector] public float minX, maxX, minY, maxY;
    private void Awake()
    {
        ýnitializeables.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IInitializeable>());
    }
    private IEnumerator Start()
    {
        ýnitializeables = ýnitializeables.OrderBy(x => x.Priority()).ToList();

        for (int i = 0; i < ýnitializeables.Count; i++)
        {
            ýnitializeables[i].Initialize(piceAmount, gridAmount, spawnPositionOnBoard);
            yield return new WaitForEndOfFrame();
        }

        ShufflePice();
    }
    private void ShufflePice()
    {
        BoxCollider2D col = piceSpawnArea.GetComponent<BoxCollider2D>();

        for (int i = 0; i < piceAmount; i++)
        {
            float screenX = Random.Range(col.bounds.min.x, col.bounds.max.x);
            float screenY = Random.Range(col.bounds.min.y, col.bounds.max.y);
            generatedPieces[i].transform.parent.DOMove(new Vector2(screenX, screenY), 1.2f, false);
        }
    }
    public Color GetColorFromColorArray(int index)
    {
        if (useRandomColors && colors.Length > 0 && index < colors.Length)
        {
            int randomIndex = Random.Range(0, colors.Length);

            if (selectedIndex.Contains(randomIndex))
            {
                do
                {
                    randomIndex = Random.Range(0, colors.Length);
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


        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}
