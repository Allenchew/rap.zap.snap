using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4InputCustom;

public enum ControllerNum
{
    P1,
    P2
}

public enum DS4ButtonKey
{
    Circle,
    Cross,
    Triangle,
    Square,
    Up,
    Down,
    Left,
    Right,
    L1,
    L3,
    R1,
    R3,
    OPTION,
    SHARE
}

public enum DS4AxisKey
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

public struct DS4InputDownKey
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

public class GamePadControl : MonoBehaviour
{
    public static GamePadControl Instance { private set; get; } = null;

    [SerializeField, Tooltip("DS4を使う")]
    private bool usePs4Controller = true;

    [SerializeField, Tooltip("DS4のスティック系の有効入力感度"), Range(0.01f, 1.0f)]
    private float axisValue = 0.8f;

    private DS4InputCustom.DS4InputCustom ds4Input = null;

    private UnityEngine.EventSystems.StandaloneInputModule inputModule;

    // コルーチン用のフラグ
    private bool isRunning = false;

    public DS4InputDownKey Input_1 { private set; get; } = new DS4InputDownKey();
    public DS4InputDownKey Input_2 { private set; get; } = new DS4InputDownKey();

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
    private struct AxisFlag
    {
        public bool Ci, Cr, Tr, Sq, Pu, Pd, Pl, Pr, L1, L2, L3, Lu, Ld, Ll, Lr, R1, R2, R3, Ru, Rd, Rl, Rr, Op, Sh;

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

