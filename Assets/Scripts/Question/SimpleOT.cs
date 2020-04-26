using System;
using Question;
using UnityEngine;

[CreateAssetMenu(menuName = "Problems/Simple OT")]
public class SimpleOT : Problem
{
    public Vector2 initialPosition;
    public float finalHeight;
    public float throwAngle;
    public float initialVelocity;
    public float gravity;
    private SimpleMU _movementX;
    private SimpleSecondMUV _movementY;

    [ContextMenu("Process")]
    
    public override void Process()
    {
        _movementX = ScriptableObject.CreateInstance<SimpleMU>();
        _movementY = ScriptableObject.CreateInstance<SimpleSecondMUV>();
        _movementY.acceleration = gravity;
        _movementY.initialPosition = initialPosition.y;
        _movementY.initialVelocity = initialVelocity * Mathf.Sin(throwAngle * Mathf.Deg2Rad);
        _movementY.finalPosition = finalHeight;
        _movementX.initialPosition = initialPosition.x;
        _movementX.velocity = initialVelocity * Mathf.Cos(throwAngle * Mathf.Deg2Rad);
        _movementY.Process();
        _movementX.finalPosition = _movementX.initialPosition + _movementX.velocity * _movementY.Answer;
        _movementX.Process();
        Answer = _movementY.Answer;
        equation = "S<sub>X</sub> = S<sub>oX</sub> + V<sub>oX</sub>t" + Environment.NewLine + _movementX.finalPosition.ToString("0.0") + " = " + _movementX.initialPosition + " + " + _movementX.velocity.ToString("0.0") +
                   "t" + Environment.NewLine + "S<sub>Y</sub> = S<sub>oY</sub> + V<sub>oY</sub>t + (a<sub>Y</sub>t²)/2" + Environment.NewLine + _movementY.finalPosition + " = " + _movementY.initialPosition +
                   " + " + _movementY.initialVelocity.ToString("0.0") + "t + (" + _movementY.acceleration + "t²)/2";
        minValue = new Vector2(_movementX.minValue.x, _movementY.minValue.x);
        maxValue = new Vector2(_movementX.maxValue.x, _movementY.maxValue.x);
    }
    public override float Evaluate(float normalizedValue)
    {
        return _movementX.Evaluate(normalizedValue);
    }

    public override float Velocity(float normalizedValue)
    {
        return _movementX.Velocity(normalizedValue);
    }

    public float EvaluateY(float normalizedValue)
    {
        return _movementY.Evaluate(normalizedValue);
    }

    public float VelocityY(float normalizedValue)
    {
        return _movementY.Velocity(normalizedValue);
    }
}