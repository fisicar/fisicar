using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemUIController : MonoBehaviour
{
    public TextMeshProUGUI minValueText;
    public TextMeshProUGUI maxValueText;

    private void Awake()
    {
        ProblemController.OnMinMaxValueChange += OnMinMaxChange;
    }

    private void OnMinMaxChange(Vector2 min, Vector2 max)
    {
        minValueText.text = min.ToString();
        maxValueText.text = max.ToString();
    }
}
