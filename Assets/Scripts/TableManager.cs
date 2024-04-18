using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private GameObject[] totalBallsOnTable;

    public void DeleteBall(GameObject currentGameObject)
    {
        int ballIndex = Array.IndexOf(totalBallsOnTable, currentGameObject);
        List<GameObject> temp = new(totalBallsOnTable);
        temp.RemoveAt(ballIndex);
        totalBallsOnTable = temp.ToArray();
        CheckIfArrayIsEmpty();
    }
    private void CheckIfArrayIsEmpty()
    {
        if (totalBallsOnTable.Length == 0)
        {
            StartCoroutine(LoadNewLevel());
        }
    }
    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        LevelManager.Instance.SpawnNextLevel();
    }
}