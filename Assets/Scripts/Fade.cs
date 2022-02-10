using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    public void ReloadScene()
    {
        SceneManager.Instance.HideSanPedro();
        SceneManager.Instance.ReloadScene();
    }

    public void ResetPlayMode()
    {
        MouseController.Instance.SetStateSelecting();
        GetComponent<Animator>().Play("Idle");

        MenuManager.ins.canPause = true;
        // DialogueManager.ins.StartDialogue(SceneManager.Instance.GetCurrentIntroPath(), SceneManager.Instance.currentSuspect);
        StoryManager.ins.StartDialogue(SceneManager.Instance.GetCurrentIntroJson());
    }

    public void StartMusic()
    {
        SceneManager.Instance.StartMusic();
    }

    public void StopMusic()
    {
        SceneManager.Instance.StopPreviousMusic();
    }

    public void HideMainMenu()
    {
        SceneManager.Instance.HideMainMenu();
    }
}
