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
        List<GameObject> temp = new List<GameObject>(totalBallsOnTable);
        temp.RemoveAt(ballIndex);
        totalBallsOnTable = temp.ToArray();
        CheckIfArrayIsEmpty();
    }
    private void CheckIfArrayIsEmpty()
    {
        if (totalBallsOnTable.Length == 0)
        {
            Debug.Log("all Ball potted");
            StartCoroutine(LoadNewLevel());
        }
    }
    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSecondsRealtime(1f);
        LevelManager.Instance.SpawnNextLevel();
    }
}