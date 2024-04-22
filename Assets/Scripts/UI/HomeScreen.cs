using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] Button playButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlay);
    }
    private void OnPlay()
    {
        UIManager.Instance.SwitchScreen(GameScreens.Play);
    }
}