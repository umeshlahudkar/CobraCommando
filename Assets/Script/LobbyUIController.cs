using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using FPS.Multiplayer;

public class LobbyUIController : MonoBehaviour
{
    [Header("Map Name (new scene)")]
    [SerializeField] private string levelToLoad;

    [Header("Create Join Screen")]
    [SerializeField] private GameObject createJoinScreen;

    [Header("Main Menu Screen")]
    [SerializeField] private GameObject mainMenuScreen;

    [Header("User Name Login")]
    [SerializeField] private GameObject userNameLoginScreen;
    [SerializeField] private TMP_InputField userNameInputField;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingScreenText;

    [Header("Create Room Screen")]
    [SerializeField] private GameObject createRoomScreen;
    [SerializeField] private TMP_InputField roomNameInputField;

    [Header("Error Screen")]
    [SerializeField] private GameObject errorScreen;
    [SerializeField] private TextMeshProUGUI erroeScreenText;

    [Header("Room Browser Screen")]
    [SerializeField] private GameObject roomBrowserScreen;
    [SerializeField] private RoomDetailUpdator RoomInfoButtonPrefab;

    [Header("Room Screen")]
    [SerializeField] private GameObject roomScreen;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private PlayerDetailUpdator playerDetailUpdatorPrefab;

    [Header("NetWorkManager script")]
    [SerializeField] private NetworkManager networkManager;

    private List<PlayerDetailUpdator> playerNameLabels = new List<PlayerDetailUpdator>();
    private List<RoomDetailUpdator> roominfoButtons = new List<RoomDetailUpdator>();

    private void Start()
    {
        if(!mainMenuScreen.activeInHierarchy)
        {
            mainMenuScreen.SetActive(true);
        }
    }

   

    public void ToggleMainMenuScreen(bool status)
    {
        createJoinScreen.SetActive(status);
    }

    public void ToggleStartGameButton(bool status)
    {
        startGameButton.gameObject.SetActive(status);
    }

    public void DisableLoadingScreen()
    {
        loadingScreen.SetActive(false);
        createJoinScreen.SetActive(true);
    }

    private void OpenLoadingScreen(string msg)
    {
        //CloseAllScreen();
        loadingScreenText.text = msg;
        loadingScreen.SetActive(true);
    }

    public void EnableErrorScreen(string msg)
    {
        erroeScreenText.text = "Failed to create room : " + msg;
        errorScreen.SetActive(true);
    }

    public void EnableRoomScreen(string roomName)
    {
        roomScreen.SetActive(true);
        roomNameText.text = "Room Name : " + roomName;
        UpdateRoomScreen();
    }

    public void UpdateRoomScreen()
    {
        foreach (PlayerDetailUpdator playerLabel in playerNameLabels)
        {
            Destroy(playerLabel.gameObject);
        }
        playerNameLabels.Clear();

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            AddPlayerNameLabels(i + 1, players[i].NickName);
        }
    }

    public void AddPlayerNameLabels(int numb, string name)
    {
        PlayerDetailUpdator nameText = Instantiate<PlayerDetailUpdator>(playerDetailUpdatorPrefab, playerDetailUpdatorPrefab.transform.parent);
        nameText.SetupDetails(numb, name);
        nameText.gameObject.SetActive(true);

        playerNameLabels.Add(nameText);
    }

    public void UpdateRoomBrowserScreen(List<RoomInfo> roomList)
    {
        foreach (RoomDetailUpdator button in roominfoButtons)
        {
            Destroy(button.gameObject);
        }
        roominfoButtons.Clear();
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount < roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomDetailUpdator button = Instantiate<RoomDetailUpdator>(RoomInfoButtonPrefab, RoomInfoButtonPrefab.transform.parent);
                button.SetupButton(i+1, this, roomList[i]);
                button.gameObject.SetActive(true);

                roominfoButtons.Add(button);
            }
        }
    }

    public void CreateRoom()
    {
        string roomeName = roomNameInputField.text;
        if (!string.IsNullOrEmpty(roomeName))
        {
            networkManager.CreateRoom(roomeName);
            DisableAllScreen();
            OpenLoadingScreen("Creating room...");
        }
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        networkManager.JoinRoom(roomInfo);
        DisableAllScreen();
        OpenLoadingScreen("Joining room...");
    }

    public void OnLoginButtonClick()
    {
        string playerName = userNameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            networkManager.SetNickName(playerName);
            DisableAllScreen();
            loadingScreen.SetActive(true);
            AudioManager.Instance.PlayButtonClickSound();
        }
    }

    public void OnCreateRoomButtonClick()
    {
        DisableAllScreen();
        createRoomScreen.SetActive(true);
        AudioManager.Instance.PlayButtonClickSound();
    }

    public void OnJoinRoomButtonClick()
    {
        DisableAllScreen();
        roomBrowserScreen.SetActive(true);
        AudioManager.Instance.PlayButtonClickSound();
    }

    public void OnLeaveButtonClick()
    {
        networkManager.LeaveRoom();
        DisableAllScreen();
        OpenLoadingScreen("Leaving Room...");
        AudioManager.Instance.PlayButtonClickSound();
    }

    public void OnStartGameButtonClick()
    {
        AudioManager.Instance.PlayButtonClickSound();
        networkManager.StartGame(levelToLoad);
    }

    public void OnQuitButtonClick()
    {
        AudioManager.Instance.PlayButtonClickSound();
        Application.Quit();
    }

    public void OnSingleModeButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnMultiplayerModeButtonClick()
    {
        userNameLoginScreen.SetActive(true);
    }

    public void DisableAllScreen()
    {
        //mainMenuScreen.SetActive(false);
        userNameLoginScreen.SetActive(false);
        loadingScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
        roomScreen.SetActive(false);
    }
}
