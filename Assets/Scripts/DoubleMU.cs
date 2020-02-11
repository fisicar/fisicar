using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Problems/Double MU")]    
public class DoubleMU : SimpleMU
{
    public float initialPositionB;
    public float finalPositionB;
    public float timeB;
    public float velocityB;

    public override void Process()
    {
        degree = 1;
        coefficients = new float[degree + 1];
        coefficients[0] = initialPosition - initialPositionB;
        coefficients[1] = velocity - velocityB;

        answer = -coefficients[0] / coefficients[1];
    }

    public override float Evaluate(float normalizedValue)
    {
        var time = normalizedValue * answer;
        return initialPosition + velocity * time;
    }

    public float EvaluateB(float normalizedValueB)
    {
        var time = normalizedValueB * answer;
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