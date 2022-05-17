using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public static GameManagerController _instance;

    private PlayerInputAction playerInput;
    private static bool isPaused = false;
    public GameObject pauseScreen;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        playerInput = new PlayerInputAction();
    }

    void Start()
    {
        playerInput.Menu.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        //-->pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseScreen.SetActive(false);
    }

    public static bool IsPaused()
    {
        return isPaused;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MetalSlug_01");
        ResumeGame();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
