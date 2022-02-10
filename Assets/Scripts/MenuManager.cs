using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public KeyCode pauseKey;
    public GameObject pauseScreen;
    public GameObject returnButton;
    public GameObject restartButton;

    public UnityEvent onPauseGame;
    [HideInInspector]
    public bool canPause;

    public bool isPaused => pauseScreen.activeInHierarchy;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(canPause && Input.GetKeyDown(pauseKey))
        {
            if (!isPaused) onPauseGame.Invoke();
            pauseScreen.SetActive(!isPaused);
        }
    }

    public void ShowFinalMenu()
    {
        pauseScreen.SetActive(true);
        returnButton.SetActive(false);
        restartButton.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
