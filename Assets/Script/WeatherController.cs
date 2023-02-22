using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// weather state
public enum WEATHERSTATE
{
    SUNNY,
    OVERCAST,
    RAIN,
    THUNDER
}

public class WeatherController : MonoBehaviour
{
    [SerializeField]
    private WEATHERSTATE wState;
    [SerializeField]
    // weather change between max and min time (random)
    private float weatherMaxTimer;
    [SerializeField]
    private float weatherMinTimer;

    // for time check
    private float weatherCurLimit;
    private float weatherCurTimer = 0;
    private bool isChangeWeather;

    private float thunderTime = 0;
    private float thunderMaxTime;
    private bool isThunder;

    void Start()
    {
        // starting is sunny weather
        wState = WEATHERSTATE.SUNNY;
        weatherCurLimit = Random.Range(weatherMinTimer, weatherMaxTimer);
        SoundManager.Instance.Play("Sunny");

        thunderMaxTime = Random.Range(1, 4);
    }

    void Update()
    {
        // change weather timer update
        weatherCurTimer += Time.deltaTime;
        if (isThunder)
            thunderTime += Time.deltaTime;

        if (weatherCurTimer > weatherCurLimit)
        {
            WeatherCheck();
        }

        if (isThunder && thunderTime > thunderMaxTime && wState == WEATHERSTATE.THUNDER)
        {
            ThunderOn();
            Invoke("ThunderOff", 0.1f);
        }
    }

    private void WeatherCheck()
    {
        // weather timer change using random
        weatherCurLimit = Random.Range(weatherMinTimer, weatherMaxTimer);
        weatherCurTimer = 0;
        isChangeWeather = true;

        // change weather depend on the weather state
        if (wState == WEATHERSTATE.SUNNY && isChangeWeather)
        {
            SoundManager.Instance.Play("Overcast");
            SoundManager.Instance.SoundFadeIn("Overcast");
            SoundManager.Instance.SoundFadeOut("Sunny");
            wState = WEATHERSTATE.OVERCAST;
            ChangeWeather(WEATHERSTATE.OVERCAST);
        }
        else if (wState == WEATHERSTATE.OVERCAST && isChangeWeather)
        {
            SoundManager.Instance.Play("Rain");
            SoundManager.Instance.SoundFadeIn("Rain");
            SoundManager.Instance.SoundFadeOut("Overcast");
            wState = WEATHERSTATE.RAIN;
            ChangeWeather(WEATHERSTATE.RAIN);
        }
        else if (wState == WEATHERSTATE.RAIN && isChangeWeather)
        {
            SoundManager.Instance.Play("Thunder");
            SoundManager.Instance.SoundFadeIn("Thunder");
            SoundManager.Instance.SoundFadeOut("Rain");
            wState = WEATHERSTATE.THUNDER;
            ChangeWeather(WEATHERSTATE.THUNDER);
            isThunder = true;
        }
        else if (wState == WEATHERSTATE.THUNDER && isChangeWeather)
        {
            SoundManager.Instance.Play("Sunny");
            SoundManager.Instance.SoundFadeIn("Sunny");
            SoundManager.Instance.SoundFadeOut("Thunder");
            wState = WEATHERSTATE.SUNNY;
            ChangeWeather(WEATHERSTATE.SUNNY);
        }
    }

    // change weather paricle
    private void ChangeWeather(WEATHERSTATE _wState)
    {
        if (_wState == WEATHERSTATE.SUNNY)
        {
            WeatherManager.Instance.OvercastOff();
            WeatherManager.Instance.RainStop();
            WeatherManager.Instance.ThunderStop();
            isThunder = false;
            isChangeWeather = false;
        }
        else if (_wState == WEATHERSTATE.OVERCAST)
        {
            WeatherManager.Instance.OvercastOn();

            isChangeWeather = false;
        }
        else if (_wState == WEATHERSTATE.RAIN)
        {
            WeatherManager.Instance.RainPlay();

            isChangeWeather = false;
        }
        else if (_wState == WEATHERSTATE.THUNDER)
        {
            WeatherManager.Instance.ThunderPlay();

            isChangeWeather = false;
        }
    }

    void ThunderOn()
    {
        WeatherManager.Instance.LightPower(3.0f);
    }
    void ThunderOff()
    {
        WeatherManager.Instance.LightPower(WeatherManager.Instance.darkLimit);
        thunderTime = 0.0f;
        thunderMaxTime = Random.Range(1, 4);
    }
}
