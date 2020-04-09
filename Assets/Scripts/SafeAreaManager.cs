using System;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaManager : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Rect _lastSafeArea;

    private void Awake()
    {
        _rectTransform = transform as RectTransform;
        if (_rectTransform == null)
            throw new NullReferenceException();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        var safeArea = GetSafeArea();

        if (safeArea != _lastSafeArea)
            ApplySafeArea(safeArea);
    }

    private static Rect GetSafeArea()
    {
        return Screen.safeArea;
    }

    private void ApplySafeArea(Rect r)
    {
        _lastSafeArea = r;

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = r.position;
        Vector2 anchorMax = r.position + r.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}