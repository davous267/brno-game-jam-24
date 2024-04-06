using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUiManager : MonoBehaviour
{
    private void Update()
    {
        foreach(var powerUp in _activePowerUps)
        {
            powerUp.Value.fillAmount = GetFillAmount(powerUp.Key);
        }
    }

    public void UpdateActivePowerUps(List<PowerUpBonus> powerUps)
    {
        ClearPowerUpIcons();
        CreatePowerUpIcons(powerUps);
    }

    private void ClearPowerUpIcons()
    {
        _activePowerUps.Clear();
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void CreatePowerUpIcons(List<PowerUpBonus> powerUps)
    {
        foreach(var powerUp in powerUps)
        {
            var newIconObject = Instantiate(_powerUpIconPrefab, transform);
            var image = newIconObject.GetComponent<Image>();
            image.sprite = powerUp.UiImage;
            image.fillAmount = GetFillAmount(powerUp);

            _activePowerUps.Add(powerUp, image);
        }
    }

    private float GetFillAmount(PowerUpBonus powerUpBonus)
    {
        return powerUpBonus.EffectDuration / powerUpBonus.InitialEffectDuration;
    }

    [SerializeField]
    private GameObject _powerUpIconPrefab;

    private Dictionary<PowerUpBonus, Image> _activePowerUps = new();
}
