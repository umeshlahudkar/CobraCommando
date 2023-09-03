using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [Header("Main Menu Screen")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("Loading Screen")]
    [SerializeField] private LoadingScreen loadingScreen;

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
    [SerializeField] private Button homeButton;
    [SerializeField] private Button retryButton;

    [Header("Player UI")]
    public DynamicJoystick moveJoystick;
    public DynamicJoystick rotateJoystick;
    public Slider healthBar;


    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonclick);
        retryButton.onClick.AddListener(OnRetryButtonclick);
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
        DisableAllScreen();
        loadingScreen.gameObject.SetActive(true);
        GameManager.Instance.PrepareGameplay(1);
        yield return StartCoroutine(loadingScreen.PlayLoadingAnimation());
        GameManager.Instance.StartLevel();
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void OnHomeButtonclick()
    {
        GameManager.Instance.ClearAll();
        DisableAllScreen();
        mainMenuScreen.SetActive(true);
    }

    private void OnRetryButtonclick()
    {
        StartCoroutine(RetryGameplay());
    }

    private IEnumerator RetryGameplay()
    {
        DisableAllScreen();
        loadingScreen.gameObject.SetActive(true);
        GameManager.Instance.RetryLevel();
        yield return StartCoroutine(loadingScreen.PlayLoadingAnimation());
        GameManager.Instance.StartLevel();
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

    public void EnableGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    private void DisableAllScreen()
    {
        gameOverScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
        quitButton.onClick.RemoveListener(OnQuitButtonClick);
        homeButton.onClick.RemoveListener(OnHomeButtonclick);
        retryButton.onClick.RemoveListener(OnRetryButtonclick);
    }
}
