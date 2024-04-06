using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Since our game design does not allow multiple types of the same power-up
// active at the same time (and thus multiplicating their strength), 
// we use these categories to identify existence of same-type power-ups
public enum PowerUpCategory
{
    AttackBoost,
    SpeedBoost,
    DashCostReduce,
}

[System.Serializable]
public class PowerUpBonus
{
    public void Initialize()
    {
        InitialEffectDuration = EffectDuration;
    }

    public void RefreshFromOther(PowerUpBonus other)
    {
        if(Category != other.Category)
        {
            Debug.LogError("Cannot mix power-up categories!");
        }

        _attackBoostMultiplier = other._attackBoostMultiplier;
        _speedBostMultiplier = other._speedBostMultiplier;
        _dashCostMultiplier = other._dashCostMultiplier;
        _effectDuration = other._effectDuration;
        InitialEffectDuration = other.InitialEffectDuration;
    }

    public PowerUpCategory Category => _category;

    public float AttackBoostMultiplier => _attackBoostMultiplier;

    public float SpeedBoostMultiplier => _speedBostMultiplier;

    public float DashCostMultiplier => _dashCostMultiplier;
    
    public float EffectDuration
    {
        get => _effectDuration;
        set => _effectDuration = value;
    }

    public float InitialEffectDuration { get; private set; }
    
    public Sprite UiImage => _uiImage;

    [SerializeField]
    private PowerUpCategory _category;

    [SerializeField]
    private float _attackBoostMultiplier = 1;

    [SerializeField]
    private float _speedBostMultiplier = 1;

    [SerializeField]
    private float _dashCostMultiplier = 1;

    [SerializeField]
    private float _effectDuration = 10;

    [SerializeField]
    private Sprite _uiImage;
}
