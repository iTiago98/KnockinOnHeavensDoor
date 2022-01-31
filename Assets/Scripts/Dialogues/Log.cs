using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (string s in DialogueManager.ins.log)
                Debug.Log(s);
        }
    }
}
