using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARScript : MonoBehaviour
{
    public GameObject placementIndicator;
    public ARRaycastManager raycastManager;

    
    private Camera _mainCamera;
    private Vector3 _screenCenter;
    private bool _poseIsValid;
    private Pose _pose;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void Awake()
    {
        if (Camera.main != null)
        {
            _mainCamera = Camera.main;
            _screenCenter = _mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        }
    }
    
    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }
    
    private void UpdatePlacementPose()
    {
        raycastManager.Raycast(_screenCenter, _hits, TrackableType.Planes);
        _poseIsValid = _hits.Count > 0;
        
        if (_poseIsValid)
        {
            _pose = _hits[0].pose;
            var cameraForward = _mainCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            _pose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (_poseIsValid)
        {
            placementIndicator.transform.SetPositionAndRotation(_pose.position, _pose.rotation);
        }
    }


}
