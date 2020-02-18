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

    private void Awake()
    {
        ProblemController.OnModelPositionUpdate += OnModelPositionUpdate;
    }

    private void OnModelPositionUpdate(int index, Vector2 position)
    {
        if (this.index == index)
        {
            var text = "";
            var format = "F" + floatPoints;
            if (printParenthesis)
                text = "(";
            if (printX)
                text += position.x.ToString(format);
            if (printY)
            {
                if (printX)
                    text += ", ";
                text += position.y.ToString(format);
            }

            if (printParenthesis)
                text += ")";

            positionText.text = text;
        }
    }
}