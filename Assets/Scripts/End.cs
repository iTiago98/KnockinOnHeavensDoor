using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public void EnablePauseMenu()
    {
        MenuManager.ins.canPause = true;
    }
}
