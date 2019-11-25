using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerNum
{
    P1,
    P2
}

public class GamePadControl : MonoBehaviour
{
    public static GamePadControl Instance { private set; get; } = null;

    [SerializeField, Tooltip("DS4を使う")]
    private bool usePs4Controller = true;

    [SerializeField, Tooltip("DS4のスティック系の有効入力感度"), Range(0.01f, 1.0f)]
    private float axisValue = 0.5f;

    private DS4InputCustom.DS4InputCustom ds4Input = null;

    private UnityEngine.EventSystems.StandaloneInputModule inputModule;

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
    /// DS4の入力キー
    /// </summary>
    public struct DS4InputKeyType
    {
        public bool Circle;
        public bool Cross;
        public bool Triangle;
        public bool Square;
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public bool L1;
        public bool L2;
        public bool L3;
        public bool LstickU;
        public bool LstickD;
        public bool LstickL;
        public bool LstickR;
        public bool R1;
        public bool R2;
        public bool R3;
        public bool RstickU;
        public bool RstickD;
        public bool RstickL;
        public bool RstickR;
        public bool OPTION;
        public bool SHARE;

        public void ResetKey()
        {
            Circle = false;
            Cross = false;
            Triangle = false;
            Square = false;
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            L1 = false;
            L2 = false;
            L3 = false;
            LstickU = false;
            LstickD = false;
            LstickL = false;
            LstickR = false;
            R1 = false;
            R2 = false;
            R3 = false;
            RstickU = false;
            RstickD = false;
            RstickL = false;
            RstickR = false;
            OPTION = false;
            SHARE = false;
        }
    }

    public DS4InputKeyType Controller1;
    public DS4InputKeyType Controller2;

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
        if (Instance == null)
        {
            Instance = this;
            Controller1.ResetKey();
            Controller2.ResetKey();
            firstAxisFlag.ResetFlag();
            secondAxisFlag.ResetFlag();
            joy1KeyName = new DS4KeyName(ControllerNum.P1);
            joy2KeyName = new DS4KeyName(ControllerNum.P2);
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
        DS4Input(ControllerNum.P1);
        DS4Input(ControllerNum.P2);
    }

