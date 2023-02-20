using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Transform winPanel;
    public void UpdateUI()
    {
        if (GameManager.Instance.gameHasWon)
        {
            winPanel.gameObject.SetActive(true);
        }
        else
        {
            winPanel.gameObject.SetActive(false);
        }
    }
    public void NextLevelButton()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
