using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [Header("Main Menu Screen")]
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Loading Screen")]
    [SerializeField] private LoadingScreen loadingScreen;

    [Header("Lobby screen Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("Gameplay Stats text ")]
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI targetText;

    [Header("Level Display screen")]
    [SerializeField] private GameObject levelDispalyScreen;
    [SerializeField] private TextMeshProUGUI levelCountText;
    [SerializeField] private TextMeshProUGUI targetCountText;

    [Header("Game Over screen")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("Player UI")]
    public DynamicJoystick moveJoystick;
    public DynamicJoystick rotateJoystick;
    public Slider healthBar; 


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

    private void OnPlayButtonClick()
    {
        StartCoroutine(StartGameplay());
    }

    private IEnumerator StartGameplay()
    {
        loadingScreen.gameObject.SetActive(true);
        GameManager.Instance.PrepareGameplay(1);
        yield return StartCoroutine(loadingScreen.PlayLoadingAnimation());
        GameManager.Instance.StartGamePlay();
        mainMenuScreen.SetActive(false);
    }

    private void OnQuitButtonClick()
    {
        Debug.Log("Quit button click");
        Application.Quit();
    }

    public void UpdateKillText(int count)
    {
        killText.text = "KILL : " + count.ToString();
    }

    public void UpdateLevelTargetText(int level)
    {
        levelText.text = "LEVEL : " + level.ToString();
        targetText.text = "TARGET : " + level.ToString();
    }

    public void EnableLevelDisplayScreen(int level)
    {
        levelDispalyScreen.SetActive(true);
        levelCountText.text = "LEVEL : " + level.ToString();
        targetCountText.text = "TARGET : " + level.ToString();
    }

    public void DisableLevelDisplayScreen()
    {
        levelDispalyScreen.SetActive(false);
    }

    public void EnableGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    private void OnDisable()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }
}
