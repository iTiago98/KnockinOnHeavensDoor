using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    public GameObject mainMenu;
    public Button startButton;
    public InputField inputField;
    public GameObject fade;
   
    public List<string> introPaths;
    public List<GameObject> interrogationFiles;

    [HideInInspector]
    public int currentSuspect;

    public Transform fafita;

    public GameObject heavenStamp;
    public GameObject hellStamp;

    public GameObject heavenImage;
    public GameObject hellImage;

    private void Awake()
    {
        instance = this;
        currentSuspect = -1;
        MusicManager.instance.PlayMenuSong();
    }

    public void LoadScene()
    {
        GameManager.playerName = inputField.text;
        inputField.gameObject.SetActive(false);
        fade.GetComponent<Animator>().Play("FadeOut");
    }

    public void ReloadScene()
    {

        if (currentSuspect >= 0)
        {
            interrogationFiles[currentSuspect].SetActive(false);
        }

        currentSuspect++;
        CharacterManager.instance.UpdateCharacters(currentSuspect);

        interrogationFiles[currentSuspect].SetActive(true);

        ResetStamps();

        //Color(characters[currentSuspect].GetComponent<Character>().myName);
    }

    public ClickableObject GetCurrentFile()
    {
        return interrogationFiles[currentSuspect].GetComponent<ClickableObject>();
    }

    public string GetCurrentIntroPath()
    {
        return introPaths[currentSuspect];
    }

    public Transform GetInterrogationOptions()
    {
        return interrogationFiles[currentSuspect].transform.Find("Options").transform;
    }

    private void ResetStamps()
    {
        heavenStamp.GetComponent<Animator>().Play("Idle");
        hellStamp.GetComponent<Animator>().Play("Idle");

        heavenImage.SetActive(false);
        hellImage.SetActive(false);
    }


    //public Transform GetCharacters()
    //{
    //    return characters[currentSuspect].transform;
    //}

    //public Transform GetSanPedro()
    //{
    //    return sanPedroCharacter.transform;
    //}

    public Transform GetFafita()
    {
        return fafita;
    }
    public void PlayHeavenSound()
    {
        MusicManager.instance.PlayStampSound(StampType.HEAVEN);
    }
    public void PlayHellSound()
    {
        MusicManager.instance.PlayStampSound(StampType.HELL);
    }

    //public void GreyCharacters()
    //{
    //    Transform image = null;

    //    foreach (GameObject c in characters)
    //    {
    //        image = c.transform.Find("Image");
    //        image.GetComponent<Image>().sprite = c.GetComponent<Character>().disabledImage;
    //    }

    //    Character character = sanPedroCharacter.GetComponent<Character>();
    //    image = character.transform.Find("Image");
    //    image.GetComponent<Image>().sprite = character.GetComponent<Character>().disabledImage;
    //}

    //public void Color(string speakerName)
    //{

    //    if (speakerName == "San Pedro")
    //    {
    //        var character = sanPedroCharacter.GetComponent<Character>();
    //        var image = character.transform.Find("Image");
    //        image.GetComponent<Image>().sprite = character.GetComponent<Character>().happyImage;
    //    }

    //    GameObject gObject = null;
    //    foreach (GameObject o in characters)
    //    {
    //        if (o.GetComponent<Character>().myName == speakerName) gObject = o;
    //    }
    //    if (gObject != null)
    //    {
    //        var image = gObject.transform.Find("Image");
    //        image.GetComponent<Image>().sprite = gObject.GetComponent<Character>().talkingImage;
    //    }

    //}

    //public void ColorAll()
    //{
    //    Character character = null;
    //    Transform image = null;

    //    foreach (GameObject o in characters)
    //    {
    //        character = o.GetComponent<Character>();
    //        image = character.transform.Find("Image");
    //        image.GetComponent<Image>().sprite = character.GetComponent<Character>().talkingImage;
    //    }

    //    character = sanPedroCharacter.GetComponent<Character>();
    //    image = character.transform.Find("Image");
    //    image.GetComponent<Image>().sprite = character.GetComponent<Character>().talkingImage;
    //}

    public void ResetPlayMode()
    {
        MouseController.instance.SetStateSelecting();
        fade.GetComponent<Animator>().Play("Idle");
        MenuManager.instance.canPause = true;
        DialogueManager.instance.StartDialogue(GetCurrentIntroPath(), currentSuspect);
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
        MenuManager.instance.canPause = false;
        MouseController.instance.SetStateStamping();
        StopMusic();
        MusicManager.instance.PlayStampSound(type);
    }

    public void ShowInputText()
    {
        startButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void FadeOut()
    {
        fade.GetComponent<Animator>().Play("FadeOut");
    }

    public bool IsLastCharacter()
    {
        return CharacterManager.instance.IsLastCharacter(currentSuspect);
    }

    public bool IsSanPedro()
    {
        return CharacterManager.instance.IsSanPedro(currentSuspect);
    }

    //public bool IsRosa()
    //{
    //    return characters[currentSuspect].GetComponent<Character>().myName == "Rosa";

    //}
}
