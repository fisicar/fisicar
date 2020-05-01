﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Problems/Double MU")]    
public class DoubleMU : SimpleMU
{
    public float initialPositionB;
    public float finalPositionB;
    public float velocityB;

    [ContextMenu("Process Double MU")]
    public override void Process()
    {
        Degree = 1;
        Coefficients = new Coefficient[Degree + 1];
        for (int i = 0; i < Coefficients.Length; i++) 
            Coefficients[i] = new Coefficient();
        Coefficients[0].value = initialPosition - initialPositionB;
        Coefficients[1].value = velocity - velocityB;

        Answer = -Coefficients[0].value / Coefficients[1].value;
        equation = "S<sub>A</sub> = S<sub>oA</sub> + V<sub>A</sub>t" + Environment.NewLine + finalPosition + " = " + initialPosition + " + " + velocity + "t" + Environment.NewLine + "S<sub>B</sub> = S<sub>oB</sub> + V<sub>B</sub>t" + Environment.NewLine + finalPositionB + " = " + initialPositionB + " + " + velocityB + "t";
        minValue = new Vector2(Mathf.Min(Evaluate(0), Evaluate(1), EvaluateB(0), EvaluateB(1)), 0);
        maxValue = new Vector2(Mathf.Max(Evaluate(0), Evaluate(1), EvaluateB(0), EvaluateB(1)), 0);
    }

    public override float Evaluate(float normalizedValue)
    {
        var time = normalizedValue * Answer;
        return initialPosition + velocity * time;
    }

    public override float Velocity(float normalizedValue)
    {
        return velocity;
    }

    public float EvaluateB(float normalizedValueB)
    {
        var time = normalizedValueB * Answer;
        return initialPositionB + velocityB * time;
    }

    public float VelocityB(float normalizedValueB)
    {
        return velocityB;
    }
}

/*
 * 5 (DoubleMU). Dois mísseis A e B,
 * um na posição 0m, outro na posição
 * 120m, A se move com uma velocidade
 * de 20 m/s, em direção ao míssil B,
 * e B se move com uma velocidade de
 * 40 m/s em direção ao míssil A, a
 * que ponto eles irão colidir?
 *
 * S = S0 + v * t
 * A : SA = Sa + Va * t
 * B : SB = Sb + Vb * t
 *
 * SA = SB
 *
 * Sa + Va * t = Sb + Vb * t
 * Sa - Sb + Va * t - Vb * t = 0
 * (Sa - Sb) + (Va - Vb) * t = 0
 *    ||          ||
 * (Coef[0]) + (Coef[1]) * t = 0
 * t = - Coef[0] / Coef[1]
*/
//panda was here