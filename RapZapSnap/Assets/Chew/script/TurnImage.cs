using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnImage : MonoBehaviour
{
    public Sprite[] CurrentImages = new Sprite[3];
    
    public void SetThisImage(int index)
    {
        if (index < 3)
            gameObject.GetComponent<Image>().sprite = CurrentImages[index];
        else
            Debug.Log("index out of range");
    }
    
}
