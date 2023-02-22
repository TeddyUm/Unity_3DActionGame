using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject enemyObj;
    public GameObject enemyRangeObj;
    public GameObject enemyBossObj;
    public List<GameObject> listEnemy = new List<GameObject>();
    public UI_MainGame uiGame;
    public bool stageChange;
    void Start()
    {
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.stageNum != 3)
        {
            for (int i = listEnemy.Count - 1; i >= 0; i--)
            {
                if (listEnemy[i].GetComponent<Enemy>().deathTrigger)
                {
                    listEnemy.Remove(listEnemy[i]);
                }
            }
        }
        else
        {
            if (listEnemy[0].GetComponent<Enemy>().deathTrigger)
            {
                listEnemy.Remove(listEnemy[0]);
            }
        }

        if(listEnemy.Count < 1)
        {
            stageChange = true;
        }

        if (stageChange)
        {
            GameManager.Instance.score += (int)GameManager.Instance.time;
            GameManager.Instance.time = 60;
            stageChange = false;
            listEnemy.Clear();
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        GameManager.Instance.stageNum++;

        if(GameManager.Instance.stageNum > 3)
        {
            SoundManager.instance.Stop("GameBGM");
            GameManager.Instance.ChangeScene("Result");
        }

        uiGame.WaveTextReveal();
        listEnemy.Clear();
       
        if(GameManager.Instance.stageNum == 3)
        {
            listEnemy.Add(Instantiate(enemyBossObj, spawnPoint[1].position, Quaternion.identity));
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if (GameManager.Instance.stageNum == 1)
                    listEnemy.Add(Instantiate(enemyObj, spawnPoint[i].position, Quaternion.identity));
                if (GameManager.Instance.stageNum == 2)
                    listEnemy.Add(Instantiate(enemyRangeObj, spawnPoint[i].position, Quaternion.identity));
            }
        }
    }
}
