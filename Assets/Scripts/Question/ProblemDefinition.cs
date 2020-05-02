using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProblemDefinition
{
    public bool isActive;
    public string title;

    [Multiline]
    public string longDescription;
    public string unit;
    public string velocityUnit;
    
    public bool printX = true;
    public bool printY = true;
    
    public Problem problem;
    public Sprite sprite;
    public GameObject[] models;
    public GameObject environment;
}

// Tanaka comment