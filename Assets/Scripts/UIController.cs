using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIController : MonoBehaviour
{
    public GameObject[] screens;
    public Button nextButton;
    public Button backButton;
    public Button[] topicButtons;
    public TextMeshProUGUI contentExplanation;
    public ContetTexts contentExplanationsTexts;
    public GameObject panel;
    public Button backButton2;
    public GameObject questionButtonPrefab;
    public GameObject questionContentArea;
    
    private int _currentScreen = 0; 
    
    public void Awake()
    {
        for (var i = 0; i < screens.Length; i++)
        {
            screens[i].gameObject.SetActive(false);
        }
        
        SetupScreen();
        InstantiateButtons();
        nextButton.onClick.AddListener(NextScreen);
        backButton.onClick.AddListener(LastScreen);
        backButton2.onClick.AddListener(ActivatePanel);    
    }

    private void NextScreen()
    {
        if (_currentScreen < screens.Length)
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

    private void InstantiateButtons()
    {
        for (var i = 0; i < contentExplanationsTexts.texts.Length; i++)
        {
            var instantiatedButton = Instantiate(questionButtonPrefab,questionContentArea.transform);
            var button = instantiatedButton.GetComponent<Button>();

            var i1 = i;
            button.onClick.AddListener((() =>
            {
                NextScreen();
                var text = contentExplanationsTexts.texts[i1];
                contentExplanation.text = text;
            }));
        }
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
                panel.SetActive(true);
                break;
            case 3:
                nextButton.gameObject.SetActive(false);
                panel.SetActive(false);
                break;
        }
    }
    
    private void ActivatePanel()
    {
        panel.SetActive(true);
        nextButton.gameObject.SetActive(true);
        _currentScreen--;
        SetupScreen();
    }
}
