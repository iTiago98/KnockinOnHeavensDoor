using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : ClickableObject
{
    public string myName;

    public Sprite defaultImage;
    public Sprite disabledImage;

    public Color highlightColor;
    public Image faceImage;

    public override void OnMouseClick()
    {
        base.OnMouseClick();
        if (transform.name == "San Pedro Side")
        {
            SceneManager.instance.StopMusic();
            SceneManager.instance.FadeOut();
        }
        else
        {
            MouseController.instance.SetStatePlaying();
        }
    }

    public override void OnMouseHoverEnter()
    {
        ShowText();
        faceImage.color = highlightColor;
    }

    public override void OnMouseHoverExit()
    {
        HideText();
        faceImage.color = Color.white;
    }
}
