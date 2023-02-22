using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainGame : MonoBehaviour
{
    public GameObject optionPanel;
    public PlayerController player;
    public Text HP;
    public Text TimeText;
    public Text ScoreText;
    public Text WaveText;
    public Text RemainText;
    public Text WeaponText;
    public Text AmmoText;
    public Image hpGuage;
    public GameObject hpObj;
    public GameObject timeText;

    private EnemySpawner enemySpawner;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        enemySpawner = GameObject.Find("Enemies").GetComponent<EnemySpawner>();
        SoundManager.instance.Play("GameBGM");
    }

    private void Update()
    {
        HP.text = "HP: " + player.playerHP;
        TimeText.text = "Time: " + (int)GameManager.Instance.time;
        ScoreText.text = "Score: " + GameManager.Instance.score;
        RemainText.text = "Remain: " + enemySpawner.listEnemy.Count;
        WeaponText.text = "Weapon: " + player.equipWeapon.weaponName;

        if(GameManager.Instance.stageNum == 3)
        {
            hpObj.SetActive(true);
            hpGuage.fillAmount = (float)enemySpawner.listEnemy[0].GetComponent<Enemy>().curHP / (float)enemySpawner.listEnemy[0].GetComponent<Enemy>().maxHP;
        }
        else
        {
            hpObj.SetActive(false);
        }

        switch (player.weaponIdx)
        {
            case 1:
                WeaponText.text = "Bow";
                AmmoText.text = "Ammo: " + player.bowAmmo;
                break;
            case 2:
                WeaponText.text = "Multiple";
                AmmoText.text = "Ammo: " + player.multiAmmo;
                break;
            case 3:
                WeaponText.text = "Homing";
                AmmoText.text = "Ammo: " + player.homingAmmo;
                break;
            case 4:
                WeaponText.text = "Grenade";
                AmmoText.text = "Ammo: " + player.grenadeAmmo;
                break;
            default:
                WeaponText.text = "Sword";
                AmmoText.text = "Ammo: Infinite";
                break;
        }

        GameManager.Instance.time -= Time.deltaTime;

        if(GameManager.Instance.time < 0)
        {
            SoundManager.instance.Stop("GameBGM");
            GameManager.Instance.ChangeScene("GameOver");
        }
    }

    public void Option()
    {
        SoundManager.instance.Play("Button");
        optionPanel.SetActive(true);

        Time.timeScale = 0;

    }
    public void OptionClose()
    {
        SoundManager.instance.Play("Button");
        Time.timeScale = 1;
        optionPanel.SetActive(false);
    }

    public void WaveTextReveal()
    {
        StartCoroutine("StageManage");
    }

    IEnumerator StageManage()
    {
        if(GameManager.Instance.stageNum < 3)
        {
            WaveText.text = "Stage " + GameManager.Instance.stageNum;
        }
        else
        {
            WaveText.text = "Boss Stage";
        }
        WaveText.enabled = true;
        yield return new WaitForSeconds(1.0f);
        WaveText.enabled = false;
    }
}
