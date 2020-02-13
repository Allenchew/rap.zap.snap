using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeturn : MonoBehaviour
{
    public GameObject SEPlayer;
    public GameObject BackLayer;
    public GameObject TurnBack;
    public GameObject TurnCharacter;

    public GameObject[] CurrentTurn;

    private Vector3 leftpos = new Vector3(-1920, 0, 0);
    private Vector3 centerpoint = new Vector3(0, 0, 0);
    private Vector3 rightpos = new Vector3(1920, 0, 0);

    public void StartMove(int characterindex,int turnindex)
    {
        StartCoroutine(moveimage(characterindex,turnindex,true,TurnBack, TurnCharacter));
    }
    IEnumerator moveimage( int characterindex, int turnindex, bool moveleft,GameObject backtarget,GameObject backchar)
    {
        Vector3 orgpos = moveleft ? rightpos : leftpos;
        Vector3 destpos = moveleft ? leftpos : rightpos;
        backtarget.transform.localPosition = orgpos;
        backchar.transform.localPosition = destpos;
        backtarget.GetComponent<TurnImage>().SetThisImage(characterindex);
        backchar.GetComponent<TurnImage>().SetThisImage(characterindex);
        CurrentTurn[turnindex].GetComponent<TurnImage>().SetThisImage(characterindex);
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            BackLayer.GetComponent<Image>().color = Color.Lerp(Color.clear,Color.black,i);
            yield return new WaitForSeconds(0.01f);
        }
        SEPlayer.GetComponent<SEContainer>().SetThisSe(1);
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            backtarget.transform.localPosition = Vector3.Lerp(orgpos,centerpoint,i);
            backchar.transform.localPosition = Vector3.Lerp(destpos, centerpoint, i);
            yield return new WaitForSeconds(0.01f); 
        }
         CurrentTurn[turnindex].SetActive(true);
        for(int i = 0; i < 10; i++)
        {
            CurrentTurn[turnindex].transform.localScale -= new Vector3(0.01f,0.01f,0);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2.0f);
        SEPlayer.GetComponent<SEContainer>().SetThisSe(1);
        for (float i = 0; i < 1.01f; i += 0.1f)
        {
            backtarget.transform.localPosition = Vector3.Lerp(centerpoint, destpos, i);
            backchar.transform.localPosition = Vector3.Lerp(centerpoint, orgpos, i);
            yield return new WaitForSeconds(0.01f);
        }
        MainGameManager.instance.f_ShowingTurn = false;
        for (float i = 0; i < 1.01f; i += 0.01f)
        {
            BackLayer.GetComponent<Image>().color = Color.Lerp(Color.black, Color.clear, i);
            yield return new WaitForSeconds(0.01f);
        }
        CurrentTurn[turnindex].SetActive(false);
        yield return null;
    }
}
