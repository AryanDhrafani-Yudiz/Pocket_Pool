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

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        SpawnNextLevel();
    }
    public void SpawnNextLevel()
    {
        if (currTable != null) Destroy(currTable);
        if (currentLevel != poolLevels.Length)
        {
            disableUserInput = false;
            SoundManager.Instance.OnLevelChange();
            currTable = Instantiate(poolLevels[currentLevel]);
            currentLevel++;
        }
        else
        {
            SoundManager.Instance.OnGameWon();
            UIManager.Instance.OnGameOver(true);
        }
    }
    public void CheckIfRespawnAvailable()
    {
        if (retriesAmt > 0) ShowRetryBtn();
        else { UIManager.Instance.OnGameOver(false); SoundManager.Instance.OnGameOver(); }
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