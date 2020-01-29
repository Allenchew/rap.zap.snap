using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaskMovement : MonoBehaviour
{
    public MaskData ThisMaskData;

    public bool StartEffect = false;
    public bool EndEffect = false;
    public bool OnceEffect = false;
    public float EffectDelay = 1.0f;
    public int EffectIndex =0;

    private int runcount = 0;
    void Start()
    {
        runcount = gameObject.transform.childCount;
        ThisMaskData.Set(runcount,StartEffect,EndEffect,EffectIndex,OnceEffect,EffectDelay);
    }
}