    /// <summary>
    /// DS4のボタン入力を取得
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    private void DS4Input(ControllerNum id)
    {
        if (ds4Input == null || usePs4Controller == false) { return; }

        DS4ControllerType type = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;
        DS4InputKeyType input = id == ControllerNum.P1 ? Controller1 : Controller2;
        AxisFlag axisFlag = id == ControllerNum.P1 ? firstAxisFlag : secondAxisFlag;

        //----------ボタン系の取得----------//

        if(ds4Input.IsButton(type, DS4ButtonType.Circle) == true)
        {
            if(axisFlag.Ci == false)
            {
                axisFlag.Ci = true;
                input.Circle = true;
            }
            else
            {
                input.Circle = false;
            }
        }
        else
        {
            if (axisFlag.Ci == true) { axisFlag.Ci = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Cross) == true)
        {
            if (axisFlag.Cr == false)
            {
                axisFlag.Cr = true;
                input.Cross = true;
            }
            else
            {
                input.Cross = false;
            }
        }
        else
        {
            if (axisFlag.Cr == true) { axisFlag.Cr = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Triangle) == true)
        {
            if (axisFlag.Tr == false)
            {
                axisFlag.Tr = true;
                input.Triangle = true;
            }
            else
            {
                input.Triangle = false;
            }
        }
        else
        {
            if (axisFlag.Tr == true) { axisFlag.Tr = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Square) == true)
        {
            if (axisFlag.Sq == false)
            {
                axisFlag.Sq = true;
                input.Square = true;
            }
            else
            {
                input.Square = false;
            }
        }
        else
        {
            if (axisFlag.Sq == true) { axisFlag.Sq = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Up) == true)
        {
            if (axisFlag.Pu == false)
            {
                axisFlag.Pu = true;
                input.Up = true;
            }
            else
            {
                input.Up = false;
            }
        }
        else
        {
            if (axisFlag.Pu == true) { axisFlag.Pu = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Down) == true)
        {
            if (axisFlag.Pd == false)
            {
                axisFlag.Pd = true;
                input.Down = true;
            }
            else
            {
                input.Down = false;
            }
        }
        else
        {
            if (axisFlag.Pd == true) { axisFlag.Pd = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Left) == true)
        {
            if (axisFlag.Pl == false)
            {
                axisFlag.Pl = true;
                input.Left = true;
            }
            else
            {
                input.Left = false;
            }
        }
        else
        {
            if (axisFlag.Pl == true) { axisFlag.Pl = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Right) == true)
        {
            if (axisFlag.Pr == false)
            {
                axisFlag.Pr = true;
                input.Right = true;
            }
            else
            {
                input.Right = false;
            }
        }
        else
        {
            if (axisFlag.Pr == true) { axisFlag.Pr = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.L1) == true)
        {
            if (axisFlag.L1 == false)
            {
                axisFlag.L1 = true;
                input.L1 = true;
            }
            else
            {
                input.L1 = false;
            }
        }
        else
        {
            if (axisFlag.L1 == true) { axisFlag.L1 = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.L3) == true)
        {
            if (axisFlag.L3 == false)
            {
                axisFlag.L3 = true;
                input.L3 = true;
            }
            else
            {
                input.L3 = false;
            }
        }
        else
        {
            if (axisFlag.L3 == true) { axisFlag.L3 = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.R1) == true)
        {
            if (axisFlag.R1 == false)
            {
                axisFlag.R1 = true;
                input.R1 = true;
            }
            else
            {
                input.R1 = false;
            }
        }
        else
        {
            if (axisFlag.R1 == true) { axisFlag.R1 = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.R3) == true)
        {
            if (axisFlag.R3 == false)
            {
                axisFlag.R3 = true;
                input.R3 = true;
            }
            else
            {
                input.R3 = false;
            }
        }
        else
        {
            if(axisFlag.R3 == true) { axisFlag.R3 = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.OPTION) == true)
        {
            if (axisFlag.Op == false)
            {
                axisFlag.Op = true;
                input.OPTION = true;
            }
            else
            {
                input.OPTION = false;
            }
        }
        else
        {
            if (axisFlag.Op == true) { axisFlag.Op = false; }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.SHARE) == true)
        {
            if (axisFlag.Sh == false)
            {
                axisFlag.Sh = true;
                input.SHARE = true;
            }
            else
            {
                input.SHARE = false;
            }
        }
        else
        {
            if(axisFlag.Sh == true) { axisFlag.Sh = false; }
        }

        //----------Axis系の取得----------//

