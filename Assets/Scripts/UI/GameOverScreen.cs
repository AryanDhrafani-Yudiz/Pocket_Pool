using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : BaseScreen
{
    [SerializeField] Button playAgainButton;
    [SerializeField] private Image winningImg;
    [SerializeField] private Image losingImg;

    private void Start()
    {
        playAgainButton.onClick.AddListener(OnRestart);
    }
    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }
    public void IsGameWin(bool value)
    {
        if (value) { winningImg.enabled = true; losingImg.enabled = false; }
        else { winningImg.enabled = false; losingImg.enabled = true; }
    }
    void OnRestart()
    {
        //UiManager.instance.SwitchScreen(GameScreens.Play);
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}