﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Problems/Double MU")]    
public class DoubleMU : SimpleMU
{
    public float initialPositionB;
    public float finalPositionB;
    public float timeB;
    public float velocityB;

    [ContextMenu("Process Double MU")]
    public override void Process()
    {
        Degree = 1;
        Coefficients = new float[Degree + 1];
        Coefficients[0] = initialPosition - initialPositionB;
        Coefficients[1] = velocity - velocityB;

        Answer = -Coefficients[0] / Coefficients[1];
    }

    public override float Evaluate(float normalizedValue)
    {
        var time = normalizedValue * Answer;
        return initialPosition + velocity * time;
    }

    public float EvaluateB(float normalizedValueB)
    {
        var time = normalizedValueB * Answer;
        return initialPositionB + velocityB * time;
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