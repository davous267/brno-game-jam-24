using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;


public class AIBrain : MonoBehaviour
{

    enum EnemyType
    {
        Civil,
        Soldier
    }

    enum EnemyState
    {
        Standing,
        Fleeting,
        Wandering,
        MovingToPlayer,
        AttackingPlayer,
        ResetWandering
    }

    [SerializeField] EnemyType enemyType;
    [SerializeField] EnemyState enemyState;

    [SerializeField] float wanderingRadius = 5f;
    [SerializeField] float targetPositionTolerance = 1f;
    [SerializeField] float raycastDistance = 10f;
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] float rotationSmooth = 5f;
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float attackDamage = 20f;

    [SerializeField] AudioSource punchAudioSource;
 

    private float timeOfLastAttack = 0f;

    private bool isAttacking = false;

    private Vector3 targetPosition = Vector3.zero;
    private Vector3 lastWanderingPosition = Vector3.zero;

    [SerializeField] Transform dungeonExit;

    private Player player;
    private AIHealth health;

    NavMeshAgent navMeshAgent;

    Animator animator;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (enemyType == EnemyType.Civil)
        {
            enemyState = EnemyState.Standing;
        }
        else if (enemyType == EnemyType.Soldier)
        {
            enemyState = EnemyState.Wandering;
        }

        //StartCoroutine(FindPlayer());
        player = GameManager.Instance.Player;
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<AIHealth>();

    }

    void Update()
    {
        if(player == null)
        {
            Debug.Log("cant find player");
            player = GameManager.Instance.Player;
        }
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        Behave();




        //Debug.Log(enemyState);
        //Debug.Log(targetPosition);
    }

    IEnumerator FindPlayer()
    {
        yield return null;
        player = GameManager.Instance.Player;
    }

    private void Behave()
    {
        switch (enemyState)
        {
            case EnemyState.Standing:
                navMeshAgent.isStopped = true;
                if(IsPlayerInSight())
                {
                    StateTransition(EnemyState.Fleeting);
                }
                break;

            case EnemyState.Wandering: 
                if (Vector3.Distance(transform.position, targetPosition) <= targetPositionTolerance || targetPosition == Vector3.zero)
                {
                    targetPosition = ChooseRandomPosition();
                    navMeshAgent.destination = targetPosition;
                }

                if (IsPlayerInSight())
                {
                    lastWanderingPosition = transform.position;
                    StateTransition(EnemyState.MovingToPlayer);
                }

                break;

            case EnemyState.Fleeting:
                navMeshAgent.destination = dungeonExit.position;
                navMeshAgent.isStopped = false;

                if (Vector3.Distance(transform.position, dungeonExit.position) <= 1f)
                {
                    Destroy(gameObject);
                }
                break;

            case EnemyState.MovingToPlayer:

                if (Vector3.Distance(transform.position, player.transform.position) <= chaseDistance)
                {
                    navMeshAgent.destination = player.transform.position;
                }
                else
                {
                    StateTransition(EnemyState.ResetWandering);
                }

                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    StateTransition(EnemyState.AttackingPlayer);
                }
                break;

            case EnemyState.AttackingPlayer:

                navMeshAgent.isStopped = true;
                Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
                Quaternion newRotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSmooth);

                if (Vector3.Distance(transform.position, player.transform.position) >= attackDistance + 0.5f && isAttacking == false)
                {
                    navMeshAgent.isStopped = false;
                    StateTransition(EnemyState.MovingToPlayer);
                }

                if (Time.time - timeOfLastAttack >= timeBetweenAttacks)
                {
                    Debug.Log("attack");
                    Attack();
                }
  
                break;

            case EnemyState.ResetWandering:

                targetPosition = lastWanderingPosition;
                navMeshAgent.destination = targetPosition;

                if(Vector3.Distance(transform.position, targetPosition) <= targetPositionTolerance)
                {
                    StateTransition(EnemyState.Wandering);
                }

                if (IsPlayerInSight())
                {
                    StateTransition(EnemyState.MovingToPlayer);
                }
                break;  

            default:
                break;
        }
    }

    private Vector3 ChooseRandomPosition()
    {

        for (int i = 0; i < 10; i++ )
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderingRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderingRadius, 1);
            Vector3 finalPosition = hit.position;

            NavMeshPath path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, finalPosition, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    return finalPosition;
                }
            }
        }

        return transform.position;
    }

    private void Attack()
    {
        timeOfLastAttack = Time.time;
        isAttacking = true;
        animator.SetTrigger("Attack");
        //Invoke("Hit", 0.35f);


    }

    private bool IsPlayerInSight()
    {
        Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position).normalized * raycastDistance, Color.blue);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, ((player.transform.position - Vector3.up) - transform.position).normalized, out hit, raycastDistance))
        {
            if (hit.collider.GetComponent<Player>() != null)
            {
                return true;
                //lastWanderingPosition = transform.position;
                //StateTransition(EnemyState.MovingToPlayer);
            }
            else return false;

        }
        else return false;
    }

    private void Hit()
    {
        if (!health.isDead)
        {
            Debug.Log("hit");
            punchAudioSource.Play();
            if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance + 0.5f)
            {
                GameManager.Instance.Player.Damage(attackDamage);
            }
        }
        isAttacking = false;
    }

    private void StateTransition(EnemyState newState) 
    {
        enemyState = newState;
    }

    private void SetDestination()
    {

    }
}
