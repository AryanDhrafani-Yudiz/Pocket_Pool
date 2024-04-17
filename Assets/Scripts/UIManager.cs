using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas gameStartingCanvas;
    [SerializeField] private Canvas gamePlayCanvas;
    [SerializeField] private Canvas gameOverCanvas;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        gameStartingCanvas.enabled = true;
        gamePlayCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }
    public void OnGameStart()
    {
        gameStartingCanvas.enabled = false;
        gamePlayCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }
    public void OnGameOver()
    {
        gameStartingCanvas.enabled = false;
        gamePlayCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }
    public void OnRestartGame()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}