using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        Energy = _maxEnergy;
    }

    private void Update()
    {
        UpdateSpeed();
    }

    public void PerformDash()
    {
        IsDashInProgress = true;
        Speed = _dashSpeed;
    }

    public float Energy { get; private set; }

    public float Speed
    {
        get => _firstPersonMovement.speed;
        private set => _firstPersonMovement.speed = value;
    }

    public bool IsDashInProgress
    {
        get => _firstPersonMovement.IsDashingForward;
        private set => _firstPersonMovement.IsDashingForward = value;
    }

    private void UpdateSpeed()
    {
        var energyBasedSpeed = GetSpeedForCurrentEnergy();

        if (IsDashInProgress)
        {
            Speed = Mathf.Max(energyBasedSpeed, Speed - _dashDeceleration * Time.deltaTime);
            
            if(Speed <= energyBasedSpeed)
            {
                IsDashInProgress = false;
            }
        }
        else
        {
            Speed = energyBasedSpeed;
        }
    }

    private float GetSpeedForCurrentEnergy() => Mathf.Lerp(_minSpeed, _maxSpeed, Energy / _maxEnergy);

    [SerializeField]
    private float _maxEnergy = 100;

    [SerializeField]
    private float _maxSpeed = 5;

    [SerializeField]
    private float _minSpeed = 1;

    [SerializeField]
    private float _dashSpeed = 15;

    [SerializeField]
    private float _dashDeceleration = 30;

    [SerializeField]
    private FirstPersonMovement _firstPersonMovement;
}
