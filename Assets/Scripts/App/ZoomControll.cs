using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomControll : MonoBehaviour
{
    public float minRealSize = 0.2f;
    public float maxRealSize = 0.3f;
    
    void Start()
    {
        UIController.OnScaleSlideValueChange += ScalingPlacement;
    }

    private void ScalingPlacement(float normalizedValue)
    {
        var finalScale = Mathf.Lerp(minRealSize, maxRealSize, normalizedValue);
        transform.localScale = Vector3.one * finalScale;
    }
}
