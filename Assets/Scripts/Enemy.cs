using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ENEMYTYPE
{
    MELEE,
    RANGE,
    BOSS
}
public class Enemy : MonoBehaviour
{
    public int maxHP;
    public int curHP;
    public Animator anim;
    public Transform target;
    public ENEMYTYPE curType;
    public bool deathTrigger;
    public GameObject deathParticle;
    public NavMeshAgent navEnemy;
    public GameObject enemyBullet;
    public GameObject enemySpecialBullet;
    public GameObject hitEffect;
    public GameObject swordObj;
    public bool isBoss;
    public GameObject timeUI;

    private Rigidbody rigid;
    private BoxCollider col;
    private bool damageTrigger;
    private bool canAttack;
    private bool canSAttack;
    private float attackDelay;
    private float attackTimer;
    private float sAttackDelay;
    private float sAttackTimer;

    private PlayerController player;
    private bool scoreCheck;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        navEnemy = GetComponent<NavMeshAgent>();
        damageTrigger = false;
        // attack delay var
        canAttack = true;
        attackTimer = 0.0f;
        attackDelay = 2.0f;
        // special attack delay var
        canSAttack = true;
        sAttackTimer = 0.0f;
        sAttackDelay = 5.0f;

        target = GameObject.Find("Player").transform;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        navEnemy.enabled = false;
        navEnemy.enabled = true;
    }

    void Update()
    {
        if (curType == ENEMYTYPE.MELEE)
        {
            navEnemy.SetDestination(new Vector3(target.position.x, target.position.y + 2, target.position.z));
            anim.SetFloat("Move", 1.0f);
        }
        else if (curType == ENEMYTYPE.RANGE)
        {
            if (Vector3.Distance(gameObject.transform.position, target.position) > 50)
            {
                navEnemy.isStopped = false;
                navEnemy.SetDestination(new Vector3(target.position.x, target.position.y + 2, target.position.z));
                anim.SetFloat("Move", 1.0f);
            }
            else
            {
                transform.LookAt(player.gameObject.transform);
                navEnemy.isStopped = true;
                rigid.velocity = new Vector3(0, 0, 0);
                Fire();
            }
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, target.position) > 50)
            {
                navEnemy.isStopped = false;
                navEnemy.SetDestination(new Vector3(target.position.x, target.position.y + 2, target.position.z));
                anim.SetFloat("Move", 1.0f);
            }
            else
            {
                transform.LookAt(player.gameObject.transform);
                navEnemy.isStopped = true;
                rigid.velocity = new Vector3(0, 0, 0);
                Fire();
            }
        }

        if (curHP < 1)
        {
            if (!scoreCheck)
            {
                SoundManager.instance.Play("EnemyDie");
                scoreCheck = true;
                GameManager.Instance.time += 3;

                if (curType == ENEMYTYPE.MELEE)
                {
                    GameManager.Instance.score += 2;
                }
                if (curType == ENEMYTYPE.RANGE)
                {
                    GameManager.Instance.score += 5;
                }
                if (curType == ENEMYTYPE.BOSS)
                {
                    GameManager.Instance.score += 30;
                }

                GameObject tempTimeUI = Instantiate(timeUI);
                tempTimeUI.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            }
            deathTrigger = true;
            anim.SetTrigger("Death");
            navEnemy.isStopped = true;
            col.enabled = false;
            canAttack = false;
            attackTimer = 0;
            deathParticle.SetActive(true);
            Invoke("SetActiveClose", 1.0f);
        }

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDelay)
            {
                attackTimer = 0;
                canAttack = true;
            }
        }

        if (!canSAttack)
        {
            sAttackTimer += Time.deltaTime;
            if (sAttackTimer > sAttackDelay)
            {
                sAttackTimer = 0;
                canSAttack = true;
            }
        }
    }

    void Fire()
    {
        if (canAttack)
        {
            canAttack = false;
            if (isBoss)
            {
                for(int i = 0; i < 10; i++)
                {
                    Quaternion qRotation = Quaternion.Euler(0, i * 36, 0);
                    Instantiate(enemyBullet, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), qRotation);
                }
            }
            else
            {
                Instantiate(enemyBullet, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.transform.rotation);
            }
            anim.SetTrigger("Attack");
        }
        if (canSAttack && isBoss)
        {
            canSAttack = false;
            Instantiate(enemySpecialBullet, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.transform.rotation);
            anim.SetTrigger("Attack");
        }
    }

    public void SetActiveClose()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            SoundManager.instance.Play("EnemyDamage");
            swordObj = GameObject.Find("Sword");
            Instantiate(hitEffect, swordObj.transform.position, Quaternion.identity);
            curHP -= player.equipWeapon.damage;
        }

        if (other.tag == "Bullet")
        {
            SoundManager.instance.Play("EnemyDamage");
            curHP -= player.equipWeapon.damage;
        }

        if (other.tag == "Explosion")
        {
            curHP -= 30;
        }
        // melee attack
        if (other.tag == "Player" && canAttack)
        {
            SoundManager.instance.Play("EnemyDamage");
            canAttack = false;
            anim.SetTrigger("Attack");
            if(curType == ENEMYTYPE.MELEE)
            {
                player.playerHP -= 5;
            }
            player.HitParticle();
        }
    }
}
