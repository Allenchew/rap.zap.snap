using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputController
{
    PlayerOne,
    PlayerTwo
}

namespace DS4
{
    public class GamePadControl : MonoBehaviour
    {
        public static GamePadControl Instance { private set; get; } = null;

        private bool isChangeController = false;    // 1Pのコントローラーと2Pのコントローラーの入力を入れ替えるか
        private bool oneShotCall = true;

        // PS4コントローラーのGetAxis用のAxis名
        private readonly string padVertical_1 = "DS4verticalPad_1";
        private readonly string padHorizontal_1 = "DS4horizontalPad_1";
        private readonly string leftVertical_1 = "DS4verticalLstick_1";
        private readonly string leftHorizontal_1 = "DS4horizontalLstick_1";
        private readonly string rightVertical_1 = "DS4verticalRstick_1";
        private readonly string rightHorizontal_1 = "DS4horizontalRstick_1";
        private readonly string padVertical_2 = "DS4verticalPad_2";
        private readonly string padHorizontal_2 = "DS4horizontalPad_2";
        private readonly string leftVertical_2 = "DS4verticalLstick_2";
        private readonly string leftHorizontal_2 = "DS4horizontalLstick_2";
        private readonly string rightVertical_2 = "DS4verticalRstick_2";
        private readonly string rightHorizontal_2 = "DS4horizontalRstick_2";

        public struct DS4Input1
        {
            public bool Circle;
            public bool Cross;
            public bool Triangle;
            public bool Square;
            public bool UpKey;
            public bool DownKey;
            public bool LeftKey;
            public bool RightKey;
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
            public bool Option;
            public bool Share;
            public bool Home;
            public bool TrackPad;

            public void ResetKey()
            {
                Circle = false;
                Cross = false;
                Triangle = false;
                Square = false;
                UpKey = false;
                DownKey = false;
                LeftKey = false;
                RightKey = false;
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
                Option = false;
                Share = false;
                Home = false;
                TrackPad = false;
            }
        }

        public struct DS4Input2
        {
            public bool Circle;
            public bool Cross;
            public bool Triangle;
            public bool Square;
            public bool UpKey;
            public bool DownKey;
            public bool LeftKey;
            public bool RightKey;
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
            public bool Option;
            public bool Share;
            public bool Home;
            public bool TrackPad;

            public void ResetKey()
            {
                Circle = false;
                Cross = false;
                Triangle = false;
                Square = false;
                UpKey = false;
                DownKey = false;
                LeftKey = false;
                RightKey = false;
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
                Option = false;
                Share = false;
                Home = false;
                TrackPad = false;
            }
        }

        public DS4Input1 Controller1;
        public DS4Input2 Controller2;

        /// <summary>
        /// Controller1のGetAxisDown用のフラグ
        /// </summary>
        private struct AxisFlag_1
        {
            public bool Pu, Pd, Pl, Pr, Lu, Ld, Ll, Lr, Ru, Rd, Rl, Rr;

            public void ResetFlag()
            {
                Pu = false;
                Pd = false;
                Pl = false;
                Pr = false;
                Lu = false;
                Ld = false;
                Ll = false;
                Lr = false;
                Ru = false;
                Rd = false;
                Rl = false;
                Rr = false;
            }
        }

        /// <summary>
        /// Controller2のGetAxisDown用のフラグ
        /// </summary>
        private struct AxisFlag_2
        {
            public bool Pu, Pd, Pl, Pr, Lu, Ld, Ll, Lr, Ru, Rd, Rl, Rr;

            public void ResetFlag()
            {
                Pu = false;
                Pd = false;
                Pl = false;
                Pr = false;
                Lu = false;
                Ld = false;
                Ll = false;
                Lr = false;
                Ru = false;
                Rd = false;
                Rl = false;
                Rr = false;
            }
        }

        private AxisFlag_1 firstAxisFlag;
        private AxisFlag_2 secondAxisFlag;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                Controller1.ResetKey();
                Controller2.ResetKey();
                firstAxisFlag.ResetFlag();
                secondAxisFlag.ResetFlag();
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            ChangeControllerInput();
            DS4_SingleInput();
        }

        /// <summary>
        /// PS4の〇ボタンを押したコントローラーを1Pとして設定する
        /// </summary>
        private void ChangeControllerInput()
        {
            if (!oneShotCall) return;

            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                isChangeController = false;
                oneShotCall = false;
                Debug.Log("1P → 1P");
                return;
            }

