using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public float playTime = 3;
    public float minRealSize = 0.2f;
    public float maxRealSize = 0.3f;
    [Range(0, 1)] public float initialValue = 0.5f;

    [Header("Screens")]
    public GameObject[] screens;
    public GameObject optionsScreen;
    public Screen currentScreen = Screen.About;
    public Screen previousScreen;
    public GameObject panel;
    public GameObject footer;
    public GameObject contentArea;

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
    public Toggle invertColor;
    public GameObject instructionArea;
    public TextMeshProUGUI title;
    public GameObject ARArea;
    public TextMeshProUGUI contentExplanation;
    public ContentList contentQuestionsList;
    public TextMeshProUGUI sliderText;
    public GameObject slider;
    public ReplacementShaderEffect invertShaderScript;
    public Transform placement;
    public Sprite[] playSprites;
    public Image playImage;

    public enum Screen
    {
        About,
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
    public static event Action<float> OnPlayClick;

    private ProblemDefinition _currentProblem;
    private bool _isPlacementPositioned;
    private float _answer;
    private bool _instructionClosed = false;
    private Coroutine _playCoroutine;

    public void Awake()
    {
        UnityEngine.Screen.sleepTimeout = SleepTimeout.NeverSleep;
        panel.gameObject.SetActive(true);

        SetupScreen(currentScreen);
        InstantiateButtons();

        repositionButton.onClick.AddListener(RepositioningPlane);
        instructionButton.onClick.AddListener(OpenInstruction);
        backButton.onClick.AddListener(BackButtonClick);
        settingButton.onClick.AddListener(() => SetupScreen(Screen.Settings));
        playButton.onClick.AddListener(OnPlay);
        
        invertColor.onValueChanged.AddListener((invertState => invertShaderScript.enabled = invertState));
        scaleSlider.onValueChanged.AddListener(ScalingPlacement);

        scaleSlider.value = initialValue;

        ProblemController.OnAnswerValueChange += f => _answer = f;
        ProblemController.OnSliderValueChange += f => sliderText.text = (f * _answer).ToString("F1") + " s";
    }

    private IEnumerator PlayScene()
    {
        var lerp = ProblemController.getSliderValue.Invoke();

        if (Math.Abs(lerp - 1) < 0.01)
        {
            lerp = 0;
        }
        
        do
        {
            lerp += Time.deltaTime / playTime;
            OnPlayClick?.Invoke(lerp);
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
    
    private void ScalingPlacement(float normalizedValue)
    {
        var finalScale = Mathf.Lerp(minRealSize, maxRealSize, normalizedValue);
        placement.localScale = Vector3.one * finalScale;
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
            case Screen.Explanation:
                SetupScreen(Screen.ContentList);
                OnBackClick?.Invoke();
                break;

            case Screen.Positioning:
                contentArea.SetActive(false);
                SetupScreen(Screen.Explanation);
                break;

            case Screen.ARVisualizer:
                contentArea.SetActive(false);
                slider.SetActive(false);
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
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        instructionButton.gameObject.SetActive(false);
        instructionArea.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        contentArea.SetActive(true);
        footer.SetActive(true);
        scaleSlider.gameObject.SetActive(false);

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
                EnableNextButton("Continuar", () => SetupScreen(Screen.ContentList));

                settingButton.gameObject.SetActive(true);
                screens[0].gameObject.SetActive(true);
                break;

            case Screen.ContentList:
                UpdateTitle("Selecione o assunto");

                footer.SetActive(false);
                settingButton.gameObject.SetActive(true);
                screens[1].gameObject.SetActive(true);
                break;

            case Screen.Explanation:
                UpdateTitle("Sobre o problema");
                EnableNextButton("Visualizar em RA", () =>
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

                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                screens[2].gameObject.SetActive(true);
                break;

            case Screen.Instruction:
                UpdateTitle("Instruções");
                EnableNextButton("Prosseguir", () =>
                {
                    SetupScreen(Screen.Positioning);
                    _instructionClosed = true;
                });

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
                contentArea.SetActive(false);
                slider.SetActive(false);
                break;

            case Screen.ARVisualizer:
                UpdateTitle("Movimento uniforme");

                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                slider.SetActive(true);
                break;

            case Screen.Settings:
                UpdateTitle("Configurações");

                optionsScreen.SetActive(true);
                backButton.gameObject.SetActive(true);
                ARArea.SetActive(previousScreen == Screen.ARVisualizer); // Can be improved
                slider.SetActive(false);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newScreen), newScreen, null);
        }
    }

    private void UpdateTitle(string newTitle)
    {
        title.text = newTitle;
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

    private void InstantiateButtons()
    {
        for (var i = 0; i < contentQuestionsList.problems.Length; i++)
        {
            if (contentQuestionsList.problems[i].isActive != true) continue;
            
            var instantiatedButton = Instantiate(questionButtonPrefab, questionContentArea.transform);
            var button = instantiatedButton.GetComponent<QuestionButton>();

            var i1 = i;
            button.questionButton.onClick.AddListener(() =>
            {
                SetupScreen(Screen.Explanation);
                var text = contentQuestionsList.problems[i1].longDescription;
                contentExplanation.text = text;
                _currentProblem = contentQuestionsList.problems[i1];
                _isPlacementPositioned = false;
                OnProblemSelected?.Invoke(_currentProblem);
            });

            button.questionImage.sprite = contentQuestionsList.problems[i1].sprite;
            button.titleText.text = contentQuestionsList.problems[i1].title;
        }
    }
}