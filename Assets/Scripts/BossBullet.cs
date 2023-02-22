using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage;
    public float bulletSpeed;
    public float lifeTime;
    public PlayerController player;
    public GameObject particle;

    private Rigidbody rigid;
    public bool isSpecial;
    

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rigid = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);

        if (isSpecial)
        {
            SoundManager.instance.Play("Weapon5");
            rigid.AddForce(transform.forward * 700);
            rigid.AddForce(transform.up * 700);
        }
    }
    private void Update()
    {
        if(!isSpecial)
            transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            player.HitParticle();
            Instantiate(particle, transform.position, Quaternion.identity);
            player.playerHP -= damage;
            Destroy(gameObject);
        }
    }
}
