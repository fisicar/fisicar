using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : ScriptableObject
{
    public int degree;
    public float[] coefficients;
    public float answer;

    public Vector2 minValue;
    public Vector2 maxValue;

    public virtual void Process()
    {
        throw new NotImplementedException();
    }

    public virtual float Evaluate(float normalizedValue)
    {
        return float.NaN;
    }
}
