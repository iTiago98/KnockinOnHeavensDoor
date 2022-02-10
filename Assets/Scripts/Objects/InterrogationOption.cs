using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationOption : ClickableObject
{
    public string dialoguePath;
    //public int logIndex;
    public Color clickedColor;

    private bool _clicked = false;

    public override void OnMouseClick()
    {
        if (_clicked) return;
        base.OnMouseClick();

        MouseController.instance.CheckObjectUp(null);
        MouseController.instance.enable = false;
        DialogueManager.instance.StartDialogue(dialoguePath, SceneManager.instance.currentSuspect);
        GetComponent<TextMesh>().color = clickedColor;
        _clicked = true;
    }

    public override void OnMouseHoverEnter()
    {
        if(!_clicked) GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
    }

    public override void OnMouseHoverExit()
    {
        if (!_clicked) GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
    }
}
