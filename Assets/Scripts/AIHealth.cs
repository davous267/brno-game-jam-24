using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AIHealth : MonoBehaviour
{
    private float healthPoints = 100f;
    Animator animator;

    [SerializeField] float energyGain = 20f;
    [SerializeField] VisualEffect bloodVFX;
    [SerializeField] GameObject ribcage;
    [SerializeField] ParticleSystem deathParticles;

    private void Start()
    {
        animator = GetComponent<Animator>();
        bloodVFX = GetComponentInChildren<VisualEffect>();
    }

    public float TakeDamage(float damage)
    {
        healthPoints -= damage;
        bloodVFX.SetVector3("World Position", transform.position + (Vector3.up * 1.5f));
        bloodVFX.Play();
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
        Instantiate(ribcage, transform.position + (Vector3.up * 1.3f), Quaternion.identity);
        deathParticles.Play();
        Destroy(gameObject);

    }
}
