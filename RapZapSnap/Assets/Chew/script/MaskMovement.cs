using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MaskData
{
    public float[] delayshowtime;
    public int commapos;
    public Vector3 steptoward;
    public Vector3 commastep;
    public void Set(float[] tmpdelay,int tmppos,Vector3 tmpstep,Vector3 tmpcomma)
    {
        delayshowtime = tmpdelay;
        commapos = tmppos;
        steptoward = tmpstep;
        commastep = tmpcomma;
    }
}
public class MaskMovement : MonoBehaviour
{
    public MaskData ThisMaskData;

    public float[] delayshowtime;
    public int commapos;
    public Vector3 steptoward;
    public Vector3 commastep;
    void Start()
    {
        ThisMaskData.Set(delayshowtime, commapos, steptoward,commastep);
    }
}
