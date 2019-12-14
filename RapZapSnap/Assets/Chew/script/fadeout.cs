using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeout : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadewords());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator fadewords()
    {
        Color color = GetComponent<Renderer>().material.color;
        while (color.a > 0)
        {
            color.a -= 1f * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }
}
