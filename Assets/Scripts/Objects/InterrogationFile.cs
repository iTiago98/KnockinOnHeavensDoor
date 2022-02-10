using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationFile : ClickableObject
{
    public Transform options;
    private List<GameObject> _optionsList;
    private int _position = 0;

    private Animator _anim;

    private void Start()
    {
        _optionsList = new List<GameObject>();
        foreach (Transform transform in options)
        {
            _optionsList.Add(transform.gameObject);
        }
        EnableOptions(false);

        _anim = GetComponent<Animator>();
    }

    public override void OnMouseClick()
    {
        base.OnMouseClick();
        switch (_position)
        {
            case 0:
                FileUp();
                break;
            case 1:
                FileDown();
                break;
        }
    }

    public void FileUp()
    {
        _anim.Play("CloseUp");
        EnableOptions(true);
        _position = 1;
    }

    public void FileDown()
    {
        _anim.Play("PutDown");
        EnableOptions(false);
        _position = 0;
    }

    private void EnableOptions(bool enable)
    {
        foreach (GameObject option in _optionsList)
        {
            option.GetComponent<Collider>().enabled = enable;
        }
    }

    public override void OnMouseHoverEnter()
    {
        ShowText();
    }
    
    public override void OnMouseHoverExit()
    {
        HideText();
    }
}
