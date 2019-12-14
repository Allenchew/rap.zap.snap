using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4InputCustom;

public enum ControllerNum
{
    P1,
    P2
}

public struct DS4InputKey
{
    public bool Circle;
    public bool Cross;
    public bool Triangle;
    public bool Square;
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;
    public bool LeftStick_Up;
    public bool LeftStick_Down;
    public bool LeftStick_Left;
    public bool LeftStick_Right;
    public bool RightStick_Up;
    public bool RightStick_Down;
    public bool RightStick_Left;
    public bool RightStick_Right;
    public bool L1;
    public bool L2;
    public bool L3;
    public bool R1;
    public bool R2;
    public bool R3;
    public bool OPTION;
    public bool SHARE;

    public void ResetFlag()
    {
        Circle = false;
        Cross = false;
        Triangle = false;
        Square = false;
        Up = false;
        Down = false;
        Left = false;
        Right = false;
        LeftStick_Up = false;
        LeftStick_Down = false;
        LeftStick_Left = false;
        LeftStick_Right = false;
        RightStick_Up = false;
        RightStick_Down = false;
        RightStick_Left = false;
        RightStick_Right = false;
        L1 = false;
        L2 = false;
        L3 = false;
        R1 = false;
        R2 = false;
        R3 = false;
        OPTION = false;
        SHARE = false;
    }
}

public class GamePadControl : SingletonMonoBehaviour<GamePadControl>
{
    private  DS4InputCustom.DS4InputCustom ds4InputCustom = null;

    [SerializeField, Tooltip("DS4を使う")]
    private bool useDS4 = true;

    [SerializeField, Tooltip("DS4のスティック系の有効入力感度"), Range(0.01f, 1.0f)]
    private float axisValue = 0.8f;

    [SerializeField]
    private bool multithreadUpdate = true;

    private UnityEngine.EventSystems.StandaloneInputModule inputModule;

    public DS4InputKey GetKeyDown_1 { private set; get; } = new DS4InputKey();
    public DS4InputKey GetKeyDown_2 { private set; get; } = new DS4InputKey();
    public DS4InputKey GetKeyUp_1 { private set; get; } = new DS4InputKey();
    public DS4InputKey GetKeyUp_2 { private set; get; } = new DS4InputKey();

    private struct DS4KeyName
    {
        // UI操作用のキー
        public readonly string Submit;
        public readonly string Cancel;
        public readonly string Horizontal;
        public readonly string Vertical;

        public DS4KeyName(ControllerNum inputPlayer)
        {
            // ※Unityの仕様上の都合、1Pコントローラーが2P(2Pが1P)として取得されてしまうので注意

            if (inputPlayer == ControllerNum.P1)
            {
                Submit = "Submit_2";
                Cancel = "Cancel_2";
                Horizontal = "Horizontal_2";
                Vertical = "Vertical_2";
            }
            else
            {
                Submit = "Submit_1";
                Cancel = "Cancel_1";
                Horizontal = "Horizontal_1";
                Vertical = "Vertical_1";
            }
        }
    }

    private DS4KeyName joy1KeyName;
    private DS4KeyName joy2KeyName;

    /// <summary>
    /// 入力管理用のフラグ
    /// </summary>
    private struct InputFlag
    {
        public bool Ci, Cr, Tr, Sq, Pu, Pd, Pl, Pr, L1, L2, L3, Lu, Ld, Ll, Lr, R1, R2, R3, Ru, Rd, Rl, Rr, Op, Sh;

        public void SetFlag(ref bool isInput, bool set)
        {
            isInput = set;
        }

        public void ResetFlag()
        {
            Ci = false;
            Cr = false;
            Tr = false;
            Sq = false;
            Pu = false;
            Pd = false;
            Pl = false;
            Pr = false;
            L1 = false;
            L2 = false;
            L3 = false;
            Lu = false;
            Ld = false;
            Ll = false;
            Lr = false;
            R1 = false;
            R2 = false;
            R3 = false;
            Ru = false;
            Rd = false;
            Rl = false;
            Rr = false;
            Op = false;
            Sh = false;
        }
    }

    private InputFlag inputDown_1;
    private InputFlag inputDown_2;
    private InputFlag inputUp_1;
    private InputFlag inputUp_2;

