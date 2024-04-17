using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI retriesTxt;
    [SerializeField] private GameObject[] poolLevels;
    private int currentLevel = 0;
    public static LevelManager Instance;
    private GameObject currTable;
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
        Debug.Log("Spawn Next Level");
        if (currTable != null) Destroy(currTable);
        if (currentLevel != poolLevels.Length)
        {
            currTable = Instantiate(poolLevels[currentLevel]);
            currentLevel++;
        }
    }
}