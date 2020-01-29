using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowLyrics : MonoBehaviour
{
    [SerializeField] LyricsData LyricsExcel;

    public GameObject Bigeffect;
    public GameObject Slideeffect;
    private ControllerNum PlayerControl;
    private bool showing_flag = true;
    private int showindex = 0;
    private int selectedtext = 0;
    private int resetindex = 0;
    private int totalruncount = 1;
    private DS4InputKey thiscontrol;

    void Start()
    {
        PlayerControl = MainGameManager.instance.currentplayer;
        setplayercontrol();
        StartCoroutine(presetshow());
    }
    
    void Update()
    {
        if (!showing_flag)
        {
            showing_flag = true;
            GameObject tmp= gameObject.transform.GetChild(showindex).gameObject;
            showindex++;
            StartCoroutine(shuftShow(tmp));
        }
        
    }

    private void setplayercontrol()
    {
        switch (PlayerControl)
        {
            case ControllerNum.P1:
                thiscontrol = GamePadControl.Instance.GetKeyDown_1;
                break;
            case ControllerNum.P2:
                thiscontrol = GamePadControl.Instance.GetKeyDown_2;
                break;
            default:
                Debug.Log("error");
                break;
        }
    }
    
    IEnumerator shuftShow(GameObject target)
    {
        MaskData thisMaskdata = target.GetComponent<MaskMovement>().ThisMaskData;
        
        if (thisMaskdata.OnceEffect)
         {
             ShowSpecifyEffect(thisMaskdata.EffectIndex);
         }
        
         for (int i = 0; i < thisMaskdata.RunCount; i++)
         {
            LyricsEntity tmplyrics;
            tmplyrics = LyricsExcel.tokiwa1.Find(entity => entity.id == (totalruncount+i));
            target.transform.GetChild(i).gameObject.SetActive(true);
             if (thisMaskdata.StartEffect)
             {
                 ShowSpecifyEffect(thisMaskdata.EffectIndex);
             }
             yield return new WaitForSeconds(tmplyrics.delaytime);
             if(thisMaskdata.EndEffect && i == thisMaskdata.RunCount - 1)
             {
                 ShowSpecifyEffect(thisMaskdata.EffectIndex);
                 yield return new WaitForSeconds(thisMaskdata.EffectDelay);
                 break;
             }
             if(tmplyrics.wholedelay != 0)
            {
                yield return new WaitForSeconds(tmplyrics.wholedelay);
            }
         }
         totalruncount += thisMaskdata.RunCount;
         if (showindex < gameObject.transform.childCount && !thisMaskdata.OnceEffect)showing_flag = false;
         if(!(showindex < gameObject.transform.childCount))
         {
             yield return new WaitForSeconds(2.0f);
             MainGameManager.instance.EndRun();
             Destroy(transform.parent.gameObject);
         }
        target.SetActive(false);
    }
    IEnumerator presetshow()
    {
        yield return new WaitForSeconds(4f);
        GameObject tmp = gameObject.transform.GetChild(showindex).gameObject;
        showindex++;
        StartCoroutine(shuftShow(tmp));
    }
 
    void ShowSpecifyEffect(int index)
    {
        switch (index)
        {
            case 1:
                Bigeffect.GetComponent<showbig>().Showwords();
                break;
            case 2:
                Slideeffect.SetActive(true);
                break;
            case 3:
                GameObject tmp = gameObject.transform.GetChild(showindex).gameObject;
                showindex++;
                StartCoroutine(shuftShow(tmp));
                break;
            default:
                break;
        }
    }
}
