using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class StoryManager : MonoBehaviour
{
    public static StoryManager ins;

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
    private string _bufferedText = "";
    private bool _typing;
    private string _line;

    private const string PLACEHOLDER_NAME = "Player";
    private const char SPEAKER_NAME_SEPARATOR = ':';
    private const int CHARACTER_LIMIT = 200;

    void Awake()
    {
        ins = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(skipDialogueKey) && _typing) SkipDialogue();
    }

    public void StartDialogue(TextAsset jsonFile)
    {
        _story = new Story(jsonFile.text);
        dialogues.SetActive(true);
        DisplayNextSentence();
    }

    public void SelectOption(int index)
    {
        _story.ChooseChoiceIndex(index);
        _story.Continue();
        optionsPanel.SetActive(false);
        DisplayNextSentence();
    }

    public void OnNextButtonClicked()
    {
        nextButton.SetActive(false);
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (_story.canContinue || _bufferedText.Length > 0)
        {
            if(_bufferedText.Length > 0)
                _line = _bufferedText;
            else
            {
                _line = _story.Continue();
                _line = SeparateSpeakerName(_line);
            }

            // _line = (_bufferedText.Length > 0) ? _bufferedText : _story.Continue();
            
            // if(_bufferedText.Length == 0) _line = SeparateSpeakerName(_line);

            if (_line.Length > CHARACTER_LIMIT)
            {
                _line = GetTrimmedLine(_line);
            }
            else
            {
                _bufferedText = "";
            }

            StopAllCoroutines();
            StartCoroutine(TypeSentence(_line));
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


    private string SeparateSpeakerName(string line)
    {
        int separatorIndex = line.IndexOf(SPEAKER_NAME_SEPARATOR);
        var speakerName = line.Substring(0, separatorIndex);

        speakerText.text = (speakerName == PLACEHOLDER_NAME) ? GameManager.playerName : speakerName;
        
        return line.Substring(separatorIndex+1, line.Length-separatorIndex-1);
    }

    private string GetTrimmedLine(string line)
    {
        string aux = line.Substring(0, CHARACTER_LIMIT);
        int separatorIndex = Mathf.Max(aux.LastIndexOf('.'), aux.LastIndexOf(','), aux.LastIndexOf('?'), aux.LastIndexOf('!'), aux.LastIndexOf(';')) + 1;
        _bufferedText = line.Substring(separatorIndex, line.Length - separatorIndex);
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

    private void SkipDialogue()
    {
        StopAllCoroutines();
        dialogueText.text = _line;
        _typing = false;
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
