using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public SanPedroFile sanPedroFile;
    public GameObject dialogueBox;
    public float characterDelay;
    public Text speaker;
    public GameObject npcPanel;
    public Text npcText;
    public KeyCode skipDialogueKey;
    public GameObject nextButton;
    public GameObject pcPanel;
    public List<Text> pcTexts;
    [HideInInspector]
    public List<string> log;

    private string _dialoguePath;

    private Dialogue _dialogueTree;
    private List<string> _option;
    private bool _typing;
    private bool _canSkip;
    private int _logIndex;

    void Awake()
    {
        instance = this;
        for (int i = 0; i < 5; i++) log.Add("");
    }

    void Update()
    {
        if (Input.GetKeyDown(skipDialogueKey) && _canSkip) SkipTypingAnimation();
    }

    private void DisplayNextSentence()
    {
        if (_dialogueTree.isPC)
        {
            pcPanel.SetActive(true);
            _option = _dialogueTree.getTextPC();
            for (int i = 0; i < pcTexts.Count; i++)
            {
                if (i < _option.Count)
                {
                    pcTexts[i].transform.parent.gameObject.SetActive(true);
                    pcTexts[i].text = _option[i];
                }
                else pcTexts[i].transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            nextButton.SetActive(false);
            pcPanel.SetActive(false);
            speaker.text = _dialogueTree.speaker == GameManager.placeholderName ? GameManager.playerName : _dialogueTree.speaker;
            SetCharacterSpeaking(speaker.text);
            StopAllCoroutines();
            StartCoroutine(TypeSentence(_dialogueTree.getTextNPC()));

            
        }
        if (_dialogueTree.isWarp())
        {
            sanPedroFile.SetCensorUnlocked(_dialogueTree.warpIndex);
            Debug.Log(_dialogueTree.warpIndex);
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _typing = true;
        npcText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            npcText.text += letter;
            yield return new WaitForSecondsRealtime(characterDelay);
        }

        EndNPCSentence();
    }

    private void EndNPCSentence()
    {
        if (!_dialogueTree.isEmpty)
        {
            if (_dialogueTree.nextIsBranched)
            {
                SelectOption(0);
            } else
            {
                nextButton.SetActive(true);
            }
        }
        else nextButton.SetActive(false);
        _canSkip = false;
        _typing = false;
    }

    public void StartDialogue(string dialoguePath, int logIndex)
    {
        this._logIndex = logIndex;
        _dialoguePath = dialoguePath;
        _dialogueTree = new Dialogue(System.IO.File.ReadAllText(@"Assets\Dialogues\" + dialoguePath + ".diag", System.Text.Encoding.UTF8));
        dialogueBox.SetActive(true);
        nextButton.SetActive(false);
        _canSkip = true;
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        log[_logIndex] += "\n----------------------------\n\n";
        dialogueBox.SetActive(false);
        MouseController.instance.enable = true;
        CharacterManager.instance.AllCharactersDefault();
        if (_dialoguePath.Contains("Intro") && SceneManager.instance.currentSuspect != 3) CharacterManager.instance.ShowSanPedro(false);
    }

    public void SelectOption(int i)
    {
        _dialogueTree.selectOption(i);
        log[_logIndex] += _dialogueTree.current + "\n\n";
        if (!_dialogueTree.isEmpty) DisplayNextSentence();
        else EndDialogue();
    }

    private void SetCharacterSpeaking(string speakerName)
    {
        CharacterManager.instance.UpdateCharactersSprites(speakerName);
    }

    public void SkipTypingAnimation()
    {
        if(_typing)
        {
            StopAllCoroutines();
            npcText.text = _dialogueTree.getTextNPC();
            EndNPCSentence();
        }
    }

    public void OnNextButtonClicked()
    {
        SelectOption(0);
        _canSkip = true;
    }
}
