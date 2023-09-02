using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardPlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI srNoText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI deathsText;

    public void SetupInfo(int srNo, string playerName, int kills, int deaths)
    {
        srNoText.text = srNo.ToString();
        playerNameText.text = playerName;
        killsText.text = kills.ToString();
        deathsText.text = deaths.ToString();
    }

}
