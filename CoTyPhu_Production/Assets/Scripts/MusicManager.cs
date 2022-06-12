using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource playing;

    public List<AudioClipEnum> backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        PlayNext();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing.isPlaying)
            PlayNext();
    }

    public void PlayNext()
    {
        playing?.Stop();
        AudioClipEnum song = NextMusic();
        SoundManager.Ins.Play(song);
        playing = SoundManager.Ins.audioSources[song];
    }

    private void OnDestroy()
    {
        if (playing != null)
            playing.Stop();
    }

    AudioClipEnum NextMusic()
    {
        int random = Random.Range(0, backgroundMusic.Count - 1);
        return backgroundMusic[random];
    }
}
