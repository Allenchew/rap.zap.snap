using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeturn : MonoBehaviour
{
    public GameObject BackLayer;
    public GameObject TurnBack;
    public GameObject TurnCharacter;

    private Vector3 leftpos = new Vector3(-1920, 0, 0);
    private Vector3 centerpoint = new Vector3(0, 0, 0);
    private Vector3 rightpos = new Vector3(1920, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMove()
    {
        StartCoroutine(moveimage(true,TurnBack, TurnCharacter));
    }
    IEnumerator moveimage(bool moveleft,GameObject backtarget,GameObject backchar)
    {
        Vector3 orgpos = moveleft ? rightpos : leftpos;
        Vector3 destpos = moveleft ? leftpos : rightpos;
        backtarget.transform.position = orgpos;
        backchar.transform.position = destpos;
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            BackLayer.GetComponent<Image>().color = Color.Lerp(Color.clear,Color.black,i);
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            backtarget.transform.position = Vector3.Lerp(orgpos,centerpoint,i);
            backchar.transform.position = Vector3.Lerp(destpos, centerpoint, i);
            yield return new WaitForSeconds(0.01f); 
        }
        yield return new WaitForSeconds(2.0f);
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            backtarget.transform.position = Vector3.Lerp(centerpoint, destpos, i);
            backchar.transform.position = Vector3.Lerp(centerpoint, orgpos, i);
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            BackLayer.GetComponent<Image>().color = Color.Lerp(Color.black, Color.clear, i);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
