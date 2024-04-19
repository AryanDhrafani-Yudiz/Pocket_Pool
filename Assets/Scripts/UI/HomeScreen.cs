using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlay);
    }
    void OnPlay()
    {
        UiManager.Instance.SwitchScreen(GameScreens.Play);
    }
}