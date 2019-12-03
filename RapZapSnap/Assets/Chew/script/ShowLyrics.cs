using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLyrics : MonoBehaviour
{
    public GameObject test1;
    
    private bool showing_flag = false;
    private int showindex = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !showing_flag)
        {
            showing_flag = true;
            GameObject tmp= gameObject.transform.GetChild(showindex).gameObject;
            showindex++;
            StartCoroutine(shuftShow(tmp));
        }
    }
    IEnumerator shuftShow(GameObject target)
    {
        MaskData thisMaskdata = target.GetComponent<MaskMovement>().ThisMaskData;
        for (int i = 0; i < thisMaskdata.delayshowtime.Length; i++)
        {
            if(i == thisMaskdata.commapos)
            {
                target.transform.position += thisMaskdata.commastep;
                yield return new WaitForSeconds(thisMaskdata.delayshowtime[i]);
                continue;
            }
            target.transform.position += thisMaskdata.steptoward;
            yield return new WaitForSeconds(thisMaskdata.delayshowtime[i]);
        }
        target.SetActive(false);
        if(showindex<gameObject.transform.childCount)showing_flag = false;
    }
}
