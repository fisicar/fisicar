using System;
using UnityEngine;

namespace Question

{
    [CreateAssetMenu(menuName = "Problems/Simple Second MUV")]
    public class SimpleSecondMUV : SimpleMUV
    {
        public override void Process()
        {
            Degree = 2;
            Coefficients = new Coefficient[Degree + 1];
            for (int i = 0; i < Coefficients.Length; i++) 
                Coefficients[i] = new Coefficient();
            Coefficients[0].value = initialPosition - finalPosition;
            Coefficients[1].value = initialVelocity;
            Coefficients[2].value = acceleration / 2;
            var delta = Mathf.Pow(Coefficients[1].value, 2) - 4 * Coefficients[2].value * Coefficients[0].value;
            if (delta < 0)
                throw new Exception("Error: delta lower than zero");
            Answer = (-Coefficients[1].value - Mathf.Sqrt(delta)) / (2 * Coefficients[2].value);
            var halfTime = Coefficients[1].value / -Coefficients[2].value / 2;
            equation = "S = S<sub>o</sub> + V<sub>o</sub>t + (at²)/2" + Environment.NewLine + finalPosition + " = " + initialPosition + " + " + initialVelocity + "t + (" + acceleration +
                       "t²)/2";
            minValue = new Vector2(Mathf.Min(initialPosition, finalPosition, Function(halfTime)), 0);
            maxValue = new Vector2(Mathf.Max(initialPosition, finalPosition, Function(halfTime)), 0);
        }

        public override float Velocity(float normalizedValue)
        {
            return initialVelocity + (acceleration * (Answer * normalizedValue));
        }
    }
}
