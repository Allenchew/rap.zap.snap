using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4;

public class PadTest : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Button buttonObj = null;

    // Start is called before the first frame update
    void Start()
    {
        buttonObj.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
