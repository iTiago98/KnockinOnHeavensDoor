using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public List<TextAsset> introFiles;

    private void Awake()
    {
        instance = this;
    }

}
