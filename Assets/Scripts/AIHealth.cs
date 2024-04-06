using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    private float healthPoints = 100f;
    Animator animator;

    [SerializeField] float energyGain = 20f;

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public float TakeDamage(float damage)
    {
        healthPoints -= damage;
        animator.SetTrigger("IsHit");
        if (healthPoints <= 0)
        {
            Die();
            return energyGain;
        }
        return 0f;
    }

    private void Die()
    {
        GetComponent<AIBrain>().enabled = false;

    }
}
