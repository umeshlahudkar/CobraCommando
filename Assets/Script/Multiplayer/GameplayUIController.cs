using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace FPS.Multiplayer
{
    public class GameplayUIController : MonoBehaviour
    {
        [Header("Stats Display")]
        [SerializeField] private TextMeshProUGUI killText;
        [SerializeField] private TextMeshProUGUI deathText;

        [Header("LeaderBoard Display")]
        [SerializeField] private GameObject leaderboardScreen;
        [SerializeField] private LeaderBoardPlayerInfo leaderBoardPlayerInfoPrefab;

        [Header("Game Over Screen")]
        [SerializeField] private GameObject gameOverScreen;

        [Header("GameManager script ref")]
        [SerializeField] private GameManager gameManager;

        [Header("Player Control UI")]
        [SerializeField] private DynamicJoystick rotateJoystick;
        [SerializeField] private DynamicJoystick moveJoystick;
        [SerializeField] private Button fireButton;

        private List<LeaderBoardPlayerInfo> playerInfos = new List<LeaderBoardPlayerInfo>();

        public void UpdateStatDisplay(int killCount, int deathCount)
        {
            killText.text = "KILL : " + killCount.ToString();
            deathText.text = "DEATH : " + deathCount.ToString();
        }

        public void EnableGameOverScreen()
        {
            gameOverScreen.SetActive(true);
        }

        private void UpdateLeaderBoard(List<PlayerInfo> players)
        {
            foreach (LeaderBoardPlayerInfo plyerInfo in playerInfos)
            {
                Destroy(plyerInfo.gameObject);
            }

            playerInfos.Clear();

            for (int i = 0; i < players.Count; i++)
            {
                LeaderBoardPlayerInfo info = Instantiate<LeaderBoardPlayerInfo>(leaderBoardPlayerInfoPrefab, leaderBoardPlayerInfoPrefab.transform.parent);
                info.SetupInfo(i + 1, players[i].name, players[i].kills, players[i].deaths);
                info.gameObject.SetActive(true);
                playerInfos.Add(info);
            }
        }

        public void ShowLeaderBoard(List<PlayerInfo> players)
        {
            if (leaderboardScreen.activeInHierarchy)
            {
                leaderboardScreen.SetActive(false);
            }
            else
            {
                leaderboardScreen.SetActive(true);
            }

            List<PlayerInfo> sortedList = SortList(players);
            UpdateLeaderBoard(sortedList);
        }

        private List<PlayerInfo> SortList(List<PlayerInfo> players)
        {
            List<PlayerInfo> sorted = new List<PlayerInfo>();
            PlayerInfo selectedPlayer = null;
            while (sorted.Count < players.Count)
            {
                int highest = -1;
                foreach (PlayerInfo pl in players)
                {
                    if (!sorted.Contains(pl))
                    {
                        if (pl.kills > highest)
                        {
                            highest = pl.kills;
                            selectedPlayer = pl;
                        }
                    }
                }
                sorted.Add(selectedPlayer);
            }
            return sorted;
        }

        public void OnMainMenuButtonClick()
        {
            Photon.Pun.PhotonNetwork.AutomaticallySyncScene = false;
            Photon.Pun.PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }

        public DynamicJoystick GetMoveJoystick()
        {
            return moveJoystick;
        }

        public DynamicJoystick GetRotateJoystick()
        {
            return rotateJoystick;
        }

        public Button GetFireButton()
        {
            return fireButton;
        }
    }
}
