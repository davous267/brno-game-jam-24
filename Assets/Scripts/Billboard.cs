using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if(_mainCamera == null)
        {
            return;
        }

        Vector3 newRotation = _mainCamera.transform.eulerAngles;

        newRotation.x = 0;
        newRotation.z = 0;

        transform.eulerAngles = newRotation;
    }

    private Camera _mainCamera;
}
