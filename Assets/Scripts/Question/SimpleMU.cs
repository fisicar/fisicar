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
        Degree = 1;
        Coefficients = new float[Degree + 1];
        Coefficients[0] = initialPosition;
        Answer = time;
        
        Coefficients[1] = Evaluate(time);
    }

    public override float Evaluate(float normalizedValue)
    {
        return Coefficients[0] / Answer; // Velocity
    }
}

/* seltz was here*/
/*
3 (SimpleMU). Uma bola se move em movimento
 retilíneo uniforme em direção a uma parede
  que está a 5 metros de distância, qual a 
  velocidade da bola para que ela chegue na
   parede em 2 segundos?
*/