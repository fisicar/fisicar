using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

 [CreateAssetMenu(menuName = "Problems/Simple")]    
public class SimpleMU : Problem
{
    public float initialPosition;
    public float finalPosition;
    public float time;
    public float velocity;

    [ContextMenu("Process")]
    
    public override void Process()
    {
        degree = 1;
        coefficients = new float[degree + 1];
        coefficients[0] = initialPosition;
        answer = time;
        
        coefficients[1] = Evaluate(time);
    }

    public override float Evaluate(float normalizedValue)
    {
        return coefficients[0] / answer; // Velocity
    }
}


/*
3 (SimpleMU). Uma bola se move em movimento
 retilíneo uniforme em direção a uma parede
  que está a 5 metros de distância, qual a 
  velocidade da bola para que ela chegue na
   parede em 2 segundos?
*/