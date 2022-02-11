using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanPedroFile : MonoBehaviour
{
    public List<GameObject> censorImages;
    public int numCensorsUnlocked;


    public void SetCensorUnlocked(int i)
    {
        censorImages[i].SetActive(false);
        numCensorsUnlocked++;

        if(numCensorsUnlocked > 3)
        {
            MouseController.instance.sanPedroJudge = true;
        }
    }

}
