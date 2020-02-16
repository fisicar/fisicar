using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProblemDefinition 
{
    public string title;
    
    [Multiline]
    public string longDescription;

    public Problem problem;
    public Sprite sprite;
    public GameObject[] models;
}

// Tanaka comment