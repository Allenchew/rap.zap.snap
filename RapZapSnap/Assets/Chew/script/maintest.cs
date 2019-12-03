using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maintest : MonoBehaviour
{
    public GameObject fadein;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeout());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator fadeout()
    {
        yield return new WaitForSeconds(1.0f);
        for(float i = 0; i < 1.1f; i += 0.01f)
        {
            fadein.GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            yield return new WaitForSeconds(0.01f);
        }
        int xstep = 1;
        int ystep =-1;
        float currentx = -8;
        float currenty = 5;
        for(int i = 0; i < 56; i++)
        {
            NotesControl.Instance.CallNotes(NotesType.CircleKey, new Vector3(0, 0, 0), new Vector3(currentx, currenty, 0));
            if (currentx > 7 && currenty > 4)
            {
                xstep = 0;
                ystep = -1;
            }else if (currentx>7 && currenty < -2)
            {
                xstep = -1;
                ystep = 0;
            }else if(currentx<-7 && currenty < -2)
            {
                xstep = 0;
                ystep = 1;
            }else if(currentx<-7 && currenty > 4)
            {
                xstep = 1;
                ystep = 0;
            }
            currentx += xstep;
            currenty += ystep;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
