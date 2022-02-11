using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    public void ReloadScene()
    {
        MySceneManager.instance.ReloadScene();
    }

    public void ResetPlayMode()
    {
        MySceneManager.instance.ResetPlayMode();
    }

    public void StartMusic()
    {
        MySceneManager.instance.StartMusic();
    }

    public void StopMusic()
    {
        MySceneManager.instance.StopMusic();
    }

    public void HideMainMenu()
    {
        MySceneManager.instance.HideMainMenu();
    }
}
