using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetPlayer
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

        private struct AxisFlag
        {
            public bool PadUp_1,
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
            RstickHorizontalR_2;

            public void ResetFlag()
            {
                PadUp_1 = false;
                PadDown_1 = false;
                PadLeft_1 = false;
                PadRight_1 = false;
                LstickVerticalU_1 = false;
                LstickVerticalD_1 = false;
                LstickHorizontalL_1 = false;
                LstickHorizontalR_1 = false;
                RstickVerticalU_1 = false;
                RstickVerticalD_1 = false;
                RstickHorizontalL_1 = false;
                RstickHorizontalR_1 = false;
                PadUp_2 = false;
                PadDown_2 = false;
                PadLeft_2 = false;
                PadRight_2 = false;
                LstickVerticalU_2 = false;
                LstickVerticalD_2 = false;
                LstickHorizontalL_2 = false;
                LstickHorizontalR_2 = false;
                RstickVerticalU_2 = false;
                RstickVerticalD_2 = false;
                RstickHorizontalL_2 = false;
                RstickHorizontalR_2 = false;
            }
        }

        private AxisFlag axisFlag;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                Controller1.ResetKey();
                Controller2.ResetKey();
                axisFlag.ResetFlag();
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

            if (Input.GetButtonDown("DS4circle_1"))
            {
                isChangeController = false;
                oneShotCall = false;
                Debug.Log("1P → 1P");
                return;
            }

            if (Input.GetButtonDown("DS4circle_2"))
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

            Controller1.Option = Input.GetButtonDown("DS4option_1");

            Controller2.Option = Input.GetButtonDown("DS4option_2");

            Controller1.Share = Input.GetButtonDown("DS4share_1");

            Controller2.Share = Input.GetButtonDown("DS4share_2");

            Controller1.Home = Input.GetButtonDown("DS4home_1");

            Controller2.Home = Input.GetButtonDown("DS4home_2");

            Controller1.TrackPad = Input.GetButtonDown("DS4trackpad_1");

            Controller2.TrackPad = Input.GetButtonDown("DS4trackpad_2");
        }

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
                        if (!axisFlag.PadUp_1)
                        {
                            axisFlag.PadUp_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadUp_1 = false;
                        return false;
                    }
                case Axies.PadDown_1:
                    if (input < 0)
                    {
                        if (!axisFlag.PadDown_1)
                        {
                            axisFlag.PadDown_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadDown_1 = false;
                        return false;
                    }
                case Axies.PadLeft_1:
                    if (input < 0)
                    {
                        if (!axisFlag.PadLeft_1)
                        {
                            axisFlag.PadLeft_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadLeft_1 = false;
                        return false;
                    }
                case Axies.PadRight_1:
                    if (input > 0)
                    {
                        if (!axisFlag.PadRight_1)
                        {
                            axisFlag.PadRight_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadRight_1 = false;
                        return false;
                    }
                case Axies.LstickVerticalU_1:
                    if (input > 0)
                    {
                        if (!axisFlag.LstickVerticalU_1)
                        {
                            axisFlag.LstickVerticalU_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickVerticalU_1 = false;
                        return false;
                    }
                case Axies.LstickVerticalD_1:
                    if (input < 0)
                    {
                        if (!axisFlag.LstickVerticalD_1)
                        {
                            axisFlag.LstickVerticalD_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickVerticalD_1 = false;
                        return false;
                    }
                case Axies.LstickHorizontalL_1:
                    if (input < 0)
                    {
                        if (!axisFlag.LstickHorizontalL_1)
                        {
                            axisFlag.LstickHorizontalL_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickHorizontalL_1 = false;
                        return false;
                    }
                case Axies.LstickHorizontalR_1:
                    if (input > 0)
                    {
                        if (!axisFlag.LstickHorizontalR_1)
                        {
                            axisFlag.LstickHorizontalR_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickHorizontalR_1 = false;
                        return false;
                    }
                case Axies.RstickVerticalU_1:
                    if (input > 0)
                    {
                        if (!axisFlag.RstickVerticalU_1)
                        {
                            axisFlag.RstickVerticalU_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickVerticalU_1 = false;
                        return false;
                    }
                case Axies.RstickVerticalD_1:
                    if (input < 0)
                    {
                        if (!axisFlag.RstickVerticalD_1)
                        {
                            axisFlag.RstickVerticalD_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickVerticalD_1 = false;
                        return false;
                    }
                case Axies.RstickHorizontalL_1:
                    if (input < 0)
                    {
                        if (!axisFlag.RstickHorizontalL_1)
                        {
                            axisFlag.RstickHorizontalL_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickHorizontalL_1 = false;
                        return false;
                    }
                case Axies.RstickHorizontalR_1:
                    if (input > 0)
                    {
                        if (!axisFlag.RstickHorizontalR_1)
                        {
                            axisFlag.RstickHorizontalR_1 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickHorizontalR_1 = false;
                        return false;
                    }
                case Axies.PadUp_2:
                    if (input > 0)
                    {
                        if (!axisFlag.PadUp_2)
                        {
                            axisFlag.PadUp_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadUp_2 = false;
                        return false;
                    }
                case Axies.PadDown_2:
                    if (input < 0)
                    {
                        if (!axisFlag.PadDown_2)
                        {
                            axisFlag.PadDown_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadDown_2 = false;
                        return false;
                    }
                case Axies.PadLeft_2:
                    if (input < 0)
                    {
                        if (!axisFlag.PadLeft_2)
                        {
                            axisFlag.PadLeft_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadLeft_2 = false;
                        return false;
                    }
                case Axies.PadRight_2:
                    if (input > 0)
                    {
                        if (!axisFlag.PadRight_2)
                        {
                            axisFlag.PadRight_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.PadRight_2 = false;
                        return false;
                    }
                case Axies.LstickVerticalU_2:
                    if (input > 0)
                    {
                        if (!axisFlag.LstickVerticalU_2)
                        {
                            axisFlag.LstickVerticalU_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickVerticalU_2 = false;
                        return false;
                    }
                case Axies.LstickVerticalD_2:
                    if (input < 0)
                    {
                        if (!axisFlag.LstickVerticalD_2)
                        {
                            axisFlag.LstickVerticalD_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickVerticalD_2 = false;
                        return false;
                    }
                case Axies.LstickHorizontalL_2:
                    if (input < 0)
                    {
                        if (!axisFlag.LstickHorizontalL_2)
                        {
                            axisFlag.LstickHorizontalL_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickHorizontalL_2 = false;
                        return false;
                    }
                case Axies.LstickHorizontalR_2:
                    if (input > 0)
                    {
                        if (!axisFlag.LstickHorizontalR_2)
                        {
                            axisFlag.LstickHorizontalR_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.LstickHorizontalR_2 = false;
                        return false;
                    }
                case Axies.RstickVerticalU_2:
                    if (input > 0)
                    {
                        if (!axisFlag.RstickVerticalU_2)
                        {
                            axisFlag.RstickVerticalU_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickVerticalU_2 = false;
                        return false;
                    }
                case Axies.RstickVerticalD_2:
                    if (input < 0)
                    {
                        if (!axisFlag.RstickVerticalD_2)
                        {
                            axisFlag.RstickVerticalD_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickVerticalD_2 = false;
                        return false;
                    }
                case Axies.RstickHorizontalL_2:
                    if (input < 0)
                    {
                        if (!axisFlag.RstickHorizontalL_2)
                        {
                            axisFlag.RstickHorizontalL_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickHorizontalL_2 = false;
                        return false;
                    }
                case Axies.RstickHorizontalR_2:
                    if (input > 0)
                    {
                        if (!axisFlag.RstickHorizontalR_2)
                        {
                            axisFlag.RstickHorizontalR_2 = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        axisFlag.RstickHorizontalR_2 = false;
                        return false;
                    }
                default:
                    return false;
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