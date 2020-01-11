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
        if(GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == true)
        {
            Debug.Log(id + " : ○ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true)
        {
            Debug.Log(id + " : ×ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true)
        {
            Debug.Log(id + " : △ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Square) == true)
        {
            Debug.Log(id + " :□ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == true)
        {
            Debug.Log(id + " : ↑ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == true)
        {
            Debug.Log(id + " : ↓ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == true)
        {
            Debug.Log(id + " : ←ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Right) == true)
        {
            Debug.Log(id + " : →ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.LeftStick_Up) == true)
        {
            Debug.Log(id + " : L↑ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.LeftStick_Down) == true)
        {
            Debug.Log(id + " : L↓ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.LeftStick_Left) == true)
        {
            Debug.Log(id + " : L←ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.LeftStick_Right) == true)
        {
            Debug.Log(id + " : L→ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.RightStick_Up) == true)
        {
            Debug.Log(id + " : R↑ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.RightStick_Down) == true)
        {
            Debug.Log(id + " : R↓ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.RightStick_Left) == true)
        {
            Debug.Log(id + " : R←ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.RightStick_Right) == true)
        {
            Debug.Log(id + " : R→ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.L1) == true)
        {
            Debug.Log(id + " : L1ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.L2) == true)
        {
            Debug.Log(id + " : L2ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.L3) == true)
        {
            Debug.Log(id + " : L3ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.R1) == true)
        {
            Debug.Log(id + " : R1ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.R2) == true)
        {
            Debug.Log(id + " : R2ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.R3) == true)
        {
            Debug.Log(id + " : R3ボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.OPTION) == true)
        {
            Debug.Log(id + " : OPTIONボタン");
        }
        if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.SHARE) == true)
        {
            Debug.Log(id + " : SHAREボタン");
        }
    }
}
