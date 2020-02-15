using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4InputCustom;

public enum ControllerNum
{
    P1,
    P2
}

public enum DS4AllKeyType
{
    Circle,
    Cross,
    Triangle,
    Square,
    Up,
    Down,
    Left,
    Right,
    LeftStick_Up,
    LeftStick_Down,
    LeftStick_Left,
    LeftStick_Right,
    RightStick_Up,
    RightStick_Down,
    RightStick_Left,
    RightStick_Right,
    L1,
    L2,
    L3,
    R1,
    R2,
    R3,
    OPTION,
    SHARE
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
            if (Instance.useDS4)
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
            else
            {
                if (inputPlayer == ControllerNum.P1)
                {
                    Submit = "Submit_1";
                    Cancel = "Cancel_1";
                    Horizontal = "Horizontal_1";
                    Vertical = "Vertical_1";
                }
                else
                {
                    Submit = "Submit_2";
                    Cancel = "Cancel_2";
                    Horizontal = "Horizontal_2";
                    Vertical = "Vertical_2";
                }
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
        SingleInputForDS4(ControllerNum.P1);
        SingleInputForDS4(ControllerNum.P2);
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
        inputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        if(ds4InputCustom == null && Instance == this)
        {
            ds4InputCustom = gameObject.AddComponent<DS4InputCustom.DS4InputCustom>();
            ds4InputCustom.MultithreadUpdate = multithreadUpdate;
            ds4InputCustom.Init();
        }
    }

