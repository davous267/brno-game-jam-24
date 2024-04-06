using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PowerUpBonus
{
    public float AttackBoostMultiplier => _attackBoostMultiplier;

    public float SpeedBoostMultiplier => _speedBostMultiplier;

    public float DashCostMultiplier => _dashCostMultiplier;
    
    public float EffectDuration
    {
        get => _effectDuration;
        set => _effectDuration = value;
    }

    public Image UiImage => _uiImage;

    [SerializeField]
    private float _attackBoostMultiplier = 1;

    [SerializeField]
    private float _speedBostMultiplier = 1;

    [SerializeField]
    private float _dashCostMultiplier = 1;

    [SerializeField]
    private float _effectDuration = 10;

    [SerializeField]
    private Image _uiImage;
}
