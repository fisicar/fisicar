using System;
using System.Collections.Generic;
using System.Globalization;
using Question;
using UnityEngine;

public class ExplanationInputController : MonoBehaviour
{
    public InputLine inputLinePrefab;
    private readonly List<InputData> _inputList = new List<InputData>();
    [SerializeField] private Color colorTrue;
    [SerializeField] private Color colorFalse;
    private bool[] _checkList;

    public static event Action OnAllCorrect;
    public void Awake()
    {
        UIController.OnProblemSelected += FillList;
    }

    private void FillList(ProblemDefinition currentProblem)
    {
        _inputList.Clear();
        switch (currentProblem.problem)
        {
            case DoubleMU problem:
                _inputList.Add(new InputData() {title = "Posição Inicial de A:", value = problem.initialPosition, unit = currentProblem.unit});
                _inputList.Add(new InputData() {title = "Velocidade de A:", value = problem.velocity, unit = currentProblem.velocityUnit});
                _inputList.Add(new InputData() {title = "Posição Inicial de B:", value = problem.initialPositionB, unit = currentProblem.unit});
                _inputList.Add(new InputData() {title = "Velocidade de B", value = problem.velocityB, unit = currentProblem.velocityUnit});
                break;
            case SimpleMU problem:
                _inputList.Add(new InputData() {title = "Posição inicial:", value = problem.initialPosition, unit = currentProblem.unit});
                _inputList.Add(new InputData() {title = "Velocidade:", value = problem.velocity, unit = currentProblem.velocityUnit});
                break;
            case SimpleMUV problem:
                _inputList.Add(new InputData() {title = "Posição inicial:", value = problem.initialPosition, unit = currentProblem.unit});
                _inputList.Add(new InputData() {title = "Velocidade inicial:", value = problem.initialVelocity, unit = currentProblem.velocityUnit});
                _inputList.Add(new InputData() {title = "Aceleração:", value = problem.acceleration, unit = currentProblem.velocityUnit + "²"});
                break;
            case SimpleOT problem:
                _inputList.Add(new InputData() {title = "Posição inicial X:", value = problem.initialPosition.x, unit = currentProblem.unit});
                _inputList.Add(new InputData() {title = "Posição inicial Y:", value = problem.initialPosition.y, unit = currentProblem.unit});
                _inputList.Add(new InputData() {title = "Velocidade Modular inicial:", value = problem.initialVelocity, unit = currentProblem.velocityUnit});
                _inputList.Add(new InputData() {title = "Ângulo de Lançamento:", value = problem.throwAngle, unit = "°"});
                _inputList.Add(new InputData() {title = "Aceleração da gravidade:", value = problem.gravity, unit = currentProblem.velocityUnit + "²"});
                break;
        }

        PrintList(_inputList);
    }

    private void PrintList(List<InputData> inputList)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            var line = transform.GetChild(i);
            if (i > 1)
                Destroy(line.gameObject);
        }
        _checkList = new bool[_inputList.Count];
        for (var i = 0; i < _inputList.Count; i++)
        {
            var t = _inputList[i];
            var inputLine = Instantiate(inputLinePrefab, transform);
            inputLine.title.text = t.title;
            inputLine.unit.text = t.unit;
            var i1 = i;
            inputLine.inputField.onEndEdit.AddListener(valueStr =>
            {
                if(string.IsNullOrEmpty(valueStr))
                    return;
                var value = (float) Convert.ToDouble(valueStr, CultureInfo.InvariantCulture);
                t.value = Mathf.Abs(t.value);
                var isTrue = Mathf.Abs(t.value - value) < 0.001;
                _checkList[i1] = isTrue;
                CheckAllTrue();
                inputLine.inputBackground.color = isTrue ? colorTrue : colorFalse;
            });
        }
    }

    private void CheckAllTrue()
    {
        foreach (var b in _checkList)
        {
            if(b == false)
                return;
        }

        if (OnAllCorrect != null)
        {
            OnAllCorrect.Invoke();
            
        }
    }
}
