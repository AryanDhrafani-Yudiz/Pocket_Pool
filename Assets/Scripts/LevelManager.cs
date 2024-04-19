using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private RectTransform retryBtnTransform;
    private Vector2 defaultSizeScale = Vector2.one;
    private Vector2 increasedSizeScale = new(1.2f, 1.2f);
    [SerializeField] private TextMeshProUGUI retriesTxt;
    [SerializeField] private GameObject[] poolLevels;
    private int currentLevel = 0;
    public static LevelManager Instance;
    private GameObject currTable;
    private int retriesAmt = 3;
    public bool disableUserInput = false;
    private GameOverScreen gameOverScreen;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        SpawnNextLevel();
        gameOverScreen = UiManager.Instance.GetScreen(GameScreens.GameOver) as GameOverScreen;
    }
    public void SpawnNextLevel()
    {
        if (currTable != null) Destroy(currTable);
        if (currentLevel != poolLevels.Length)
        {
            SoundManager.Instance.OnLevelChange();
            currTable = Instantiate(poolLevels[currentLevel]);
            disableUserInput = false;
            currentLevel++;
        }
        else
        {
            UiManager.Instance.SwitchScreen(GameScreens.GameOver);
            gameOverScreen?.IsGameWin(true);
            SoundManager.Instance.OnGameWon();
        }
    }
    public void CheckIfRespawnAvailable()
    {
        if (retriesAmt > 0) ShowRetryBtn();
        else
        {
            UiManager.Instance.SwitchScreen(GameScreens.GameOver);
            gameOverScreen?.IsGameWin(false);
            SoundManager.Instance.OnGameOver();
        }
    }
    public void ShowRetryBtn()
    {
        disableUserInput = true;
        retryBtnTransform.localScale = increasedSizeScale;
    }
    public void RespawnCurrentLevel()
    {
        if (retriesAmt > 0)
        {
            if (currTable != null) Destroy(currTable); currTable = Instantiate(poolLevels[currentLevel - 1]);
            retriesAmt--;
            retriesTxt.text = retriesAmt.ToString();
            disableUserInput = false;
            retryBtnTransform.localScale = defaultSizeScale;
        }
    }
}