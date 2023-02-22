using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// each sound set up
[System.Serializable]
public class Sound
{
    // sound name 
    public string name;
    // sound clip
    public AudioClip clip;
    // sound source
    public AudioSource source;

    // volumn
    public bool loop;
    public float Volumn;

    // audio source setup
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volumn;
    }

    // volumn
    public void SetVolumn()
    {
        source.volume = Volumn;
    }
    // play the sound
    public void Play()
    {
        source.Play();
    }
    // stop the sound
    public void Stop()
    {
        source.Stop();
    }
    // sound loop
    public void SetLoop()
    {
        source.loop = true;
    }
    // sound loop cancel
    public void SetLoopCancel()
    {
        source.loop = false;
    }
}


public class SoundManager : MonoBehaviour
{
    // singleton
    private static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // sound init and make a sound array
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("Sound name : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // number of sound array
    public Sound[] sounds;
    // fade sound slaw rate
    public int soundChangeSlowRate = 1;

    // for sound fade in out check timer and bool
    private string increaseSoundName;
    private string decreaseSoundName;
    private bool isIncreaseSound;
    private bool isDeacreaseSound;
    private float soundIncrease = 0.0f;
    private float soundDecrease = 1.0f;

    private void Update()
    {
        // for fade in out
        SoundFadeInOut();
    }

    // play the sound depend on name
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
    // stop the sound depend on name
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }
    // loop the sound depend on name
    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }
    // stop loop the sound depend on name
    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }
    // volumn control
    public void SetVolumn(string _name, float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Volumn = _Volumn;
                sounds[i].SetVolumn();
                return;
            }
        }
    }

    // sound fade in
    public void SoundFadeIn(string _soundName)
    {
        increaseSoundName = _soundName;
        isIncreaseSound = true;
    }

    // sound fade out
    public void SoundFadeOut(string _soundName)
    {
        decreaseSoundName = _soundName;
        isDeacreaseSound = true;
    }

    private void SoundFadeInOut()
    {
        // volume up
        if (isIncreaseSound)
        {
            soundIncrease += Time.deltaTime / soundChangeSlowRate;
            SetVolumn(increaseSoundName, soundIncrease);

            if (soundIncrease > 1.0f)
            {
                isIncreaseSound = false;
                soundIncrease = 0.0f;
            }
        }
        // volume down
        if (isDeacreaseSound)
        {
            soundDecrease -= Time.deltaTime / soundChangeSlowRate;
            SetVolumn(decreaseSoundName, soundDecrease);

            if (soundDecrease < 0.0f)
            {
                Stop(decreaseSoundName);
                isDeacreaseSound = false;
                soundDecrease = 1.0f;
            }
        }
    }
}
