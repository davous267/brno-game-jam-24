using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCaller : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void Hit()
    {
        if (player != null)
        {
            player.DealDamage();
        }
    }
}
