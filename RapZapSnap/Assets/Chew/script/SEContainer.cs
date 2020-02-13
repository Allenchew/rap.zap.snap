using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEContainer : MonoBehaviour
{
    public AudioClip[] SElist = new AudioClip[2];

    public void SetThisSe(int index)
    {
        gameObject.GetComponent<AudioSource>().clip = SElist[index];
        gameObject.GetComponent<AudioSource>().Play();
    }
}
