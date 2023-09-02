using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

namespace FPS.Multiplayer
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {

        [SerializeField] private LobbyUIController lobbyUIController;
        private string nickName;

        public void SetNickName(string name)
        {
            nickName = name;
            SetUpPhoton();
        }

        private void SetUpPhoton()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.NickName = nickName;
            lobbyUIController.DisableLoadingScreen();
        }

        public override void OnJoinedRoom()
        {
            lobbyUIController.DisableAllScreen();
            lobbyUIController.EnableRoomScreen(PhotonNetwork.CurrentRoom.Name);
            if (PhotonNetwork.IsMasterClient)
            {
                lobbyUIController.ToggleStartGameButton(true);
            }
            else
            {
                lobbyUIController.ToggleStartGameButton(false);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            lobbyUIController.EnableErrorScreen(message);
        }

        public override void OnLeftRoom()
        {
            lobbyUIController.DisableAllScreen();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            lobbyUIController.UpdateRoomBrowserScreen(roomList);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            lobbyUIController.AddPlayerNameLabels(0, newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            lobbyUIController.UpdateRoomScreen();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                lobbyUIController.ToggleStartGameButton(true);
            }
            else
            {
                lobbyUIController.ToggleStartGameButton(false);
            }
        }

        public void CreateRoom(string roomName)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 8;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public void JoinRoom(RoomInfo roomName)
        {
            PhotonNetwork.JoinRoom(roomName.Name);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void StartGame(string levelToLoad)
        {
            PhotonNetwork.LoadLevel(levelToLoad);
        }
    }
}