        if(ds4Input.IsAxis(type, DS4AxisType.LeftStickX) > axisValue)
        {
            if(axisFlag.Lr == false)
            {
                axisFlag.Lr = true;
                input.LstickR = true;
            }
            else
            {
                input.LstickR = false;
            }
        }
        else
        {
            if(axisFlag.Lr == true) { axisFlag.Lr = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.LeftStickX) < -axisValue)
        {
            if (axisFlag.Ll == false)
            {
                axisFlag.Ll = true;
                input.LstickL = true;
            }
            else
            {
                input.LstickL = false;
            }
        }
        else
        {
            if (axisFlag.Ll == true) { axisFlag.Ll = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.LeftStickY) > axisValue)
        {
            if (axisFlag.Ld == false)
            {
                axisFlag.Ld = true;
                input.LstickD = true;
            }
            else
            {
                input.LstickD = false;
            }
        }
        else
        {
            if (axisFlag.Ld == true) { axisFlag.Ld = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.LeftStickY) < -axisValue)
        {
            if (axisFlag.Lu == false)
            {
                axisFlag.Lu = true;
                input.LstickU = true;
            }
            else
            {
                input.LstickU = false;
            }
        }
        else
        {
            if (axisFlag.Lu == true) { axisFlag.Lu = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.RightStickX) > axisValue)
        {
            if (axisFlag.Rr == false)
            {
                axisFlag.Rr = true;
                input.RstickR = true;
            }
            else
            {
                input.RstickR = false;
            }
        }
        else
        {
            if (axisFlag.Rr == true) { axisFlag.Rr = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.RightStickX) < -axisValue)
        {
            if (axisFlag.Rl == false)
            {
                axisFlag.Rl = true;
                input.RstickL = true;
            }
            else
            {
                input.RstickL = false;
            }
        }
        else
        {
            if (axisFlag.Rl == true) { axisFlag.Rl = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.RightStickY) > axisValue)
        {
            if (axisFlag.Rd == false)
            {
                axisFlag.Rd = true;
                input.RstickD = true;
            }
            else
            {
                input.RstickD = false;
            }
        }
        else
        {
            if (axisFlag.Rd == true) { axisFlag.Rd = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.RightStickY) < -axisValue)
        {
            if (axisFlag.Ru == false)
            {
                axisFlag.Ru = true;
                input.RstickU = true;
            }
            else
            {
                input.RstickU = false;
            }
        }
        else
        {
            if (axisFlag.Ru == true) { axisFlag.Ru = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.L2) > axisValue)
        {
            if (axisFlag.L2 == false)
            {
                axisFlag.L2 = true;
                input.L2 = true;
            }
            else
            {
                input.L2 = false;
            }
        }
        else
        {
            if (axisFlag.L2 == true) { axisFlag.L2 = false; }
        }

        if (ds4Input.IsAxis(type, DS4AxisType.R2) > axisValue)
        {
            if (axisFlag.R2 == false)
            {
                axisFlag.R2 = true;
                input.R2 = true;
            }
            else
            {
                input.R2 = false;
            }
        }
        else
        {
            if (axisFlag.R2 == true) { axisFlag.R2 = false; }
        }

        //----------入力の結果を反映させる----------//
        _ = id == ControllerNum.P1 ? Controller1 = input : Controller2 = input;
        _ = id == ControllerNum.P1 ? firstAxisFlag = axisFlag : secondAxisFlag = axisFlag;
    }

    /// <summary>
    /// StandaloneInputModuleをPS4入力に対応させる
    /// </summary>
    /// <param name="input">入力を許可するプレイヤー</param>
    public void SetInputModule(ControllerNum input)
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
        inputModule.submitButton = input == ControllerNum.P1 ? joy1KeyName.Submit : joy2KeyName.Submit;
        inputModule.cancelButton = input == ControllerNum.P1 ? joy1KeyName.Cancel : joy2KeyName.Cancel;
        inputModule.horizontalAxis = input == ControllerNum.P1 ? joy1KeyName.Horizontal : joy2KeyName.Horizontal;
        inputModule.verticalAxis = input == ControllerNum.P1 ? joy1KeyName.Vertical : joy2KeyName.Vertical;
    }

    /// <summary>
    /// PS4コントローラーを振動させる
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="vibration">振動値</param>
    /// <param name="duration">振動時間</param>
    public void SetVibration(ControllerNum id, float vibration, float duration)
    {
        if(vibration <= 0f || duration <= 0f || ds4Input.IsController((DS4ControllerType)id) == false) { return; }
        StartCoroutine(DoVibration((DS4ControllerType)id, vibration, duration));
    }

    private IEnumerator DoVibration(DS4ControllerType id, float vibration, float duration)
    {
        var time = 0f;
        ds4Input.SetVibration(id, new DS4InputCustom.DS4Vibration((byte)vibration, (byte)vibration));
        while (time <= duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        ds4Input.SetVibration(id, DS4InputCustom.DS4Vibration.Min);
    }
}