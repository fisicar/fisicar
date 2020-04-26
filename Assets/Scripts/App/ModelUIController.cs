using System;
using TMPro;
using UnityEngine;

public class ModelUIController : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public int index;
    public int floatPoints = 2;
    
    private bool _printParenthesis = true;
    private bool _printX = true;
    private bool _printY = true;
    private string _unit;

    private void Awake()
    {
         ProblemController.OnUnitChange += OnUnitChange;
         ProblemController.OnModelPositionUpdate += OnModelPositionUpdate;
         ProblemController.UpdateModelDetails += OnUpdateModelDetails;
    }

    private void OnUpdateModelDetails(bool printParenthesis, bool printX, bool printY, string unit)
    {
        _printParenthesis = printParenthesis;
        _printX = printX;
        _printY = printY;
        _unit = unit;
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
                text += "Vx = "+ velocity.x.ToString(format) + Environment.NewLine;
            if (_printY)
            {
                if (_printX)
                    text += ", ";
                text += "Vy = "+ velocity.y.ToString(format) + Environment.NewLine;
            }

            
            
            if (_printParenthesis)
                text += "(";
            if (_printX)
                text += position.x.ToString(format) + _unit;
            if (_printY)
            {
                if (_printX)
                    text += ", ";
                text += position.y.ToString(format) + _unit;
            }

            if (_printParenthesis)
                text += ")";
    	        
            positionText.text = text;
        }
    }
}