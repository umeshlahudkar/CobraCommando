using System.Collections;
using UnityEngine;
using TMPro;

public class RoomDetailUpdator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI srNoText;
    [SerializeField] private TextMeshProUGUI maxPlayerCountText;
    [SerializeField] private TextMeshProUGUI availablePlayerCountText;

    private Photon.Realtime.RoomInfo roomInfo;
    private LobbyUIController lobbyUIController;


    public void SetupButton(int srNo, LobbyUIController manager, Photon.Realtime.RoomInfo info)
    {
        roomInfo = info;
        lobbyUIController = manager;
        srNoText.text = srNo.ToString();
        roomNameText.text = roomInfo.Name;
        maxPlayerCountText.text = roomInfo.MaxPlayers.ToString();
        availablePlayerCountText.text = roomInfo.PlayerCount.ToString();
    }

    public void OnJoinButtonClick()
    {
        lobbyUIController.JoinRoom(roomInfo);
    }
}
