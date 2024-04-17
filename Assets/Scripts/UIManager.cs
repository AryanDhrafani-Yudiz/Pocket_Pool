using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas gameStartingCanvas;
    [SerializeField] private Canvas gamePlayCanvas;
    [SerializeField] private Canvas gameOverCanvas;

    private void Awake()
    {
        gameStartingCanvas.enabled = true;
        gamePlayCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }
}
