using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBlur : MonoBehaviour
{
    public GameObject Blurmask;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(hideblur());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator hideblur()
    {
        Blurmask.SetActive(true);
       Material tmp = Blurmask.GetComponent<SpriteRenderer>().material;
        for(float i = 8; i > 0; i-=0.1f)
        {
           tmp.SetFloat("_Size", i);
           yield return null;
        }
    }
}
