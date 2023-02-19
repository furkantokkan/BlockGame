using System.Collections;
using System.Collections.Generic;
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
    public List<GameObject> generatedPices = new List<GameObject>();
    public Transform gridTrnasform = null;
    public Transform[,] dots;
    private List<int> selectedIndex = new List<int>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
