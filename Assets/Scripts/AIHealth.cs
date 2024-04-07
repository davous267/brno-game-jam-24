using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class AIHealth : MonoBehaviour
{
    private float healthPoints = 100f;
    Animator animator;

    [SerializeField] float energyGain = 20f;
    [SerializeField] VisualEffect bloodVFX;
    [SerializeField] GameObject ribcage;
    [SerializeField] ParticleSystem deathParticles;

    public bool isDead = false;

    [SerializeField] List<GameObject> gameObjectsToDisable;

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
        isDead = true;
        GetComponent<AIBrain>().enabled = false;
        GetComponentInParent<Collider>().enabled = false;
        GetComponentInParent<NavMeshAgent>().enabled = false;
        Instantiate(ribcage, transform.position + (Vector3.up * 1.3f), Quaternion.Euler(90f, 0f, 0f));

        deathParticles.Play();
        Destroy(gameObject, 5f);
        DisableMesh();
        this.enabled = false;

    }

    private void DisableMesh()
    {
        foreach (GameObject obj in gameObjectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
