using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon // should be scriptable object
{
    private int minDmg;
    private int maxDmg;
    private float range;

    public float Range => range;
    
    public Weapon()
    {
        minDmg = Random.Range(1, 13);
        maxDmg = Random.Range(minDmg, 13);
        range = 2f;
    }

    public void Hit(CharacterController damageable)
    {
        damageable.TakeDamage(RollDamage());
    }

    public int RollDamage()
    {
        return Random.Range(minDmg, maxDmg);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var controller = other.gameObject.GetComponent<CharacterController>();
        if (controller != null)
        {
            Hit(controller);
        }
    }
}
