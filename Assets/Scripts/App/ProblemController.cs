using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemController : MonoBehaviour
{
    private Problem _currentProblem;
    private GameObject[] _instantiatedModels;

    public GameObject plane;
    private void Awake()
    {
        UIController.OnProblemSelected += OnProblemSelected;
    }

    private void OnProblemSelected(ProblemDefinition obj)
    {
        _currentProblem = obj.problem;
        _instantiatedModels = new GameObject[obj.models.Length];

        for (var i = 0; i < obj.models.Length; i++)
        {
            var model = obj.models[i];
            
            if (model != null)
            {
                _instantiatedModels[i] = Instantiate(model, plane.transform);
            }
        }
    }
}
