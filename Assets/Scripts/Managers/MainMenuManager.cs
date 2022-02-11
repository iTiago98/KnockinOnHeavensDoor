using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject mainMenu;
    public Button startButton;
    public InputField inputField;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene()
    {
        GameManager.playerName = inputField.text;
        inputField.gameObject.SetActive(false);
    }

    public void ShowInputText()
    {
        startButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.Select();
    }

    public void Hide()
    {
        mainMenu.SetActive(false);
    }
}
