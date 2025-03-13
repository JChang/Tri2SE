using UnityEngine;

public class TitleMusicManager : MonoBehaviour
{
    public AudioClip titleMusic;
    private AudioSource audioSource;
    public float volume = 1f;

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = titleMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = volume;

        audioSource.Play();
    }

    public void StopTitleMusic()
    {
        audioSource.Stop();
    }
}
