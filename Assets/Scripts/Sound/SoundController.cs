using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    [SerializeField] private SoundButtonData fxButton;
    [SerializeField] private SoundButtonData musicButton;
    [Space]
    [Range(0, 1)] 
    [SerializeField] private float musicVolume = 0.5f;
    [Range(0, 1)] 
    [SerializeField] private float fxVolume;
    [Range(0.7f, 1)]
    [SerializeField] private float lowPitch = 0.8f;
    [Range(1, 1.3f)]
    [SerializeField] private float highPitch = 1.2f;

    [SerializeField] private AudioClip musicTemplate;
    [SerializeField] private AudioClip[] winSounds;
    [SerializeField] private AudioClip[] ballSound;
    [SerializeField] private AudioClip netSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip click1;
    [SerializeField] private AudioClip click2;


    private AudioSource currentMusic;

    private void Start()
    {
        instance = this;
        
        if (PlayerPrefs.GetFloat("fxVolume") == -1)
            ChangeFxVolume(fxButton);
        if (PlayerPrefs.GetFloat("MusicVolume") == -1)
            ChangeMusicVolume(musicButton);
    }

    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1)
    {
        if (clip != null)
        {
            GameObject go = new GameObject("SoundFX " + clip.name);
            go.transform.position = position;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;

            float randomPitch = Random.Range(lowPitch, highPitch);
            source.pitch = randomPitch;
            source.volume = volume;

            source.Play();
            Destroy(go, clip.length);
            return source;
        }

        return null;
    }

    private AudioSource PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1)
    {
        if (clips != null)
        {
            if (clips.Length != 0)
            {
                int randomIndex = Random.Range(0, clips.Length);

                if (clips[randomIndex] != null)
                {
                    AudioSource source = PlayClipAtPoint(clips[randomIndex], position, volume);
                    return source;
                }
            }
        }

        return null;
    }
    
    private IEnumerator PlayRandomMusicRoutine()
    {
        yield return new WaitForSeconds(currentMusic.clip.length);
        StartCoroutine(PlayRandomMusicRoutine());
    }

    public void PlayWinSound()
    {
        PlayRandom(winSounds, Vector3.zero, fxVolume);
    }

    public void PlayBallSound()
    {
        PlayRandom(ballSound, Vector3.zero, fxVolume);
    }

    public void PlayNetSound()
    {
        PlayClipAtPoint(netSound, Vector3.zero, fxVolume);
    }

    public void PlayClick1()
    {
        PlayClipAtPoint(click1, Vector3.zero, fxVolume);
    }
    
    public void PlayClick2()
    {
        PlayClipAtPoint(click2, Vector3.zero, fxVolume);
    }
    
    public void PlayBombSound()
    {
        PlayClipAtPoint(bombSound, Vector3.zero, fxVolume);
    }
    
    public void PlayShotSound()
    {
        PlayClipAtPoint(shotSound, Vector3.zero, fxVolume);
    }

    public void ChangeMusicVolume(SoundButtonData soundButtonData)
    {
        musicVolume = soundButtonData.ChangeVolume(musicVolume);
        MusicPlayer.Instance.music.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void ChangeFxVolume(SoundButtonData soundButtonData)
    {
        fxVolume = soundButtonData.ChangeVolume(fxVolume);
        PlayerPrefs.SetFloat("fxVolume", fxVolume);
    }
}