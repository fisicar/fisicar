using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARController : MonoBehaviour
{
    public GameObject placementIndicator;
    public ARRaycastManager raycastManager;
    public bool poseIsValid;
    public bool isPositioning { get; set; } = true;
    
    private Camera _mainCamera;
    private Vector3 _screenCenter;
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
        if (!isPositioning)
            return;
        
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }
    
    private void UpdatePlacementPose()
    {
        raycastManager.Raycast(_screenCenter, _hits, TrackableType.Planes);
        poseIsValid = _hits.Count > 0;
        
        if (poseIsValid)
        {
            _pose = _hits[0].pose;
            var cameraForward = _mainCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            _pose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
#if !UNITY_EDITOR
        placementIndicator.gameObject.SetActive(poseIsValid);
#endif
        if (poseIsValid)
        {
            placementIndicator.transform.SetPositionAndRotation(_pose.position, _pose.rotation);
        }
    }
}
