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
            Coefficients = new float[Degree + 1];
            Coefficients[0] = initialPosition - finalPosition;
            Coefficients[1] = initialVelocity;
            Coefficients[2] = acceleration / 2;
            var delta = Mathf.Pow(Coefficients[1], 2) - 4 * Coefficients[2] * Coefficients[0];
            if (delta < 0)
                throw new Exception("Error: delta lower than zero");
            Answer = (-Coefficients[1] - Mathf.Sqrt(delta)) / (2 * Coefficients[2]);
            var halfTime = Coefficients[1] / -Coefficients[2] / 2;
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
