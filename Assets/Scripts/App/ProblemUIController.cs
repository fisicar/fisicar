using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemUIController : MonoBehaviour
{
    public TextMeshProUGUI minValueText;
    public TextMeshProUGUI maxValueText;
    
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
    
    private void OnUpdateModelDetails( bool printX, bool printY, string unit, string velocityUnit)
    {
        
        _printX = printX;
        _printY = printY;
        _unit = unit;
        _velocityUnit = velocityUnit;
    }

    private void OnUnitChange(string unit)
    {
        
        _unit = unit;
    }

    private void OnMinMaxChange(Vector2 min, Vector2 max, Vector2 minVelocity, Vector2 maxVelocity)
    {
        var text = "";
        var format = "F" + 2;
        
        if (_printX)
            text += "V<sub>o</sub>" + (_printX && _printY ? "x" : "") + ": " + minVelocity.x.ToString(format) + _unit +
                    Environment.NewLine;
            
        if (_printY)
        {
            text += "S<sub>o</sub>y: "+ minVelocity.y.ToString(format)+ _unit + Environment.NewLine;
        }
        
        if (_printX)
            text += "S<sub>o</sub>" + (_printX && _printY ? "x" : "") + ": " + min.x.ToString(format) + _unit +
                    Environment.NewLine;
            
        if (_printY)
        {
            text += "S<sub>o</sub>y: "+ min.y.ToString(format)+ _unit + Environment.NewLine;
        }
        
        minValueText.text = text;

        var maxText = "";
        
        if (_printX)
            maxText += "V" + (_printX && _printY ? "x" : "") + ": " + maxVelocity.x.ToString(format) + _unit +
                       Environment.NewLine;
                    
        if (_printY)
        {
            maxText += "Vy: "+ maxVelocity.y.ToString(format)+ _unit + Environment.NewLine;
        }
                
        if (_printX)
            maxText += "S" + (_printX && _printY ? "x" : "") + ": " + max.x.ToString(format) + _unit +
                    Environment.NewLine;
            
        if (_printY)
        {
            maxText += "Sy: "+ max.y.ToString(format)+ _unit + Environment.NewLine;
        }
        
        

        maxValueText.text = maxText;
    }
}
