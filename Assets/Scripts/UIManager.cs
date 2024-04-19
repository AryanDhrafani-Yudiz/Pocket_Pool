using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas gameStartingCanvas;
    [SerializeField] private Canvas gamePlayCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Image winningImg;
    [SerializeField] private Image losingImg;

    [SerializeField] private Image volumeBtnImage;
    [SerializeField] private Sprite volumeOnBtn;
    [SerializeField] private Sprite volumeOffBtn;
    public static UIManager Instance;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        gameStartingCanvas.enabled = true;
        gamePlayCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }
    public void OnVolumeOnOffBtn()
    {
        if (volumeBtnImage.sprite == volumeOnBtn)
        {
            SoundManager.Instance.SoundMute(true);
            volumeBtnImage.sprite = volumeOffBtn;
        }
        else if (volumeBtnImage.sprite == volumeOffBtn)
        {
            SoundManager.Instance.SoundMute(false);
            volumeBtnImage.sprite = volumeOnBtn;
        }
    }
    public void OnGameStart()
    {
        gameStartingCanvas.enabled = false;
        gamePlayCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }
    public void OnGameOver(bool value)
    {
        gameStartingCanvas.enabled = false;
        gamePlayCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        if (value) { winningImg.enabled = true; losingImg.enabled = false; }
        else { winningImg.enabled = false; losingImg.enabled = true; }
    }
    public void OnRestartGame()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}