using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showbig : MonoBehaviour
{
    [Tooltip("Show how many words then run once")]
    public bool SkipOnce = false;
    public int Exceptionindex = 0;

    private bool runonce = false;
    private int index = 0;
    public void Showwords()
    {
        if(SkipOnce && runonce )
        {
            runonce = !runonce;
            return;
        }
        gameObject.transform.GetChild(index).gameObject.SetActive(true);
        if(index != Exceptionindex)runonce = !runonce;
        index++;
    }
}
