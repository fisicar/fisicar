using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : ScriptableObject
{
    public int degree;
    public float[] coefficients;
    public float[] roots;

    public Vector2 minValue;
    public Vector2 maxValue;

    public virtual void Process()
    {
        Debug.Log("call 2");
    }

    public virtual float Evaluate(float i)
    {
        return float.NaN;
        
    }
}
