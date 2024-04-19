using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> ballsOnTable;

    public void DeleteBall(GameObject currentGameObject)
    {
        ballsOnTable.Remove(currentGameObject);
        IsListEmpty();
    }
    private bool IsListEmpty()
    {
        if (ballsOnTable.Count == 0)
        {
            LevelManager.Instance.disableUserInput = true;
            StartCoroutine(LoadNewLevel());
            return true;
        }
        else return false;
    }
    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        LevelManager.Instance.SpawnNextLevel();
    }
}