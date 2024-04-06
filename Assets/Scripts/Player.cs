using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        Energy = _maxEnergy;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * _attackDistance, Color.blue);

        UpdateEnergy();
        UpdateSpeed();

        if(Input.GetKeyDown(_dashKey))
        {
            PerformDash();
        }

        if(Input.GetKeyDown(_attackButton))
        {
            TryToAttack();
        }
    }

    public void PerformDash()
    {
        if(Energy < _dashEnergyCost)
        {
            return;
        }

        var currentTime = Time.time;
        if (currentTime - _lastDashTime > _dashDelaySec)
        {
            IsDashInProgress = true;
            _lastDashTime = currentTime;

            Energy -= _dashEnergyCost;
            Speed = _dashSpeed;
        }
    }

    public void Damage(float amount)
    {
        if (amount >= Energy)
        {
            if (Energy < Mathf.Epsilon)
            {
                GameManager.Instance.GameOver();
                return;
            }
            else
            {
                Energy = 0;
            }
        }
        else
        {
            Energy -= amount;
        }
    }

    public float Energy
    {
        get => _energy;
        set
        {
            _energy = Mathf.Clamp(value, 0f, _maxEnergy);
            _energySlider.value = _energy;
        }
    }

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

    private void TryToAttack()
    {
        var currentTime = Time.time;
        if (currentTime - _lastAttackTime > _attackDelaySec)
        {
            Debug.Log("Player attack");
            _lastAttackTime = currentTime;
            RaycastHit hit;
            
            if(Physics.SphereCast(transform.position, _attackRaycastRadius, transform.forward, out hit, _attackDistance))
            {
                Debug.Log("Player attack hit: " + hit.collider.name);

                var aiHealth = hit.collider.GetComponent<AIHealth>();
                if(aiHealth != null)
                {
                    Energy += aiHealth.TakeDamage(_attackStrength);
                }
            }
        }
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

    private void UpdateEnergy()
    {
        Energy -= _energyDrain * Time.deltaTime;
    }

    private float GetSpeedForCurrentEnergy() => Mathf.Lerp(_minSpeed, _maxSpeed, Energy / _maxEnergy);

    [SerializeField]
    private float _maxEnergy = 100;

    [SerializeField]
    private float _energyDrain = 5;

    [SerializeField]
    private Slider _energySlider;

    [SerializeField]
    private float _maxSpeed = 5;

    [SerializeField]
    private float _minSpeed = 1;

    [SerializeField]
    private KeyCode _dashKey = KeyCode.LeftShift;

    [SerializeField]
    private float _dashSpeed = 15;

    [SerializeField]
    private float _dashDeceleration = 30;

    [SerializeField]
    private float _dashEnergyCost = 30;

    [SerializeField]
    private float _dashDelaySec = 3;

    [SerializeField]
    private FirstPersonMovement _firstPersonMovement;

    [SerializeField]
    private KeyCode _attackButton = KeyCode.Mouse0;

    [SerializeField]
    private float _attackStrength = 10;

    [SerializeField]
    private float _attackDistance = 1;

    [SerializeField]
    private float _attackRaycastRadius = 1;

    [SerializeField]
    private float _attackDelaySec = 1;

    private float _energy;
    private float _lastDashTime = float.MinValue;
    private float _lastAttackTime = float.MinValue;
}
