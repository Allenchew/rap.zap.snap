using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tset : MonoBehaviour
{
    private static bool testing = false;
    public static bool Testing { get { return testing; }set { testing = value; } }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
    }

}
