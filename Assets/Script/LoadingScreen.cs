using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;

    private float elapsedTime = 0f;
    private float animTime = 5.0f;
    private float animElapcedTime = 0;
    private float textChangeTime = 0.5f;
    private string[] loadingPhases = { "Loading.", "Loading..", "Loading..." };
    private int currentPhaseIndex = 0;

    private void OnEnable()
    {
        animElapcedTime = 0;
        elapsedTime = 0;
        currentPhaseIndex = 0;
    }

    public IEnumerator PlayLoadingAnimation()
    {
        while (animElapcedTime <= animTime)
        {
            elapsedTime += Time.deltaTime;
            animElapcedTime += Time.deltaTime;

            if (elapsedTime >= textChangeTime)
            {
                currentPhaseIndex = (currentPhaseIndex + 1) % loadingPhases.Length;
                loadingText.text = loadingPhases[currentPhaseIndex];
                elapsedTime = 0f;
            }

            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
