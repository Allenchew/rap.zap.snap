using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Keybind
{
    public KeyCode leftb { get; set; }
    public KeyCode rightb { get; set; }
    public KeyCode upb { get; set; }
    public KeyCode downb { get; set; }
    public KeyCode tri { get; set; }
    public KeyCode squ { get; set; }
    public KeyCode ob { get; set; }
    public KeyCode xb { get; set; }
    public KeyCode enterb { get; set; }
}

public class CharacterSelectp1 : MonoBehaviour
{
    public GameObject[] images = new GameObject[6];

    private bool f_movingslide = false;
    private Keybind pcontrol = new Keybind();
    private Vector3 tmppos;
    // Start is called before the first frame update
    void Start()
    {
        pcontrol = setupdefault(gameObject.tag);
    }

    // Update is called once per frame
    void Update()
    {
        if (!f_movingslide)
            if (Input.GetKeyDown(pcontrol.leftb))
            {
                StartCoroutine(i_sliding(images[1],tmppos,true));
            }
            /*else if (Input.GetKeyDown(pcontrol.rightb))
            {
                StartCoroutine(i_sliding());
            }*/
            else if (Input.GetKeyDown(pcontrol.xb))
            {
                tset.Testing = true;
            }
    }
    IEnumerator i_sliding(GameObject target,Vector3 destination,bool fadeout)
    {
        f_movingslide = true;
        Vector3 tmporgpos = target.transform.localPosition;
        
        for(float i = 0f; i < 1.02f; i += 0.02f)
        {
            if (fadeout)
                target.GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, i);
            target.transform.localPosition = Vector3.Lerp(tmporgpos, destination, i);
            yield return new WaitForSeconds(0.01f);
        }
        target.transform.localPosition= Vector3.Lerp(tmporgpos, destination, 1f);
        f_movingslide = false;
    }
    private Keybind setupdefault(string playernum)
    {
        Keybind controlkey = new Keybind();
        if (playernum == "p1" || playernum == "p2")
        {
            controlkey.leftb = (playernum == "p1" ? KeyCode.A : KeyCode.LeftArrow);
            controlkey.rightb = (playernum == "p1" ? KeyCode.D : KeyCode.RightArrow);
            controlkey.upb = (playernum == "p1" ? KeyCode.W : KeyCode.UpArrow);
            controlkey.downb = (playernum == "p1" ? KeyCode.S : KeyCode.DownArrow);
            controlkey.tri = (playernum == "p1" ? KeyCode.I : KeyCode.Keypad8);
            controlkey.squ = (playernum == "p1" ? KeyCode.J : KeyCode.Keypad4);
            controlkey.ob = (playernum == "p1" ? KeyCode.L : KeyCode.Keypad6);
            controlkey.xb = (playernum == "p1" ? KeyCode.K : KeyCode.Keypad5);
            controlkey.enterb = (playernum == "p1" ? KeyCode.Return : KeyCode.KeypadEnter);
        }
        else
        {
            Debug.Log("playernum setting error");
        }
        Debug.Log(controlkey.leftb);
        return controlkey;
    }
}
