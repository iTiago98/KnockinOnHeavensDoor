using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : ClickableObject
{
    public string myName;
    public Sprite happyImage;
    public Sprite disabledImage;
    public Color highlightColor;
    public Image faceImage;

    public override void OnMouseClick()
    {
        if (transform.name == "San Pedro Side")
        {
            SceneManager.Instance.StopMusic();
            SceneManager.Instance.FadeOut();
        }
        else
        {
            if (myName == "Rosa") SceneManager.Instance.HideSanPedro();
            MouseController.Instance.SetStatePlaying();
        }
    }

    public override void OnMouseHoverEnter()
    {
        ShowText();
        faceImage.color = highlightColor;
        //var material = GetComponent<Renderer>().material;
        //material.EnableKeyword("_EMISSION");
    }

    public override void OnMouseHoverExit()
    {
        HideText();
        faceImage.color = Color.white;
        //var material = GetComponent<Renderer>().material;
        //material.DisableKeyword("_EMISSION");
    }
}
