using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharSelectionKari : MonoBehaviour
{
    public GameObject[] P1CharBimg = new GameObject[3];
    public GameObject[] P2CharBimg = new GameObject[3];
    public GameObject[] Selectedbg1;
    public GameObject[] Selectedbg2;
    public GameObject[] Vsbgp1;
    public GameObject[] Vsbgp2;
    public GameObject Fadein;
    public GameObject p2cover;
    public float Centerpoint;
    public float Movedist;
    public int selectedchar { get { return currentChar[0]; } }
        
    private List<int> currentChar = new List<int>();
    private string currentplayer;
    //private int selectedp1 =-1;
    private bool shiftingflag = false;
    private bool selectedp1_flag = false;
    
    void Start()
    {
        currentplayer = gameObject.tag;
        currentChar.Add(0);
        currentChar.Add(1);
        currentChar.Add(2);
    }
    void Update()
    {
        if (!shiftingflag)
        {
            if (!selectedp1_flag)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    shiftingflag = true;
                    shufflelist(true);
                    StartCoroutine(graphshifting(P1CharBimg, true));
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    shiftingflag = true;
                    shufflelist(false);
                    StartCoroutine(graphshifting(P1CharBimg, false));
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    shiftingflag = true;
                    CharacterMng.Instance.SelectedCharacter.Add(currentChar[0]);
                    StartCoroutine(showplayer2(p2cover));
                    StartCoroutine(showselected(Selectedbg1,true));
                }
            }
            else if (selectedp1_flag)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    shiftingflag = true;
                    shufflelist(true);
                    StartCoroutine(graphshifting(P2CharBimg, true));
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    shiftingflag = true;
                    shufflelist(false);
                    StartCoroutine(graphshifting(P2CharBimg, false));
                }else if (Input.GetKeyDown(KeyCode.P)) {
                    shiftingflag = true;
                    CharacterMng.Instance.SelectedCharacter.Add(currentChar[0]);
                    StartCoroutine(showselected(Selectedbg2, true));
                }
            }

        }
    }
    IEnumerator graphshifting(GameObject[] target, bool shiftLeft)
    {
        Vector3[] tmpos = new Vector3[3];
        int bypassindex = 0;
        float dest;
        dest = shiftLeft ? -Movedist : Movedist;
        for (int i = 0; i < 3; i++)
        {
            tmpos[i] = target[i].transform.localPosition;
           
            if (shiftLeft && tmpos[i].x < Centerpoint)
            {
                    target[i].transform.localPosition = new Vector3(Centerpoint + Movedist, 0, 0);
                    bypassindex = i;
                    if(selectedp1_flag)tmpos[i] = target[i].transform.localPosition;
                    continue;
            }
            else if (!shiftLeft && tmpos[i].x > Centerpoint)
            {
                target[i].transform.localPosition = new Vector3(Centerpoint - Movedist, 0, 0);
                bypassindex = i;
                if (selectedp1_flag) tmpos[i] = target[i].transform.localPosition;
                continue;
            }
        }
        for (float i = 0f; i < 1.02f; i += 0.02f)
        {
            for (int j = 0; j < 3;j++)
            {
                if (j == bypassindex && !selectedp1_flag) continue;
                target[j].transform.localPosition = Vector3.Lerp(tmpos[j], tmpos[j] + new Vector3(dest, 0, 0), i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        shiftingflag = false;
    }
    IEnumerator showplayer2(GameObject cover)
    {
        Movedist = 1860f;
        Centerpoint = 410f;
        Vector3[] tmpsetup = new Vector3[2];
        tmpsetup[0] = new Vector3(Centerpoint, 0, -50);
        tmpsetup[1] = new Vector3(Centerpoint + Movedist, 0, -50);
        Vector3 tmpos = cover.transform.localPosition;
        for (float i = 0f; i < 1.1f; i += 0.1f)
        { 
           cover.transform.localPosition = Vector3.Lerp(tmpos, new Vector3(410, 0, 0), i);
           yield return new WaitForSeconds(0.01f);
        }
        P2CharBimg[CharacterMng.Instance.SelectedCharacter[0]].SetActive(false);
        int tmpindex = 0;
        for(int i = 0; i < 3; i++)
        {
            P1CharBimg[i].SetActive(false);
            if (i == CharacterMng.Instance.SelectedCharacter[0])
            {
                P2CharBimg[i].SetActive(false);
                continue;
            }
            P2CharBimg[i].transform.localPosition = tmpsetup[tmpindex];
            tmpindex++;
        }
        for (float i = 0f; i < 1.1f; i += 0.1f)
        {
            cover.GetComponent<Image>().color = Color.Lerp(Color.grey,Color.clear,i);
            yield return new WaitForSeconds(0.01f);
        }
        selectedp1_flag = true;
        shiftingflag = false;
    }

    IEnumerator showselected(GameObject[] target, bool p1)
    {
        bool tmpflag = new bool();
        tmpflag = selectedp1_flag;
        Vector3 tmppos = new Vector3(0,0,0);
        if (tmpflag)
        {
            tmppos = P2CharBimg[currentChar[0]].transform.localPosition;
        }
        List<Vector3> tmpos = new List<Vector3>();

        foreach (GameObject tmp in target)
            tmpos.Add(tmp.transform.localPosition);
        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            for (int j = 0; j < 2; j++)
            {
                target[j].transform.localPosition = Vector3.Lerp(tmpos[j], new Vector3(tmpos[j].x, 0, tmpos[j].z), i);
            }
            if(tmpflag) P2CharBimg[currentChar[0]].transform.localPosition = Vector3.Lerp(tmppos, tmppos + new Vector3(Movedist, 0, 0), i);
            target[2 + currentChar[0]].transform.localPosition = Vector3.Lerp(tmpos[2 + currentChar[0]], new Vector3(tmpos[2 + currentChar[0]].x + 80f, 0, tmpos[2 + currentChar[0]].z), i);
            target[2 + currentChar[0]].GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, i);
            yield return new WaitForSeconds(0.01f);
        }
        currentChar.Remove(currentChar[0]);
        currentChar.Sort();
        if (tmpflag) StartCoroutine(showVsbg());
    }
    IEnumerator showVsbg()
    {
        List<int> tmplist = new List<int>();
        tmplist.AddRange(CharacterMng.Instance.SelectedCharacter);
        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            Selectedbg1[0].GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            Selectedbg1[1].GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            Selectedbg2[0].GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            Selectedbg2[1].GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            Vsbgp1[tmplist[0]].transform.localPosition = Vector3.Lerp(Vsbgp1[tmplist[0]].transform.localPosition, new Vector3(0, 0, 0), i);
            Vsbgp2[tmplist[1]].transform.localPosition = Vector3.Lerp(Vsbgp2[tmplist[1]].transform.localPosition, new Vector3(0, 0, 0), i);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(loadnewscene());
    }
    IEnumerator loadnewscene()
    {
        AsyncOperation loadmain = SceneManager.LoadSceneAsync("maintest");
        loadmain.allowSceneActivation = false;
        while (!loadmain.isDone)
        {
            yield return new WaitForSeconds(0.01f);
            if (loadmain.progress >= 0.9f)
            {
                for (float i = 0f; i < 1.1f; i += 0.01f)
                {
                    Fadein.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, i);
                    yield return new WaitForSeconds(0.01f);
                }
                loadmain.allowSceneActivation = true;
            }
        }

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
            int tmp = currentChar[currentChar.Count-1];
            currentChar.Remove(tmp);
            currentChar.Insert(0, tmp);
        }
    }
}
