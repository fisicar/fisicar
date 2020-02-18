﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UIElements.Image;

public class UIController : MonoBehaviour
{
    public GameObject[] screens;
    public GameObject optionsScreen;
    public Button optionsBackButton;
    public Button nextButton;
    public Button startButton;
    public Button optionsButton;
    public Button backButton;
    public TextMeshProUGUI contentExplanation;
    public ContentList contentQuestionsList;
    public GameObject panel;
    public Button backButton2;
    public GameObject questionButtonPrefab;
    public GameObject questionContentArea; 
    public static event Action<ProblemDefinition> OnProblemSelected;
    public static event Action OnBackClick;

    private ProblemDefinition _currentProblem;
    private int _currentScreen = 0; 
    
    public void Awake()
    {
        foreach (var t in screens)
        {
            t.gameObject.SetActive(false);
        }
        panel.gameObject.SetActive(true);
        
        SetupScreen();
        InstantiateButtons();
        startButton.onClick.AddListener(NextScreen);
        nextButton.onClick.AddListener(NextScreen);
        backButton.onClick.AddListener(LastScreen);
        optionsButton.onClick.AddListener(OptionsScreen);
        backButton2.onClick.AddListener(ActivatePanel);    
        optionsBackButton.onClick.AddListener(ExitOptions);
    }

    private void ExitOptions()
    {
        optionsScreen.gameObject.SetActive(false);
        gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        SetupScreen();
    }

    private void OptionsScreen()
    {
        gameObject.SetActive(false);
        optionsScreen.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(false);
        
    }

    private void NextScreen()
    {
        if (_currentScreen < screens.Length)
        {
            screens[_currentScreen].SetActive(false);
            _currentScreen++;    
            SetupScreen();
        }
    }
    
    private void LastScreen()
    {
        if (_currentScreen >= 0)
        {
            screens[_currentScreen].SetActive(false);
            _currentScreen--;
            SetupScreen();
        }
        Debug.Log(_currentScreen);
    }

    private void InstantiateButtons()
    {
        for (var i = 0; i < contentQuestionsList.problems.Length; i++)
        {
            var instantiatedButton = Instantiate(questionButtonPrefab,questionContentArea.transform);
            var button = instantiatedButton.GetComponent<QuestionButton>();

            var i1 = i;
            button.questionButton.onClick.AddListener((() =>
            {
                NextScreen();
                var text = contentQuestionsList.problems[i1].longDescription;
                contentExplanation.text = text;
                _currentProblem = contentQuestionsList.problems[i1];
            }));

            button.questionImage.sprite = contentQuestionsList.problems[i1].sprite;
            button.titleText.text = contentQuestionsList.problems[i1].title;
        }
    }
    private void SetupScreen()
    {
        switch (_currentScreen)
        {
            case 0:
                screens[0].SetActive(true);
                optionsButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(true);
                break;
            case 1:
                screens[1].SetActive(true);
                startButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                break;
            case 2:
                screens[2].SetActive(true);
                nextButton.gameObject.SetActive(true);
                panel.SetActive(true);
                break;
            case 3:
                nextButton.gameObject.SetActive(false);
                panel.SetActive(false);
                if (OnProblemSelected != null)
                {
                    OnProblemSelected.Invoke(_currentProblem);
                }
                break;
        }
    }
    
    private void ActivatePanel()
    {
        panel.SetActive(true);
        nextButton.gameObject.SetActive(true);
        _currentScreen--;
        SetupScreen();
        if (OnBackClick != null)
        {
            OnBackClick.Invoke();
        }
    }
}
