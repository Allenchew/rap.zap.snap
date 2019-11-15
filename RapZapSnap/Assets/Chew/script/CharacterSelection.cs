using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CharacterSelection : MonoBehaviour
{

    [SerializeField]
    public Vector3[] Presetposition= new Vector3[3];

    public Image[] CharacterImages = new Image[3];

  
    
    private bool moving_flag = false;
    private Vector3 imagestepping;
    enum movetoward
    {
        left=-1,
        right=1
    }
    // Start is called before the first frame update
    void Start()
    {
        
        imagestepping = new Vector3(Mathf.Abs(Presetposition[1].x - Presetposition[0].x),0,0);
    }

    // Update is called once per frame
    void Update()
    {
       /* if (!moving_flag)
            if (Input.GetKeyDown(pcontrol.leftb))
            {
                StartCoroutine(i_movepicture(movetoward.left));
            }
            else if (Input.GetKeyDown(pcontrol.rightb))
            {
                StartCoroutine(i_movepicture(movetoward.right));
            }else if (Input.GetKeyDown(pcontrol.xb))
            {
                tset.Testing = true;
            }*/
    }
    IEnumerator i_movepicture(movetoward toward)
    {
        moving_flag = true;
        int tmpindex =0;
        var tmppos = toward > 0 ? 0 : 2;
        Vector3[] tmpimgpos = new Vector3[3];
        float[] tmpalpha = new float[3];
        for(int i = 0; i < 3; i++)
        {
            tmpalpha[i] = CharacterImages[i].color.a;
            tmpimgpos[i] = CharacterImages[i].transform.localPosition;
        }
        for (int i = 0; i < 3; i++)
        {
            if(Mathf.Abs(CharacterImages[i].transform.localPosition.x - Presetposition[tmppos].x) <= 0.02f)
            {
                Debug.Log("entered");
                CharacterImages[i].transform.localPosition = Presetposition[2-tmppos];
                tmpindex = i;
            }
        }
        
        for (float i=0f;i<1.02f;i+=0.02f)
        {
            for(int j = 0; j < 3; j++)
            {
                if (j == tmpindex)
                    continue;
                CharacterImages[j].transform.localPosition = Vector3.Lerp(tmpimgpos[j], tmpimgpos[j] + ((int)toward * imagestepping),i);

                if(tmpalpha[j]>0)
                {
                    CharacterImages[j].color = Color.Lerp(Color.white, Color.clear, i);
                }
                else
                {
                    CharacterImages[j].color = Color.Lerp(Color.clear, Color.white, i);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        moving_flag = false;
        yield return null;
    }
   
}
