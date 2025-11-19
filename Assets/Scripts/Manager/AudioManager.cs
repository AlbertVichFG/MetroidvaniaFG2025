using System.Collections;
using System.Data.Common;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource musicSource;
    private AudioSource ambinetSource;
    private AudioSource[] sfxSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        ambinetSource = gameObject.AddComponent<AudioSource>();
        ambinetSource.loop = true;
        sfxSource = new AudioSource[5];
        for(int i = 0; i < sfxSource.Length; i++)
        {
            sfxSource[i] = gameObject.AddComponent<AudioSource>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayMusic(AudioClip _music, float _volume =1)
    {
        musicSource.clip = _music;
        musicSource.volume = _volume;
        musicSource.Play();
    }

    public void StopMusic(AudioClip _music)
    {
        musicSource.Stop();
    }

    public void FadeOutMusic(float _speed)
    {
        StartCoroutine(FadeOutAudio(musicSource, _speed));
    }

    IEnumerator FadeOutAudio(AudioSource _source, float _speed)
    {
        float volume = _source.volume;
        while (volume > 0)
        {
            volume -= Time.deltaTime * _speed;
            _source.volume = volume;
            yield return null;
        }
    }

    public void PlayAmbient(AudioClip _ambient, float _volume=1)
    {
        ambinetSource.clip = _ambient;
        ambinetSource.volume = _volume;
        ambinetSource.Play();
    }


    public void StopAmbient(AudioClip _ambient)
    {
        ambinetSource.Stop();
    }

    public void FadeOutAmbient(float _speed)
    {
        StartCoroutine(FadeOutAudio(ambinetSource, _speed));
    }

    public void PlaySFX(AudioClip _sfx, float _volume = 1)
    {
        for (int i = 0; i < sfxSource.Length; i++) {
            if (sfxSource[i].isPlaying == false)
            {
                sfxSource[i].clip = _sfx;
                sfxSource[i].volume = _volume;
                sfxSource[i].Play();
                break;
            }
        }
    }
}
