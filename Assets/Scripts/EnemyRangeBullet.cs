using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeBullet : MonoBehaviour
{
    public int damage;
    public float bulletSpeed;
    public float lifeTime;
    public PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            player.HitParticle();
            player.playerHP -= damage;
            Destroy(gameObject);
        }
    }
}
