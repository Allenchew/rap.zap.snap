using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSelect : MonoBehaviour
{
    public int index = 0;
    public GameObject[] showselect;
    public Text[] SelectionText;
    // Update is called once per frame
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < 2)
            {
                index++;
                setSelectMark();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
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
}
