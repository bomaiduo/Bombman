using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy,IDamageable
{
    public void GetHit(float damage)
    {
        health -= damage;
        if(health <1)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }
   /*public void Setoff()//Animation Event
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
    }*/
}