    private enum DS4AxisKey
    {
        LeftStick_Up,
        LeftStick_Down,
        LeftStick_Left,
        LeftStick_Right,
        RightStick_Up,
        RightStick_Down,
        RightStick_Left,
        RightStick_Right,
        L2,
        R2
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetInputModule(ControllerNum.P1);
    }

    // Update is called once per frame
    void Update()
    {
        DS4SingleInputKeyDown(ControllerNum.P1);
        DS4SingleInputKeyDown(ControllerNum.P2);

        DS4SingleInputKeyUp(ControllerNum.P1);
        DS4SingleInputKeyUp(ControllerNum.P2);
    }

    /// <summary>
    /// GamePadControlの初期化
    /// </summary>
    private void Init()
    {
        inputDown_1.ResetFlag();
        inputDown_2.ResetFlag();
        inputUp_1.ResetFlag();
        inputUp_2.ResetFlag();
        joy1KeyName = new DS4KeyName(ControllerNum.P1);
        joy2KeyName = new DS4KeyName(ControllerNum.P2);
        GetKeyDown_1.ResetFlag();
        GetKeyDown_2.ResetFlag();
        GetKeyUp_1.ResetFlag();
        GetKeyUp_2.ResetFlag();
        inputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        if(ds4InputCustom == null && Instance == this)
        {
            ds4InputCustom = gameObject.AddComponent<DS4InputCustom.DS4InputCustom>();
            ds4InputCustom.MultithreadUpdate = multithreadUpdate;
            ds4InputCustom.Init();
        }
    }

    /// <summary>
    /// コントローラの入力が終わったタイミングで取得
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    private void DS4SingleInputKeyUp(ControllerNum id)
    {
        DS4InputKey input = id == ControllerNum.P1 ? GetKeyUp_1 : GetKeyUp_2;
        input.Circle = GetButtonUp(id, DS4ButtonType.Circle);
        input.Cross = GetButtonUp(id, DS4ButtonType.Cross);
        input.Triangle = GetButtonUp(id, DS4ButtonType.Triangle);
        input.Square = GetButtonUp(id, DS4ButtonType.Square);
        input.Up = GetButtonUp(id, DS4ButtonType.Up);
        input.Down = GetButtonUp(id, DS4ButtonType.Down);
        input.Left = GetButtonUp(id, DS4ButtonType.Left);
        input.Right = GetButtonUp(id, DS4ButtonType.Right);
        input.LeftStick_Up = GetAxisUp(id, DS4AxisKey.LeftStick_Up);
        input.LeftStick_Down = GetAxisUp(id, DS4AxisKey.LeftStick_Down);
        input.LeftStick_Left = GetAxisUp(id, DS4AxisKey.LeftStick_Left);
        input.LeftStick_Right = GetAxisUp(id, DS4AxisKey.LeftStick_Right);
        input.RightStick_Up = GetAxisUp(id, DS4AxisKey.RightStick_Up);
        input.RightStick_Down = GetAxisUp(id, DS4AxisKey.RightStick_Down);
        input.RightStick_Left = GetAxisUp(id, DS4AxisKey.RightStick_Left);
        input.RightStick_Right = GetAxisUp(id, DS4AxisKey.RightStick_Right);
        input.L1 = GetButtonUp(id, DS4ButtonType.L1);
        input.L2 = GetAxisUp(id, DS4AxisKey.L2);
        input.L3 = GetButtonUp(id, DS4ButtonType.L3);
        input.R1 = GetButtonUp(id, DS4ButtonType.R1);
        input.R2 = GetAxisUp(id, DS4AxisKey.R2);
        input.R3 = GetButtonUp(id, DS4ButtonType.R3);
        input.OPTION = GetButtonUp(id, DS4ButtonType.OPTION);
        input.SHARE = GetButtonUp(id, DS4ButtonType.SHARE);
        _ = id == ControllerNum.P1 ? GetKeyUp_1 = input : GetKeyUp_2 = input;
    }

