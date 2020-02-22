using System;
using UnityEngine;

namespace Question
{
    [CreateAssetMenu(menuName = "Problems/Simple MUV")]
    public class SimpleMUV : Problem
    {
        public float initialPosition;
        public float finalPosition;
        public float initialVelocity;
        public float acceleration;

        [ContextMenu("Process MUV")]
        public override void Process()
        {
            Degree = 2;
            Coefficients = new float[Degree + 1];
            Coefficients[0] = initialPosition - finalPosition;
            Coefficients[1] = initialVelocity;
            Coefficients[2] = acceleration / 2;
            var delta = Mathf.Pow(Coefficients[1], 2) - 4 * Coefficients[2] * Coefficients[0];
            if (delta < 0) throw new Exception("Error: delta lower than zero");
            Answer = (-Coefficients[1] + Mathf.Sqrt(delta)) / (2 * Coefficients[2]);
            minValue = new Vector2(Mathf.Min(initialPosition, finalPosition), 0);
            maxValue = new Vector2(Mathf.Max(initialPosition, finalPosition), 0);
        }

        public override float Evaluate(float normalizedValue)
        {
            var time = normalizedValue * Answer;
            return Function(time);
        }

        protected float Function(float time)
        {
            return initialPosition + initialVelocity * time + acceleration * time * time / 2;
        }
    }
}
// Certo móvel, inicialmente na velocidade de 3 m/s, acelera constantemente a 2 m/s2 até se distanciar 4 m de sua posição inicial. O intervalo de tempo decorrido até o término desse deslocamento foi de:
// S=So+Vo*t+(at^2)/2
// y=ax^2+bx+c
// x = (-b+-sqrt(b^2-4*a*c))/2a
// time = x, a = acceleration/2, b = initialTime, c = initialPosition - finalPosition, 