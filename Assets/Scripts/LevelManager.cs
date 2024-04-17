using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI retriesTxt;
    [SerializeField] private GameObject[] poolLevels;
    private int currentLevel = 0;
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance != null) Instance = this;
        SpawnNextLevel();
    }
    public void SpawnNextLevel()
    {
        if (currentLevel != 0) Destroy(poolLevels[currentLevel]);
        if (currentLevel != poolLevels.Length - 1)
        {
            Instantiate(poolLevels[currentLevel]);
            currentLevel++;
        }
    }
}
