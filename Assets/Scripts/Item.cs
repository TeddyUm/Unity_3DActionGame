using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ITEMTYPE
    {
        AMMO,
        GRENADE,
        WEAPON,
        COIN, 
        HEART
    }

    public ITEMTYPE itemType;
    public int value;

    private void Update()
    {
        transform.Rotate(Vector2.up * 30 * Time.deltaTime);
    }
}
