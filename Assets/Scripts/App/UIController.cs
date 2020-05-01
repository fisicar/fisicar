using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public float playTime = 3;
    [Range(0, 1)] public float initialValue = 0.5f;

    [Header("Screens")]
    public GameObject[] screens;
    public GameObject optionsScreen;
    public Screen currentScreen = Screen.About;
    public Screen previousScreen;
    public Screen previousScreenOverride;
    public GameObject panel;
    public GameObject footer;
    public GameObject scrollViewContainer;
    public GameObject ARContainer;

    [Header("Buttons")]
    public Button nextButton;
    public TextMeshProUGUI nextButtonText;
    public Button settingButton;
    public Button backButton;
    public GameObject questionButtonPrefab;
    public GameObject questionContentArea;
    public Button repositionButton;
    public Button instructionButton;
    public Button playButton;

    [Header("???")]
    public Slider scaleSlider;
    public Toggle showEquation;
    public GameObject instructionArea;
    public GameObject viewArea;
    public TextMeshProUGUI equationText;
    public TextMeshProUGUI title;
    public GameObject ARArea;
    public TextMeshProUGUI contentExplanation;
    public ContentList contentQuestionsList;
    public GameObject selectTopicArea;
    public TextMeshProUGUI topicExplanationText;
    public Slider controllerSlider;
    public TextMeshProUGUI sliderText;
    public GameObject sliderArea;
    public Sprite[] playSprites;
    public Image playImage;

    public enum Screen
    {
        About,
        TopicList,
        TopicText,
        ContentList,
        Explanation,
        Instruction,
        Positioning,
        ARVisualizer,
        Settings,
    }

    public static event Action<ProblemDefinition> OnProblemSelected;
    public static event Action OnBackClick;
    public static event Action<bool> IsPositioning;
    public static event Action<float> OnScaleSlideValueChange;
    public static event Action<float> OnControllerSliderValueChange;

    private ProblemDefinition _currentProblem;
    private bool _isPlacementPositioned;
    private float _answer;
    private bool _instructionClosed = false;
    private Coroutine _playCoroutine;
    private TopicDefinition _currentTopic;
    private List<GameObject> _questionButtons = new List<GameObject>();

    public void Awake()
    {
        UnityEngine.Screen.sleepTimeout = SleepTimeout.NeverSleep;
        panel.gameObject.SetActive(true);

        SetupScreen(currentScreen);
        InstantiateTopicButtons();
        
        repositionButton.onClick.AddListener(RepositioningPlane);
        instructionButton.onClick.AddListener(OpenInstruction);
        backButton.onClick.AddListener(BackButtonClick);
        settingButton.onClick.AddListener(SettingsButtonClick);
        playButton.onClick.AddListener(OnPlay);

        scaleSlider.onValueChanged.AddListener(arg0 => OnScaleSlideValueChange?.Invoke(arg0));
        scaleSlider.value = initialValue;

        controllerSlider.onValueChanged.AddListener(UpdateSliderText);
        controllerSlider.onValueChanged.AddListener((arg0 => OnControllerSliderValueChange?.Invoke(arg0)));

        ProblemController.OnAnswerValueChange += f => _answer = f;
        ProblemController.OnUpdateControllerSliderValue += f => controllerSlider.value = f;
        ExplanationInputController.OnAllCorrect += () => nextButton.interactable = true;
    }

    private void UpdateSliderText(float arg0)
    {
        sliderText.text = (arg0 * _answer).ToString("F1") + " s";
    }

    private void SettingsButtonClick()
    {
        SetupScreen(Screen.Settings);
        previousScreenOverride = previousScreen;
    }

    private IEnumerator PlayScene()
    {
        var lerp = controllerSlider.value;
        
        if (Math.Abs(lerp - 1) < 0.01)
        {
            lerp = 0;
        }
        
        do
        {
            lerp += Time.deltaTime / playTime;
            controllerSlider.value = lerp;
            yield return null;
            
        } while (lerp < 1);
        
        UpdatePlaySprite(2);
        _playCoroutine = null;
    }

    private void OnPlay()
    {
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
            UpdatePlaySprite(0);
        }
        else
        {
            UpdatePlaySprite(1);
            _playCoroutine = StartCoroutine(nameof(PlayScene));
        }
    }

    private void UpdatePlaySprite(int i)
    {
        playImage.sprite = playSprites[i];
    }

    private void OpenInstruction()
    {
        SetupScreen(Screen.Instruction);
        instructionButton.gameObject.SetActive(false);
    }

    private void RepositioningPlane()
    {
        SetupScreen(Screen.Positioning);
        _isPlacementPositioned = false;
    }

    private void BackButtonClick()
    {
        switch (currentScreen)
        {
            case Screen.About:
                SetupScreen(Screen.Settings);
                previousScreen = previousScreenOverride;
                break;
            
            case Screen.TopicText:
                SetupScreen(Screen.TopicList);
                break;
            
            case Screen.ContentList:
                SetupScreen(Screen.TopicText);
                break;
            
            case Screen.Explanation:
                SetupScreen(Screen.ContentList);
                OnBackClick?.Invoke();
                nextButton.interactable = true;
                break;

            case Screen.Positioning:
                scrollViewContainer.SetActive(false);
                SetupScreen(Screen.Explanation);
                break;

            case Screen.ARVisualizer:
                scrollViewContainer.SetActive(false);
                sliderArea.SetActive(false);
                SetupScreen(Screen.Explanation);
                break;

            case Screen.Settings:
                SetupScreen(previousScreen);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetupScreen(Screen newScreen)
    {
        viewArea.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        instructionButton.gameObject.SetActive(false);
        instructionArea.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        scrollViewContainer.SetActive(true);
        footer.SetActive(true);
        scaleSlider.gameObject.SetActive(false);
        ARContainer.SetActive(false);

        foreach (var t in screens)
        {
            t.gameObject.SetActive(false);
        }

        optionsScreen.gameObject.SetActive(false);

        previousScreen = currentScreen;
        currentScreen = newScreen;

        switch (newScreen)
        {
            case Screen.About:
                if (previousScreen == Screen.About)
                {
                    EnableNextButton("Continuar", () => SetupScreen(Screen.TopicList));
                }
                else
                {
                    
                    backButton.gameObject.SetActive(true);
                }

                screens[0].gameObject.SetActive(true);
                break;
            
            case Screen.TopicList:
                UpdateTitle("Selecione o assunto");
                screens[1].gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                break;
            
            case Screen.TopicText:
                EnableNextButton("Continuar", (() => SetupScreen(Screen.ContentList)));
                UpdateTitle(_currentTopic.topicTitle);
                if (previousScreen == Screen.TopicList)
                {
                    InstantiateProblemButtons();
                }
                screens[2].gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
                break;

            case Screen.ContentList:
                UpdateTitle("Selecione o problema");

                footer.SetActive(false);
                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                screens[3].gameObject.SetActive(true);
                break;

            case Screen.Explanation:
                UpdateTitle("Sobre o problema");
                EnableNextButton("Visualizar Questão", () =>
                {
                    if (_instructionClosed)
                    {
                        SetupScreen(_isPlacementPositioned ? Screen.ARVisualizer : Screen.Positioning);
                    }
                    else
                    {
                        SetupScreen(Screen.Instruction);
                    }
                });

                if (previousScreen == Screen.ContentList)
                    nextButton.interactable = false;
                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                screens[4].gameObject.SetActive(true);
                break;

            case Screen.Instruction:
                UpdateTitle("Instruções");
                EnableNextButton("Prosseguir", () =>
                {
                    SetupScreen(Screen.Positioning);
                    _instructionClosed = true;
                });
                ARContainer.SetActive(true);
                scrollViewContainer.SetActive(false);
                settingButton.gameObject.SetActive(true);
                instructionArea.gameObject.SetActive(true);
                break;

            case Screen.Positioning:
                IsPositioning?.Invoke(true);
                UpdateTitle("Posicionar o plano");
                EnableNextButton("Posicionar", SetPosition);

                scaleSlider.gameObject.SetActive(true);
                instructionButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                ARContainer.SetActive(true);
                scrollViewContainer.SetActive(false);
                sliderArea.SetActive(false);
                break;

            case Screen.ARVisualizer:
                UpdateTitle(_currentProblem.title);
                UpdateEquationText(_currentProblem.problem.equation);
                
                ARContainer.SetActive(true);
                scrollViewContainer.SetActive(false);
                viewArea.gameObject.SetActive(showEquation.isOn);
                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                sliderArea.SetActive(true);
                break;

            case Screen.Settings:
                UpdateTitle("Configurações");
                EnableNextButton("Sobre", () => SetupScreen(Screen.About));

                optionsScreen.SetActive(true);
                backButton.gameObject.SetActive(true);
                ARArea.SetActive(previousScreen == Screen.ARVisualizer); // Can be improved
                sliderArea.SetActive(false);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(newScreen), newScreen, null);
        }
    }

    private void UpdateTitle(string newTitle)
    {
        title.text = newTitle;
    }
    private void UpdateEquationText(string newEquation)
    {
        equationText.text = newEquation;
    }

    private void SetPosition()
    {
        _isPlacementPositioned = true;
        IsPositioning?.Invoke(false);
        SetupScreen(Screen.ARVisualizer);
    }

    private void EnableNextButton(string title, Action onClickAction)
    {
        nextButton.gameObject.SetActive(true);
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => onClickAction?.Invoke());
        nextButtonText.text = title;
    }

    private void InstantiateProblemButtons()
    {
        foreach (var button in _questionButtons)
        {
            Destroy(button);
        }
        _questionButtons.Clear();
        
        for (var i = 0; i < _currentTopic.problems.Length; i++)
        {
            if (_currentTopic.problems[i].isActive != true) continue;
            
            var instantiatedButton = Instantiate(questionButtonPrefab, questionContentArea.transform);
            var button = instantiatedButton.GetComponent<QuestionButton>();
            _questionButtons.Add(instantiatedButton);

            var i1 = i;
            button.questionButton.onClick.AddListener(() =>
            {
                _currentProblem = _currentTopic.problems[i1];
                SetupScreen(Screen.Explanation);
                var text = _currentProblem.longDescription;
                contentExplanation.text = text;
                _isPlacementPositioned = false;
                OnProblemSelected?.Invoke(_currentProblem);
            });

            button.questionImage.sprite = _currentTopic.problems[i].sprite;
            button.titleText.text = _currentTopic.problems[i].title;
        }
    }

    private void InstantiateTopicButtons()
    {
        for (var i = 0; i < contentQuestionsList.topics.Length; i++)
        {
            var instantiatedButton = Instantiate(questionButtonPrefab, selectTopicArea.transform);
            var button = instantiatedButton.GetComponent<QuestionButton>();
            
            var i1 = i;
            button.questionButton.onClick.AddListener(() =>
            {
                _currentTopic = contentQuestionsList.topics[i1];
                SetupScreen(Screen.TopicText);
                var text = _currentTopic.topicDescription;
                topicExplanationText.text = text;
            });
            
            button.questionImage.sprite = contentQuestionsList.topics[i1].topicSprite;
            button.titleText.text = contentQuestionsList.topics[i1].topicTitle;
        }
    }
}