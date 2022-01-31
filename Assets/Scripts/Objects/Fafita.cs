using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fafita : ClickableObject
{
    private int _position = 0;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public override void OnMouseClick()
    {
        if (_position == 0)
        {
            _anim.Play("CloseUp");
            _position = 1;
        }
        else
        {
            _anim.Play("PutDown");
            _position = 0;
        }
    }

    public override void OnMouseHoverEnter()
    {
    }

    public override void OnMouseHoverExit()
    {
    }
}
