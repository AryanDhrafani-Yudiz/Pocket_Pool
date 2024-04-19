using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : BaseScreen
{
    [SerializeField] Button volumeButton;

    [SerializeField] private Image volumeBtnImage;
    [SerializeField] private Sprite volumeOnBtn;
    [SerializeField] private Sprite volumeOffBtn;

    private void Start()
    {
        volumeButton.onClick.AddListener(OnVolumeBtnPressed);
    }
    private void OnVolumeBtnPressed()
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
}