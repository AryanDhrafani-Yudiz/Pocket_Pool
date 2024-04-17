using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI retriesTxt;
    [SerializeField] private GameObject[] poolLevels;
    private int currentLevel = 0;
    public static LevelManager Instance;
    private GameObject currTable;
    private int retriesAmt = 3;

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
        SpawnNextLevel();
    }
    public void SpawnNextLevel()
    {
        if (currTable != null) Destroy(currTable);
        if (currentLevel != poolLevels.Length)
        {
            currTable = Instantiate(poolLevels[currentLevel]);
            currentLevel++;
        }
        else
        {
            UIManager.Instance.OnGameOver(true);
        }
    }
    public void RespawnCurrentLevel()
    {
        if (retriesAmt > 0)
        {
            if (currTable != null) Destroy(currTable); currTable = Instantiate(poolLevels[currentLevel - 1]);
            retriesAmt--;
            retriesTxt.text = retriesAmt.ToString();
        }
        //else UIManager.Instance.OnGameOver(false);
    }
}