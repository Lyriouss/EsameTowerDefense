using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey;

    public static event Action OnGamePaused;

    private void Update()
    {
        if (!Input.GetKeyDown(pauseKey))
            return;

        OnGamePaused?.Invoke();
    }
}
