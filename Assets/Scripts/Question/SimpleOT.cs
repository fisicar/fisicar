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
        minValue = new Vector2(_movementX.minValue.x, _movementY.minValue.x);
        maxValue = new Vector2(_movementX.maxValue.x, _movementY.maxValue.x);
    }
    public override float Evaluate(float normalizedValue)
    {
        return _movementX.Evaluate(normalizedValue);
    }
    
    public float EvaluateY(float normalizedValue)
    {
        return _movementY.Evaluate(normalizedValue);
    }
}