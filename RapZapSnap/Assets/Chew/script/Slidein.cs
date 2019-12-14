using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidein : MonoBehaviour
{
    public float Totalusetime=1.0f;
    public bool RunExtra = false;
    public GameObject[] Externalobject;
    public int[] Runindex;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(slideaction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator slideaction()
    {
        int tmpindex = 0;
        float tmpdelay = (Totalusetime /(gameObject.transform.childCount + Externalobject.Length))/10.0f;
        Debug.Log(tmpdelay);
        for (int i = 0; i < gameObject.transform.childCount+Externalobject.Length; i++)
        {
            Transform tmp;
            if (RunExtra && i == Runindex[tmpindex])
            {
                 tmp = Externalobject[tmpindex].transform;
                if (tmpindex < Externalobject.Length)
                    tmpindex++;
            }
            else
            {
                 tmp = transform.GetChild(i-tmpindex);
            }
            for (float j = 0; j < 1.1f; j += 0.1f)
            {
                
                tmp.position = Vector3.Lerp(tmp.position, new Vector3(0, 0, 0), j);
                yield return new WaitForSeconds(tmpdelay);
            }
            yield return null;
        }
    }
}
