using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
    }

    // weather particles
    [SerializeField]
    ParticleSystem rain;
    [SerializeField]
    ParticleSystem thunder;
    // global light
    [SerializeField]
    private UnityEngine.Experimental.Rendering.Universal.Light2D sunLight;
    // dark rate at overcast
    public float darkLimit;

    // for fade in out function
    private bool isFadeInRain;
    private float fadeInRainTimer;
    private bool isFadeOutRain;
    private float fadeOutRainTimer;
    private bool isFadeInOverCast;
    private float fadeInOvercastTimer;
    private bool isFadeOutOvercast;
    private float fadeOutOvercastTimer;

    private void Start()
    {
        // init data
        darkLimit = 0.5f;
        fadeInRainTimer = 0.0f;
        fadeOutRainTimer = 1.0f;
        fadeInOvercastTimer = 1.0f;
        fadeOutOvercastTimer = darkLimit;
    }

    private void Update()
    {
        RainCheck();
        OvercastCheck();
    }

    private void RainCheck()
    {
        // fade in rain (using particle emit control)
        if (isFadeInRain)
        {
            fadeInRainTimer += Time.deltaTime / 2;
            RainDrop((int)(fadeInRainTimer));

            if (fadeInRainTimer > 1.0f)
            {
                rain.Play();
                fadeInRainTimer = 0.0f;
                isFadeInRain = false;
            }
        }
        // fade out rain particle
        if (isFadeOutRain)
        {
            fadeOutRainTimer -= Time.deltaTime / 2;
            RainDrop((int)(fadeOutRainTimer));
            rain.Stop();

            if (fadeOutRainTimer < 0.0f)
            {
                rain.Stop();
                fadeOutRainTimer = 1.0f;
                isFadeOutRain = false;
            }
        }
    }

    private void OvercastCheck()
    {
        // darker than before
        if (isFadeInOverCast)
        {
            fadeInOvercastTimer -= Time.deltaTime / 2;
            sunLight.intensity = fadeInOvercastTimer;
            if (fadeInOvercastTimer < darkLimit)
            {
                sunLight.intensity = darkLimit;
                fadeInOvercastTimer = 1.0f;
                isFadeInOverCast = false;
            }
        }
        // change to sunny weather
        if (isFadeOutOvercast)
        {
            fadeOutOvercastTimer += Time.deltaTime / 2;
            sunLight.intensity = fadeOutOvercastTimer;
            if (fadeOutOvercastTimer > 1.0f)
            {
                sunLight.intensity = 1.0f;
                fadeOutOvercastTimer = darkLimit;
                isFadeOutOvercast = false;
            }
        }
    }

    // for set weather (bool change)
    public void RainPlay()
    {
        isFadeInRain = true;
    }

    public void RainStop()
    {
        isFadeOutRain = true;
    }

    public void RainDrop(int num)
    {
        rain.Play();
        rain.Emit(num);
    }

    public void ThunderPlay()
    {
        thunder.Play();
    }

    public void ThunderStop()
    {
        thunder.Stop();
    }
    public void OvercastOn()
    {
        isFadeInOverCast = true;
    }

    public void OvercastOff()
    {
        isFadeOutOvercast = true;
    }
    public void LightPower(float lightIntensity)
    {
        sunLight.intensity = lightIntensity;
    }
}
