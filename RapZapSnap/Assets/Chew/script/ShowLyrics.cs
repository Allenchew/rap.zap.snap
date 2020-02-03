using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowLyrics : MonoBehaviour
{
    [SerializeField] LyricsData LyricsExcel;

    public GameObject Bigeffect;
    public GameObject Slideeffect;
    public float StartDelay = 0f;
    private ControllerNum PlayerControl;
    private bool showing_flag = true;
    private int showindex = 0;
    private int totalruncount = 1;
    private Character currentcharacter;
    int[] localsequal = new int[2];

    private DS4InputKey thiscontrol;

    void Start()
    {
        localsequal = MainGameManager.instance.character_sequal;
        PlayerControl = MainGameManager.instance.currentplayer;
        currentcharacter = GameData.Instance.GetCharacterData(PlayerControl);
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
            tmplyrics = GetDatabyId(i+1);
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
            BgmManager.Instance.StopPlay();
            MainGameManager.instance.EndRun();
             Destroy(transform.parent.gameObject);
         }
        target.SetActive(false);
    }
    IEnumerator presetshow()
    {
        yield return new WaitForSeconds(StartDelay);
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
    public LyricsEntity GetDatabyId(int id)
    {
        if (currentcharacter == Character.Tokiwa)
        {
            switch (localsequal[(int)PlayerControl])
            {
                case 0:
                    return LyricsExcel.tokiwa1.Find(entity => entity.id == id);
                case 1:
                    return LyricsExcel.tokiwa2.Find(entity => entity.id == id);
                case 2:
                    return LyricsExcel.tokiwa3.Find(entity => entity.id == id);
                default:
                    return null;
            }
        }
        else if (currentcharacter == Character.Hajime)
        {
            switch (localsequal[(int)PlayerControl])
            {
                case 0:
                    return LyricsExcel.hajime1.Find(entity => entity.id == id);
                case 1:
                    return LyricsExcel.hajime2.Find(entity => entity.id == id);
                case 2:
                    return LyricsExcel.hajime3.Find(entity => entity.id == id);
                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    }
}
