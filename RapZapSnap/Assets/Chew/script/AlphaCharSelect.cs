using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlphaCharSelect : MonoBehaviour
{
    public GameObject[] images;
    public GameObject Playertwo;
    public GameObject p1bg;
    public GameObject Coverimage;
    private bool p1picked = false;
    private bool movingflag = false;
    private Vector3 centerpoint = new Vector3(-250f,0,0);
    private Vector3 movingpoint = new Vector3();

    private List<int> currentChar = new List<int>();
    private List<int> tmpregister = new List<int>();
    void Start()
    {
        currentChar.Add(0);
        currentChar.Add(1);
    }
    void Update()
    {
        if (!p1picked)
        {
            if ((GamePadControl.Instance.GetKeyDown_1.Left || GamePadControl.Instance.GetKeyDown_1.LeftStick_Left) && !movingflag)
            {
                movingflag = true;
                shufflelist(true);
                StartCoroutine(progressmoving(false));
            }
            else if ((GamePadControl.Instance.GetKeyDown_1.Right|| GamePadControl.Instance.GetKeyDown_1.RightStick_Right) && !movingflag)
            {
                movingflag = true;
                shufflelist(false);
                StartCoroutine(progressmoving(true));
            }
            else if (GamePadControl.Instance.GetKeyDown_1.Cross && !movingflag)
            {
                GameData.Instance.SetCharacterData(ControllerNum.P1, (Character)currentChar[0]+1);
                movingflag = true;
                tmpregister.Add(currentChar[0]);
                currentChar.Remove(currentChar[0]);
                StartCoroutine(showplayertwo());
            }
        }
        else
        {
            if ((GamePadControl.Instance.GetKeyDown_2.Left || GamePadControl.Instance.GetKeyDown_2.LeftStick_Left) && !movingflag)
            {
                //p2 selection moving process
            }
            else if ((GamePadControl.Instance.GetKeyDown_2.Right || GamePadControl.Instance.GetKeyDown_2.RightStick_Right) && !movingflag)
            {
                //p2 selection moving process
            }
            else if (GamePadControl.Instance.GetKeyDown_2.Cross && !movingflag)
            {
                GameData.Instance.SetCharacterData(ControllerNum.P2, (Character)currentChar[0]+1);
                movingflag = true;
                tmpregister.Add(currentChar[0]);
                currentChar.Remove(currentChar[0]);
                StartCoroutine(fadeout());
                //selected animation and scene moving
            }
        }
    }
    IEnumerator progressmoving(bool moveright)
    {
        int tmpindex = 0;
        bool laterunflag = false;
        Vector3[] tmppos = new Vector3[2];
        Color[] tmpcolor = new Color[2];
        Color fullcolor = new Color(255, 255, 255, 0);
       for(int i = 0; i < 2; i++)
        {
            if(moveright && images[i].transform.localPosition.x > -300)
            {

                laterunflag = true;
                movingpoint = new Vector3(2000, 0, 0);
                tmpindex = i;

            }
            else if(!moveright && images[i].transform.localPosition.x < -1000)
            {
                images[i].transform.localPosition = new Vector3(1750, 0, 0);
                movingpoint = new Vector3(-2000, 0, 0);
                tmpindex = i;
            }
            tmppos[i] = images[i].transform.localPosition;
            tmpcolor[i] = images[i].GetComponent<Image>().color;
        }
       for(float i = 0; i < 1.1f; i += 0.1f)
        {
            images[tmpindex].transform.localPosition = Vector3.Lerp(tmppos[tmpindex], tmppos[tmpindex]+movingpoint, i);
            images[tmpindex].GetComponent<Image>().color = Color.Lerp(tmpcolor[tmpindex],Color.white-tmpcolor[tmpindex]+fullcolor,i);
            images[tmpindex].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(tmpcolor[tmpindex], Color.white- tmpcolor[tmpindex] + fullcolor, i);
            images[tmpindex].transform.GetChild(1).GetComponent<Image>().color = Color.Lerp(tmpcolor[tmpindex], Color.white - tmpcolor[tmpindex] + fullcolor, i);

            images[1-tmpindex].transform.localPosition = Vector3.Lerp(tmppos[1-tmpindex], tmppos[1 - tmpindex]+movingpoint, i);
            images[1 - tmpindex].GetComponent<Image>().color = Color.Lerp(tmpcolor[1-tmpindex], Color.white - tmpcolor[1-tmpindex] + fullcolor, i);
            images[1 - tmpindex].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(tmpcolor[1-tmpindex], Color.white - tmpcolor[1-tmpindex] + fullcolor, i);
            images[1 - tmpindex].transform.GetChild(1).GetComponent<Image>().color = Color.Lerp(tmpcolor[1 - tmpindex], Color.white - tmpcolor[1 - tmpindex] + fullcolor, i);

            yield return new WaitForSeconds(0.01f);
        }
       if(laterunflag) images[tmpindex].transform.localPosition = new Vector3(-2250, 0, 0);
        movingflag = false;
    }

    IEnumerator showplayertwo()
    {
        Vector3 tmppos = Playertwo.transform.GetChild(currentChar[0]).transform.localPosition;
        Vector3 tmpp1 = images[tmpregister[0]].transform.localPosition;
        Vector3 tmpmove = new Vector3(2000, 0, 0);
        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            images[tmpregister[0]].transform.localPosition = Vector3.Lerp(tmpp1,tmpp1-tmpmove,i);
            p1bg.GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            Playertwo.transform.GetChild(currentChar[0]).transform.localPosition = Vector3.Lerp(tmppos, tmppos - tmpmove, i);
            yield return new WaitForSeconds(0.01f);
        }
        p1picked = true;
    }
    void shufflelist(bool shuftLeft)
    {
        if (shuftLeft)
        {
            int tmp = currentChar[0];
            currentChar.Remove(tmp);
            currentChar.Add(tmp);
        }
        else
        {
            int tmp = currentChar[currentChar.Count - 1];
            currentChar.Remove(tmp);
            currentChar.Insert(0, tmp);
        }
    }
    IEnumerator fadeout()
    {
        for (float i = 0; i < 1.1f; i += 0.05f)
        {
            Coverimage.GetComponent<Image>().color = Color.Lerp(Color.clear,Color.white , i);
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene(2);
    }
}
