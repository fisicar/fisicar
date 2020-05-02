using System;
using TMPro;
using UnityEngine;

public class ModelUIController : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public int index;
    public int floatPoints = 2;
    
    private bool _printX = true;
    private bool _printY = true;
    private string _unit;
    private string _velocityUnit;

    private void Awake()
    {
         ProblemController.OnUnitChange += OnUnitChange;
         ProblemController.OnModelPositionUpdate += OnModelPositionUpdate;
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


    private void OnModelPositionUpdate(int index, Vector2 position, Vector2 velocity)
    {
        if (this.index == index)
        {
            var text = "";
            var format = "F" + floatPoints;


            if (_printX)
                text += "V" + (_printX && _printY ? "x" : "") + ": " + velocity.x.ToString(format) + _velocityUnit +
                        Environment.NewLine;
            if (_printY)
            {
                text += "Vy: "+ velocity.y.ToString(format)+ _velocityUnit + Environment.NewLine;
            }
            
            if (_printX)
                text += "S" + (_printX && _printY ? "x" : "") + ": " + position.x.ToString(format) + _unit +
                        Environment.NewLine;
            
            if (_printY)
            {
                text += "Sy: "+ position.y.ToString(format)+ _unit + Environment.NewLine;
            }
            
    	        
            positionText.text = text;
        }
    }
}