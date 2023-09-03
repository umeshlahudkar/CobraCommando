using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : Singleton<InputController>
{
    private EventSystem eventSystem;
    private bool hasInitialise = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        eventSystem = EventSystem.current;
        hasInitialise = true;
    }

    public void DisableInput()
    {
        if(!hasInitialise) { Initialize(); }
        eventSystem.enabled = false;
    } 

    public void EnableInput()
    {
        if (!hasInitialise) { Initialize(); }
        eventSystem.enabled = true;
    }

    public void DisableInputForDuration(float duration = 2.0f)
    {
        DisableInput();
        StartCoroutine(EnableInput(duration));
    }

    public void EnableInputForDuration(float duration = 2.0f)
    {
        EnableInput();
        StartCoroutine(DisableInput(duration));
    }

    private IEnumerator EnableInput(float time)
    {
        yield return new WaitForSeconds(time);
        EnableInput();
    }

    private IEnumerator DisableInput(float time)
    {
        yield return new WaitForSeconds(time);
        DisableInput();
    }
}
