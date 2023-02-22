using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject goldObj;
    public GameObject heartObj;
    public GameObject ammoObj;
    public List<GameObject> listItem = new List<GameObject>();
    public EnemySpawner enemySpawner;

    void Start()
    {
        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawner.stageChange)
        {
            if (listItem.Count > 0)
            {
                for (int i = 0; i < listItem.Count; i++)
                {
                    Destroy(listItem[i].gameObject);
                }
            }

            listItem.Clear();
            SpawnItem();
            enemySpawner.stageChange = false;
        }
    }

    void SpawnItem()
    {
        for (int i = 0; i < 5; i++)
        {
            int randNum = Random.Range(0, 100);

            if (randNum < 30)
            {
                listItem.Add(Instantiate(goldObj, spawnPoint[i].position, Quaternion.identity));
            }
            else if (randNum >= 30 && randNum < 60)
            {
                listItem.Add(Instantiate(heartObj, spawnPoint[i].position, Quaternion.identity));
            }
            else
            {
                listItem.Add(Instantiate(ammoObj, spawnPoint[i].position, Quaternion.identity));
            }
        }
    }
}