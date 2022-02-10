using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : ClickableObject
{
    public enum StampType
    {
        HEAVEN, HELL
    }

    public StampType type;
    public GameObject sealImage;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public override void OnMouseClick()
    {
        MenuManager.ins.canPause = false;
        MouseController.Instance.SetStateStamping();
        SceneManager.Instance.StopMusic();
        if (type == StampType.HEAVEN)
        {
            SceneManager.Instance.PlayHeavenSound();
            _anim.Play("SealHeaven");
        }
        else
        {
            SceneManager.Instance.PlayHellSound();
            _anim.Play("SealHell");
        }
    }


    public void ShowSeal()
    {
        var rotationZ = Random.Range(-25f, 25f);
        sealImage.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        sealImage.SetActive(true);
        MouseController.Instance.SetStateStamped();
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
