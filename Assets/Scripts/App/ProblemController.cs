using UnityEngine;
using UnityEngine.UI;

public class ProblemController : MonoBehaviour
{
    private Problem _currentProblem;
    private GameObject[] _instantiatedModels;
    public Vector2 minValue = new Vector2(-0.5f, 0);
    public Vector2 maxValue = new Vector2(0.5f, 1);
    public Slider controllerSlider;

    private GameObject _instantiatedEnvironment;

    private void Awake()
    {
        controllerSlider.onValueChanged.AddListener(UpdatePosition);
        UIController.OnProblemSelected += OnProblemSelected;
        UIController.OnBackClick += UIControllerOnOnBackClick;
    }

    private void UIControllerOnOnBackClick()
    {
        for (int i = 0; i < _instantiatedModels.Length; i++)
        {
            Destroy(_instantiatedModels[i]);
        }
        
        Destroy(_instantiatedEnvironment);

        _instantiatedModels = null;
        controllerSlider.value = 0;
    }

    private void OnProblemSelected(ProblemDefinition currentQuestion)
    {
        _currentProblem = (currentQuestion.problem);
        _currentProblem.Process();
        _instantiatedModels = new GameObject[currentQuestion.models.Length];
        
        _instantiatedEnvironment = Instantiate(currentQuestion.environment, transform);

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
        if (_instantiatedModels == null || _instantiatedModels.Length <= 0)
            return;

        var normalizedAnswer = Mathf.InverseLerp(_currentProblem.minValue.x, _currentProblem.maxValue.x,
            _currentProblem.Evaluate(normalizedValue));
        var realX = Mathf.Lerp(minValue.x, maxValue.x, normalizedAnswer);
        _instantiatedModels[0].transform.localPosition = new Vector3(realX, 0);

        if (_currentProblem is DoubleMU doubleProblem && _instantiatedModels.Length > 1)
        {
            normalizedAnswer = Mathf.InverseLerp(doubleProblem.minValue.x, doubleProblem.maxValue.x,
                doubleProblem.EvaluateB(normalizedValue));
            realX = Mathf.Lerp(minValue.x, maxValue.x, normalizedAnswer);
            _instantiatedModels[1].transform.localPosition = new Vector3(realX, 0);
        }
    }
}