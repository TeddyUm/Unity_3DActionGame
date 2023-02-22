using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum AttackType
    {
        MELEE,
        RANGE,
        MULTIPLE,
        HORMING,
        GRENADE
    }

    public AttackType atkType;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer atkTrail;
    public string weaponName;

    public void Use()
    {
        if(atkType == AttackType.MELEE)
        {
            SoundManager.instance.Play("Weapon1");
            StopCoroutine("Bash");
            StartCoroutine("Bash");
        }
    }
    IEnumerator Bash()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        atkTrail.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
        atkTrail.enabled = false;
    }


    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.1f);
    }

}
