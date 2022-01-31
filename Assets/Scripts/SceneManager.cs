using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance;

    public static SceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
            }

            return _instance;
        }
    }

    public GameObject mainMenu;
    public Button startButton;
    public InputField inputField;
    public GameObject fade;

    public List<GameObject> characters;
    public GameObject sanPedroCharacter;
    public List<string> introPaths;
    public List<GameObject> interrogationFiles;

    [HideInInspector]
    public int currentSuspect;

    public Transform fafita;

    public GameObject heavenStamp;
    public GameObject hellStamp;

    public GameObject heavenImage;
    public GameObject hellImage;

    public AudioSource heavenSound;
    public AudioSource hellSound;

    public AudioSource menuIntro;
    public AudioSource menuLoop;

    public List<AudioSource> intros;
    public List<AudioSource> loops;

    public float loopDelay;

    private void Awake()
    {
        currentSuspect = -1;
        PlayMenuMusic();
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
            characters[currentSuspect].SetActive(false);
        }

        currentSuspect++;

        interrogationFiles[currentSuspect].SetActive(true);
        characters[currentSuspect].SetActive(true);
        if (currentSuspect == 1 || currentSuspect == 3) sanPedroCharacter.SetActive(true);

        heavenStamp.GetComponent<Animator>().Play("Idle");
        hellStamp.GetComponent<Animator>().Play("Idle");

        heavenImage.SetActive(false);
        hellImage.SetActive(false);

        Color(characters[currentSuspect].GetComponent<Character>().myName);
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

    public Transform GetCharacters()
    {
        return characters[currentSuspect].transform;
    }

    public Transform GetSanPedro()
    {
        return sanPedroCharacter.transform;
    }

    public Transform GetFafita()
    {
        return fafita;
    }
    public void PlayHeavenSound()
    {
        heavenSound.Play();
    }
    public void PlayHellSound()
    {
        hellSound.Play();
    }

    public void GreyCharacters()
    {
        Transform image = null;

        foreach (GameObject c in characters)
        {
            image = c.transform.Find("Image");
            image.GetComponent<Image>().sprite = c.GetComponent<Character>().disabledImage;
        }

        Character character = sanPedroCharacter.GetComponent<Character>();
        image = character.transform.Find("Image");
        image.GetComponent<Image>().sprite = character.GetComponent<Character>().disabledImage;
    }

    public void Color(string speakerName)
    {

        if (speakerName == "San Pedro")
        {
            var character = sanPedroCharacter.GetComponent<Character>();
            var image = character.transform.Find("Image");
            image.GetComponent<Image>().sprite = character.GetComponent<Character>().happyImage;
        }

        GameObject gObject = null;
        foreach (GameObject o in characters)
        {
            if (o.GetComponent<Character>().myName == speakerName) gObject = o;
        }
        if (gObject != null)
        {
            var image = gObject.transform.Find("Image");
            image.GetComponent<Image>().sprite = gObject.GetComponent<Character>().happyImage;
        }

    }

    public void ColorAll()
    {
        Character character = null;
        Transform image = null;

        foreach (GameObject o in characters)
        {
            character = o.GetComponent<Character>();
            image = character.transform.Find("Image");
            image.GetComponent<Image>().sprite = character.GetComponent<Character>().happyImage;
        }

        character = sanPedroCharacter.GetComponent<Character>();
        image = character.transform.Find("Image");
        image.GetComponent<Image>().sprite = character.GetComponent<Character>().happyImage;
    }

    public void StopPreviousMusic()
    {
        if (currentSuspect <= 0)
        {
            menuIntro.Stop();
            menuLoop.Stop();
            CancelInvoke("PlayMenuLoop");
        }
        else
        {
            intros[currentSuspect - 1].Stop();
            loops[currentSuspect - 1].Stop();
            CancelInvoke("PlayLoop");
        }
    }

    public void StopMusic()
    {

        intros[currentSuspect].Stop();
        loops[currentSuspect].Stop();
        CancelInvoke("PlayLoop");

    }

    public void StartMusic()
    {
        if (characters[currentSuspect].GetComponent<Character>().myName == "Ramónica" || characters[currentSuspect].GetComponent<Character>().myName == "Rosa")
        {
            PlayLoop();
        }
        else
        {
            intros[currentSuspect].Play();
            Invoke("PlayLoop", intros[currentSuspect].clip.length + loopDelay);
        }
    }

    private void PlayLoop()
    {
        loops[currentSuspect].Play();
    }

    private void PlayMenuMusic()
    {
        menuIntro.Play();
        Invoke("PlayMenuLoop", menuIntro.clip.length - 2);
    }

    private void PlayMenuLoop()
    {
        menuLoop.Play();
    }

    public void ShowInputText()
    {
        startButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
    }

    public void HideSanPedro()
    {
        sanPedroCharacter.SetActive(false);
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void FadeOut()
    {
        fade.GetComponent<Animator>().Play("FadeOut");
    }

    public bool IsSanPedro()
    {
        return characters[currentSuspect].GetComponent<Character>().myName == "San Pedro";
    }

    public bool IsRosa()
    {
        return characters[currentSuspect].GetComponent<Character>().myName == "Rosa";

    }
}