    /// <summary>
    /// DS4のシングル入力を取得
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    private void SingleInputForDS4(ControllerNum id)
    {
        DS4InputKey inputDown = new DS4InputKey();
        DS4InputKey inputUp = new DS4InputKey();

        // downを取得
        inputDown.Circle = GetDS4KeyUpOrDown(id, DS4AllKeyType.Circle, true);
        inputDown.Cross = GetDS4KeyUpOrDown(id, DS4AllKeyType.Cross, true);
        inputDown.Triangle = GetDS4KeyUpOrDown(id, DS4AllKeyType.Triangle, true);
        inputDown.Square = GetDS4KeyUpOrDown(id, DS4AllKeyType.Square, true);
        inputDown.Up = GetDS4KeyUpOrDown(id, DS4AllKeyType.Up, true);
        inputDown.Down = GetDS4KeyUpOrDown(id, DS4AllKeyType.Down, true);
        inputDown.Left = GetDS4KeyUpOrDown(id, DS4AllKeyType.Left, true);
        inputDown.Right = GetDS4KeyUpOrDown(id, DS4AllKeyType.Right, true);
        inputDown.LeftStick_Up = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Up, true);
        inputDown.LeftStick_Down = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Down, true);
        inputDown.LeftStick_Left = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Left, true);
        inputDown.LeftStick_Right = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Right, true);
        inputDown.L1 = GetDS4KeyUpOrDown(id, DS4AllKeyType.L1, true);
        inputDown.L2 = GetDS4KeyUpOrDown(id, DS4AllKeyType.L2, true);
        inputDown.L3 = GetDS4KeyUpOrDown(id, DS4AllKeyType.L3, true);
        inputDown.RightStick_Up = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Up, true);
        inputDown.RightStick_Down = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Down, true);
        inputDown.RightStick_Left = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Left, true);
        inputDown.RightStick_Right = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Right, true);
        inputDown.R1 = GetDS4KeyUpOrDown(id, DS4AllKeyType.R1, true);
        inputDown.R2 = GetDS4KeyUpOrDown(id, DS4AllKeyType.R2, true);
        inputDown.R3 = GetDS4KeyUpOrDown(id, DS4AllKeyType.R3, true);
        inputDown.OPTION = GetDS4KeyUpOrDown(id, DS4AllKeyType.OPTION, true);
        inputDown.SHARE = GetDS4KeyUpOrDown(id, DS4AllKeyType.SHARE, true);

        // upを取得
        inputUp.Circle = GetDS4KeyUpOrDown(id, DS4AllKeyType.Circle, false);
        inputUp.Cross = GetDS4KeyUpOrDown(id, DS4AllKeyType.Cross, false);
        inputUp.Triangle = GetDS4KeyUpOrDown(id, DS4AllKeyType.Triangle, false);
        inputUp.Square = GetDS4KeyUpOrDown(id, DS4AllKeyType.Square, false);
        inputUp.Up = GetDS4KeyUpOrDown(id, DS4AllKeyType.Up, false);
        inputUp.Down = GetDS4KeyUpOrDown(id, DS4AllKeyType.Down, false);
        inputUp.Left = GetDS4KeyUpOrDown(id, DS4AllKeyType.Left, false);
        inputUp.Right = GetDS4KeyUpOrDown(id, DS4AllKeyType.Right, false);
        inputUp.LeftStick_Up = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Up, false);
        inputUp.LeftStick_Down = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Down, false);
        inputUp.LeftStick_Left = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Left, false);
        inputUp.LeftStick_Right = GetDS4KeyUpOrDown(id, DS4AllKeyType.LeftStick_Right, false);
        inputUp.L1 = GetDS4KeyUpOrDown(id, DS4AllKeyType.L1, false);
        inputUp.L2 = GetDS4KeyUpOrDown(id, DS4AllKeyType.L2, false);
        inputUp.L3 = GetDS4KeyUpOrDown(id, DS4AllKeyType.L3, false);
        inputUp.RightStick_Up = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Up, false);
        inputUp.RightStick_Down = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Down, false);
        inputUp.RightStick_Left = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Left, false);
        inputUp.RightStick_Right = GetDS4KeyUpOrDown(id, DS4AllKeyType.RightStick_Right, false);
        inputUp.R1 = GetDS4KeyUpOrDown(id, DS4AllKeyType.R1, false);
        inputUp.R2 = GetDS4KeyUpOrDown(id, DS4AllKeyType.R2, false);
        inputUp.R3 = GetDS4KeyUpOrDown(id, DS4AllKeyType.R3, false);
        inputUp.OPTION = GetDS4KeyUpOrDown(id, DS4AllKeyType.OPTION, false);
        inputUp.SHARE = GetDS4KeyUpOrDown(id, DS4AllKeyType.SHARE, false);

        // 値をreturn
        _ = id == ControllerNum.P1 ? GetKeyDown_1 = inputDown : GetKeyDown_2 = inputDown;
        _ = id == ControllerNum.P1 ? GetKeyUp_1 = inputUp : GetKeyUp_2 = inputUp;
    }

    /// <summary>
    /// DS4のキー入力をboolで取得
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="type">DS4の取得したいキー</param>
    /// <returns></returns>
    public bool GetDS4Key(ControllerNum id, DS4AllKeyType type)
    {
        if(useDS4 == false) { return false; }

        DS4ControllerType controller = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;

        // キーボード用の入力
        KeyCode key;
        bool secondInput;

        if (type == DS4AllKeyType.Circle || type == DS4AllKeyType.Cross || type == DS4AllKeyType.Triangle || type == DS4AllKeyType.Square || type == DS4AllKeyType.Up || type == DS4AllKeyType.Down || type == DS4AllKeyType.Left || type == DS4AllKeyType.Right || type == DS4AllKeyType.L1 || type == DS4AllKeyType.L3 || type == DS4AllKeyType.R1 || type == DS4AllKeyType.R3 || type == DS4AllKeyType.OPTION || type == DS4AllKeyType.SHARE)
        {
            DS4ButtonType buttonName;
            switch (type)
            {
                case DS4AllKeyType.Circle:
                    buttonName = DS4ButtonType.Circle;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha1 : KeyCode.Z;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Cross:
                    buttonName = DS4ButtonType.Cross;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha2 : KeyCode.X;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Triangle:
                    buttonName = DS4ButtonType.Triangle;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha3 : KeyCode.C;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Square:
                    buttonName = DS4ButtonType.Square;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha4 : KeyCode.V;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Up:
                    buttonName = DS4ButtonType.Up;
                    key = id == ControllerNum.P1 ? KeyCode.W : KeyCode.UpArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Down:
                    buttonName = DS4ButtonType.Down;
                    key = id == ControllerNum.P1 ? KeyCode.S : KeyCode.DownArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Left:
                    buttonName = DS4ButtonType.Left;
                    key = id == ControllerNum.P1 ? KeyCode.A : KeyCode.LeftArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.Right:
                    buttonName = DS4ButtonType.Right;
                    key = id == ControllerNum.P1 ? KeyCode.D : KeyCode.RightArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.L1:
                    buttonName = DS4ButtonType.L1;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha5 : KeyCode.B;
                    secondInput = false;
                    break;
                case DS4AllKeyType.L3:
                    buttonName = DS4ButtonType.L3;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha5 : KeyCode.B;
                    secondInput = true;
                    break;
                case DS4AllKeyType.R1:
                    buttonName = DS4ButtonType.R1;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha6 : KeyCode.N;
                    secondInput = false;
                    break;
                case DS4AllKeyType.R3:
                    buttonName = DS4ButtonType.R3;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha6 : KeyCode.N;
                    secondInput = true;
                    break;
                case DS4AllKeyType.OPTION:
                    buttonName = DS4ButtonType.OPTION;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha7 : KeyCode.M;
                    secondInput = false;
                    break;
                case DS4AllKeyType.SHARE:
                    buttonName = DS4ButtonType.SHARE;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha8 : KeyCode.L;
                    secondInput = false;
                    break;
                default:
                    return false;
            }

            // コントローラーが接続されているかチェック
            if (ds4InputCustom.IsController(controller) == true)
            {
                return ds4InputCustom.IsButton(controller, buttonName);
            }
            else
            {
                if(secondInput == true)
                {
                    if (id == ControllerNum.P1 ? (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(key) == true) : (Input.GetKey(KeyCode.RightShift) == true && Input.GetKey(key) == true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return Input.GetKey(key);
                }
            }
        }
        else
        {
            DS4AxisType axisName;
            bool isPositive;
            switch (type)
            {
                case DS4AllKeyType.LeftStick_Up:
                    axisName = DS4AxisType.LeftStickY;
                    isPositive = false;
                    key = id == ControllerNum.P1 ? KeyCode.W : KeyCode.UpArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.LeftStick_Down:
                    axisName = DS4AxisType.LeftStickY;
                    isPositive = true;
                    key = id == ControllerNum.P1 ? KeyCode.S : KeyCode.DownArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.LeftStick_Left:
                    axisName = DS4AxisType.LeftStickX;
                    isPositive = false;
                    key = id == ControllerNum.P1 ? KeyCode.A : KeyCode.LeftArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.LeftStick_Right:
                    axisName = DS4AxisType.LeftStickX;
                    isPositive = true;
                    key = id == ControllerNum.P1 ? KeyCode.D : KeyCode.RightArrow;
                    secondInput = false;
                    break;
                case DS4AllKeyType.L2:
                    axisName = DS4AxisType.L2;
                    isPositive = true;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha5 : KeyCode.B;
                    secondInput = false;
                    break;
                case DS4AllKeyType.RightStick_Up:
                    axisName = DS4AxisType.RightStickY;
                    isPositive = false;
                    key = id == ControllerNum.P1 ? KeyCode.W : KeyCode.UpArrow;
                    secondInput = true;
                    break;
                case DS4AllKeyType.RightStick_Down:
                    axisName = DS4AxisType.RightStickY;
                    isPositive = true;
                    key = id == ControllerNum.P1 ? KeyCode.S : KeyCode.DownArrow;
                    secondInput = true;
                    break;
                case DS4AllKeyType.RightStick_Left:
                    axisName = DS4AxisType.RightStickX;
                    isPositive = false;
                    key = id == ControllerNum.P1 ? KeyCode.A : KeyCode.LeftArrow;
                    secondInput = true;
                    break;
                case DS4AllKeyType.RightStick_Right:
                    axisName = DS4AxisType.RightStickX;
                    isPositive = true;
                    key = id == ControllerNum.P1 ? KeyCode.D : KeyCode.RightArrow;
                    secondInput = true;
                    break;
                case DS4AllKeyType.R2:
                    axisName = DS4AxisType.R2;
                    isPositive = true;
                    key = id == ControllerNum.P1 ? KeyCode.Alpha6 : KeyCode.N;
                    secondInput = false;
                    break;
                default:
                    return false;
            }

            // コントローラーが接続されているかチェック
            if (ds4InputCustom.IsController(controller) == true)
            {
                if (_ = isPositive == true ? (ds4InputCustom.IsAxis(controller, axisName) >= axisValue) : (ds4InputCustom.IsAxis(controller, axisName) <= -axisValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (secondInput == true)
                {
                    if (id == ControllerNum.P1 ? (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(key) == true) : (Input.GetKey(KeyCode.RightShift) == true && Input.GetKey(key) == true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return Input.GetKey(key);
                }
            }
        }
    }

    /// <summary>
    /// DS4のキーが入力されたタイミングまたはキーを離したタイミングをboolで取得
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="isGetDown"></param>
    /// <returns></returns>
    private bool GetDS4KeyUpOrDown(ControllerNum id, DS4AllKeyType type, bool isGetDown)
    {
        switch (type)
        {
            case DS4AllKeyType.Circle:
                if(GetDS4Key(id, type) == true)
                {
                    if(isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Ci == false : inputDown_2.Ci == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Ci = true : inputDown_2.Ci = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Ci = true : inputUp_2.Ci = true;
                    }
                }
                else
                {
                    if(isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Ci = false : inputDown_2.Ci = false;
                    }
                    else
                    {
                        if(id == ControllerNum.P1 ? inputUp_1.Ci == true : inputUp_2.Ci == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Ci = false : inputUp_2.Ci = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Cross:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Cr == false : inputDown_2.Cr == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Cr = true : inputDown_2.Cr = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Cr = true : inputUp_2.Cr = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Cr = false : inputDown_2.Cr = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Cr == true : inputUp_2.Cr == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Cr = false : inputUp_2.Cr = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Triangle:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Tr == false : inputDown_2.Tr == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Tr = true : inputDown_2.Tr = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Tr = true : inputUp_2.Tr = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Tr = false : inputDown_2.Tr = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Tr == true : inputUp_2.Tr == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Tr = false : inputUp_2.Tr = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Square:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Sq == false : inputDown_2.Sq == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Sq = true : inputDown_2.Sq = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Sq = true : inputUp_2.Sq = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Sq = false : inputDown_2.Sq = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Sq == true : inputUp_2.Sq == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Sq = false : inputUp_2.Sq = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Up:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Pu == false : inputDown_2.Pu == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Pu = true : inputDown_2.Pu = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Pu = true : inputUp_2.Pu = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Pu = false : inputDown_2.Pu = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Pu == true : inputUp_2.Pu == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Pu = false : inputUp_2.Pu = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Down:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Pd == false : inputDown_2.Pd == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Pd = true : inputDown_2.Pd = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Pd = true : inputUp_2.Pd = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Pd = false : inputDown_2.Pd = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Pd == true : inputUp_2.Pd == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Pd = false : inputUp_2.Pd = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Left:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Pl == false : inputDown_2.Pl == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Pl = true : inputDown_2.Pl = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Pl = true : inputUp_2.Pl = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Pl = false : inputDown_2.Pl = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Pl == true : inputUp_2.Pl == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Pl = false : inputUp_2.Pl = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.Right:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Pr == false : inputDown_2.Pr == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Pr = true : inputDown_2.Pr = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Pr = true : inputUp_2.Pr = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Pr = false : inputDown_2.Pr = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Pr == true : inputUp_2.Pr == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Pr = false : inputUp_2.Pr = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.L1:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.L1 == false : inputDown_2.L1 == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.L1 = true : inputDown_2.L1 = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.L1 = true : inputUp_2.L1 = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.L1 = false : inputDown_2.L1 = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.L1 == true : inputUp_2.L1 == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.L1 = false : inputUp_2.L1 = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.L3:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.L3 == false : inputDown_2.L3 == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.L3 = true : inputDown_2.L3 = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.L3 = true : inputUp_2.L3 = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.L3 = false : inputDown_2.L3 = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.L3 == true : inputUp_2.L3 == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.L3 = false : inputUp_2.L3 = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.R1:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.R1 == false : inputDown_2.R1 == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.R1 = true : inputDown_2.R1 = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.R1 = true : inputUp_2.R1 = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.R1 = false : inputDown_2.R1 = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.R1 == true : inputUp_2.R1 == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.R1 = false : inputUp_2.R1 = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.R3:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.R3 == false : inputDown_2.R3 == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.R3 = true : inputDown_2.R3 = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.R3 = true : inputUp_2.R3 = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.R3 = false : inputDown_2.R3 = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.R3 == true : inputUp_2.R3 == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.R3 = false : inputUp_2.R3 = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.OPTION:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Op == false : inputDown_2.Op == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Op = true : inputDown_2.Op = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Op = true : inputUp_2.Op = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Op = false : inputDown_2.Op = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Op == true : inputUp_2.Op == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Op = false : inputUp_2.Op = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.SHARE:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Sh == false : inputDown_2.Sh == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Sh = true : inputDown_2.Sh = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Sh = true : inputUp_2.Sh = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Sh = false : inputDown_2.Sh = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Sh == true : inputUp_2.Sh == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Sh = false : inputUp_2.Sh = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.LeftStick_Up:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Lu == false : inputDown_2.Lu == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Lu = true : inputDown_2.Lu = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Lu = true : inputUp_2.Lu = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Lu = false : inputDown_2.Lu = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Lu == true : inputUp_2.Lu == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Lu = false : inputUp_2.Lu = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.LeftStick_Down:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Ld == false : inputDown_2.Ld == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Ld = true : inputDown_2.Ld = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Ld = true : inputUp_2.Ld = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Ld = false : inputDown_2.Ld = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Ld == true : inputUp_2.Ld == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Ld = false : inputUp_2.Ld = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.LeftStick_Left:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Ll == false : inputDown_2.Ll == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Ll = true : inputDown_2.Ll = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Ll = true : inputUp_2.Ll = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Ll = false : inputDown_2.Ll = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Ll == true : inputUp_2.Ll == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Ll = false : inputUp_2.Ll = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.LeftStick_Right:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Lr == false : inputDown_2.Lr == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Lr = true : inputDown_2.Lr = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Lr = true : inputUp_2.Lr = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Lr = false : inputDown_2.Lr = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Lr == true : inputUp_2.Lr == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Lr = false : inputUp_2.Lr = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.L2:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.L2 == false : inputDown_2.L2 == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.L2 = true : inputDown_2.L2 = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.L2 = true : inputUp_2.L2 = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.L2 = false : inputDown_2.L2 = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.L2 == true : inputUp_2.L2 == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.L2 = false : inputUp_2.L2 = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.RightStick_Up:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Ru == false : inputDown_2.Ru == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Ru = true : inputDown_2.Ru = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Ru = true : inputUp_2.Ru = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Ru = false : inputDown_2.Ru = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Ru == true : inputUp_2.Ru == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Ru = false : inputUp_2.Ru = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.RightStick_Down:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Rd == false : inputDown_2.Rd == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Rd = true : inputDown_2.Rd = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Rd = true : inputUp_2.Rd = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Rd = false : inputDown_2.Rd = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Rd == true : inputUp_2.Rd == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Rd = false : inputUp_2.Rd = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.RightStick_Left:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Rl == false : inputDown_2.Rl == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Rl = true : inputDown_2.Rl = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Rl = true : inputUp_2.Rl = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Rl = false : inputDown_2.Rl = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Rl == true : inputUp_2.Rl == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Rl = false : inputUp_2.Rl = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.RightStick_Right:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.Rr == false : inputDown_2.Rr == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.Rr = true : inputDown_2.Rr = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.Rr = true : inputUp_2.Rr = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.Rr = false : inputDown_2.Rr = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.Rr == true : inputUp_2.Rr == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.Rr = false : inputUp_2.Rr = false;
                            return true;
                        }
                    }
                }
                return false;
            case DS4AllKeyType.R2:
                if (GetDS4Key(id, type) == true)
                {
                    if (isGetDown == true)
                    {
                        if (id == ControllerNum.P1 ? inputDown_1.R2 == false : inputDown_2.R2 == false)
                        {
                            _ = id == ControllerNum.P1 ? inputDown_1.R2 = true : inputDown_2.R2 = true;
                            return true;
                        }
                    }
                    else
                    {
                        _ = id == ControllerNum.P1 ? inputUp_1.R2 = true : inputUp_2.R2 = true;
                    }
                }
                else
                {
                    if (isGetDown == true)
                    {
                        _ = id == ControllerNum.P1 ? inputDown_1.R2 = false : inputDown_2.R2 = false;
                    }
                    else
                    {
                        if (id == ControllerNum.P1 ? inputUp_1.R2 == true : inputUp_2.R2 == true)
                        {
                            _ = id == ControllerNum.P1 ? inputUp_1.R2 = false : inputUp_2.R2 = false;
                            return true;
                        }
                    }
                }
                return false;
            default:
                return false;
        }
    }

    /// <summary>
    /// DS4のキーが何か入力されているかを取得
    /// </summary>
    /// <param name="id"></param>
    public bool GetDS4AnyKey(ControllerNum id)
    {
        if(GetDS4Key(id, DS4AllKeyType.Circle) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Cross) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Triangle) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Square) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Up) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Down) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Left) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.Right) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.LeftStick_Up) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.LeftStick_Down) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.LeftStick_Left) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.LeftStick_Right) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.RightStick_Up) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.RightStick_Down) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.RightStick_Left) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.RightStick_Right) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.L1) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.L2) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.L3) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.R1) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.R2) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.R3) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.OPTION) == true)
        {
            return true;
        }

        if (GetDS4Key(id, DS4AllKeyType.SHARE) == true)
        {
            return true;
        }

        return false;
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