using TMPro;
using UnityEngine;

public class ModelUIController : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public int index;

    private void Awake()
    {
        ProblemController.OnModelPositionUpdate+= OnModelPositionUpdate;
    }

    private void OnModelPositionUpdate(int index, Vector2 Position)
    {
        if (this.index == index)
        {
            positionText.text = Position.ToString();
        }
    }
}