    /// <summary>
    /// コントローラの入力されたタイミングで取得
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    private void DS4SingleInputKeyDown(ControllerNum id)
    {
        DS4InputKey input = id == ControllerNum.P1 ? GetKeyDown_1 : GetKeyDown_2;
        input.Circle = GetButtonDown(id, DS4ButtonType.Circle);
        input.Cross = GetButtonDown(id, DS4ButtonType.Cross);
        input.Triangle = GetButtonDown(id, DS4ButtonType.Triangle);
        input.Square = GetButtonDown(id, DS4ButtonType.Square);
        input.Up = GetButtonDown(id, DS4ButtonType.Up);
        input.Down = GetButtonDown(id, DS4ButtonType.Down);
        input.Left = GetButtonDown(id, DS4ButtonType.Left);
        input.Right = GetButtonDown(id, DS4ButtonType.Right);
        input.LeftStick_Up = GetAxisDown(id, DS4AxisKey.LeftStick_Up);
        input.LeftStick_Down = GetAxisDown(id, DS4AxisKey.LeftStick_Down);
        input.LeftStick_Left = GetAxisDown(id, DS4AxisKey.LeftStick_Left);
        input.LeftStick_Right = GetAxisDown(id, DS4AxisKey.LeftStick_Right);
        input.RightStick_Up = GetAxisDown(id, DS4AxisKey.RightStick_Up);
        input.RightStick_Down = GetAxisDown(id, DS4AxisKey.RightStick_Down);
        input.RightStick_Left = GetAxisDown(id, DS4AxisKey.RightStick_Left);
        input.RightStick_Right = GetAxisDown(id, DS4AxisKey.RightStick_Right);
        input.L1 = GetButtonDown(id, DS4ButtonType.L1);
        input.L2 = GetAxisDown(id, DS4AxisKey.L2);
        input.L3 = GetButtonDown(id, DS4ButtonType.L3);
        input.R1 = GetButtonDown(id, DS4ButtonType.R1);
        input.R2 = GetAxisDown(id, DS4AxisKey.R2);
        input.R3 = GetButtonDown(id, DS4ButtonType.R3);
        input.OPTION = GetButtonDown(id, DS4ButtonType.OPTION);
        input.SHARE = GetButtonDown(id, DS4ButtonType.SHARE);
        _ = id == ControllerNum.P1 ? GetKeyDown_1 = input : GetKeyDown_2 = input;
    }

