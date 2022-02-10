using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public void ShowPauseMenu()
    {
        MenuManager.instance.canPause = true;
        MenuManager.instance.ShowFinalMenu();
    }
}
