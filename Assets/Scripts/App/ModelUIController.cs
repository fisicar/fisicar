using TMPro;
using UnityEngine;

public class ModelUIController : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public int index;
    public bool printParenthesis = true;
    public bool printX = true;
    public bool printY = true;
    public int floatPoints = 2;
    public string _unit;

    private void Awake()
    {
         ProblemController.OnUnitChange += OnUnitChange;
         ProblemController.OnModelPositionUpdate += OnModelPositionUpdate;
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
            if (printParenthesis)
                text = "(";
            Debug.Log(_unit);
            if (printX)
                text += position.x.ToString(format) + _unit;
            if (printY)
            {
                if (printX)
                    text += ", ";
                text += position.y.ToString(format) + _unit;
            }

            if (printParenthesis)
                text += ")";
    	        
            positionText.text = text;
        }
    }
}