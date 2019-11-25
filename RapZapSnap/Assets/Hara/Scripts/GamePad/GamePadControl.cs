using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4InputCustom;

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
    private float axisValue = 0.8f;

    private DS4InputCustom.DS4InputCustom ds4Input = null;

    private UnityEngine.EventSystems.StandaloneInputModule inputModule;

    // コルーチン用のフラグ
    private bool isRunning = false;

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
        if (Instance == null && Instance != this)
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
        if (ds4Input == null || usePs4Controller == false || ds4Input.IsController((DS4ControllerType)id) == false) { return; }

        DS4ControllerType type = id == ControllerNum.P1 ? DS4ControllerType.P1 : DS4ControllerType.P2;

        //----------ボタン系の取得----------//

        if(ds4Input.IsButton(type, DS4ButtonType.Circle) == true)
        {
            if(id == ControllerNum.P1)
            {
                if(firstAxisFlag.Ci == false)
                {
                    firstAxisFlag.Ci = true;
                    Controller1.Circle = true;
                }
                else
                {
                    Controller1.Circle = false;
                }
            }
            else
            {
                if (secondAxisFlag.Ci == false)
                {
                    secondAxisFlag.Ci = true;
                    Controller2.Circle = true;
                }
                else
                {
                    Controller2.Circle = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if(firstAxisFlag.Ci == true) { firstAxisFlag.Ci = false; }
                if(Controller1.Circle == true) { Controller1.Circle = false; }
            }
            else
            {
                if(secondAxisFlag.Ci == true) { secondAxisFlag.Ci = false; }
                if(Controller2.Circle == true) { Controller2.Circle = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Cross) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Cr == false)
                {
                    firstAxisFlag.Cr = true;
                    Controller1.Cross = true;
                }
                else
                {
                    Controller1.Cross = false;
                }
            }
            else
            {
                if (secondAxisFlag.Cr == false)
                {
                    secondAxisFlag.Cr = true;
                    Controller2.Cross = true;
                }
                else
                {
                    Controller2.Cross = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Cr == true) { firstAxisFlag.Cr = false; }
                if (Controller1.Cross == true) { Controller1.Cross = false; }
            }
            else
            {
                if (secondAxisFlag.Cr == true) { secondAxisFlag.Cr = false; }
                if (Controller2.Cross == true) { Controller2.Cross = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Triangle) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Tr == false)
                {
                    firstAxisFlag.Tr = true;
                    Controller1.Triangle = true;
                }
                else
                {
                    Controller1.Triangle = false;
                }
            }
            else
            {
                if (secondAxisFlag.Tr == false)
                {
                    secondAxisFlag.Tr = true;
                    Controller2.Triangle = true;
                }
                else
                {
                    Controller2.Triangle = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Tr == true) { firstAxisFlag.Tr = false; }
                if (Controller1.Triangle == true) { Controller1.Triangle = false; }
            }
            else
            {
                if (secondAxisFlag.Tr == true) { secondAxisFlag.Tr = false; }
                if (Controller2.Triangle == true) { Controller2.Triangle = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Square) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Sq == false)
                {
                    firstAxisFlag.Sq = true;
                    Controller1.Square = true;
                }
                else
                {
                    Controller1.Square = false;
                }
            }
            else
            {
                if (secondAxisFlag.Sq == false)
                {
                    secondAxisFlag.Sq = true;
                    Controller2.Square = true;
                }
                else
                {
                    Controller2.Square = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Sq == true) { firstAxisFlag.Sq = false; }
                if (Controller1.Square == true) { Controller1.Square = false; }
            }
            else
            {
                if (secondAxisFlag.Sq == true) { secondAxisFlag.Sq = false; }
                if (Controller2.Square == true) { Controller2.Square = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Up) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pu == false)
                {
                    firstAxisFlag.Pu = true;
                    Controller1.Up = true;
                }
                else
                {
                    Controller1.Up = false;
                }
            }
            else
            {
                if (secondAxisFlag.Pu == false)
                {
                    secondAxisFlag.Pu = true;
                    Controller2.Up = true;
                }
                else
                {
                    Controller2.Up = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pu == true) { firstAxisFlag.Pu = false; }
                if (Controller1.Up == true) { Controller1.Up = false; }
            }
            else
            {
                if (secondAxisFlag.Pu == true) { secondAxisFlag.Pu = false; }
                if (Controller2.Up == true) { Controller2.Up = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Down) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pd == false)
                {
                    firstAxisFlag.Pd = true;
                    Controller1.Down = true;
                }
                else
                {
                    Controller1.Down = false;
                }
            }
            else
            {
                if (secondAxisFlag.Pd == false)
                {
                    secondAxisFlag.Pd = true;
                    Controller2.Down = true;
                }
                else
                {
                    Controller2.Down = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pd == true) { firstAxisFlag.Pd = false; }
                if (Controller1.Down == true) { Controller1.Down = false; }
            }
            else
            {
                if (secondAxisFlag.Pd == true) { secondAxisFlag.Pd = false; }
                if (Controller2.Down == true) { Controller2.Down = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Left) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pl == false)
                {
                    firstAxisFlag.Pl = true;
                    Controller1.Left = true;
                }
                else
                {
                    Controller1.Left = false;
                }
            }
            else
            {
                if (secondAxisFlag.Pl == false)
                {
                    secondAxisFlag.Pl = true;
                    Controller2.Left = true;
                }
                else
                {
                    Controller2.Left = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pl == true) { firstAxisFlag.Pl = false; }
                if (Controller1.Left == true) { Controller1.Left = false; }
            }
            else
            {
                if (secondAxisFlag.Pl == true) { secondAxisFlag.Pl = false; }
                if (Controller2.Left == true) { Controller2.Left = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.Right) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pr == false)
                {
                    firstAxisFlag.Pr = true;
                    Controller1.Right = true;
                }
                else
                {
                    Controller1.Right = false;
                }
            }
            else
            {
                if (secondAxisFlag.Pr == false)
                {
                    secondAxisFlag.Pr = true;
                    Controller2.Right = true;
                }
                else
                {
                    Controller2.Right = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Pr == true) { firstAxisFlag.Pr = false; }
                if (Controller1.Right == true) { Controller1.Right = false; }
            }
            else
            {
                if (secondAxisFlag.Pr == true) { secondAxisFlag.Pr = false; }
                if (Controller2.Right == true) { Controller2.Right = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.L1) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.L1 == false)
                {
                    firstAxisFlag.L1 = true;
                    Controller1.L1 = true;
                }
                else
                {
                    Controller1.L1 = false;
                }
            }
            else
            {
                if (secondAxisFlag.L1 == false)
                {
                    secondAxisFlag.L1 = true;
                    Controller2.L1 = true;
                }
                else
                {
                    Controller2.L1 = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.L1 == true) { firstAxisFlag.L1 = false; }
                if (Controller1.L1 == true) { Controller1.L1 = false; }
            }
            else
            {
                if (secondAxisFlag.L1 == true) { secondAxisFlag.L1 = false; }
                if (Controller2.L1 == true) { Controller2.L1 = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.L3) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.L3 == false)
                {
                    firstAxisFlag.L3 = true;
                    Controller1.L3 = true;
                }
                else
                {
                    Controller1.L3 = false;
                }
            }
            else
            {
                if (secondAxisFlag.L3 == false)
                {
                    secondAxisFlag.L3 = true;
                    Controller2.L3 = true;
                }
                else
                {
                    Controller2.L3 = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.L3 == true) { firstAxisFlag.L3 = false; }
                if (Controller1.L3 == true) { Controller1.L3 = false; }
            }
            else
            {
                if (secondAxisFlag.L3 == true) { secondAxisFlag.L3 = false; }
                if (Controller2.L3 == true) { Controller2.L3 = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.R1) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.R1 == false)
                {
                    firstAxisFlag.R1 = true;
                    Controller1.R1 = true;
                }
                else
                {
                    Controller1.R1 = false;
                }
            }
            else
            {
                if (secondAxisFlag.R1 == false)
                {
                    secondAxisFlag.R1 = true;
                    Controller2.R1 = true;
                }
                else
                {
                    Controller2.R1 = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.R1 == true) { firstAxisFlag.R1 = false; }
                if (Controller1.R1 == true) { Controller1.R1 = false; }
            }
            else
            {
                if (secondAxisFlag.R1 == true) { secondAxisFlag.R1 = false; }
                if (Controller2.R1 == true) { Controller2.R1 = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.R3) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.R3 == false)
                {
                    firstAxisFlag.R3 = true;
                    Controller1.R3 = true;
                }
                else
                {
                    Controller1.R3 = false;
                }
            }
            else
            {
                if (secondAxisFlag.R3 == false)
                {
                    secondAxisFlag.R3 = true;
                    Controller2.R3 = true;
                }
                else
                {
                    Controller2.R3 = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.R3 == true) { firstAxisFlag.R3 = false; }
                if (Controller1.R3 == true) { Controller1.R3 = false; }
            }
            else
            {
                if (secondAxisFlag.R3 == true) { secondAxisFlag.R3 = false; }
                if (Controller2.R3 == true) { Controller2.R3 = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.OPTION) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Op == false)
                {
                    firstAxisFlag.Op = true;
                    Controller1.OPTION = true;
                }
                else
                {
                    Controller1.OPTION = false;
                }
            }
            else
            {
                if (secondAxisFlag.Op == false)
                {
                    secondAxisFlag.Op = true;
                    Controller2.OPTION = true;
                }
                else
                {
                    Controller2.OPTION = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Op == true) { firstAxisFlag.Op = false; }
                if (Controller1.OPTION == true) { Controller1.OPTION = false; }
            }
            else
            {
                if (secondAxisFlag.Op == true) { secondAxisFlag.Op = false; }
                if (Controller2.OPTION == true) { Controller2.OPTION = false; }
            }
        }

        if (ds4Input.IsButton(type, DS4ButtonType.SHARE) == true)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Sh == false)
                {
                    firstAxisFlag.Sh = true;
                    Controller1.SHARE = true;
                }
                else
                {
                    Controller1.SHARE = false;
                }
            }
            else
            {
                if (secondAxisFlag.Sh == false)
                {
                    secondAxisFlag.Sh = true;
                    Controller2.SHARE = true;
                }
                else
                {
                    Controller2.SHARE = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Sh == true) { firstAxisFlag.Sh = false; }
                if (Controller1.SHARE == true) { Controller1.SHARE = false; }
            }
            else
            {
                if (secondAxisFlag.Sh == true) { secondAxisFlag.Sh = false; }
                if (Controller2.SHARE == true) { Controller2.SHARE = false; }
            }
        }

        //----------Axis系の取得----------//

        float axisLeftStickX = ds4Input.IsAxis(type, DS4AxisType.LeftStickX);
        float axisLeftStickY = ds4Input.IsAxis(type, DS4AxisType.LeftStickY);
        float axisRightStickX = ds4Input.IsAxis(type, DS4AxisType.RightStickX);
        float axisRightStickY = ds4Input.IsAxis(type, DS4AxisType.RightStickY);
        float axisL2 = ds4Input.IsAxis(type, DS4AxisType.L2);
        float axisR2 = ds4Input.IsAxis(type, DS4AxisType.R2);

        if (axisLeftStickX >= axisValue && axisLeftStickX <= 1.0f)
        {
            if(id == ControllerNum.P1)
            {
                if (firstAxisFlag.Lr == false)
                {
                    firstAxisFlag.Lr = true;
                    Controller1.LstickR = true;
                    Controller1.LstickL = false;
                }
                else
                {
                    Controller1.LstickR = false;
                }
            }
            else
            {
                if (secondAxisFlag.Lr == false)
                {
                    secondAxisFlag.Lr = true;
                    Controller2.LstickR = true;
                    Controller2.LstickL = false;
                }
                else
                {
                    Controller2.LstickR = false;
                }
            }
        }
        else if (axisLeftStickX <= -axisValue && axisLeftStickX >= -1.0f)
        {
            if(id == ControllerNum.P1)
            {
                if (firstAxisFlag.Ll == false)
                {
                    firstAxisFlag.Ll = true;
                    Controller1.LstickL = true;
                    Controller1.LstickR = false;
                }
                else
                {
                    Controller1.LstickL = false;
                }
            }
            else
            {
                if (secondAxisFlag.Ll == false)
                {
                    secondAxisFlag.Ll = true;
                    Controller2.LstickL = true;
                    Controller2.LstickR = false;
                }
                else
                {
                    Controller2.LstickL = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if(firstAxisFlag.Lr == true) { firstAxisFlag.Lr = false; }
                if(firstAxisFlag.Ll == true) { firstAxisFlag.Ll = false; }
                if(Controller1.LstickR == true) { Controller1.LstickR = false; }
                if(Controller1.LstickL == true) { Controller1.LstickL = false; }
            }
            else
            {
                if (secondAxisFlag.Lr == true) { secondAxisFlag.Lr = false; }
                if (secondAxisFlag.Ll == true) { secondAxisFlag.Ll = false; }
                if (Controller2.LstickR == true) { Controller2.LstickR = false; }
                if (Controller2.LstickL == true) { Controller2.LstickL = false; }
            }
        }

        if(axisLeftStickY >= axisValue && axisLeftStickY <= 1.0f)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Ld == false)
                {
                    firstAxisFlag.Ld = true;
                    Controller1.LstickD = true;
                    Controller1.LstickU = false;
                }
                else
                {
                    Controller1.LstickD = false;
                }
            }
            else
            {
                if (secondAxisFlag.Ld == false)
                {
                    secondAxisFlag.Ld = true;
                    Controller2.LstickD = true;
                    Controller2.LstickU = false;
                }
                else
                {
                    Controller2.LstickD = false;
                }
            }
        }
        else if (axisLeftStickY <= -axisValue && axisLeftStickY >= -1.0f)
        {
            if(id == ControllerNum.P1)
            {
                if (firstAxisFlag.Lu == false)
                {
                    firstAxisFlag.Lu = true;
                    Controller1.LstickU = true;
                    Controller1.LstickD = false;
                }
                else
                {
                    Controller1.LstickU = false;
                }
            }
            else
            {
                if (secondAxisFlag.Lu == false)
                {
                    secondAxisFlag.Lu = true;
                    Controller2.LstickU = true;
                    Controller2.LstickD = false;
                }
                else
                {
                    Controller2.LstickU = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Ld == true) { firstAxisFlag.Ld = false; }
                if (firstAxisFlag.Lu == true) { firstAxisFlag.Lu = false; }
                if (Controller1.LstickD == true) { Controller1.LstickD = false; }
                if (Controller1.LstickU == true) { Controller1.LstickU = false; }
            }
            else
            {
                if (secondAxisFlag.Ld == true) { secondAxisFlag.Ld = false; }
                if (secondAxisFlag.Lu == true) { secondAxisFlag.Lu = false; }
                if (Controller2.LstickD == true) { Controller2.LstickD = false; }
                if (Controller2.LstickU == true) { Controller2.LstickU = false; }
            }
        }

        if (axisRightStickX >= axisValue && axisRightStickX <= 1.0f)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Rr == false)
                {
                    firstAxisFlag.Rr = true;
                    Controller1.RstickR = true;
                    Controller1.RstickL = false;
                }
                else
                {
                    Controller1.RstickR = false;
                }
            }
            else
            {
                if (secondAxisFlag.Rr == false)
                {
                    secondAxisFlag.Rr = true;
                    Controller2.RstickR = true;
                    Controller2.RstickL = false;
                }
                else
                {
                    Controller2.RstickR = false;
                }
            }
        }
        else if (axisRightStickX <= -axisValue && axisRightStickX >= -1.0f)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Rl == false)
                {
                    firstAxisFlag.Rl = true;
                    Controller1.RstickL = true;
                    Controller1.RstickR = false;
                }
                else
                {
                    Controller1.RstickL = false;
                }
            }
            else
            {
                if (secondAxisFlag.Rl == false)
                {
                    secondAxisFlag.Rl = true;
                    Controller2.RstickL = true;
                    Controller2.RstickR = false;
                }
                else
                {
                    Controller2.RstickL = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Rr == true) { firstAxisFlag.Rr = false; }
                if (firstAxisFlag.Rl == true) { firstAxisFlag.Rl = false; }
                if (Controller1.RstickR == true) { Controller1.RstickR = false; }
                if (Controller1.RstickL == true) { Controller1.RstickL = false; }
            }
            else
            {
                if (secondAxisFlag.Rr == true) { secondAxisFlag.Rr = false; }
                if (secondAxisFlag.Rl == true) { secondAxisFlag.Rl = false; }
                if (Controller2.RstickR == true) { Controller2.RstickR = false; }
                if (Controller2.RstickL == true) { Controller2.RstickL = false; }
            }
        }

        if (axisRightStickY >= axisValue && axisRightStickY <= 1.0f)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Rd == false)
                {
                    firstAxisFlag.Rd = true;
                    Controller1.RstickD = true;
                    Controller1.RstickU = false;
                }
                else
                {
                    Controller1.RstickD = false;
                }
            }
            else
            {
                if (secondAxisFlag.Rd == false)
                {
                    secondAxisFlag.Rd = true;
                    Controller2.RstickD = true;
                    Controller2.RstickU = false;
                }
                else
                {
                    Controller2.RstickD = false;
                }
            }
        }
        else if (axisRightStickY <= -axisValue && axisRightStickY >= -1.0f)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Ru == false)
                {
                    firstAxisFlag.Ru = true;
                    Controller1.RstickU = true;
                    Controller1.RstickD = false;
                }
                else
                {
                    Controller1.RstickU = false;
                }
            }
            else
            {
                if (secondAxisFlag.Ru == false)
                {
                    secondAxisFlag.Ru = true;
                    Controller2.RstickU = true;
                    Controller2.RstickD = false;
                }
                else
                {
                    Controller2.RstickU = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.Rd == true) { firstAxisFlag.Rd = false; }
                if (firstAxisFlag.Ru == true) { firstAxisFlag.Ru = false; }
                if (Controller1.RstickD == true) { Controller1.RstickD = false; }
                if (Controller1.RstickU == true) { Controller1.RstickU = false; }
            }
            else
            {
                if (secondAxisFlag.Rd == true) { secondAxisFlag.Rd = false; }
                if (secondAxisFlag.Ru == true) { secondAxisFlag.Ru = false; }
                if (Controller2.RstickD == true) { Controller2.RstickD = false; }
                if (Controller2.RstickU == true) { Controller2.RstickU = false; }
            }
        }

        if(axisL2 >= axisValue)
        {
            if(id == ControllerNum.P1)
            {
                if (firstAxisFlag.L2 == false)
                {
                    firstAxisFlag.L2 = true;
                    Controller1.L2 = true;
                }
                else
                {
                    Controller1.L2 = false;
                }
            }
            else
            {
                if(secondAxisFlag.L2 == false)
                {
                    secondAxisFlag.L2 = true;
                    Controller2.L2 = true;
                }
                else
                {
                    Controller2.L2 = false;
                }
            }
        }
        else
        {
            if(id == ControllerNum.P1)
            {
                if(firstAxisFlag.L2 == true) { firstAxisFlag.L2 = false; }
                if(Controller1.L2 == true) { Controller1.L2 = false; }
            }
            else
            {
                if (secondAxisFlag.L2 == true) { secondAxisFlag.L2 = false; }
                if (Controller2.L2 == true) { Controller2.L2 = false; }
            }
        }

        if (axisR2 >= axisValue)
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.R2 == false)
                {
                    firstAxisFlag.R2 = true;
                    Controller1.R2 = true;
                }
                else
                {
                    Controller1.R2 = false;
                }
            }
            else
            {
                if (secondAxisFlag.R2 == false)
                {
                    secondAxisFlag.R2 = true;
                    Controller2.R2 = true;
                }
                else
                {
                    Controller2.R2 = false;
                }
            }
        }
        else
        {
            if (id == ControllerNum.P1)
            {
                if (firstAxisFlag.R2 == true) { firstAxisFlag.R2 = false; }
                if (Controller1.R2 == true) { Controller1.R2 = false; }
            }
            else
            {
                if (secondAxisFlag.R2 == true) { secondAxisFlag.R2 = false; }
                if (Controller2.R2 == true) { Controller2.R2 = false; }
            }
        }
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