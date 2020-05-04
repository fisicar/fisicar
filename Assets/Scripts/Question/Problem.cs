using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : ScriptableObject
{
    protected int Degree;
    protected float[] Coefficients;
    public string equation;
    public float Answer;

    public Vector2 minValue;
    public Vector2 maxValue;
    public Vector2 minVelocity;
    public Vector2 maxVelocity;

    public virtual void Process()
    {
        Debug.LogError("error");
    }

    public virtual float Evaluate(float normalizedValue)
    {
        return float.NaN;
    }

    public virtual float Velocity(float normalizedValue)
    {
        return float.NaN;
    }
    
}