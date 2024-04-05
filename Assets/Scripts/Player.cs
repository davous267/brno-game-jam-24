using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Energy => _energy;

    [SerializeField]
    private float _energy = 100;

    [SerializeField]
    private FirstPersonMovement _firstPersonMovement;
}
