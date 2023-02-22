using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    void Start()
    {
        SoundManager.instance.Play("GameoverBGM");
    }
    public void RetryButton()
    {
        GameManager.Instance.stageNum = 0;
        GameManager.Instance.score = 0;
        GameManager.Instance.time = 100;
        SoundManager.instance.Play("Button");
        SoundManager.instance.Stop("GameoverBGM");
        GameManager.Instance.ChangeScene("MainMenu");
    }
    public void QuitButton()
    {
        SoundManager.instance.Play("Button");
        Application.Quit();
    }
}
