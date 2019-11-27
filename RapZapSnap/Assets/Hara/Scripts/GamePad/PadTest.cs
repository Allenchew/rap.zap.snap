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
        if(GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Circle) == true)
        {
            Debug.Log(id + " : ○ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Cross) == true)
        {
            Debug.Log(id + " : ×ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Triangle) == true)
        {
            Debug.Log(id + " : △ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Square) == true)
        {
            Debug.Log(id + " :□ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Up) == true)
        {
            Debug.Log(id + " : ↑ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Down) == true)
        {
            Debug.Log(id + " : ↓ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Left) == true)
        {
            Debug.Log(id + " : ←ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.Right) == true)
        {
            Debug.Log(id + " : →ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.LeftStick_Up) == true)
        {
            Debug.Log(id + " : L↑ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.LeftStick_Down) == true)
        {
            Debug.Log(id + " : L↓ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.LeftStick_Left) == true)
        {
            Debug.Log(id + " : L←ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.LeftStick_Right) == true)
        {
            Debug.Log(id + " : L→ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.RightStick_Up) == true)
        {
            Debug.Log(id + " : R↑ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.RightStick_Down) == true)
        {
            Debug.Log(id + " : R↓ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.RightStick_Left) == true)
        {
            Debug.Log(id + " : R←ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.RightStick_Right) == true)
        {
            Debug.Log(id + " : R→ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.L1) == true)
        {
            Debug.Log(id + " : L1ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.L2) == true)
        {
            Debug.Log(id + " : L2ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.L3) == true)
        {
            Debug.Log(id + " : L3ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.R1) == true)
        {
            Debug.Log(id + " : R1ボタン");
        }
        if (GamePadControl.Instance.GetAxisDown(id, DS4AxisKey.R2) == true)
        {
            Debug.Log(id + " : R2ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.R3) == true)
        {
            Debug.Log(id + " : R3ボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.OPTION) == true)
        {
            Debug.Log(id + " : OPTIONボタン");
        }
        if (GamePadControl.Instance.GetButtonDown(id, DS4ButtonKey.SHARE) == true)
        {
            Debug.Log(id + " : SHAREボタン");
        }
    }
}
