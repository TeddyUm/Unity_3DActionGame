using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Result : MonoBehaviour
{
    public Text score;
    void Start()
    {
        SoundManager.instance.Play("WinBGM");
    }
    private void Update()
    {
        score.text = "Final Score: " + GameManager.Instance.score;
    }
    public void RetryButton()
    {
        GameManager.Instance.stageNum = 0;
        GameManager.Instance.score = 0;
        GameManager.Instance.time = 100;
        SoundManager.instance.Play("Button");
        SoundManager.instance.Stop("WinBGM");
        GameManager.Instance.ChangeScene("MainMenu");
    }
    public void QuitButton()
    {
        SoundManager.instance.Play("Button");
        Application.Quit();
    }
}
