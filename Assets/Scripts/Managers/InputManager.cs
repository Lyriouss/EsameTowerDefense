using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey;

    public static event Action OnGamePaused;

    private void Update()
    {
        //Calls PauseGame function in GameManager when pauseKey is pressed
        if (!Input.GetKeyDown(pauseKey))
            return;

        OnGamePaused?.Invoke();
    }
}
