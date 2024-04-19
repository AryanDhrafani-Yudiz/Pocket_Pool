using System.Collections.Generic;
using UnityEngine;
public enum GameScreens
{
    Home,
    Play,
    GameOver
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] BaseScreen _currentScreen;
    [SerializeField] List<BaseScreen> _screens;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        _currentScreen.ActivateScreen();
    }
    public void SwitchScreen(GameScreens screen)
    {
        foreach (BaseScreen baseScreen in _screens)
        {
            if (baseScreen.screen == screen)
            {
                baseScreen.ActivateScreen();
                _currentScreen.DeactivateScreen();
                _currentScreen = baseScreen;
            }
        }
    }
    public BaseScreen GetScreen(GameScreens screen)
    {
        foreach (BaseScreen baseScreen in _screens)
        {
            if (baseScreen.screen == screen)
            {
                return baseScreen;
            }
        }
        return null;
    }
}