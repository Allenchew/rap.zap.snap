using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSelect : MonoBehaviour
{
    public int index = 0;
    public GameObject[] showselect;
    public Text[] SelectionText;

    private DS4InputKey controllingplayer = new DS4InputKey();
    // Update is called once per frame
    void Start()
    {
        
    }
    void OnEnable()
    {
        controllingplayer = Getinput(MainGameManager.instance.currentplayer);
    }
    void Update()
    {
        if (controllingplayer.Down)
        {
            if (index < 2)
            {
                index++;
                setSelectMark();
            }
        }
        else if (controllingplayer.Up)
        {
            if (index > 0)
            {
                index--;
                setSelectMark();
            }
        }
    }
    public void reset(string[] newtext,int newtextindex)
    {
        for(int i = 0; i < 3; i++)
        {
            SelectionText[i].text = newtext[newtextindex+i];
        }
        index = 0;
        setSelectMark();
    }
    public void setSelectMark()
    {
        
        for (int i = 0; i < 3; i++)
        {
            if (i == index)
                showselect[i].SetActive(true);
            else
                showselect[i].SetActive(false);
        }
    }
    private DS4InputKey Getinput(ControllerNum currentplayer)
    {
        switch (currentplayer)
        {
            case ControllerNum.P1:
                return GamePadControl.Instance.GetKeyDown_1;
            case ControllerNum.P2:
                return GamePadControl.Instance.GetKeyDown_2;
            default:
                Debug.Log("Word Select Get input error");
                return GamePadControl.Instance.GetKeyDown_1;
        }
    }
}
