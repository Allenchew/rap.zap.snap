using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PvStorage : MonoBehaviour
{
    public VideoClip[] CurrentMv = new VideoClip[3];
    public void SetThisVideo(int index)
    {
        gameObject.GetComponent<VideoPlayer>().clip = CurrentMv[index];
        gameObject.GetComponent<VideoPlayer>().Play();
    }
    
}
