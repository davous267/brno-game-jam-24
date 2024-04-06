using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] float risingRatePerMinute = 1.0f;
    private float risingRate;
    private float currentYPosiiton = 0;

    private void Start()
    {
        risingRate = risingRatePerMinute / 60;
        currentYPosiiton = transform.position.y;
    }

    private void Update()
    {
        currentYPosiiton += risingRate * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, currentYPosiiton, transform.position.z);
    }
}
