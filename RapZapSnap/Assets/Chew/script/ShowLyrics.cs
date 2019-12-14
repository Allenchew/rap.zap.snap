using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MaskData
{
    public float[] delayshowtime;
    public bool OnceEffect;
    public bool StartEffect;
    public bool EndEffect;
    public bool IsOption;
    public bool ShowOption;
    public int EffectIndex;
    public int OptionCount;
    public float EffectDelay;
    public void Set(float[] tmpdelay, bool SE, bool EE, bool isop, int Eindex, int opcount,bool ShowOp,bool onceEf,float effectdelay)
    {
        delayshowtime = tmpdelay;
        StartEffect = SE;
        EndEffect = EE;
        IsOption = isop;
        EffectIndex = Eindex;
        OptionCount = opcount;
        ShowOption = ShowOp;
        OnceEffect = onceEf;
        EffectDelay = effectdelay;
    }
}

public class ShowLyrics : MonoBehaviour
{
    public GameObject Bigeffect;
    public GameObject Slideeffect;
    public GameObject showWords;
    public string[] OptionText;

    private bool showing_flag = true;
    private int showindex = 0;
    private int selectedtext = 0;
    private int resetindex = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(presetshow());
    }

    // Update is called once per frame
    void Update()
    {
        if (!showing_flag)
        {
            showing_flag = true;
            GameObject tmp= gameObject.transform.GetChild(showindex).gameObject;
            showindex++;
            StartCoroutine(shuftShow(tmp));
        }
        if (Input.GetKeyDown(KeyCode.E) && showWords.transform.GetChild(0).gameObject.activeSelf){
            StopCoroutine(selectioncountdown());
            activeselected();
        }
    }
    IEnumerator shuftShow(GameObject target)
    {
        MaskData thisMaskdata = target.GetComponent<MaskMovement>().ThisMaskData;
       
        if(thisMaskdata.ShowOption)
        {
            showWords.transform.GetChild(0).gameObject.SetActive(true);
            showWords.GetComponent<WordSelect>().reset(OptionText, resetindex);
            resetindex += 3;
            StartCoroutine(selectioncountdown());
        }
        if (thisMaskdata.OnceEffect)
        {
            ShowSpecifyEffect(thisMaskdata.EffectIndex);
        }
        int tmpindex = target.GetComponent<MaskMovement>().OptionCount;

        for (int i = 0; i < thisMaskdata.delayshowtime.Length; i++)
        {
            if(target.GetComponent<MaskMovement>().IsOption)
                target.transform.GetChild((selectedtext*tmpindex)+i).gameObject.SetActive(true);
            else
                target.transform.GetChild(i).gameObject.SetActive(true);
            if (thisMaskdata.StartEffect)
            {
                ShowSpecifyEffect(thisMaskdata.EffectIndex);
            }
            yield return new WaitForSeconds(thisMaskdata.delayshowtime[i]);
            if(thisMaskdata.EndEffect && i == thisMaskdata.delayshowtime.Length - 1)
            {
                ShowSpecifyEffect(thisMaskdata.EffectIndex);
                yield return new WaitForSeconds(thisMaskdata.EffectDelay);
                break;
            }
        }
        
        if(showindex<gameObject.transform.childCount && !thisMaskdata.OnceEffect)showing_flag = false;
    }
    IEnumerator presetshow()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject tmp = gameObject.transform.GetChild(showindex).gameObject;
        showindex++;
        StartCoroutine(shuftShow(tmp));
    }
    IEnumerator selectioncountdown()
    {
        yield return new WaitForSeconds(1.0f);
        activeselected();
    }
    void activeselected()
    {
        selectedtext = showWords.GetComponent<WordSelect>().index;
        showWords.transform.GetChild(0).gameObject.SetActive(false);
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
