using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [Header("Main Menu Screen")]
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingScreenText;

    [Header("Lobby screen Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void Start()
    {
        if(!mainMenuScreen.activeInHierarchy)
        {
            mainMenuScreen.SetActive(true);
        }
    }

    private void OpenLoadingScreen(string msg)
    {
        loadingScreenText.text = msg;
        loadingScreen.SetActive(true);
    }

    private void OnPlayButtonClick()
    {
        Debug.Log("Play button click");
    }
  
    private void OnQuitButtonClick()
    {
        Debug.Log("Quit button click");
        Application.Quit();
    }

    private void OnDisable()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }
}
