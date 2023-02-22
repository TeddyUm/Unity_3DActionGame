using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public GameObject howtoplayPanel;
    public GameObject creditPanel;

    void Start()
    {
        SoundManager.instance.Play("MenuBGM");
    }

    public void HowtoPlayButton()
    {
        SoundManager.instance.Play("Button");
        howtoplayPanel.SetActive(true);
    }
    public void CloseHowtoPlayButton()
    {
        SoundManager.instance.Play("Button");
        howtoplayPanel.SetActive(false);
    }
    public void CreditButton()
    {
        SoundManager.instance.Play("Button");
        creditPanel.SetActive(true);
    }
    public void CloseCreditButton()
    {
        SoundManager.instance.Play("Button");
        creditPanel.SetActive(false);
    }
    public void GameStartButton()
    {
        SoundManager.instance.Play("Button");
        SoundManager.instance.Stop("MenuBGM");
        GameManager.Instance.ChangeScene("MainGame");
    }
}
