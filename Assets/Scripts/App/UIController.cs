using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UIElements.Image;

public class UIController : MonoBehaviour
{
    //screens
    public GameObject[] screens;
    public GameObject optionsScreen;
    public Screen currentScreen = Screen.About;
    public Screen previousScreen;    public GameObject panel;
    public GameObject footer;
    public GameObject contentArea;
    
    //buttons
    public Button nextButton;
    public TextMeshProUGUI nextButtonText;
    public Button settingButton;
    public Button backButton;
    public GameObject questionButtonPrefab;
    public GameObject questionContentArea;
    public Button repositionButton;

    public TextMeshProUGUI title;
    public GameObject ARArea;
    public TextMeshProUGUI contentExplanation;
    public ContentList contentQuestionsList;
    
    public GameObject slider;
    public enum Screen
    {
        About,
        ContentList,
        Explanation,
        Positioning,
        ARVisualizer,
        Settings,
    }

    public static event Action<ProblemDefinition> OnProblemSelected;
    public static event Action OnBackClick;
    public static event Action <bool> IsPositioning;

    private ProblemDefinition _currentProblem;
    private bool _isPlacementPositioned;

    public void Awake()
    {
        panel.gameObject.SetActive(true);

        SetupScreen(currentScreen);
        InstantiateButtons();
        
        repositionButton.onClick.AddListener(RepositioningPlane);

        //Panel
        backButton.onClick.AddListener(BackButtonClick);

        //Options Panel
        settingButton.onClick.AddListener(() => SetupScreen(Screen.Settings));
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
                break;
            
            case Screen.Positioning:
                contentArea.SetActive(false);
                SetupScreen(Screen.Explanation);
                OnBackClick?.Invoke();
                break;
            
            case Screen.ARVisualizer:
                contentArea.SetActive(false);
                slider.SetActive(false);
                SetupScreen(Screen.Explanation);
                OnBackClick?.Invoke();
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
        settingButton.gameObject.SetActive(false);
        contentArea.SetActive(true);
        footer.SetActive(true);

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
                EnableNextButton("Visualizar RA", () =>
                {
                    OnProblemSelected?.Invoke(_currentProblem);
                    SetupScreen(_isPlacementPositioned ? Screen.ARVisualizer : Screen.Positioning);
                });
                UpdateTitle("Sobre o problema");
                backButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                screens[2].gameObject.SetActive(true);
                break;

            case Screen.Positioning:
                IsPositioning ? .Invoke(true);
                EnableNextButton("Posicionar", SetPosition);
                UpdateTitle("Posicionar o plano");
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
            });

            button.questionImage.sprite = contentQuestionsList.problems[i1].sprite;
            button.titleText.text = contentQuestionsList.problems[i1].title;
        }
    }
}
