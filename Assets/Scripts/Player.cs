using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        Energy = _maxEnergy;
        _energySlider.maxValue = _maxEnergy;
    }

    private void Update()
    {
        _firstPersonLook.SetFreezeState(GameManager.Instance.IsGamePaused);

        Debug.DrawRay(transform.position, transform.forward * _attackDistance, Color.blue);

        UpdateEnergy();
        UpdateSpeed();
        UpdatePowerUps();
        UpdateDashIcon();

        if (Input.GetKeyDown(_dashKey))
        {
            PerformDash();
        }

        if (Input.GetKeyDown(_attackButton))
        {
            TryToAttack();
        }
    }

    public void PerformDash()
    {
        if (DashAvailable)
        {
            IsDashInProgress = true;
            _lastDashTime = Time.time;

            Energy -= DashEnergyCost;
            Speed = _dashSpeed;
        }
    }

    public void Damage(float amount)
    {
        _getHitAudioSource.Play();
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

    public void AddPowerUp(PowerUpBonus powerUp)
    {
        var sameCategoryPowerUpIndex = _powerUps.FindIndex(x => x.Category == powerUp.Category);

        _pickupAudioSource.Play();

        if (sameCategoryPowerUpIndex >= 0)
        {
            _powerUps[sameCategoryPowerUpIndex].RefreshFromOther(powerUp);
            return;
        }

        _powerUps.Add(powerUp);
        _powerUpUiManager.UpdateActivePowerUps(PowerUps);
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

    public List<PowerUpBonus> PowerUps => _powerUps;

    private void TryToAttack()
    {
        var currentTime = Time.time;
        if (currentTime - _lastAttackTime > _attackDelaySec)
        {
            Debug.Log("Player tries to attack with strength " + AttackStrength);
            _lastAttackTime = currentTime;
            attackAnimation.SetTrigger("Attack");
            _attackAudioSource.Play();
        }
    }

    public void DealDamage()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, _attackDistance))
        {
            Debug.Log("Player attack hit: " + hit.collider.name);

            var aiHealth = hit.collider.GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                Energy += aiHealth.TakeDamage(AttackStrength);
            }
        }
    }

    private void UpdateSpeed()
    {
        var energyBasedSpeed = GetSpeedForCurrentEnergy();

        if (IsDashInProgress)
        {
            Speed = Mathf.Max(energyBasedSpeed, Speed - _dashDeceleration * Time.deltaTime);

            if (Speed <= energyBasedSpeed)
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

    private void UpdatePowerUps()
    {
        for (int i = PowerUps.Count - 1; i >= 0; --i)
        {
            PowerUps[i].EffectDuration -= Time.deltaTime;

            if (PowerUps[i].EffectDuration <= 0)
            {
                RemovePowerUp(i);
            }
        }
    }

    private float GetSpeedForCurrentEnergy()
    {
        var speed = Mathf.Lerp(_minSpeed, _maxSpeed, Energy / _maxEnergy);

        foreach (var powerUp in PowerUps)
        {
            speed *= powerUp.SpeedBoostMultiplier;
        }

        return speed;
    }

    private void RemovePowerUp(int index)
    {
        PowerUps.RemoveAt(index);
        _powerUpUiManager.UpdateActivePowerUps(PowerUps);
    }

    private void UpdateDashIcon() => _dashUiIcon.SetActive(DashAvailable);

    private float DashEnergyCost
    {
        get
        {
            var dashCost = _dashEnergyCost;

            foreach (var powerUp in PowerUps)
            {
                dashCost *= powerUp.DashCostMultiplier;
            }

            return dashCost;
        }
    }

    private bool DashAvailable => DashEnergyCost <= Energy && Time.time - _lastDashTime > _dashDelaySec;

    private float AttackStrength
    {
        get
        {
            var attack = _attackStrength;

            foreach (var powerUp in PowerUps)
            {
                attack *= powerUp.AttackBoostMultiplier;
            }

            return attack;
        }
    }

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
    private GameObject _dashUiIcon;

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
    private FirstPersonLook _firstPersonLook;

    [SerializeField]
    private KeyCode _attackButton = KeyCode.Mouse0;

    [SerializeField]
    private float _attackStrength = 10;

    [SerializeField]
    private float _attackDistance = 1;

    [SerializeField]
    private float _attackDelaySec = 1;

    [SerializeField]
    private PowerUpUiManager _powerUpUiManager;

    [SerializeField]
    private Animator attackAnimation;

    [SerializeField]
    private AudioSource _attackAudioSource;

    [SerializeField]
    private AudioSource _pickupAudioSource;

    [SerializeField]
    private AudioSource _getHitAudioSource;

    private List<PowerUpBonus> _powerUps = new();

    private float _energy;
    private float _lastDashTime = float.MinValue;
    private float _lastAttackTime = float.MinValue;
}
