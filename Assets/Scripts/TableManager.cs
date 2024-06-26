using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> ballsOnTable;
    [SerializeField] private float loadLevelDelay = 0.3f;

    public void DeleteBall(GameObject currentGameObject)
    {
        ballsOnTable.Remove(currentGameObject);
        IsListEmpty();
    }
    private void IsListEmpty()
    {
        if (ballsOnTable.Count == 0)
        {
            WhiteBallMovement.userInputEnabled = false;
            StartCoroutine(LoadNewLevel());
        }
    }
    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSecondsRealtime(loadLevelDelay);
        LevelManager.Instance.SpawnNextLevel();
    }
}