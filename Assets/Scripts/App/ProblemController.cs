using System;
using UnityEngine;
using UnityEngine.UI;

public class ProblemController : MonoBehaviour
{
    private Problem _currentProblem;
    private GameObject[] _instantiatedModels;
    public Vector2 minValue = new Vector2(-0.5f, 0);
    public Vector2 maxValue = new Vector2(0.5f, 1);
    public Slider controllerSlider;
    
    public static event Action<Vector2, Vector2> OnMinMaxValueChange;
    public static event Action<int, Vector2> OnModelPositionUpdate;
    public static event Action<float> OnSliderValueChange;
    public static event Action<float> OnAnswerValueChange;
    public static Func<float> getSliderValue;
    
    private GameObject _instantiatedEnvironment;
    
    private void Awake()
    {
        controllerSlider.onValueChanged.AddListener(UpdatePosition);
        UIController.OnProblemSelected += OnProblemSelected;
        UIController.OnBackClick += UIControllerOnOnBackClick;
        UIController.OnPlayClick += UIControllerOnPlayClick;
        getSliderValue += () => controllerSlider.value;
    }

    private void UIControllerOnPlayClick(float normalizedValue)
    {
        controllerSlider.value = normalizedValue;
    }

    private void UIControllerOnOnBackClick()
    {
        
        if(_instantiatedModels != null)
        { 
            for (int i = 0; i < _instantiatedModels.Length; i++)
            {
                Destroy(_instantiatedModels[i]);
            }
        }

        if (_instantiatedEnvironment != null)
        {
            Destroy(_instantiatedEnvironment);
        }

        _instantiatedModels = null;
        controllerSlider.value = 0;
    }

    private void OnProblemSelected(ProblemDefinition currentQuestion)
    {
        _currentProblem = (currentQuestion.problem);
        _currentProblem.Process();
        
        OnAnswerValueChange?.Invoke(_currentProblem.Answer);

        if (OnMinMaxValueChange != null)
        {
            OnMinMaxValueChange.Invoke(_currentProblem.minValue, _currentProblem.maxValue);
        }
        _instantiatedModels = new GameObject[currentQuestion.models.Length];

        if (currentQuestion.environment != null)
        {
            _instantiatedEnvironment = Instantiate(currentQuestion.environment, transform);
        }

        for (var i = 0; i < currentQuestion.models.Length; i++)
        {
            var model = currentQuestion.models[i];

            if (model != null)
            {
                _instantiatedModels[i] = Instantiate(model, transform);
            }
        }

        UpdatePosition(0);
    }

    private void UpdatePosition(float normalizedValue)
    {
        OnSliderValueChange?.Invoke(normalizedValue);

        if (_instantiatedModels == null || _instantiatedModels.Length <= 0)
            return;

        
        var normalizedAnswer = Mathf.InverseLerp(_currentProblem.minValue.x, _currentProblem.maxValue.x,
            _currentProblem.Evaluate(normalizedValue));
        var realX = Mathf.Lerp(minValue.x, maxValue.x, normalizedAnswer);
        var realY = 0f;
        var xValue = _currentProblem.Evaluate(normalizedValue);
        var yValue = 0f;
        if (_currentProblem is SimpleOT simpleOt)
        {
            var normalizedAnswerY = Mathf.InverseLerp(_currentProblem.minValue.y, _currentProblem.maxValue.y,
                simpleOt.EvaluateY(normalizedValue));
            yValue = simpleOt.EvaluateY(normalizedValue);
            realY = Mathf.Lerp(minValue.y, maxValue.y, normalizedAnswerY);
        }
        _instantiatedModels[0].transform.localPosition = new Vector3(realX, realY);

        if (OnModelPositionUpdate != null)
        {
            OnModelPositionUpdate.Invoke(0, new Vector2(xValue, yValue));
        }

        if (_currentProblem is DoubleMU doubleProblem && _instantiatedModels.Length > 1)
        {
            normalizedAnswer = Mathf.InverseLerp(doubleProblem.minValue.x, doubleProblem.maxValue.x,
                doubleProblem.EvaluateB(normalizedValue));
            realX = Mathf.Lerp(minValue.x, maxValue.x, normalizedAnswer);
            _instantiatedModels[1].transform.localPosition = new Vector3(realX, 0);
            
            if (OnModelPositionUpdate != null)
            {
                OnModelPositionUpdate.Invoke(1, new Vector2(doubleProblem.EvaluateB(normalizedValue), 0));
            }
        }
    }
}