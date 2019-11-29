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
        DS4InputDownKey input = id == ControllerNum.P1 ? GamePadControl.Instance.Input_1 : GamePadControl.Instance.Input_2;
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
        if (input.LeftStick_Up == true)
        {
            Debug.Log(id + " : L↑ボタン");
        }
        if (input.LeftStick_Down == true)
        {
            Debug.Log(id + " : L↓ボタン");
        }
        if (input.LeftStick_Left == true)
        {
            Debug.Log(id + " : L←ボタン");
        }
        if (input.LeftStick_Right == true)
        {
            Debug.Log(id + " : L→ボタン");
        }
        if (input.RightStick_Up == true)
        {
            Debug.Log(id + " : R↑ボタン");
        }
        if (input.RightStick_Down == true)
        {
            Debug.Log(id + " : R↓ボタン");
        }
        if (input.RightStick_Left == true)
        {
            Debug.Log(id + " : R←ボタン");
        }
        if (input.RightStick_Right == true)
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
