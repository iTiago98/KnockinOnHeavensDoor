using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    public void ReloadScene()
    {
        SceneManager.instance.ReloadScene();
    }

    public void ResetPlayMode()
    {
        SceneManager.instance.ResetPlayMode();
    }

    public void StartMusic()
    {
        SceneManager.instance.StartMusic();
    }

    public void StopMusic()
    {
        SceneManager.instance.StopMusic();
    }

    public void HideMainMenu()
    {
        SceneManager.instance.HideMainMenu();
    }
}
