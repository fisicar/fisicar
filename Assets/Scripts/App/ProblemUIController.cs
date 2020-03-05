using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemUIController : MonoBehaviour
{
    public TextMeshProUGUI minValueText;
    public TextMeshProUGUI maxValueText;
    private string _unit;

    private void Awake()
    {
        ProblemController.OnMinMaxValueChange += OnMinMaxChange;
        ProblemController.OnUnitChange += OnUnitChange;
    }

    private void OnUnitChange(string unit)
    {
        
        _unit = unit;
    }

    private void OnMinMaxChange(Vector2 min, Vector2 max)
    {
        minValueText.text = min.ToString() + _unit;
        maxValueText.text = max.ToString() + _unit;
        
    }
}
