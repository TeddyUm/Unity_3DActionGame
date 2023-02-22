using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BULLETTYPE
{
    ARROW,
    HOMING,
    SPREAD,
    GRENADE
}

public class PlayerBullet : MonoBehaviour
{
    public int damage;
    public float bulletSpeed;
    public float lifeTime;
    public BULLETTYPE bulletType;
    public GameObject particle;
    public EnemySpawner enemyspawner;

    // homing variable
    public GameObject target;
    public Vector3 dirVec;

    private Rigidbody rigid;
    private SphereCollider col;
    private GameObject originObj;
    private int targetNum;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        originObj = GameObject.Find("Player");
        enemyspawner = GameObject.Find("Enemies").GetComponent<EnemySpawner>();

        if (bulletType == BULLETTYPE.HOMING)
        {
            float distance;
            float minDistance = 100000;

            for (int i = 0; i < enemyspawner.listEnemy.Count; i++)
            {
                distance = Vector3.Distance(enemyspawner.listEnemy[i].transform.position, originObj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetNum = i;
                }
            }
            target = enemyspawner.listEnemy[targetNum];
        }
    }

    private void Start()
    {
        transform.position = new Vector3(originObj.transform.position.x, originObj.transform.position.y + 1.5f, originObj.transform.position.z);
        if (bulletType != BULLETTYPE.SPREAD)
            transform.rotation = originObj.transform.rotation;

        if (bulletType == BULLETTYPE.GRENADE)
        {
            rigid.AddForce(originObj.transform.forward * 700);
            rigid.AddForce(originObj.transform.up * 500);
        }
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        target = GameObject.Find("rangeEnemy");

        if (bulletType == BULLETTYPE.ARROW)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        }
        if (bulletType == BULLETTYPE.SPREAD)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        }
    }
    void FixedUpdate()
    {
        if (bulletType == BULLETTYPE.HOMING)
        {
            if (target != null)
            {
                dirVec = (new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z) - transform.position).normalized;
                transform.position += dirVec * Time.deltaTime * bulletSpeed;
                transform.forward = dirVec;
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            Instantiate(particle, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), Quaternion.identity);
            rigid.velocity = new Vector3(0, 0, 0);
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            Instantiate(particle, gameObject.transform.position, Quaternion.identity);
            rigid.velocity = new Vector3(0, 0, 0);
            Destroy(gameObject);
        }
    }
}
