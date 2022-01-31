using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager ins;

    public KeyCode pauseKey;
    public GameObject pauseScreen;
    public UnityEvent onPauseGame;
    [HideInInspector]
    public bool canPause;

    public bool isPaused => pauseScreen.activeInHierarchy;

    private float _godProbability;
    private float _devilProbability;

    void Awake()
    {
        ins = this;
    }

    private void Update()
    {
        if(canPause && Input.GetKeyDown(pauseKey))
        {
            if (!isPaused) onPauseGame.Invoke();
            pauseScreen.SetActive(!isPaused);
        }
    }

    private void CheckGodCameo()
    {

    }

    private void CheckDevilCameo()
    {

    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