    /// <summary>
    /// DS4のボタン入力を検知する (複数のUpdate処理で呼び出そうとすると取得できない場合があります)
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="type">入力を検知したいボタンの種類
    /// <para>Example :　GetButtonDown(ControllerNum.P1, DS4ButtonKey.Circle)　1Pの〇ボタンの入力を検知する</para>
    /// </param>
    /// <returns></returns>
    private bool GetButtonUp(ControllerNum id, DS4ButtonType type)
    {
        if(useDS4 == false || ds4InputCustom == null) { return false; }

        bool buttonFlag;
        DS4ControllerType inputID;
        if (id == ControllerNum.P1)
        {
            inputID = DS4ControllerType.P1;
        }
        else if (id == ControllerNum.P2)
        {
            inputID = DS4ControllerType.P2;
        }
        else
        {
            return false;
        }
        bool result;

        // コントローラーが接続されているかチェック
        if (ds4InputCustom.IsController(inputID) == false) { return false; }

        switch (type)
        {
            case DS4ButtonType.Circle:
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Ci : inputUp_2.Ci;
                break;                                               
            case DS4ButtonType.Cross:                                
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Cr : inputUp_2.Cr;
                break;                                               
            case DS4ButtonType.Triangle:                             
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Tr : inputUp_2.Tr;
                break;                                               
            case DS4ButtonType.Square:                               
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Sq : inputUp_2.Sq;
                break;                                               
            case DS4ButtonType.Up:                                   
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Pu : inputUp_2.Pu;
                break;                                               
            case DS4ButtonType.Down:                                 
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Pd : inputUp_2.Pd;
                break;                                               
            case DS4ButtonType.Left:                                 
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Pl : inputUp_2.Pl;
                break;                                               
            case DS4ButtonType.Right:                                
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Pr : inputUp_2.Pr;
                break;                                               
            case DS4ButtonType.L1:                                   
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.L1 : inputUp_2.L1;
                break;                                               
            case DS4ButtonType.L3:                                   
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.L3 : inputUp_2.L3;
                break;                                               
            case DS4ButtonType.R1:                                   
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.R1 : inputUp_2.R1;
                break;                                               
            case DS4ButtonType.R3:                                   
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.R3 : inputUp_2.R3;
                break;                                               
            case DS4ButtonType.OPTION:                               
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Op : inputUp_2.Op;
                break;                                                
            case DS4ButtonType.SHARE:                                
                buttonFlag = id == ControllerNum.P1 ? inputUp_1.Sh : inputUp_2.Sh;
                break;
            default:
                return false;
        }

        if(ds4InputCustom.MultithreadUpdate == false) { return ds4InputCustom.IsButtonUp(inputID, type); }

        if(ds4InputCustom.IsButton(inputID, type) == true)
        {
            if (buttonFlag == false) { buttonFlag = true; }
            result = false;
        }
        else
        {
            if (buttonFlag == true)
            {
                buttonFlag = false;
                result = true;
            }
            else
            {
                result = false;
            }
        }

        switch (type)
        {
            case DS4ButtonType.Circle:
                _ = id == ControllerNum.P1 ? inputUp_1.Ci = buttonFlag : inputUp_2.Ci = buttonFlag;
                break;
            case DS4ButtonType.Cross:
                _ = id == ControllerNum.P1 ? inputUp_1.Cr = buttonFlag : inputUp_2.Cr = buttonFlag;
                break;
            case DS4ButtonType.Triangle:
                _ = id == ControllerNum.P1 ? inputUp_1.Tr = buttonFlag : inputUp_2.Tr = buttonFlag;
                break;
            case DS4ButtonType.Square:
                _ = id == ControllerNum.P1 ? inputUp_1.Sq = buttonFlag : inputUp_2.Sq = buttonFlag;
                break;
            case DS4ButtonType.Up:
                _ = id == ControllerNum.P1 ? inputUp_1.Pu = buttonFlag : inputUp_2.Pu = buttonFlag;
                break;
            case DS4ButtonType.Down:
                _ = id == ControllerNum.P1 ? inputUp_1.Pd = buttonFlag : inputUp_2.Pd = buttonFlag;
                break;
            case DS4ButtonType.Left:
                _ = id == ControllerNum.P1 ? inputUp_1.Pl = buttonFlag : inputUp_2.Pl = buttonFlag;
                break;
            case DS4ButtonType.Right:
                _ = id == ControllerNum.P1 ? inputUp_1.Pr = buttonFlag : inputUp_2.Pr = buttonFlag;
                break;
            case DS4ButtonType.L1:
                _ = id == ControllerNum.P1 ? inputUp_1.L1 = buttonFlag : inputUp_2.L1 = buttonFlag;
                break;
            case DS4ButtonType.L3:
                _ = id == ControllerNum.P1 ? inputUp_1.L3 = buttonFlag : inputUp_2.L3 = buttonFlag;
                break;
            case DS4ButtonType.R1:
                _ = id == ControllerNum.P1 ? inputUp_1.R1 = buttonFlag : inputUp_2.R1 = buttonFlag;
                break;
            case DS4ButtonType.R3:
                _ = id == ControllerNum.P1 ? inputUp_1.R3 = buttonFlag : inputUp_2.R3 = buttonFlag;
                break;
            case DS4ButtonType.OPTION:
                _ = id == ControllerNum.P1 ? inputUp_1.Op = buttonFlag : inputUp_2.Op = buttonFlag;
                break;
            case DS4ButtonType.SHARE:
                _ = id == ControllerNum.P1 ? inputUp_1.Sh = buttonFlag : inputUp_2.Sh = buttonFlag;
                break;
        }

        return result;
    }

