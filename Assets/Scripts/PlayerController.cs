using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    // arrow
    public GameObject arrowBullet;
    // multiple shot (spread)
    public GameObject multipleBullet;
    // homing 
    public GameObject homingBullet;
    // grenade
    public GameObject grenadeBullet;
    public GameObject goldParticle;
    public GameObject heartParticle;
    public GameObject hitParticle;
    public int playerHP;
    public Weapon equipWeapon;

    public int bowAmmo;
    public int multiAmmo;
    public int homingAmmo;
    public int grenadeAmmo;

    private float hAxis;
    private float vAxis;
    private Vector3 moveVec;
    private bool jumpDown;
    private bool isJump;
    private bool BoostDown;
    private bool isBoost;
    private bool isDown;
    private bool isWeapon1;
    private bool isWeapon2;
    private bool isWeapon3;
    private bool isWeapon4;
    private bool isWeapon5;
    private bool atkDown;
    private bool canAtk;

    private Rigidbody rigid;
    private GameObject weapon;
    public int weaponIdx;

    private float fireDelay;
    private Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        equipWeapon = weapons[0].GetComponent<Weapon>();
        fireDelay = 1.0f;
        playerHP = 100;
        equipWeapon.weaponName = "Sword";
    }

    void Update()
    {
        InputKey();
        PlayerMove();
        Jump();
        Boost();
        Fire();
        Attack();
        Swap();

        if(playerHP < 1)
        {
            SoundManager.instance.Stop("GameBGM");
            GameManager.Instance.ChangeScene("GameOver");
        }
    }

    private void PlayerMove()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

        if(Mathf.Abs(hAxis) > 0.1f || Mathf.Abs(vAxis) > 0.1f)
        {
            anim.SetFloat("Move", 1);
        }
        else
        {
            anim.SetFloat("Move", 0);
        }
    }

    private void InputKey()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jumpDown = Input.GetButtonDown("Jump");
        BoostDown = Input.GetButtonDown("Boost");
        isDown = Input.GetButtonDown("Interaction");
        isWeapon1 = Input.GetButtonDown("Swap1");
        isWeapon2 = Input.GetButtonDown("Swap2");
        isWeapon3 = Input.GetButtonDown("Swap3");
        isWeapon4 = Input.GetButtonDown("Swap4");
        isWeapon5 = Input.GetButtonDown("Swap5");
        atkDown = Input.GetButtonDown("Fire1");
    }

    private void Swap()
    {
        if(isWeapon1)
        {
            weaponIdx = 0;
        }
        if (isWeapon2)
        {
            weaponIdx = 1;
        }
        if (isWeapon3)
        {
            weaponIdx = 2;
        }
        if (isWeapon4)
        {
            weaponIdx = 3;
        }
        if (isWeapon5)
        {
            weaponIdx = 4;
        }

        if ((isWeapon1 || isWeapon2 || isWeapon3 || isWeapon4 || isWeapon5) && !isJump && !isBoost)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
                equipWeapon = weapons[weaponIdx].GetComponent<Weapon>();
                equipWeapon.gameObject.SetActive(true);

                isWeapon1 = false;
                isWeapon2 = false;
                isWeapon3 = false;
                isWeapon4 = false;
                isWeapon5 = false;
            }
        }
    }

    // melee attack
    public void Attack()
    {
        if (equipWeapon == null)
            return;
        fireDelay += Time.deltaTime;
        canAtk = equipWeapon.rate < fireDelay;

        if(atkDown && canAtk && !isBoost)
        {
            equipWeapon.Use();
            anim.SetTrigger("Bash");
            fireDelay = 0;
        }
    }

    private void Fire()
    {
        // arrow
        if(Input.GetMouseButtonDown(0) && weaponIdx == 1)
        {
            if (bowAmmo > 0)
            {
                SoundManager.instance.Play("Weapon2");
                bowAmmo--;
                Instantiate(arrowBullet, transform.position, Quaternion.identity);
            }
        }
        // multiple
        if (Input.GetMouseButtonDown(0) && weaponIdx == 2)
        {
            SoundManager.instance.Play("Weapon3");
            if (multiAmmo > 0)
            {
                multiAmmo--;

                for (int i = 0; i < 3; i++)
                {
                    switch (i)
                    {
                        case 0:

                            Instantiate(multipleBullet, transform.position, transform.rotation);
                            break;
                        case 1:
                            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 20, transform.eulerAngles.z));
                            Instantiate(multipleBullet, transform.position, transform.rotation);
                            break;
                        case 2:
                            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 40, transform.eulerAngles.z));
                            Instantiate(multipleBullet, transform.position, transform.rotation);
                            break;
                    }
                }
            }
        }
        // homing
        if (Input.GetMouseButtonDown(0) && weaponIdx == 3)
        {
            SoundManager.instance.Play("Weapon4");
            if (homingAmmo > 0)
            {
                homingAmmo--;
                Instantiate(homingBullet, transform.position, Quaternion.identity);
            }
        }
        // grenade
        if (Input.GetMouseButtonDown(0) && weaponIdx == 4)
        {
            SoundManager.instance.Play("Weapon5");
            if (grenadeAmmo > 0)
            {
                grenadeAmmo--;
                Instantiate(grenadeBullet, transform.position, Quaternion.identity);
            }
        }
    }

    void Jump()
    {
        if(jumpDown && !isJump)
        {
            SoundManager.instance.Play("Jump");
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }
    void Boost()
    {
        if (BoostDown && !isBoost && !isJump)
        {
            SoundManager.instance.Play("Dash");
            speed *= 2;
            isBoost = true;

            Invoke("BoostOut", 0.5f);
        }
    }

    void BoostOut()
    {
        speed *= 0.5f;
        isBoost = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "Gold")
        {
            SoundManager.instance.Play("Item");
            GameManager.Instance.score += 5;
            Instantiate(goldParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            Destroy(other.gameObject);
        }


        if (other.tag == "Heart")
        {
            SoundManager.instance.Play("Item");
            playerHP += 5;
            Instantiate(heartParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            if (playerHP > 100)
            {
                playerHP = 100;
            }

            Destroy(other.gameObject);
        }

        if (other.tag == "Ammo")
        {
            SoundManager.instance.Play("Item");
            bowAmmo += 10;
            multiAmmo += 10;
            homingAmmo += 5;
            grenadeAmmo += 1;

            Instantiate(goldParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);

            Destroy(other.gameObject);
        }
    }

    public void HitParticle()
    {
        Instantiate(hitParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
    }
}
