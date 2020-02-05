using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject[] screens;
    public Button nextButton;
    public Button backButton;
    public Button[] topicButtons;
    public TextMeshProUGUI contentExplanation;
    public ContetTexts contentExplanationsTexts;
    
    private int _currentScreen = 0; 
    
    public void Awake()
    {
        SetupScreen();
        nextButton.onClick.AddListener(NextScreen);
        backButton.onClick.AddListener(LastScreen);
        for (var i = 0; i < topicButtons.Length; i++)
        {
            var i1 = i;
            topicButtons[i].onClick.AddListener(() =>
            {
                NextScreen();
                var text = contentExplanationsTexts.texts[i1];
                contentExplanation.text = text;
            });
        }
    }

    private void NextScreen()
    {
        if (_currentScreen < screens.Length - 1)
        {
            screens[_currentScreen].SetActive(false);
            _currentScreen++;
            SetupScreen();
        }
        Debug.Log(_currentScreen);
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

    private void SetupScreen()
    {
        switch (_currentScreen)
        {
            case 0:
                screens[0].SetActive(true);
                backButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(true);
                break;
            case 1:
                screens[1].SetActive(true);
                backButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                break;
            case 2:
                screens[2].SetActive(true);
                nextButton.gameObject.SetActive(true);
                break;
        }
    }
}
