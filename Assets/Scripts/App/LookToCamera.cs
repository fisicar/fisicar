using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    private static Camera _mainCamera;

    private void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
        
    }

    void Update()
    {
        
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
        
    }
}
