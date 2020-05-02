using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemUIController : MonoBehaviour
{
    public TextMeshProUGUI minValueText;
    public TextMeshProUGUI maxValueText;
    
    private bool _printParenthesis = true;
    private bool _printX = true;
    private bool _printY = true;
    private string _unit;
    private string _velocityUnit;

    private void Awake()
    {
        ProblemController.OnMinMaxValueChange += OnMinMaxChange;
        ProblemController.OnUnitChange += OnUnitChange;
        ProblemController.UpdateModelDetails += OnUpdateModelDetails;
    }
    
    private void OnUpdateModelDetails(bool printParenthesis, bool printX, bool printY, string unit, string velocityUnit)
    {
        _printParenthesis = printParenthesis;
        _printX = printX;
        _printY = printY;
        _unit = unit;
        _velocityUnit = velocityUnit;
    }

    private void OnUnitChange(string unit)
    {
        
        _unit = unit;
    }

    private void OnMinMaxChange(Vector2 min, Vector2 max)
    {
        var text = "";
        var format = "F" + 2;
        if (_printX)
            text += "S<sub>0</sub>" + (_printX && _printY ? "x" : "") + ": " + min.x.ToString(format) + _unit +
                    Environment.NewLine;
            
        if (_printY)
        {
            text += "S<sub>0</sub>y: "+ min.y.ToString(format)+ _unit + Environment.NewLine;
        }
        
        minValueText.text = text;

        var maxText = "";
        if (_printX)
            maxText += "S<sub>0</sub>" + (_printX && _printY ? "x" : "") + ": " + max.x.ToString(format) + _unit +
                    Environment.NewLine;
            
        if (_printY)
        {
            maxText += "S<sub>0</sub>y: "+ max.y.ToString(format)+ _unit + Environment.NewLine;
        }

        maxValueText.text = maxText;
    }
}
