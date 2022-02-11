using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager instance;
   
    public List<GameObject> interrogationFiles;

    [HideInInspector]
    public int currentSuspect;

    public Transform fafita;

    public GameObject heavenStamp;
    public GameObject hellStamp;

    public GameObject heavenImage;
    public GameObject hellImage;
    public GameObject fadeImage;
    public GameObject endImage;

    private void Awake()
    {
        instance = this;
        currentSuspect = -1;
    }

    private void Start()
    {
        MusicManager.instance.PlayMenuSong();
    }

    public void LoadScene()
    {
        MainMenuManager.instance.LoadScene();
        PlayFadeAnimation();
    }

    public void ReloadScene()
    {
        if (currentSuspect >= 0)
        {
            interrogationFiles[currentSuspect].SetActive(false);
        }

        currentSuspect++;
        CharacterManager.instance.UpdateCharacters(currentSuspect);
        CharacterManager.instance.AllCharactersDisabled();

        interrogationFiles[currentSuspect].SetActive(true);

        ResetStamps();
    }

    public ClickableObject GetCurrentFile()
    {
        return interrogationFiles[currentSuspect].GetComponent<ClickableObject>();
    }

    public TextAsset GetCurrentIntroPath()
    {
        return DialogueManager.instance.introFiles[currentSuspect];
    }

    private void ResetStamps()
    {
        heavenStamp.GetComponent<Animator>().Play("Idle");
        hellStamp.GetComponent<Animator>().Play("Idle");

        heavenImage.SetActive(false);
        hellImage.SetActive(false);
    }

    public void ResetPlayMode()
    {
        MouseController.instance.SetState(GameState.SELECTING);
        fadeImage.GetComponent<Animator>().Play("Idle");
        PauseMenuManager.instance.canPause = true;
        StoryManager.ins.StartDialogue(GetCurrentIntroPath());
    }

    public void StartMusic()
    {
        MusicManager.instance.PlayCharacterSong(currentSuspect);
    }

    public void StopMusic()
    {
        MusicManager.instance.StopMusic(currentSuspect);
    }

    public void Stamp(StampType type)
    {
        PauseMenuManager.instance.canPause = false;
        MouseController.instance.SetState(GameState.STAMPING);
        StopMusic();
        MusicManager.instance.PlayStampSound(type);
    }

    public void HideMainMenu()
    {
        MainMenuManager.instance.Hide();
    }

    public void PlayFadeAnimation()
    {
        fadeImage.GetComponent<Animator>().Play("FadeOut");
    }

    public void PlayEndAnimation()
    {
        endImage.GetComponent<Animator>().Play("End");
    }

    public bool IsLastCharacter()
    {
        return CharacterManager.instance.IsLastCharacter(currentSuspect);
    }

    public bool IsSanPedro()
    {
        return CharacterManager.instance.IsSanPedro(currentSuspect);
    }

}
