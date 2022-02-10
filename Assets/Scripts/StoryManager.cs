using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class StoryManager : MonoBehaviour
{
    public GameObject dialogues;
    public GameObject optionsPanel;

    public GameObject nextButton;
    public KeyCode skipDialogueKey;
    public float characterDelay;

    public Text speakerText;
    public Text dialogueText;
    public List<Text> optionsTexts;

    public SanPedroFile sanPedroFile;

    private Story _story;
    private string _bufferedText;
    //private bool _typing;

    private const string PLACEHOLDER_NAME = "Player";
    private const char SPEAKER_NAME_SEPARATOR = ':';
    private const int CHARACTER_LIMIT = 120;


    public void StartDialogue(TextAsset jsonFile)
    {
        _story = new Story(jsonFile.text);
        dialogues.SetActive(true);
        DisplayNextSentence();
    }

    public void SelectOption(int index)
    {
        _story.ChooseChoiceIndex(index);
    }

    public void OnNextButtonClicked()
    {
        nextButton.SetActive(false);
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (_story.canContinue)
        {
            string line = (_bufferedText.Length > 0) ? _bufferedText : _story.Continue();

            SetSpeakerName(line);

            if (line.Length > CHARACTER_LIMIT)
            {
                line = GetTrimmedLine(line);
            }
            else
            {
                _bufferedText = "";
            }

            dialogueText.text = line;
        }
        else if (_story.currentChoices.Count > 0)
        {
            optionsPanel.SetActive(true);

            for (int i = 0; i < optionsTexts.Count; i++)
            {
                GameObject button = optionsTexts[i].transform.parent.gameObject;

                bool showText = i < _story.currentChoices.Count;
                button.SetActive(showText);

                if (showText)
                {
                    optionsTexts[i].text = _story.currentChoices[i].text;
                }
            }
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogues.SetActive(false);
    }


    private void SetSpeakerName(string line)
    {
        int separatorIndex = line.IndexOf(SPEAKER_NAME_SEPARATOR);
        var speakerName = line.Substring(0, separatorIndex);

        speakerText.text = (speakerName == PLACEHOLDER_NAME) ? GameManager.playerName : speakerName;
    }

    private string GetTrimmedLine(string line)
    {
        string aux = line.Substring(0, CHARACTER_LIMIT);
        int separatorIndex = aux.LastIndexOf('.');
        _bufferedText = line.Substring(separatorIndex, line.Length - 1);
        return line.Substring(0, separatorIndex);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _typing = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(characterDelay);
        }

        EndSentence();
    }

    private void EndSentence()
    {
        if (_story.currentChoices.Count > 0)
        {
            DisplayNextSentence();
        }
        else
        {
            nextButton.SetActive(true);
        }
        _typing = false;
    }
}
