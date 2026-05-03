using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip mergeClip;

    private const string SoundOnKey = "SOUND_ON";
    private bool isSoundOn = true;

    private void Awake()
    {
        LoadSoundSetting();
    }

    public void PlayClick()
    {
        PlayOneShot(clickClip);
    }

    public void PlayMove()
    {
        PlayOneShot(moveClip);
    }

    public void PlayMerge()
    {
        PlayOneShot(mergeClip);
    }

    public void SetSoundEnabled(bool enabled)
    {
        isSoundOn = enabled;
        PlayerPrefs.SetInt(SoundOnKey, isSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"Sound Enabled: {isSoundOn}");
    }

    public void ToggleSound()
    {
        SetSoundEnabled(!isSoundOn);
    }

    public bool IsSoundOn()
    {
        return isSoundOn;
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (!isSoundOn)
            return;

        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    private void LoadSoundSetting()
    {
        isSoundOn = PlayerPrefs.GetInt(SoundOnKey, 1) == 1;
    }
}