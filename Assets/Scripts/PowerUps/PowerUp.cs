using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            _powerUpBonus.Initialize();
            player.AddPowerUp(_powerUpBonus);

            Destroy(gameObject);
        }
    }

    [SerializeField]
    private PowerUpBonus _powerUpBonus;
}