            if (Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                isChangeController = true;
                oneShotCall = false;
                Debug.Log("2P → 1P");
                return;
            }
        }

        /// <summary>
        /// DS4のキーを1度だけ押したことを検知する
        /// </summary>
        private void DS4_SingleInput()
        {
            GetAxisDown(Axis.PadVertical_1);
            /*
            Controller1.Circle = isChangeController ? Input.GetButtonDown("DS4circle_2") : Input.GetButtonDown("DS4circle_1");

            Controller2.Circle = isChangeController ? Input.GetButtonDown("DS4circle_1") : Input.GetButtonDown("DS4circle_2");

            Controller1.Cross = isChangeController ? Input.GetButtonDown("DS4cross_2") : Input.GetButtonDown("DS4cross_1");

            Controller2.Cross = isChangeController ? Input.GetButtonDown("DS4cross_1") : Input.GetButtonDown("DS4cross_2");

            Controller1.Triangle = isChangeController ? Input.GetButtonDown("DS4triangle_2") : Input.GetButtonDown("DS4triangle_1");

            Controller2.Triangle = isChangeController ? Input.GetButtonDown("DS4triangle_1") : Input.GetButtonDown("DS4triangle_2");

            Controller1.Square = isChangeController ? Input.GetButtonDown("DS4square_2") : Input.GetButtonDown("DS4square_1");

            Controller2.Square = isChangeController ? Input.GetButtonDown("DS4square_1") : Input.GetButtonDown("DS4square_2");

            Controller1.UpKey = isChangeController ? GetAxisDown("DS4verticalPad_2", Axies.PadUp_2) : GetAxisDown("DS4verticalPad_1", Axies.PadUp_1);

            Controller2.UpKey = isChangeController ? GetAxisDown("DS4verticalPad_1", Axies.PadUp_1) : GetAxisDown("DS4verticalPad_2", Axies.PadUp_2);

            Controller1.DownKey = isChangeController ? GetAxisDown("DS4verticalPad_2", Axies.PadDown_2) : GetAxisDown("DS4verticalPad_1", Axies.PadDown_1);

            Controller2.DownKey = isChangeController ? GetAxisDown("DS4verticalPad_1", Axies.PadDown_1) : GetAxisDown("DS4verticalPad_2", Axies.PadDown_2);

            Controller1.LeftKey = isChangeController ? GetAxisDown("DS4horizontalPad_2", Axies.PadLeft_2) : GetAxisDown("DS4horizontalPad_1", Axies.PadLeft_1);

            Controller2.LeftKey = isChangeController ? GetAxisDown("DS4horizontalPad_1", Axies.PadLeft_1) : GetAxisDown("DS4horizontalPad_2", Axies.PadLeft_2);

            Controller1.RightKey = isChangeController ? GetAxisDown("DS4horizontalPad_2", Axies.PadRight_2) : GetAxisDown("DS4horizontalPad_1", Axies.PadRight_1);

            Controller2.RightKey = isChangeController ? GetAxisDown("DS4horizontalPad_1", Axies.PadRight_1) : GetAxisDown("DS4horizontalPad_2", Axies.PadRight_2);

            Controller1.L1 = isChangeController ? Input.GetButtonDown("DS4L1_2") : Input.GetButtonDown("DS4L1_1");

            Controller2.L1 = isChangeController ? Input.GetButtonDown("DS4L1_1") : Input.GetButtonDown("DS4L1_2");

            Controller1.L2 = isChangeController ? Input.GetButtonDown("DS4L2_2") : Input.GetButtonDown("DS4L2_1");

            Controller2.L2 = isChangeController ? Input.GetButtonDown("DS4L2_1") : Input.GetButtonDown("DS4L2_2");

            Controller1.L3 = isChangeController ? Input.GetButtonDown("DS4L3_2") : Input.GetButtonDown("DS4L3_1");

            Controller2.L3 = isChangeController ? Input.GetButtonDown("DS4L3_1") : Input.GetButtonDown("DS4L3_2");

            Controller1.LstickU = isChangeController ? GetAxisDown("DS4verticalLstick_2", Axies.LstickVerticalU_2) : GetAxisDown("DS4verticalLstick_1", Axies.LstickVerticalU_1);

            Controller2.LstickU = isChangeController ? GetAxisDown("DS4verticalLstick_1", Axies.LstickVerticalU_1) : GetAxisDown("DS4verticalLstick_2", Axies.LstickVerticalU_2);

            Controller1.LstickD = GetAxisDown("DS4verticalLstick_1", Axies.LstickVerticalD_1);

            Controller2.LstickD = GetAxisDown("DS4verticalLstick_2", Axies.LstickVerticalD_2);

            Controller1.LstickL = GetAxisDown("DS4horizontalLstick_1", Axies.LstickHorizontalL_1);

            Controller2.LstickL = GetAxisDown("DS4horizontalLstick_2", Axies.LstickHorizontalL_2);

            Controller1.LstickR = GetAxisDown("DS4horizontalLstick_1", Axies.LstickHorizontalR_1);

            Controller2.LstickR = GetAxisDown("DS4horizontalLstick_2", Axies.LstickHorizontalR_2);

            Controller1.R1 = Input.GetButtonDown("DS4R1_1");

            Controller2.R1 = Input.GetButtonDown("DS4R1_2");

            Controller1.R2 = Input.GetButtonDown("DS4R2_1");

            Controller2.R2 = Input.GetButtonDown("DS4R2_2");

            Controller1.R3 = Input.GetButtonDown("DS4R3_1");

            Controller2.R3 = Input.GetButtonDown("DS4R3_2");

            Controller1.RstickU = GetAxisDown("DS4verticalRstick_1", Axies.RstickVerticalU_1);

            Controller2.RstickU = GetAxisDown("DS4verticalRstick_2", Axies.RstickVerticalU_2);

            Controller1.RstickD = GetAxisDown("DS4verticalRstick_1", Axies.RstickVerticalD_1);

            Controller2.RstickD = GetAxisDown("DS4verticalRstick_2", Axies.RstickVerticalD_2);

            Controller1.RstickL = GetAxisDown("DS4horizontalRstick_1", Axies.RstickHorizontalL_1);

            Controller2.RstickL = GetAxisDown("DS4horizontalRstick_2", Axies.RstickHorizontalL_2);

            Controller1.RstickR = GetAxisDown("DS4horizontalRstick_1", Axies.RstickHorizontalR_1);

            Controller2.RstickR = GetAxisDown("DS4horizontalRstick_2", Axies.RstickHorizontalR_2);
            */
        }

        /*
        /// <summary>
        /// GetAxisを入力を検知したタイミングだけ取得
        /// </summary>
        /// <param name="axisName">取得したいAxis(InputManagerに登録した名前)</param>
        /// <param name="axis">取得したい入力方向</param>
        /// <returns></returns>
        private bool GetAxisDown(string axisName, Axies axis)
        {
            var input = Input.GetAxis(axisName);
            switch (axis)
            {
                case Axies.PadUp_1:
                    if(input > 0)
                    {
                        if (!firstAxisFlag.Pu)
                        {
                            firstAxisFlag.Pu = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Pu = false;
                        return false;
                    }
                case Axies.PadDown_1:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.Pd)
                        {
                            firstAxisFlag.Pd = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Pd = false;
                        return false;
                    }
                case Axies.PadLeft_1:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.Pl)
                        {
                            firstAxisFlag.Pl = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Pl = false;
                        return false;
                    }
                case Axies.PadRight_1:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Pr)
                        {
                            firstAxisFlag.Pr = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Pr = false;
                        return false;
                    }
                case Axies.LstickVerticalU_1:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Lu)
                        {
                            firstAxisFlag.Lu = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Lu = false;
                        return false;
                    }
                case Axies.LstickVerticalD_1:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.Ld)
                        {
                            firstAxisFlag.Ld = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Ld = false;
                        return false;
                    }
                case Axies.LstickHorizontalL_1:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.Ll)
                        {
                            firstAxisFlag.Ll = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Ll = false;
                        return false;
                    }
                case Axies.LstickHorizontalR_1:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Lr)
                        {
                            firstAxisFlag.Lr = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Lr = false;
                        return false;
                    }
                case Axies.RstickVerticalU_1:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Ru)
                        {
                            firstAxisFlag.Ru = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Ru = false;
                        return false;
                    }
                case Axies.RstickVerticalD_1:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.Rd)
                        {
                            firstAxisFlag.Rd = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Rd = false;
                        return false;
                    }
                case Axies.RstickHorizontalL_1:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.Rs)
                        {
                            firstAxisFlag.Rs = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Rs = false;
                        return false;
                    }
                case Axies.RstickHorizontalR_1:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Rr)
                        {
                            firstAxisFlag.Rr = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.Rr = false;
                        return false;
                    }
                case Axies.PadUp_2:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.PadUp_2)
                        {
                            firstAxisFlag.PadUp_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.PadUp_2 = false;
                        return false;
                    }
                case Axies.PadDown_2:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.PadDown_2)
                        {
                            firstAxisFlag.PadDown_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.PadDown_2 = false;
                        return false;
                    }
                case Axies.PadLeft_2:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.PadLeft_2)
                        {
                            firstAxisFlag.PadLeft_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.PadLeft_2 = false;
                        return false;
                    }
                case Axies.PadRight_2:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.PadRight_2)
                        {
                            firstAxisFlag.PadRight_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.PadRight_2 = false;
                        return false;
                    }
                case Axies.LstickVerticalU_2:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.LstickVerticalU_2)
                        {
                            firstAxisFlag.LstickVerticalU_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.LstickVerticalU_2 = false;
                        return false;
                    }
                case Axies.LstickVerticalD_2:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.LstickVerticalD_2)
                        {
                            firstAxisFlag.LstickVerticalD_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.LstickVerticalD_2 = false;
                        return false;
                    }
                case Axies.LstickHorizontalL_2:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.LstickHorizontalL_2)
                        {
                            firstAxisFlag.LstickHorizontalL_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.LstickHorizontalL_2 = false;
                        return false;
                    }
                case Axies.LstickHorizontalR_2:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.LstickHorizontalR_2)
                        {
                            firstAxisFlag.LstickHorizontalR_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.LstickHorizontalR_2 = false;
                        return false;
                    }
                case Axies.RstickVerticalU_2:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.RstickVerticalU_2)
                        {
                            firstAxisFlag.RstickVerticalU_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.RstickVerticalU_2 = false;
                        return false;
                    }
                case Axies.RstickVerticalD_2:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.RstickVerticalD_2)
                        {
                            firstAxisFlag.RstickVerticalD_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.RstickVerticalD_2 = false;
                        return false;
                    }
                case Axies.RstickHorizontalL_2:
                    if (input < 0)
                    {
                        if (!firstAxisFlag.RstickHorizontalL_2)
                        {
                            firstAxisFlag.RstickHorizontalL_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.RstickHorizontalL_2 = false;
                        return false;
                    }
                case Axies.RstickHorizontalR_2:
                    if (input > 0)
                    {
                        if (!firstAxisFlag.RstickHorizontalR_2)
                        {
                            firstAxisFlag.RstickHorizontalR_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        firstAxisFlag.RstickHorizontalR_2 = false;
                        return false;
                    }
                default:
                    return false;
            }
        }
        */

        /// <summary>
        /// GetAxisをGetButtonDownのように入力したときだけ取得する
        /// </summary>
        /// <param name="axis">取得したいAxis</param>
        /// <returns></returns>
        private void GetAxisDown(Axis axis)
        {
            float input;
            switch (axis)
            {
                case Axis.PadVertical_1:
                    input = Input.GetAxis(padVertical_1);
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Pu)
                        {
                            firstAxisFlag.Pu = true;
                            _ = isChangeController ? Controller2.UpKey = true : Controller1.UpKey = true;
                            Debug.Log((_=Controller1.UpKey?"Pad1: ":"Pad2: ") + "Up");
                            return;
                        }
                        _ = isChangeController ? Controller2.UpKey = false : Controller1.UpKey = false;
                        return;
                    }
                    else if (input < 0)
                    {
                        if (!firstAxisFlag.Pd)
                        {
                            firstAxisFlag.Pd = true;
                            _ = isChangeController ? Controller2.DownKey = true : Controller1.DownKey = true;
                            Debug.Log((_ = Controller1.DownKey ? "Pad1: " : "Pad2: ") + "Down");
                            return;
                        }
                        _ = isChangeController ? Controller2.DownKey = false : Controller1.DownKey = false;
                        return;
                    }
                    else
                    {
                        if (firstAxisFlag.Pu) firstAxisFlag.Pu = false;
                        if (firstAxisFlag.Pd) firstAxisFlag.Pd = false;
                        return;
                    }
                case Axis.PadVertical_2:
                    input = Input.GetAxis(padVertical_2);
                    if(input > 0)
                    {
                        if (!secondAxisFlag.Pu)
                        {
                            secondAxisFlag.Pu = true;
                            _ = isChangeController ? Controller1.UpKey = true : Controller2.UpKey = true;
                            Debug.Log((_ = Controller1.UpKey ? "Pad1: " : "Pad2: ") + "Up");
                            return;
                        }
                        _ = isChangeController ? Controller1.UpKey = false : Controller2.UpKey = false;
                        return;
                    }
                    else if(input < 0)
                    {
                        if (!secondAxisFlag.Pd)
                        {
                            secondAxisFlag.Pd = true;
                            _ = isChangeController ? Controller1.DownKey = true : Controller2.DownKey = true;
                            Debug.Log((_ = Controller1.DownKey ? "Pad1: " : "Pad2: ") + "Down");
                            return;
                        }
                        _ = isChangeController ? Controller1.DownKey = false : Controller2.DownKey = false;
                        return;
                    }
                    else
                    {
                        if (secondAxisFlag.Pu) secondAxisFlag.Pu = false;
                        if (secondAxisFlag.Pd) secondAxisFlag.Pd = false;
                        return;
                    }
                case Axis.PadHorizontal_1:
                    input = Input.GetAxis(padHorizontal_1);
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Pr)
                        {
                            firstAxisFlag.Pr = true;
                            _ = isChangeController ? Controller2.RightKey = true : Controller1.RightKey = true;
                            Debug.Log((_ = Controller1.RightKey ? "Pad1: " : "Pad2: ") + "Right");
                            return;
                        }
                        _ = isChangeController ? Controller2.RightKey = false : Controller1.RightKey = false;
                        return;
                    }
                    else if (input < 0)
                    {
                        if (!firstAxisFlag.Pl)
                        {
                            firstAxisFlag.Pl = true;
                            _ = isChangeController ? Controller2.LeftKey = true : Controller1.LeftKey = true;
                            Debug.Log((_ = Controller1.LeftKey ? "Pad1: " : "Pad2: ") + "Left");
                            return;
                        }
                        _ = isChangeController ? Controller2.LeftKey = false : Controller1.LeftKey = false;
                        return;
                    }
                    else
                    {
                        if (firstAxisFlag.Pr) firstAxisFlag.Pr = false;
                        if (firstAxisFlag.Pl) firstAxisFlag.Pl = false;
                        return;
                    }
                case Axis.PadHorizontal_2:
                    input = Input.GetAxis(padHorizontal_2);
                    if (input > 0)
                    {
                        if (!secondAxisFlag.Pr)
                        {
                            secondAxisFlag.Pr = true;
                            _ = isChangeController ? Controller1.RightKey = true : Controller2.RightKey = true;
                            Debug.Log((_ = Controller1.RightKey ? "Pad1: " : "Pad2: ") + "Right");
                            return;
                        }
                        _ = isChangeController ? Controller1.RightKey = false : Controller2.RightKey = false;
                        return;
                    }
                    else if (input < 0)
                    {
                        if (!secondAxisFlag.Pl)
                        {
                            secondAxisFlag.Pl = true;
                            _ = isChangeController ? Controller1.LeftKey = true : Controller2.LeftKey = true;
                            Debug.Log((_ = Controller1.LeftKey ? "Pad1: " : "Pad2: ") + "Left");
                            return;
                        }
                        _ = isChangeController ? Controller1.LeftKey = false : Controller2.LeftKey = false;
                        return;
                    }
                    else
                    {
                        if (secondAxisFlag.Pr) secondAxisFlag.Pr = false;
                        if (secondAxisFlag.Pl) secondAxisFlag.Pl = false;
                        return;
                    }
                case Axis.LstickVertical_1:
                    input = Input.GetAxis(leftVertical_1);
                    if (input > 0)
                    {
                        if (!firstAxisFlag.Lu)
                        {
                            firstAxisFlag.Lu = true;
                            _ = isChangeController ? Controller2.LstickU = true : Controller1.LstickU = true;
                            Debug.Log((_ = Controller1.LstickU ? "Lstick1: " : "Lstick2: ") + "Up");
                            return;
                        }
                        _ = isChangeController ? Controller2.LstickU = false : Controller1.LstickU = false;
                        return;
                    }
                    else if (input < 0)
                    {
                        if (!firstAxisFlag.Ld)
                        {
                            firstAxisFlag.Ld = true;
                            _ = isChangeController ? Controller2.LstickD = true : Controller1.LstickD = true;
                            Debug.Log((_ = Controller1.LstickD ? "Lstick1: " : "Lstick2: ") + "Down");
                            return;
                        }
                        _ = isChangeController ? Controller2.LstickD = false : Controller1.LstickD = false;
                        return;
                    }
                    else
                    {
                        if (firstAxisFlag.Lu) firstAxisFlag.Lu = false;
                        if (firstAxisFlag.Ld) firstAxisFlag.Ld = false;
                        return;
                    }
                case Axis.LstickVertical_2:
                    input = Input.GetAxis(leftVertical_2);
                    if (input > 0)
                    {
                        if (!secondAxisFlag.Lu)
                        {
                            secondAxisFlag.Lu = true;
                            _ = isChangeController ? Controller1.LstickU = true : Controller2.LstickU = true;
                            Debug.Log((_ = Controller1.LstickU ? "Lstick1: " : "Lstick2: ") + "Up");
                            return;
                        }
                        _ = isChangeController ? Controller1.LstickU = false : Controller2.LstickU = false;
                        return;
                    }
                    else if (input < 0)
                    {
                        if (!secondAxisFlag.Ld)
                        {
                            secondAxisFlag.Ld = true;
                            _ = isChangeController ? Controller1.LstickD = true : Controller2.LstickD = true;
                            Debug.Log((_ = Controller1.LstickD ? "Lstick1: " : "Lstick2: ") + "Down");
                            return;
                        }
                        _ = isChangeController ? Controller1.LstickD = false : Controller2.LstickD = false;
                        return;
                    }
                    else
                    {
                        if (secondAxisFlag.Lu) secondAxisFlag.Lu = false;
                        if (secondAxisFlag.Ld) secondAxisFlag.Ld = false;
                        return;
                    }
                case Axis.LstickHorizontal_1:
                    input = Input.GetAxis(leftHorizontal_1);
                    if (input > 0)
                    {
                        if (firstAxisFlag.Lr)
                        {
                            firstAxisFlag.Lr = true;
                            _ = isChangeController ? Controller2.LstickR = true : Controller1.LstickR = true;
                            Debug.Log("");
                            return;
                        }
                        _ = isChangeController ? Controller2.LstickR = false : Controller1.LstickR = false;
                        return;
                    }
                    else if (input < 0)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                case Axis.LstickHorizontal_2:
                    input = Input.GetAxis(leftHorizontal_2);
                    if (input > 0)
                    {
                        return;
                    }
                    else if (input < 0)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                case Axis.RstickVertical_1:
                    input = Input.GetAxis(rightVertical_1);
                    if (input > 0)
                    {
                        return;
                    }
                    else if (input < 0)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                case Axis.RstickVertical_2:
                    input = Input.GetAxis(rightVertical_2);
                    if (input > 0)
                    {
                        return;
                    }
                    else if (input < 0)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                case Axis.RstickHorizontal_1:
                    input = Input.GetAxis(rightHorizontal_1);
                    if (input > 0)
                    {
                        return;
                    }
                    else if (input < 0)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                case Axis.RstickHorizontal_2:
                    input = Input.GetAxis(rightHorizontal_2);
                    if (input > 0)
                    {
                        return;
                    }
                    else if (input < 0)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                default:
                    return;
            }
        }

        private enum Axies
        {
            PadUp_1,
            PadDown_1,
            PadLeft_1,
            PadRight_1,
            LstickVerticalU_1,
            LstickVerticalD_1,
            LstickHorizontalL_1,
            LstickHorizontalR_1,
            RstickVerticalU_1,
            RstickVerticalD_1,
            RstickHorizontalL_1,
            RstickHorizontalR_1,
            PadUp_2,
            PadDown_2,
            PadLeft_2,
            PadRight_2,
            LstickVerticalU_2,
            LstickVerticalD_2,
            LstickHorizontalL_2,
            LstickHorizontalR_2,
            RstickVerticalU_2,
            RstickVerticalD_2,
            RstickHorizontalL_2,
            RstickHorizontalR_2
        }

        private enum Axis
        {
            PadVertical_1,
            PadHorizontal_1,
            LstickVertical_1,
            LstickHorizontal_1,
            RstickVertical_1,
            RstickHorizontal_1,
            PadVertical_2,
            PadHorizontal_2,
            LstickVertical_2,
            LstickHorizontal_2,
            RstickVertical_2,
            RstickHorizontal_2
        }
    }
}