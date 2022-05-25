using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestroy : MonoBehaviour, IAttackable
{
    public int Health = 30;
    public ParticleSystem DamageParticle;

    private void FixedUpdate()
    {
        if (Health <= 0) DestroiBox();
    }

    public void DealDamage(int Conts)
    {
        Health -= Conts;
        DamageParticle.Play();
    }

    public void DestroiBox()
    {
        DamageParticle.transform.parent = null;
        DamageParticle.Play();
        Destroy(gameObject);
    }


}
