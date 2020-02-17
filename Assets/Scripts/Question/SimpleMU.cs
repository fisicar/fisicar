using UnityEngine;

[CreateAssetMenu(menuName = "Problems/Simple")]    
public class SimpleMU : Problem
{
    public float initialPosition;
    public float finalPosition;
    public float velocity;

    [ContextMenu("Process")]
    
    public override void Process()
    {
        Degree = 1;
        Coefficients = new float[Degree + 1];
        Coefficients[0] = initialPosition;
        Answer = (finalPosition - initialPosition) / velocity;
        Coefficients[1] = velocity;
        minValue = new Vector2(Mathf.Min(Evaluate(0), Evaluate(1)), 0);
        maxValue = new Vector2(Mathf.Max(Evaluate(0), Evaluate(1)), 0);
    }

    public override float Evaluate(float normalizedValue)
    {
        var time = normalizedValue * Answer;
        return initialPosition + velocity * time;
    }
}

/* seltz was here*/
/*
3 (SimpleMU). Uma bola se move em movimento
 retilíneo uniforme em direção a uma parede
  que está a 5 metros de distância, qual a 
  velocidade da bola para que ela chegue na
   parede em 2 segundos?
*/