using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource music;
    public static MusicPlayer Instance;
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine(PlayMusic());
        }
    }

    private IEnumerator PlayMusic()
    {
        music.Play();
        if (PlayerPrefs.GetFloat("MusicVolume") == -1)
            music.volume = -1;
        
        yield return new WaitForSeconds(music.clip.length);
        StartCoroutine(PlayMusic());
    }
}
