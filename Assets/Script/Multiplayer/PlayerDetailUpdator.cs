using UnityEngine;
using TMPro;

public class PlayerDetailUpdator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI srNoText;
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void SetupDetails(int number, string playerName)
    {
        srNoText.text = number.ToString();
        playerNameText.text = playerName;
    }
}