    private AxisFlag firstAxisFlag;
    private AxisFlag secondAxisFlag;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            firstAxisFlag.ResetFlag();
            secondAxisFlag.ResetFlag();
            joy1KeyName = new DS4KeyName(ControllerNum.P1);
            joy2KeyName = new DS4KeyName(ControllerNum.P2);
            Input_1.ResetFlag();
            Input_2.ResetFlag();
            inputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            ds4Input = GetComponent<DS4InputCustom.DS4InputCustom>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetInputModule(ControllerNum.P1);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 2; i++) { SetDS4Input((ControllerNum)i); }
    }

    private void SetDS4Input(ControllerNum id)
    {
        DS4InputDownKey input = id == ControllerNum.P1 ? Input_1 : Input_2;
        input.Circle = GetButtonDown(id, DS4ButtonKey.Circle);
        input.Cross = GetButtonDown(id, DS4ButtonKey.Cross);
        input.Triangle = GetButtonDown(id, DS4ButtonKey.Triangle);
        input.Square = GetButtonDown(id, DS4ButtonKey.Square);
        input.Up = GetButtonDown(id, DS4ButtonKey.Up);
        input.Down = GetButtonDown(id, DS4ButtonKey.Down);
        input.Left = GetButtonDown(id, DS4ButtonKey.Left);
        input.Right = GetButtonDown(id, DS4ButtonKey.Right);
        input.LeftStick_Up = GetAxisDown(id, DS4AxisKey.LeftStick_Up);
        input.LeftStick_Down = GetAxisDown(id, DS4AxisKey.LeftStick_Down);
        input.LeftStick_Left = GetAxisDown(id, DS4AxisKey.LeftStick_Left);
        input.LeftStick_Right = GetAxisDown(id, DS4AxisKey.LeftStick_Right);
        input.RightStick_Up = GetAxisDown(id, DS4AxisKey.RightStick_Up);
        input.RightStick_Down = GetAxisDown(id, DS4AxisKey.RightStick_Down);
        input.RightStick_Left = GetAxisDown(id, DS4AxisKey.RightStick_Left);
        input.RightStick_Right = GetAxisDown(id, DS4AxisKey.RightStick_Right);
        input.L1 = GetButtonDown(id, DS4ButtonKey.L1);
        input.L2 = GetAxisDown(id, DS4AxisKey.L2);
        input.L3 = GetButtonDown(id, DS4ButtonKey.L3);
        input.R1 = GetButtonDown(id, DS4ButtonKey.R1);
        input.R2 = GetAxisDown(id, DS4AxisKey.R2);
        input.R3 = GetButtonDown(id, DS4ButtonKey.R3);
        input.OPTION = GetButtonDown(id, DS4ButtonKey.OPTION);
        input.SHARE = GetButtonDown(id, DS4ButtonKey.SHARE);
        _ = id == ControllerNum.P1 ? Input_1 = input : Input_2 = input;
    }

    /// <summary>
    /// DS4のボタン入力を検知する (複数のUpdate処理で呼び出そうとすると取得できない場合があります)
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="type">入力を検知したいボタンの種類
    /// <para>Example :　GetButtonDown(ControllerNum.P1, DS4ButtonKey.Circle)　1Pの〇ボタンの入力を検知する</para>
    /// </param>
    /// <returns></returns>
    private bool GetButtonDown(ControllerNum id, DS4ButtonKey type)
    {
        if(usePs4Controller == false) { return false; }

        bool buttonFlag;
        DS4ControllerType inputID = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;
        DS4ButtonType buttonType;
        bool result;

        // コントローラーが接続されているかチェック
        if (ds4Input.IsController(inputID) == false) { return false; }

        switch (type)
        {
            case DS4ButtonKey.Circle:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Ci : secondAxisFlag.Ci;
                buttonType = DS4ButtonType.Circle;
                break;
            case DS4ButtonKey.Cross:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Cr : secondAxisFlag.Cr;
                buttonType = DS4ButtonType.Cross;
                break;
            case DS4ButtonKey.Triangle:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Tr : secondAxisFlag.Tr;
                buttonType = DS4ButtonType.Triangle;
                break;
            case DS4ButtonKey.Square:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Sq : secondAxisFlag.Sq;
                buttonType = DS4ButtonType.Square;
                break;
            case DS4ButtonKey.Up:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Pu : secondAxisFlag.Pu;
                buttonType = DS4ButtonType.Up;
                break;
            case DS4ButtonKey.Down:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Pd : secondAxisFlag.Pd;
                buttonType = DS4ButtonType.Down;
                break;
            case DS4ButtonKey.Left:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Pl : secondAxisFlag.Pl;
                buttonType = DS4ButtonType.Left;
                break;
            case DS4ButtonKey.Right:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Pr : secondAxisFlag.Pr;
                buttonType = DS4ButtonType.Right;
                break;
            case DS4ButtonKey.L1:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.L1 : secondAxisFlag.L1;
                buttonType = DS4ButtonType.L1;
                break;
            case DS4ButtonKey.L3:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.L3 : secondAxisFlag.L3;
                buttonType = DS4ButtonType.L3;
                break;
            case DS4ButtonKey.R1:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.R1 : secondAxisFlag.R1;
                buttonType = DS4ButtonType.R1;
                break;
            case DS4ButtonKey.R3:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.R3 : secondAxisFlag.R3;
                buttonType = DS4ButtonType.R3;
                break;
            case DS4ButtonKey.OPTION:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Op : secondAxisFlag.Op;
                buttonType = DS4ButtonType.OPTION;
                break;
            case DS4ButtonKey.SHARE:
                buttonFlag = id == ControllerNum.P1 ? firstAxisFlag.Sh : secondAxisFlag.Sh;
                buttonType = DS4ButtonType.SHARE;
                break;
            default:
                return false;
        }

        if(ds4Input.MultithreadUpdate == false) { return ds4Input.IsButtonDown(inputID, buttonType); }

        if(ds4Input.IsButton(inputID, buttonType) == true)
        {
            if(buttonFlag == false)
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
            case DS4ButtonKey.Circle:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Ci = buttonFlag : secondAxisFlag.Ci = buttonFlag;
                break;
            case DS4ButtonKey.Cross:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Cr = buttonFlag : secondAxisFlag.Cr = buttonFlag;
                break;
            case DS4ButtonKey.Triangle:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Tr = buttonFlag : secondAxisFlag.Tr = buttonFlag;
                break;
            case DS4ButtonKey.Square:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Sq = buttonFlag : secondAxisFlag.Sq = buttonFlag;
                break;
            case DS4ButtonKey.Up:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Pu = buttonFlag : secondAxisFlag.Pu = buttonFlag;
                break;
            case DS4ButtonKey.Down:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Pd = buttonFlag : secondAxisFlag.Pd = buttonFlag;
                break;
            case DS4ButtonKey.Left:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Pl = buttonFlag : secondAxisFlag.Pl = buttonFlag;
                break;
            case DS4ButtonKey.Right:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Pr = buttonFlag : secondAxisFlag.Pr = buttonFlag;
                break;
            case DS4ButtonKey.L1:
                _ = id == ControllerNum.P1 ? firstAxisFlag.L1 = buttonFlag : secondAxisFlag.L1 = buttonFlag;
                break;
            case DS4ButtonKey.L3:
                _ = id == ControllerNum.P1 ? firstAxisFlag.L3 = buttonFlag : secondAxisFlag.L3 = buttonFlag;
                break;
            case DS4ButtonKey.R1:
                _ = id == ControllerNum.P1 ? firstAxisFlag.R1 = buttonFlag : secondAxisFlag.R1 = buttonFlag;
                break;
            case DS4ButtonKey.R3:
                _ = id == ControllerNum.P1 ? firstAxisFlag.R3 = buttonFlag : secondAxisFlag.R3 = buttonFlag;
                break;
            case DS4ButtonKey.OPTION:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Op = buttonFlag : secondAxisFlag.Op = buttonFlag;
                break;
            case DS4ButtonKey.SHARE:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Sh = buttonFlag : secondAxisFlag.Sh = buttonFlag;
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
        if (usePs4Controller == false) { return false; }

        bool axisFlag;
        DS4ControllerType inputID = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;
        DS4AxisType axisType;
        bool isPositive;
        bool result;

        // コントローラーが接続されているかチェック
        if (ds4Input.IsController(inputID) == false) { return false; }

        switch (type)
        {
            case DS4AxisKey.LeftStick_Up:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Lu : secondAxisFlag.Lu;
                isPositive = false;
                axisType = DS4AxisType.LeftStickY;
                break;
            case DS4AxisKey.LeftStick_Down:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Ld : secondAxisFlag.Ld;
                isPositive = true;
                axisType = DS4AxisType.LeftStickY;
                break;
            case DS4AxisKey.LeftStick_Left:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Ll : secondAxisFlag.Ll;
                isPositive = false;
                axisType = DS4AxisType.LeftStickX;
                break;
            case DS4AxisKey.LeftStick_Right:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Lr : secondAxisFlag.Lr;
                isPositive = true;
                axisType = DS4AxisType.LeftStickX;
                break;
            case DS4AxisKey.RightStick_Up:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Ru : secondAxisFlag.Ru;
                isPositive = false;
                axisType = DS4AxisType.RightStickY;
                break;
            case DS4AxisKey.RightStick_Down:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Rd : secondAxisFlag.Rd;
                isPositive = true;
                axisType = DS4AxisType.RightStickY;
                break;
            case DS4AxisKey.RightStick_Left:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Rl : secondAxisFlag.Rl;
                isPositive = false;
                axisType = DS4AxisType.RightStickX;
                break;
            case DS4AxisKey.RightStick_Right:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.Rr : secondAxisFlag.Rr;
                isPositive = true;
                axisType = DS4AxisType.RightStickX;
                break;
            case DS4AxisKey.L2:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.L2 : secondAxisFlag.L2;
                isPositive = true;
                axisType = DS4AxisType.L2;
                break;
            case DS4AxisKey.R2:
                axisFlag = id == ControllerNum.P1 ? firstAxisFlag.R2 : secondAxisFlag.R2;
                isPositive = true;
                axisType = DS4AxisType.R2;
                break;
            default:
                return false;
        }

        if(_ = isPositive == true ? (ds4Input.IsAxis(inputID, axisType) >= axisValue) : (ds4Input.IsAxis(inputID, axisType) <= -axisValue))
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
                _ = id == ControllerNum.P1 ? firstAxisFlag.Lu = axisFlag : secondAxisFlag.Lu = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Down:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Ld = axisFlag : secondAxisFlag.Ld = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Left:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Ll = axisFlag : secondAxisFlag.Ll = axisFlag;
                break;
            case DS4AxisKey.LeftStick_Right:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Lr = axisFlag : secondAxisFlag.Lr = axisFlag;
                break;
            case DS4AxisKey.RightStick_Up:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Ru = axisFlag : secondAxisFlag.Ru = axisFlag;
                break;
            case DS4AxisKey.RightStick_Down:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Rd = axisFlag : secondAxisFlag.Rd = axisFlag;
                break;
            case DS4AxisKey.RightStick_Left:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Rl = axisFlag : secondAxisFlag.Rl = axisFlag;
                break;
            case DS4AxisKey.RightStick_Right:
                _ = id == ControllerNum.P1 ? firstAxisFlag.Rr = axisFlag : secondAxisFlag.Rr = axisFlag;
                break;
            case DS4AxisKey.L2:
                _ = id == ControllerNum.P1 ? firstAxisFlag.L2 = axisFlag : secondAxisFlag.L2 = axisFlag;
                break;
            case DS4AxisKey.R2:
                _ = id == ControllerNum.P1 ? firstAxisFlag.R2 = axisFlag : secondAxisFlag.R2 = axisFlag;
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

        if (ds4Input.IsController(DS4ControllerType.P2) == false)
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
    /// PS4コントローラーを振動させる
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="vibration">振動値</param>
    /// <param name="duration">振動時間</param>
    public void SetVibration(ControllerNum id, float vibration, float duration)
    {
        if(vibration <= 0f || duration <= 0f || ds4Input.IsController((DS4ControllerType)id) == false || isRunning == true) { return; }
        StartCoroutine(DoVibration((DS4ControllerType)id, vibration, duration));
    }

    private IEnumerator DoVibration(DS4ControllerType id, float vibration, float duration)
    {
        isRunning = true;

        var time = 0f;

        // 振動開始
        ds4Input.SetVibration(id, new DS4Vibration((byte)vibration, (byte)vibration));

        while (time <= duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 振動停止
        ds4Input.SetVibration(id, DS4Vibration.Min);

        isRunning = false;
    }
}