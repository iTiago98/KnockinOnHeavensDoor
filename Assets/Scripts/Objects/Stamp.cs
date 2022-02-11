using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : ClickableObject
{
    public StampType type;
    public GameObject sealImage;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public override void OnMouseClick()
    {
        base.OnMouseClick();
        MySceneManager.instance.Stamp(type);
        
        if (type == StampType.HEAVEN)
        {
            _anim.Play("SealHeaven");
        }
        else
        {
            _anim.Play("SealHell");
        }
    }

    public void ShowSeal()
    {
        var rotationZ = Random.Range(-25f, 25f);
        sealImage.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        sealImage.SetActive(true);
        MouseController.instance.SetStateStamped();
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
