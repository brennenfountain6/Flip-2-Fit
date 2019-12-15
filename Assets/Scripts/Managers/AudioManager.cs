using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource efxSource;
    public AudioSource musicSource;
    public AudioSource miscSource;
    public static AudioManager instance = null;

    public AudioClip mainMenuMusic;
    public AudioClip[] inGameSongs;

    public AudioClip buttonClickSound;
    public AudioClip gameWonSound;
    public AudioClip bridgeSound;
    public AudioClip deathSound;
    public AudioClip playerMovementSound;
    public AudioClip scoreIndicatorSound;

    private bool isPlayingGameMusic = false;
    private bool isPlayingMainMenuMusic = false;

    float startingMusicVolume;

    int lastSongIndex = -1;

	// Use this for initialization
	void Awake ()
    {
		
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

	}

    private void Start()
    {
        SetMusicVolumeSettings();
    }

    // Update is called once per frame
    void Update ()
    {
		if (isPlayingGameMusic)
        {
            if (!musicSource.isPlaying)
            {
                instance.SetRandomInGameMusic();
            }
        }

        if (isPlayingMainMenuMusic)
        {
            if (!musicSource.isPlaying)
            {
                instance.SetMainMenuMusic();
            }
        }
	}

    public void SetMainMenuMusic()
    {
        instance.musicSource.volume = startingMusicVolume;
        isPlayingMainMenuMusic = true;
        isPlayingGameMusic = false;

        instance.musicSource.Pause();
        instance.musicSource.clip = mainMenuMusic;
        instance.musicSource.Play();

    }

    public void SetRandomInGameMusic()
    {
        musicSource.volume = startingMusicVolume;
        isPlayingGameMusic = true;
        isPlayingMainMenuMusic = false;
        instance.musicSource.Pause();


           
        int randomIndex = Random.Range(0, inGameSongs.Length);

        while (randomIndex == lastSongIndex)
        {
            randomIndex = Random.Range(0, inGameSongs.Length);

        }

        lastSongIndex = randomIndex;
        
        instance.musicSource.clip = inGameSongs[randomIndex];

        instance.musicSource.Play();


    }

    public void PlayButtonClick()
    {
        miscSource.Pause();
        miscSource.clip = buttonClickSound;
        miscSource.Play();

    }

    public void PlayGameWonSound()
    {
        efxSource.Pause();
        efxSource.clip = gameWonSound;
        efxSource.Play();
    }

    public void PlayBridgeSound()
    {
        miscSource.Pause();
        miscSource.clip = bridgeSound;
        miscSource.Play();
    }

    public void PlayDeathSound()
    {
        efxSource.Pause();
        efxSource.clip = deathSound;
        efxSource.Play();
    }

    public void PlayPlayerMovementSound()
    {
        efxSource.Pause();
        efxSource.clip = playerMovementSound;
        efxSource.Play();
    }

    public void PlayScoreIndicatorSound()
    {
        efxSource.Pause();
        efxSource.clip = scoreIndicatorSound;
        efxSource.Play();
    }

    public void DecreaseMusicVolumeForWin()
    {
        if (musicSource.volume > 0)
        {
            musicSource.volume -= 0.9f * Time.deltaTime;
        }
        else
        {
            musicSource.volume = 0;
            isPlayingGameMusic = false;
            //musicSource.Pause();
        }
    }

    public void SetMusicVolumeToZero()
    {
        musicSource.volume = 0;
        isPlayingGameMusic = false;
    }

    public void SetMusicVolumeSettings()
    {
        if (SaveManager.miscInfo.soundDisabled)
        {
            startingMusicVolume = 0;
            musicSource.volume = 0;
            efxSource.volume = 0;
            miscSource.volume = 0;

        }
        else
        {
            musicSource.volume = 0.3f;
            efxSource.volume = 1;
            miscSource.volume = 1;
            startingMusicVolume = musicSource.volume;

        }
    }



}
