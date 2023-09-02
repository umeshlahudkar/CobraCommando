using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace FPS.SinglePlayer
{
    public class GameplayUIController : Singleton<GameplayUIController>
    {
        [SerializeField] private TextMeshProUGUI killText;
        [SerializeField] private TextMeshProUGUI DeathText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI targetText;

        [Header("Level Display screen")]
        [SerializeField] private GameObject levelDispalyScreen;
        [SerializeField] private TextMeshProUGUI levelCountText;
        [SerializeField] private TextMeshProUGUI targetCountText;

        [SerializeField] private GameObject gameOverScreen;


        public void UpdateKillText(int count)
        {
            killText.text = "KILL : " + count.ToString();
        }

        public void UpdateDeathText(int count)
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

        public void OnMainMenuButtonClick()
        {
            SceneManager.LoadScene(0);
        }
    }

}