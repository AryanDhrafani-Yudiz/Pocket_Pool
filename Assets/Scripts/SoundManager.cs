using UnityEngine;
using System.Collections;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Sound[] audioClips;

    public enum SoundName
    {
        GamePlayBGM,
        ButtonClick,
        GameWon,
        BallInHole,
        LevelChange,
        ShotPlay,
        GameOver,
        WallHit
    }
    [System.Serializable]
    public class Sound
    {
        public SoundName name;
        public AudioClip clip;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        audioSource.enabled = true;
    }
    private AudioClip GetAudioClip(SoundName name)
    {
        Sound currentAudio = Array.Find(audioClips, yolo => yolo.name == name);
        if (currentAudio != null) return currentAudio.clip;
        else return null;
    }
    public void PlaySound(SoundName name)
    {
        Sound currentAudio = Array.Find(audioClips, yolo => yolo.name == name);
        audioSource.PlayOneShot(currentAudio.clip);
    }
    public void SoundMute(bool value) { audioSource.mute = value; }

    public void OnButtonClick() { PlaySound(SoundName.ButtonClick); }

    public void OnBallInHole() { PlaySound(SoundName.BallInHole); }

    public void OnLevelChange() { PlaySound(SoundName.LevelChange); }

    public void OnShotPlay() { PlaySound(SoundName.ShotPlay); }

    public void OnWallHit() { PlaySound(SoundName.WallHit); }

    public void OnGameWon() { PlaySound(SoundName.GameWon); StartCoroutine(Timer(GetAudioClip(SoundName.GameWon).length)); }

    public void OnGameOver() { PlaySound(SoundName.GameOver); StartCoroutine(Timer(GetAudioClip(SoundName.GameOver).length)); }

    private IEnumerator Timer(float seconds) // Coroutine For Giving Delay Of Certain Seconds
    {
        yield return new WaitForSecondsRealtime(seconds);
        audioSource.enabled = false;
    }
}