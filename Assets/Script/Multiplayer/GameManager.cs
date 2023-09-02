using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace FPS.Multiplayer
{
    public class GameManager : Singleton<GameManager>, IOnEventCallback
    {
        [SerializeField] private Slider healthBarSlider;
        [SerializeField] private GameplayUIController gameplayUIController;
        [SerializeField] private List<PlayerInfo> players = new List<PlayerInfo>();

        private int killsToWin = 3;
        private GameState gameState = GameState.Waiting;

        private int index;

        public void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code < 200)
            {
                EventCode eventCode = (EventCode)photonEvent.Code;
                object[] data = (object[])photonEvent.CustomData;
                switch (eventCode)
                {
                    case EventCode.NewPlayer:
                        NewPlayerEventReceive(data);
                        break;

                    case EventCode.ListPlayer:
                        ListPlayerEventReceive(data);
                        break;

                    case EventCode.UpdateState:
                        UpdateStatEventReceive(data);
                        break;
                }
            }
        }

        private void Start()
        {
            SpawnManager.Instance.SpawnPlayer(healthBarSlider,
                gameplayUIController.GetRotateJoystick(), gameplayUIController.GetMoveJoystick(), gameplayUIController.GetFireButton());
            NewPlayerEventSend(PhotonNetwork.NickName);
            gameState = GameState.Playing;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //gameplayUIController.ShowLeaderBoard(players);
            }
        }

        public void NewPlayerEventSend(string userName)
        {
            object[] package = new object[4];
            package[0] = userName;
            package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
            package[2] = 0;
            package[3] = 0;

            PhotonNetwork.RaiseEvent(
                (byte)EventCode.NewPlayer,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true }
                );
        }

        public void NewPlayerEventReceive(object[] data)
        {
            PlayerInfo player = new PlayerInfo((string)data[0], (int)data[1], (int)data[2], (int)data[3]);
            players.Add(player);

            ListPlayerEventSend();
        }

        public void ListPlayerEventSend()
        {
            object[] package = new object[players.Count + 1];
            package[0] = gameState;

            for (int i = 0; i < players.Count; i++)
            {
                object[] piece = new object[4];
                piece[0] = players[i].name;
                piece[1] = players[i].actorNumber;
                piece[2] = players[i].kills;
                piece[3] = players[i].deaths;

                package[i + 1] = piece;
            }

            PhotonNetwork.RaiseEvent(
                (byte)EventCode.ListPlayer,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void ListPlayerEventReceive(object[] data)
        {
            players.Clear();

            gameState = (GameState)data[0];
            for (int i = 1; i < data.Length; i++)
            {
                object[] piece = (object[])data[i];
                PlayerInfo player = new PlayerInfo((string)piece[0], (int)piece[1], (int)piece[2], (int)piece[3]);

                players.Add(player);

                if (PhotonNetwork.LocalPlayer.ActorNumber == player.actorNumber)
                {
                    index = i - 1;
                }
            }

            StateCheck();
        }

        public void UpdateStatEventSend(int actorSending, int statToUpdate, int amountToChange)
        {
            object[] package = new object[] { actorSending, statToUpdate, amountToChange };

            PhotonNetwork.RaiseEvent(
               (byte)EventCode.UpdateState,
               package,
               new RaiseEventOptions { Receivers = ReceiverGroup.All },
               new SendOptions { Reliability = true }
               );
        }

        public void UpdateStatEventReceive(object[] data)
        {
            int actor = (int)data[0];
            int stat = (int)data[1];
            int amount = (int)data[2];

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].actorNumber == actor)
                {
                    switch (stat)
                    {
                        case 0:
                            players[i].kills += amount;
                            break;

                        case 1:
                            players[i].deaths += amount;
                            break;
                    }
                    if (i == index)
                    {
                        gameplayUIController.UpdateStatDisplay(players[i].kills, players[i].deaths);
                    }
                    break;
                }
            }

            ScoreCheck();
        }

        public void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void ScoreCheck()
        {
            bool winnerFound = false;
            foreach (PlayerInfo player in players)
            {
                if (player.kills >= killsToWin && killsToWin >= 0)
                {
                    winnerFound = true;
                    break;
                }
            }

            if (winnerFound)
            {
                if (PhotonNetwork.IsMasterClient && gameState != GameState.Ending)
                {
                    gameState = GameState.Ending;
                    ListPlayerEventSend();
                }
            }
        }

        private void StateCheck()
        {
            if (gameState == GameState.Ending)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.DestroyAll();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //gameplayUIController.ShowLeaderBoard(players);
            gameplayUIController.EnableGameOverScreen();
        }
    }
}

[System.Serializable]
public class PlayerInfo
{
    public string name;
    public int actorNumber;
    public int kills;
    public int deaths;

    public PlayerInfo(string _name, int _actorNum, int _kills, int _deaths)
    {
        name = _name;
        actorNumber = _actorNum;
        kills = _kills;
        deaths = _deaths;
    }
}

public enum EventCode : byte
{
    NewPlayer,
    ListPlayer,
    UpdateState
}

public enum GameState
{
    Waiting,
    Playing,
    Ending
}
