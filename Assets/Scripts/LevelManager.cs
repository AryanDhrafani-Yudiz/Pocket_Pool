using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] poolLevels;

    [SerializeField] private RectTransform retryBtnTransform;
    private Vector2 defaultSizeScale = Vector2.one;
    private Vector2 increasedSizeScale = new(1.2f, 1.2f);

    private int currentLevel = 0;
    private GameObject currTable;
    private int retriesAmt = 3;
    [SerializeField] private TextMeshProUGUI retriesTxt;

    public static LevelManager Instance;
    private GameOverScreen gameOverScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        SpawnNextLevel();
        gameOverScreen = UIManager.Instance.GetScreen(GameScreens.GameOver) as GameOverScreen;
    }
    public void SpawnNextLevel() // Spawns Next Level If Available Else Switches To Game Won Screen
    {
        if (currTable != null) Destroy(currTable);
        if (currentLevel != poolLevels.Length)
        {
            SoundManager.Instance.OnLevelChange();
            currTable = Instantiate(poolLevels[currentLevel]);
            WhiteBallMovement.userInputEnabled = true;
            currentLevel++;
        }
        else
        {
            UIManager.Instance.SwitchScreen(GameScreens.GameOver);
            if (gameOverScreen) gameOverScreen.IsGameWin(true);
            SoundManager.Instance.OnGameWon();
        }
    }
    public void CheckIfRetryAvailable() // To Check If Retry Is Available
    {
        if (retriesAmt > 0) ShowRetryBtn();
        else
        {
            UIManager.Instance.SwitchScreen(GameScreens.GameOver);
            if (gameOverScreen) gameOverScreen.IsGameWin(false);
            SoundManager.Instance.OnGameOver();
        }
    }
    public void ShowRetryBtn() // Focus On Retry Button
    {
        WhiteBallMovement.userInputEnabled = false;
        retryBtnTransform.localScale = increasedSizeScale;
    }
    public void RespawnCurrentLevel() // Respawns Current Level
    {
        if (retriesAmt > 0)
        {
            if (currTable != null) Destroy(currTable); currTable = Instantiate(poolLevels[currentLevel - 1]);
            retriesAmt--;
            retriesTxt.text = retriesAmt.ToString();
            WhiteBallMovement.userInputEnabled = true;
            retryBtnTransform.localScale = defaultSizeScale;
        }
    }
}