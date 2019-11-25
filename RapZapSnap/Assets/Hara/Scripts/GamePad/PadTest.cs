using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        CheckInput(ControllerNum.P1);
        CheckInput(ControllerNum.P2);
    }

    private void CheckInput(ControllerNum id)
    {
        GamePadControl.DS4InputKeyType input = id == ControllerNum.P1 ? GamePadControl.Instance.Controller1 : GamePadControl.Instance.Controller2;
        
        if(input.Circle == true)
        {
            Debug.Log(id + " : ○ボタン");
        }
        if (input.Cross == true)
        {
            Debug.Log(id + " : ×ボタン");
        }
        if (input.Triangle == true)
        {
            Debug.Log(id + " : △ボタン");
        }
        if (input.Square == true)
        {
            Debug.Log(id + " :□ボタン");
        }
        if (input.Up == true)
        {
            Debug.Log(id + " : ↑ボタン");
        }
        if (input.Down == true)
        {
            Debug.Log(id + " : ↓ボタン");
        }
        if (input.Left == true)
        {
            Debug.Log(id + " : ←ボタン");
        }
        if (input.Right == true)
        {
            Debug.Log(id + " : →ボタン");
        }
        if (input.LstickU == true)
        {
            Debug.Log(id + " : L↑ボタン");
        }
        if (input.LstickD == true)
        {
            Debug.Log(id + " : L↓ボタン");
        }
        if (input.LstickL == true)
        {
            Debug.Log(id + " : L←ボタン");
        }
        if (input.LstickR == true)
        {
            Debug.Log(id + " : L→ボタン");
        }
        if (input.RstickU == true)
        {
            Debug.Log(id + " : R↑ボタン");
        }
        if (input.RstickD == true)
        {
            Debug.Log(id + " : R↓ボタン");
        }
        if (input.RstickL == true)
        {
            Debug.Log(id + " : R←ボタン");
        }
        if (input.RstickR == true)
        {
            Debug.Log(id + " : R→ボタン");
        }
        if (input.L1 == true)
        {
            Debug.Log(id + " : L1ボタン");
        }
        if (input.L2 == true)
        {
            Debug.Log(id + " : L2ボタン");
        }
        if (input.L3 == true)
        {
            Debug.Log(id + " : L3ボタン");
        }
        if (input.R1 == true)
        {
            Debug.Log(id + " : R1ボタン");
        }
        if (input.R2 == true)
        {
            Debug.Log(id + " : R2ボタン");
        }
        if (input.R3 == true)
        {
            Debug.Log(id + " : R3ボタン");
        }
        if (input.OPTION == true)
        {
            Debug.Log(id + " : OPTIONボタン");
        }
        if (input.SHARE == true)
        {
            Debug.Log(id + " : SHAREボタン");
        }
    }
}
