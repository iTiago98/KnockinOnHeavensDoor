using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ClickableObject : MonoBehaviour
{
    public GameObject Text;

    virtual public void OnMouseClick()
    {
        OnMouseHoverExit();
    }
    abstract public void OnMouseHoverEnter();
    abstract public void OnMouseHoverExit();

    public void ShowText()
    {
        Text.SetActive(true);
    }

    public void HideText()
    {
        Text.SetActive(false);
    }
}
