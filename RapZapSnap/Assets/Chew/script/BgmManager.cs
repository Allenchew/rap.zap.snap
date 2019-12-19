using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance;

    public AudioClip[] BgmClip;

    private bool scalingvol = false;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator volumeio(float delaytime,bool increment)
    {
        for(int i = 0; i < 50; i++)
        {
            gameObject.GetComponent<AudioSource>().volume += increment ? 0.02f : -0.02f;
            yield return new WaitForSeconds(delaytime);
        }
        scalingvol = false;
    }
    public void StartPlay(int playindex)
    {
        if (!scalingvol)
        {
            scalingvol = true;
            gameObject.GetComponent<AudioSource>().clip = BgmClip[playindex];
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(volumeio(0.01f, true));
        }
    }
    public void StopPlay()
    {
        if (!scalingvol)
        {
            scalingvol = true;
            StartCoroutine(volumeio(0.01f, false));
        }
    }
}
