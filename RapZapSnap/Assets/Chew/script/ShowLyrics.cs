using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowLyrics : MonoBehaviour
{
    public GameObject Bigeffect;
    public GameObject Slideeffect;
    public GameObject showWords;
    public string[] OptionText;
    public int[] OptionScore = new int[6];

    private ControllerNum PlayerControl;
    private bool showing_flag = true;
    private int showindex = 0;
    private int selectedtext = 0;
    private int resetindex = 0;
    private DS4InputKey thiscontrol;
    // Start is called before the first frame update
    void Start()
    {
        showWords = GameObject.Find("ShowWord");
        PlayerControl = MainGameManager.instance.currentplayer;
        setplayercontrol();
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
        if (thiscontrol.Cross && showWords.transform.GetChild(0).gameObject.activeSelf){
            StopCoroutine(selectioncountdown());
            activeselected();
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
        if (showindex < gameObject.transform.childCount && !thisMaskdata.OnceEffect)showing_flag = false;
        if(!(showindex < gameObject.transform.childCount))
        {
            yield return new WaitForSeconds(2.0f);
            MainGameManager.instance.EndRun();
            Destroy(transform.parent.gameObject);
        }
    }
    IEnumerator presetshow()
    {
        yield return new WaitForSeconds(4.0f);
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
        GameData.Instance.PlusTotalScore(PlayerControl, OptionScore[selectedtext+resetindex-3]);
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
