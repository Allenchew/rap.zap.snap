using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaskMovement : MonoBehaviour
{
    public MaskData ThisMaskData;

    public bool StartEffect = false;
    public bool EndEffect = false;
    public bool IsOption = false;
    public bool showoption = false;
    public bool OnceEffect = false;
    public float EffectDelay = 1.0f;
    public int EffectIndex =0;
    public int OptionCount = 0;
    
    public float[] delayshowtime;
    void Start()
    {
        ThisMaskData.Set(delayshowtime,StartEffect,EndEffect,IsOption,EffectIndex,OptionCount,showoption,OnceEffect,EffectDelay);
    }
}
