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

    [SerializeField, Tooltip("PS4コントローラーを使う")]
    private bool usePs4Controller = true;

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
        DS4Input_1P();
        DS4Input_2P();
    }

    /// <summary>
    /// 1PのDS4のButtonDownを取得する
    /// </summary>
    /// <param name="player">コントローラ番号</param>
    private void DS4Input_1P()
    {
        if(ds4Input == null || usePs4Controller == false) { return; }

        // ボタン系の取得

        if(ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Circle) == true)
        {
            if(firstAxisFlag.Ci == false)
            {
                firstAxisFlag.Ci = true;
                Controller1.Circle = true;
                return;
            }
            Controller1.Circle = false;
            return;
        }
        else
        {
            firstAxisFlag.Ci = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Cross) == true)
        {
            if (firstAxisFlag.Cr == false)
            {
                firstAxisFlag.Cr = true;
                Controller1.Cross = true;
                return;
            }
            Controller1.Cross = false;
            return;
        }
        else
        {
            firstAxisFlag.Cr = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Triangle) == true)
        {
            if (firstAxisFlag.Tr == false)
            {
                firstAxisFlag.Tr = true;
                Controller1.Triangle = true;
                return;
            }
            Controller1.Triangle = false;
            return;
        }
        else
        {
            firstAxisFlag.Tr = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Square) == true)
        {
            if (firstAxisFlag.Sq == false)
            {
                firstAxisFlag.Sq = true;
                Controller1.Square = true;
                return;
            }
            Controller1.Square = false;
            return;
        }
        else
        {
            firstAxisFlag.Sq = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Up) == true)
        {
            if (firstAxisFlag.Pu == false)
            {
                firstAxisFlag.Pu = true;
                Controller1.Up = true;
                return;
            }
            Controller1.Up = false;
            return;
        }
        else
        {
            firstAxisFlag.Pu = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Down) == true)
        {
            if (firstAxisFlag.Pd == false)
            {
                firstAxisFlag.Pd = true;
                Controller1.Down = true;
                return;
            }
            Controller1.Down = false;
            return;
        }
        else
        {
            firstAxisFlag.Pd = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Left) == true)
        {
            if (firstAxisFlag.Pl == false)
            {
                firstAxisFlag.Pl = true;
                Controller1.Left = true;
                return;
            }
            Controller1.Left = false;
            return;
        }
        else
        {
            firstAxisFlag.Pl = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.Right) == true)
        {
            if (firstAxisFlag.Pr == false)
            {
                firstAxisFlag.Pr = true;
                Controller1.Right = true;
                return;
            }
            Controller1.Right = false;
            return;
        }
        else
        {
            firstAxisFlag.Pr = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.L1) == true)
        {
            if (firstAxisFlag.L1 == false)
            {
                firstAxisFlag.L1 = true;
                Controller1.L1 = true;
                return;
            }
            Controller1.L1 = false;
            return;
        }
        else
        {
            firstAxisFlag.L1 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.L3) == true)
        {
            if (firstAxisFlag.L3 == false)
            {
                firstAxisFlag.L3 = true;
                Controller1.L3 = true;
                return;
            }
            Controller1.L3 = false;
            return;
        }
        else
        {
            firstAxisFlag.L3 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.R1) == true)
        {
            if (firstAxisFlag.R1 == false)
            {
                firstAxisFlag.R1 = true;
                Controller1.R1 = true;
                return;
            }
            Controller1.R1 = false;
            return;
        }
        else
        {
            firstAxisFlag.R1 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.R3) == true)
        {
            if (firstAxisFlag.R3 == false)
            {
                firstAxisFlag.R3 = true;
                Controller1.R3 = true;
                return;
            }
            Controller1.R3 = false;
            return;
        }
        else
        {
            firstAxisFlag.R3 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.OPTION) == true)
        {
            if (firstAxisFlag.Op == false)
            {
                firstAxisFlag.Op = true;
                Controller1.OPTION = true;
                return;
            }
            Controller1.OPTION = false;
            return;
        }
        else
        {
            firstAxisFlag.Op = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P1, DS4ButtonType.SHARE) == true)
        {
            if (firstAxisFlag.Sh == false)
            {
                firstAxisFlag.Sh = true;
                Controller1.SHARE = true;
                return;
            }
            Controller1.SHARE = false;
            return;
        }
        else
        {
            firstAxisFlag.Sh = false;
        }

        // Axis系の取得

        if(ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.LeftStickX) > 0.5f)
        {
            if(firstAxisFlag.Lr == false)
            {
                firstAxisFlag.Lr = true;
                Controller1.LstickR = true;
                return;
            }
            Controller1.LstickR = false;
            return;
        }
        else
        {
            firstAxisFlag.Lr = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.LeftStickX) < -0.5f)
        {
            if (firstAxisFlag.Ll == false)
            {
                firstAxisFlag.Ll = true;
                Controller1.LstickL = true;
                return;
            }
            Controller1.LstickL = false;
            return;
        }
        else
        {
            firstAxisFlag.Ll = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.LeftStickY) > 0.5f)
        {
            if (firstAxisFlag.Ld == false)
            {
                firstAxisFlag.Ld = true;
                Controller1.LstickD = true;
                return;
            }
            Controller1.LstickD = false;
            return;
        }
        else
        {
            firstAxisFlag.Ld = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.LeftStickY) < -0.5f)
        {
            if (firstAxisFlag.Lu == false)
            {
                firstAxisFlag.Lu = true;
                Controller1.LstickU = true;
                return;
            }
            Controller1.LstickU = false;
            return;
        }
        else
        {
            firstAxisFlag.Lu = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.RightStickX) > 0.5f)
        {
            if (firstAxisFlag.Rr == false)
            {
                firstAxisFlag.Rr = true;
                Controller1.RstickR = true;
                return;
            }
            Controller1.RstickR = false;
            return;
        }
        else
        {
            firstAxisFlag.Rr = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.RightStickX) < -0.5f)
        {
            if (firstAxisFlag.Rl == false)
            {
                firstAxisFlag.Rl = true;
                Controller1.RstickL = true;
                return;
            }
            Controller1.RstickL = false;
            return;
        }
        else
        {
            firstAxisFlag.Rl = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.RightStickY) > 0.5f)
        {
            if (firstAxisFlag.Rd == false)
            {
                firstAxisFlag.Rd = true;
                Controller1.RstickD = true;
                return;
            }
            Controller1.RstickD = false;
            return;
        }
        else
        {
            firstAxisFlag.Rd = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.RightStickY) < -0.5f)
        {
            if (firstAxisFlag.Ru == false)
            {
                firstAxisFlag.Ru = true;
                Controller1.RstickU = true;
                return;
            }
            Controller1.RstickU = false;
            return;
        }
        else
        {
            firstAxisFlag.Ru = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.L2) > 0.5f)
        {
            if (firstAxisFlag.L2 == false)
            {
                firstAxisFlag.L2 = true;
                Controller1.L2 = true;
                return;
            }
            Controller1.L2 = false;
            return;
        }
        else
        {
            firstAxisFlag.L2 = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P1, DS4AxisType.R2) > 0.5f)
        {
            if (firstAxisFlag.R2 == false)
            {
                firstAxisFlag.R2 = true;
                Controller1.R2 = true;
                return;
            }
            Controller1.R2 = false;
            return;
        }
        else
        {
            firstAxisFlag.R2 = false;
        }
    }

    /// <summary>
    /// 2PのDS4のButtonDownを取得する
    /// </summary>
    /// <param name="player">コントローラ番号</param>
    private void DS4Input_2P()
    {
        if (ds4Input == null || usePs4Controller == false) { return; }

        // ボタン系の取得

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Circle) == true)
        {
            if (secondAxisFlag.Ci == false)
            {
                secondAxisFlag.Ci = true;
                Controller2.Circle = true;
                return;
            }
            Controller2.Circle = false;
            return;
        }
        else
        {
            secondAxisFlag.Ci = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Cross) == true)
        {
            if (secondAxisFlag.Cr == false)
            {
                secondAxisFlag.Cr = true;
                Controller2.Cross = true;
                return;
            }
            Controller2.Cross = false;
            return;
        }
        else
        {
            secondAxisFlag.Cr = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Triangle) == true)
        {
            if (secondAxisFlag.Tr == false)
            {
                secondAxisFlag.Tr = true;
                Controller2.Triangle = true;
                return;
            }
            Controller2.Triangle = false;
            return;
        }
        else
        {
            secondAxisFlag.Tr = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Square) == true)
        {
            if (secondAxisFlag.Sq == false)
            {
                secondAxisFlag.Sq = true;
                Controller2.Square = true;
                return;
            }
            Controller2.Square = false;
            return;
        }
        else
        {
            secondAxisFlag.Sq = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Up) == true)
        {
            if (secondAxisFlag.Pu == false)
            {
                secondAxisFlag.Pu = true;
                Controller2.Up = true;
                return;
            }
            Controller2.Up = false;
            return;
        }
        else
        {
            secondAxisFlag.Pu = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Down) == true)
        {
            if (secondAxisFlag.Pd == false)
            {
                secondAxisFlag.Pd = true;
                Controller2.Down = true;
                return;
            }
            Controller2.Down = false;
            return;
        }
        else
        {
            secondAxisFlag.Pd = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Left) == true)
        {
            if (secondAxisFlag.Pl == false)
            {
                secondAxisFlag.Pl = true;
                Controller2.Left = true;
                return;
            }
            Controller2.Left = false;
            return;
        }
        else
        {
            secondAxisFlag.Pl = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.Right) == true)
        {
            if (secondAxisFlag.Pr == false)
            {
                secondAxisFlag.Pr = true;
                Controller2.Right = true;
                return;
            }
            Controller2.Right = false;
            return;
        }
        else
        {
            secondAxisFlag.Pr = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.L1) == true)
        {
            if (secondAxisFlag.L1 == false)
            {
                secondAxisFlag.L1 = true;
                Controller2.L1 = true;
                return;
            }
            Controller2.L1 = false;
            return;
        }
        else
        {
            secondAxisFlag.L1 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.L3) == true)
        {
            if (secondAxisFlag.L3 == false)
            {
                secondAxisFlag.L3 = true;
                Controller2.L3 = true;
                return;
            }
            Controller2.L3 = false;
            return;
        }
        else
        {
            secondAxisFlag.L3 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.R1) == true)
        {
            if (secondAxisFlag.R1 == false)
            {
                secondAxisFlag.R1 = true;
                Controller2.R1 = true;
                return;
            }
            Controller2.R1 = false;
            return;
        }
        else
        {
            secondAxisFlag.R1 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.R3) == true)
        {
            if (secondAxisFlag.R3 == false)
            {
                secondAxisFlag.R3 = true;
                Controller2.R3 = true;
                return;
            }
            Controller2.R3 = false;
            return;
        }
        else
        {
            secondAxisFlag.R3 = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.OPTION) == true)
        {
            if (secondAxisFlag.Op == false)
            {
                secondAxisFlag.Op = true;
                Controller2.OPTION = true;
                return;
            }
            Controller2.OPTION = false;
            return;
        }
        else
        {
            secondAxisFlag.Op = false;
        }

        if (ds4Input.IsButton(DS4ControllerType.P2, DS4ButtonType.SHARE) == true)
        {
            if (secondAxisFlag.Sh == false)
            {
                secondAxisFlag.Sh = true;
                Controller2.SHARE = true;
                return;
            }
            Controller2.SHARE = false;
            return;
        }
        else
        {
            secondAxisFlag.Sh = false;
        }

        // Axis系の取得

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.LeftStickX) > 0.5f)
        {
            if (secondAxisFlag.Lr == false)
            {
                secondAxisFlag.Lr = true;
                Controller2.LstickR = true;
                return;
            }
            Controller2.LstickR = false;
            return;
        }
        else
        {
            secondAxisFlag.Lr = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.LeftStickX) < -0.5f)
        {
            if (secondAxisFlag.Ll == false)
            {
                secondAxisFlag.Ll = true;
                Controller2.LstickL = true;
                return;
            }
            Controller2.LstickL = false;
            return;
        }
        else
        {
            secondAxisFlag.Ll = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.LeftStickY) > 0.5f)
        {
            if (secondAxisFlag.Ld == false)
            {
                secondAxisFlag.Ld = true;
                Controller2.LstickD = true;
                return;
            }
            Controller2.LstickD = false;
            return;
        }
        else
        {
            secondAxisFlag.Ld = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.LeftStickY) < -0.5f)
        {
            if (secondAxisFlag.Lu == false)
            {
                secondAxisFlag.Lu = true;
                Controller2.LstickU = true;
                return;
            }
            Controller2.LstickU = false;
            return;
        }
        else
        {
            secondAxisFlag.Lu = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.RightStickX) > 0.5f)
        {
            if (secondAxisFlag.Rr == false)
            {
                secondAxisFlag.Rr = true;
                Controller2.RstickR = true;
                return;
            }
            Controller2.RstickR = false;
            return;
        }
        else
        {
            secondAxisFlag.Rr = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.RightStickX) < -0.5f)
        {
            if (secondAxisFlag.Rl == false)
            {
                secondAxisFlag.Rl = true;
                Controller2.RstickL = true;
                return;
            }
            Controller2.RstickL = false;
            return;
        }
        else
        {
            secondAxisFlag.Rl = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.RightStickY) > 0.5f)
        {
            if (secondAxisFlag.Rd == false)
            {
                secondAxisFlag.Rd = true;
                Controller2.RstickD = true;
                return;
            }
            Controller2.RstickD = false;
            return;
        }
        else
        {
            secondAxisFlag.Rd = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.RightStickY) < -0.5f)
        {
            if (secondAxisFlag.Ru == false)
            {
                secondAxisFlag.Ru = true;
                Controller2.RstickU = true;
                return;
            }
            Controller2.RstickU = false;
            return;
        }
        else
        {
            secondAxisFlag.Ru = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.L2) > 0.5f)
        {
            if (secondAxisFlag.L2 == false)
            {
                secondAxisFlag.L2 = true;
                Controller2.L2 = true;
                return;
            }
            Controller2.L2 = false;
            return;
        }
        else
        {
            secondAxisFlag.L2 = false;
        }

        if (ds4Input.IsAxis(DS4ControllerType.P2, DS4AxisType.R2) > 0.5f)
        {
            if (secondAxisFlag.R2 == false)
            {
                secondAxisFlag.R2 = true;
                Controller2.R2 = true;
                return;
            }
            Controller2.R2 = false;
            return;
        }
        else
        {
            secondAxisFlag.R2 = false;
        }
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