    /// <summary>
    /// DS4のボタン入力を検知する (複数のUpdate処理で呼び出そうとすると取得できない場合があります)
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="type">入力を検知したいボタンの種類
    /// <para>Example :　GetButtonDown(ControllerNum.P1, DS4ButtonKey.Circle)　1Pの〇ボタンの入力を検知する</para>
    /// </param>
    /// <returns></returns>
    private bool GetButtonDown(ControllerNum id, DS4ButtonType type)
    {
        if (useDS4 == false || ds4InputCustom == null) { return false; }

        bool buttonFlag;
        DS4ControllerType inputID;
        if(id == ControllerNum.P1)
        {
            inputID = DS4ControllerType.P1;
        }
        else if (id == ControllerNum.P2)
        {
            inputID = DS4ControllerType.P2;
        }
        else
        {
            return false;
        }
        bool result;

        // コントローラーが接続されているかチェック
        if (ds4InputCustom.IsController(inputID) == false) { return false; }

        switch (type)
        {
            case DS4ButtonType.Circle:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Ci : inputDown_2.Ci;
                break;
            case DS4ButtonType.Cross:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Cr : inputDown_2.Cr;
                break;
            case DS4ButtonType.Triangle:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Tr : inputDown_2.Tr;
                break;
            case DS4ButtonType.Square:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Sq : inputDown_2.Sq;
                break;
            case DS4ButtonType.Up:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Pu : inputDown_2.Pu;
                break;
            case DS4ButtonType.Down:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Pd : inputDown_2.Pd;
                break;
            case DS4ButtonType.Left:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Pl : inputDown_2.Pl;
                break;
            case DS4ButtonType.Right:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Pr : inputDown_2.Pr;
                break;
            case DS4ButtonType.L1:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.L1 : inputDown_2.L1;
                break;
            case DS4ButtonType.L3:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.L3 : inputDown_2.L3;
                break;
            case DS4ButtonType.R1:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.R1 : inputDown_2.R1;
                break;
            case DS4ButtonType.R3:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.R3 : inputDown_2.R3;
                break;
            case DS4ButtonType.OPTION:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Op : inputDown_2.Op;
                break;
            case DS4ButtonType.SHARE:
                buttonFlag = id == ControllerNum.P1 ? inputDown_1.Sh : inputDown_2.Sh;
                break;
            default:
                return false;
        }

        if (ds4InputCustom.MultithreadUpdate == false) { return ds4InputCustom.IsButtonDown(inputID, type); }

        if (ds4InputCustom.IsButton(inputID, type) == true)
        {
            if (buttonFlag == false)
            {
                buttonFlag = true;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            if (buttonFlag == true) { buttonFlag = false; }
            result = false;
        }

        switch (type)
        {
            case DS4ButtonType.Circle:
                _ = id == ControllerNum.P1 ? inputDown_1.Ci = buttonFlag : inputDown_2.Ci = buttonFlag;
                break;
            case DS4ButtonType.Cross:
                _ = id == ControllerNum.P1 ? inputDown_1.Cr = buttonFlag : inputDown_2.Cr = buttonFlag;
                break;
            case DS4ButtonType.Triangle:
                _ = id == ControllerNum.P1 ? inputDown_1.Tr = buttonFlag : inputDown_2.Tr = buttonFlag;
                break;
            case DS4ButtonType.Square:
                _ = id == ControllerNum.P1 ? inputDown_1.Sq = buttonFlag : inputDown_2.Sq = buttonFlag;
                break;
            case DS4ButtonType.Up:
                _ = id == ControllerNum.P1 ? inputDown_1.Pu = buttonFlag : inputDown_2.Pu = buttonFlag;
                break;
            case DS4ButtonType.Down:
                _ = id == ControllerNum.P1 ? inputDown_1.Pd = buttonFlag : inputDown_2.Pd = buttonFlag;
                break;
            case DS4ButtonType.Left:
                _ = id == ControllerNum.P1 ? inputDown_1.Pl = buttonFlag : inputDown_2.Pl = buttonFlag;
                break;
            case DS4ButtonType.Right:
                _ = id == ControllerNum.P1 ? inputDown_1.Pr = buttonFlag : inputDown_2.Pr = buttonFlag;
                break;
            case DS4ButtonType.L1:
                _ = id == ControllerNum.P1 ? inputDown_1.L1 = buttonFlag : inputDown_2.L1 = buttonFlag;
                break;
            case DS4ButtonType.L3:
                _ = id == ControllerNum.P1 ? inputDown_1.L3 = buttonFlag : inputDown_2.L3 = buttonFlag;
                break;
            case DS4ButtonType.R1:
                _ = id == ControllerNum.P1 ? inputDown_1.R1 = buttonFlag : inputDown_2.R1 = buttonFlag;
                break;
            case DS4ButtonType.R3:
                _ = id == ControllerNum.P1 ? inputDown_1.R3 = buttonFlag : inputDown_2.R3 = buttonFlag;
                break;
            case DS4ButtonType.OPTION:
                _ = id == ControllerNum.P1 ? inputDown_1.Op = buttonFlag : inputDown_2.Op = buttonFlag;
                break;
            case DS4ButtonType.SHARE:
                _ = id == ControllerNum.P1 ? inputDown_1.Sh = buttonFlag : inputDown_2.Sh = buttonFlag;
                break;
        }

        return result;
    }

    /// <summary>
    /// DS4のスティック・L2・R2ボタンの入力を検知する
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="type">入力を検知したいAxisの種類
    /// <para>Example :　GetAxisDown(ControllerNum.P1, DS4AxisKey.LeftStick_Up)　1PのLスティックの↑方向の入力を検知する</para>
    /// </param>
    /// <returns></returns>
    private bool GetAxisUp(ControllerNum id, DS4AxisKey type)
    {
        if (useDS4 == false || ds4InputCustom == null) { return false; }

        bool axisFlag;
        DS4ControllerType inputID;
        if (id == ControllerNum.P1)
        {
            inputID = DS4ControllerType.P1;
        }
        else if (id == ControllerNum.P2)
        {
            inputID = DS4ControllerType.P2;
        }
        else
        {
            return false;
        }
        DS4AxisType axisType;
        bool isPositive;
        bool result;

        // コントローラーが接続されているかチェック
        if (ds4InputCustom.IsController(inputID) == false) { return false; }

        switch (type)
        {
            case DS4AxisKey.LeftStick_Up:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Lu : inputUp_2.Lu;
                isPositive = false;
                axisType = DS4AxisType.LeftStickY;
                break;
            case DS4AxisKey.LeftStick_Down:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Ld : inputUp_2.Ld;
                isPositive = true;
                axisType = DS4AxisType.LeftStickY;
                break;
            case DS4AxisKey.LeftStick_Left:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Ll : inputUp_2.Ll;
                isPositive = false;
                axisType = DS4AxisType.LeftStickX;
                break;
            case DS4AxisKey.LeftStick_Right:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Lr : inputUp_2.Lr;
                isPositive = true;
                axisType = DS4AxisType.LeftStickX;
                break;
            case DS4AxisKey.RightStick_Up:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Ru : inputUp_2.Ru;
                isPositive = false;
                axisType = DS4AxisType.RightStickY;
                break;
            case DS4AxisKey.RightStick_Down:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Rd : inputUp_2.Rd;
                isPositive = true;
                axisType = DS4AxisType.RightStickY;
                break;
            case DS4AxisKey.RightStick_Left:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Rl : inputUp_2.Rl;
                isPositive = false;
                axisType = DS4AxisType.RightStickX;
                break;
            case DS4AxisKey.RightStick_Right:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.Rr : inputUp_2.Rr;
                isPositive = true;
                axisType = DS4AxisType.RightStickX;
                break;
            case DS4AxisKey.L2:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.L2 : inputUp_2.L2;
                isPositive = true;
                axisType = DS4AxisType.L2;
                break;
            case DS4AxisKey.R2:
                axisFlag = id == ControllerNum.P1 ? inputUp_1.R2 : inputUp_2.R2;
                isPositive = true;
                axisType = DS4AxisType.R2;
                break;
            default:
                return false;
        }

        if (_ = isPositive == true ? (ds4InputCustom.IsAxis(inputID, axisType) >= axisValue) : (ds4InputCustom.IsAxis(inputID, axisType) <= -axisValue))
        {
            if (axisFlag == false) { axisFlag = true; }
            result = false;  
        }
        else
        {
            if (axisFlag == true)
            {
                axisFlag = false;
                result = true;
            }
            else
            {
                result = false;
            }
        }

        switch (type)
        {
            case DS4AxisKey.LeftStick_Up:
                _ = id == ControllerNum.P1 ? inputUp_1.Lu = axisFlag : inputUp_2.Lu = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Down:
                _ = id == ControllerNum.P1 ? inputUp_1.Ld = axisFlag : inputUp_2.Ld = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Left:
                _ = id == ControllerNum.P1 ? inputUp_1.Ll = axisFlag : inputUp_2.Ll = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Right:
                _ = id == ControllerNum.P1 ? inputUp_1.Lr = axisFlag : inputUp_2.Lr = axisFlag;
                break;
            case DS4AxisKey.RightStick_Up:
                _ = id == ControllerNum.P1 ? inputUp_1.Ru = axisFlag : inputUp_2.Ru = axisFlag;
                break;
            case DS4AxisKey.RightStick_Down:
                _ = id == ControllerNum.P1 ? inputUp_1.Rd = axisFlag : inputUp_2.Rd = axisFlag;
                break;
            case DS4AxisKey.RightStick_Left:
                _ = id == ControllerNum.P1 ? inputUp_1.Rl = axisFlag : inputUp_2.Rl = axisFlag;
                break;
            case DS4AxisKey.RightStick_Right:
                _ = id == ControllerNum.P1 ? inputUp_1.Rr = axisFlag : inputUp_2.Rr = axisFlag;
                break;
            case DS4AxisKey.L2:
                _ = id == ControllerNum.P1 ? inputUp_1.L2 = axisFlag : inputUp_2.L2 = axisFlag;
                break;
            case DS4AxisKey.R2:
                _ = id == ControllerNum.P1 ? inputUp_1.R2 = axisFlag : inputUp_2.R2 = axisFlag;
                break;
        }

        return result;
    }

    /// <summary>
    /// DS4のスティック・L2・R2ボタンの入力を検知する
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="type">入力を検知したいAxisの種類
    /// <para>Example :　GetAxisDown(ControllerNum.P1, DS4AxisKey.LeftStick_Up)　1PのLスティックの↑方向の入力を検知する</para>
    /// </param>
    /// <returns></returns>
    private bool GetAxisDown(ControllerNum id, DS4AxisKey type)
    {
        if (useDS4 == false || ds4InputCustom == null) { return false; }

        bool axisFlag;
        DS4ControllerType inputID;
        if (id == ControllerNum.P1)
        {
            inputID = DS4ControllerType.P1;
        }
        else if (id == ControllerNum.P2)
        {
            inputID = DS4ControllerType.P2;
        }
        else
        {
            return false;
        }
        DS4AxisType axisType;
        bool isPositive;
        bool result;

        // コントローラーが接続されているかチェック
        if (ds4InputCustom.IsController(inputID) == false) { return false; }

        switch (type)
        {
            case DS4AxisKey.LeftStick_Up:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Lu : inputDown_2.Lu;
                isPositive = false;
                axisType = DS4AxisType.LeftStickY;
                break;
            case DS4AxisKey.LeftStick_Down:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Ld : inputDown_2.Ld;
                isPositive = true;
                axisType = DS4AxisType.LeftStickY;
                break;
            case DS4AxisKey.LeftStick_Left:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Ll : inputDown_2.Ll;
                isPositive = false;
                axisType = DS4AxisType.LeftStickX;
                break;
            case DS4AxisKey.LeftStick_Right:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Lr : inputDown_2.Lr;
                isPositive = true;
                axisType = DS4AxisType.LeftStickX;
                break;
            case DS4AxisKey.RightStick_Up:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Ru : inputDown_2.Ru;
                isPositive = false;
                axisType = DS4AxisType.RightStickY;
                break;
            case DS4AxisKey.RightStick_Down:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Rd : inputDown_2.Rd;
                isPositive = true;
                axisType = DS4AxisType.RightStickY;
                break;
            case DS4AxisKey.RightStick_Left:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Rl : inputDown_2.Rl;
                isPositive = false;
                axisType = DS4AxisType.RightStickX;
                break;
            case DS4AxisKey.RightStick_Right:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.Rr : inputDown_2.Rr;
                isPositive = true;
                axisType = DS4AxisType.RightStickX;
                break;
            case DS4AxisKey.L2:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.L2 : inputDown_2.L2;
                isPositive = true;
                axisType = DS4AxisType.L2;
                break;
            case DS4AxisKey.R2:
                axisFlag = id == ControllerNum.P1 ? inputDown_1.R2 : inputDown_2.R2;
                isPositive = true;
                axisType = DS4AxisType.R2;
                break;
            default:
                return false;
        }

        if(_ = isPositive == true ? (ds4InputCustom.IsAxis(inputID, axisType) >= axisValue) : (ds4InputCustom.IsAxis(inputID, axisType) <= -axisValue))
        {
            if (axisFlag == false)
            {
                axisFlag = true;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            if (axisFlag == true) { axisFlag = false; }
            result = false;
        }

        switch (type)
        {
            case DS4AxisKey.LeftStick_Up:
                _ = id == ControllerNum.P1 ? inputDown_1.Lu = axisFlag : inputDown_2.Lu = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Down:
                _ = id == ControllerNum.P1 ? inputDown_1.Ld = axisFlag : inputDown_2.Ld = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Left:
                _ = id == ControllerNum.P1 ? inputDown_1.Ll = axisFlag : inputDown_2.Ll = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Right:
                _ = id == ControllerNum.P1 ? inputDown_1.Lr = axisFlag : inputDown_2.Lr = axisFlag;
                break;
            case DS4AxisKey.RightStick_Up:
                _ = id == ControllerNum.P1 ? inputDown_1.Ru = axisFlag : inputDown_2.Ru = axisFlag;
                break;
            case DS4AxisKey.RightStick_Down:
                _ = id == ControllerNum.P1 ? inputDown_1.Rd = axisFlag : inputDown_2.Rd = axisFlag;
                break;
            case DS4AxisKey.RightStick_Left:
                _ = id == ControllerNum.P1 ? inputDown_1.Rl = axisFlag : inputDown_2.Rl = axisFlag;
                break;
            case DS4AxisKey.RightStick_Right:
                _ = id == ControllerNum.P1 ? inputDown_1.Rr = axisFlag : inputDown_2.Rr = axisFlag;
                break;
            case DS4AxisKey.L2:
                _ = id == ControllerNum.P1 ? inputDown_1.L2 = axisFlag : inputDown_2.L2 = axisFlag;
                break;
            case DS4AxisKey.R2:
                _ = id == ControllerNum.P1 ? inputDown_1.R2 = axisFlag : inputDown_2.R2 = axisFlag;
                break;
        }

        return result;
    }

    /// <summary>
    /// StandaloneInputModuleをPS4入力に対応させる
    /// </summary>
    /// <param name="id">入力を許可するプレイヤー</param>
    public void SetInputModule(ControllerNum id)
    {
        // StandaloneInputModuleが無ければ処理を終了
        if (inputModule == null) { return; }

        if (ds4InputCustom.IsController(DS4ControllerType.P2) == false)
        {
            inputModule.submitButton = joy2KeyName.Submit;
            inputModule.cancelButton = joy2KeyName.Cancel;
            inputModule.horizontalAxis = joy2KeyName.Horizontal;
            inputModule.verticalAxis = joy2KeyName.Vertical;
            return;
        }

        // PS4入力に対応させる
        inputModule.submitButton = id == ControllerNum.P1 ? joy1KeyName.Submit : joy2KeyName.Submit;
        inputModule.cancelButton = id == ControllerNum.P1 ? joy1KeyName.Cancel : joy2KeyName.Cancel;
        inputModule.horizontalAxis = id == ControllerNum.P1 ? joy1KeyName.Horizontal : joy2KeyName.Horizontal;
        inputModule.verticalAxis = id == ControllerNum.P1 ? joy1KeyName.Vertical : joy2KeyName.Vertical;
    }

    /// <summary>
    /// DS4を振動させる
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="vibration">振動値</param>
    public void SetVibration(ControllerNum id, byte vibration)
    {
        DS4ControllerType type = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;

        if(ds4InputCustom.IsController(type) == false || ds4InputCustom == null) { return; }

        ds4InputCustom.SetVibration(type, new DS4Vibration(vibration, vibration));
    }

    /// <summary>
    /// DS4の振動を停止させる
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    public void StopVibration(ControllerNum id)
    {
        DS4ControllerType type = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;

        if (ds4InputCustom.IsController(type) == false || ds4InputCustom == null) { return; }

        ds4InputCustom.SetVibration(type, DS4Vibration.Min);
    }
}