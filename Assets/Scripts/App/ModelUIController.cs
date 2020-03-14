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
         ProblemController.UpdateModelDetails += OnProblemSelectedUpdateBools;
    }

    private void OnProblemSelectedUpdateBools(bool arg1, bool arg2, bool arg3, string arg4)
    {
        _printParenthesis = arg1;
        _printX = arg2;
        _printY = arg3;
        _unit = arg4;
    }

    private void OnUnitChange(string unit)
    {
        Debug.Log(unit+"b");
        _unit = unit;
    }


    private void OnModelPositionUpdate(int index, Vector2 position)
    {
        if (this.index == index)
        {
            var text = "";
            var format = "F" + floatPoints;
            if (_printParenthesis)
                text = "(";
            Debug.Log(_unit);
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