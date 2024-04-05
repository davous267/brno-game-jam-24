using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    Vector3 targetPosition = Vector3.zero;

    [SerializeField] Transform dungeonExit;

    NavMeshAgent navMeshAgent;


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
    }

    void Update()
    {
        Behave();
    }

    private void Behave()
    {
        switch (enemyState)
        {
            case EnemyState.Standing:
                break;

            case EnemyState.Wandering: 
                if (Vector3.Distance(transform.position, targetPosition) <= targetPositionTolerance || targetPosition == Vector3.zero)
                {
                    targetPosition = ChooseRandomPosition();
                    navMeshAgent.destination = targetPosition;
                }


                break;

            case EnemyState.Fleeting:
                navMeshAgent.destination = dungeonExit.position;
                break;

            case EnemyState.MovingToPlayer:
                break;

            case EnemyState.AttackingPlayer:
                break;

            case EnemyState.ResetWandering:
                break;  

            default:
                break;
        }
    }

    private Vector3 ChooseRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderingRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderingRadius, 1);
        Vector3 finalPosition = hit.position;

        return finalPosition;
    }    

    private void StateTransition(EnemyState currentState, EnemyState newState) 
    {

    }

    private void SetDestination()
    {

    }
}
