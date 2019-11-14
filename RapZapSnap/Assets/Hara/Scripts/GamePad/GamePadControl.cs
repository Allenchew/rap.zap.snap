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

        public bool IsChangeController { private set; get; } = false;    // 1Pのコントローラーと2Pのコントローラーの入力を入れ替えるか
        private bool oneShotCall = true;

        [SerializeField, Tooltip("PS4コントローラーを使う")]
        private bool usePs4Controller = true;

        private UnityEngine.EventSystems.StandaloneInputModule inputModule;

        // 現在、操作しているプレイヤー
        public InputController NowControlPlayer { set; private get; } = InputController.PlayerOne;

        private struct DS4KeyName
        {
            // UI操作用のキー
            public readonly string Submit;
            public readonly string Cancel;
            public readonly string Horizontal;
            public readonly string Vertical;

            // 十字キーやアナログスティックの個別入力キー
            public readonly string PadHorizontal;
            public readonly string PadVertical;
            public readonly string LstickHorizontal;
            public readonly string LstickVertical;
            public readonly string RstickHorizontal;
            public readonly string RstickVertical;

            public DS4KeyName(InputController inputPlayer)
            {
                if(inputPlayer == InputController.PlayerOne)
                {
                    Submit = "Submit_1";
                    Cancel = "Cancel_1";
                    Horizontal = "Horizontal_1";
                    Vertical = "Vertical_1";

                    PadHorizontal = "DS4horizontalPad_1";
                    PadVertical = "DS4verticalPad_1";
                    LstickHorizontal = "DS4horizontalLstick_1";
                    LstickVertical = "DS4verticalLstick_1";
                    RstickHorizontal = "DS4horizontalRstick_1";
                    RstickVertical = "DS4verticalRstick_1";
                }
                else
                {
                    Submit = "Submit_2";
                    Cancel = "Cancel_2";
                    Horizontal = "Horizontal_2";
                    Vertical = "Vertical_2";

                    PadHorizontal = "DS4horizontalPad_2";
                    PadVertical = "DS4verticalPad_2";
                    LstickHorizontal = "DS4horizontalLstick_2";
                    LstickVertical = "DS4verticalLstick_2";
                    RstickHorizontal = "DS4horizontalRstick_2";
                    RstickVertical = "DS4verticalRstick_2";
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

        public DS4InputKeyType Controller1;
        public DS4InputKeyType Controller2;

        /// <summary>
        /// GetAxisDown用のフラグ
        /// </summary>
        private struct AxisFlag
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

        private AxisFlag firstAxisFlag;
        private AxisFlag secondAxisFlag;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                Controller1.ResetKey();
                Controller2.ResetKey();
                firstAxisFlag.ResetFlag();
                secondAxisFlag.ResetFlag();
                joy1KeyName = new DS4KeyName(InputController.PlayerOne);
                joy2KeyName = new DS4KeyName(InputController.PlayerTwo);
                inputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
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
            
        }

        // Update is called once per frame
        void Update()
        {
            if (usePs4Controller)
            {
                ChangeControllerInput();
                DS4_SingleInput();
            }
        }

        /// <summary>
        /// PS4の〇ボタンを押したコントローラーを1Pとして設定する
        /// </summary>
        private void ChangeControllerInput()
        {
            if (!oneShotCall) return;

            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                IsChangeController = false;
                oneShotCall = false;
                Debug.Log("1P → 1P");
                SetInputModule(InputController.PlayerOne);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                IsChangeController = true;
                oneShotCall = false;
                SetInputModule(InputController.PlayerOne);
                Debug.Log("2P → 1P");
                return;
            }
        }

        /// <summary>
        /// DS4のキーを1度だけ押したことを検知する
        /// </summary>
        private void DS4_SingleInput()
        {
            // コントローラ１の入力
            GetAxisDown(Axis.PadVertical);
            GetAxisDown(Axis.PadHorizontal);
            GetAxisDown(Axis.LstickVertical);
            GetAxisDown(Axis.LstickHorizontal);
            Controller1.Circle = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button2) : Input.GetKeyDown(KeyCode.Joystick1Button2);
            Controller1.Cross = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button1) : Input.GetKeyDown(KeyCode.Joystick1Button1);
            Controller1.Triangle = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button3) : Input.GetKeyDown(KeyCode.Joystick1Button3);
            Controller1.Square = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button0) : Input.GetKeyDown(KeyCode.Joystick1Button0);
            Controller1.L1 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button4) : Input.GetKeyDown(KeyCode.Joystick1Button4);
            Controller1.R1 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button5) : Input.GetKeyDown(KeyCode.Joystick1Button5);
            Controller1.L2 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button6) : Input.GetKeyDown(KeyCode.Joystick1Button6);
            Controller1.R2 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button7) : Input.GetKeyDown(KeyCode.Joystick1Button7);

            // コントローラ２の入力
            GetAxisDown(Axis.PadVertical, InputController.PlayerTwo);
            GetAxisDown(Axis.PadHorizontal, InputController.PlayerTwo);
            GetAxisDown(Axis.LstickVertical, InputController.PlayerTwo);
            GetAxisDown(Axis.LstickHorizontal, InputController.PlayerTwo);
            Controller2.Circle = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button2) : Input.GetKeyDown(KeyCode.Joystick2Button2);
            Controller2.Cross = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button1) : Input.GetKeyDown(KeyCode.Joystick2Button1);
            Controller2.Triangle = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button3) : Input.GetKeyDown(KeyCode.Joystick2Button3);
            Controller2.Square = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button0) : Input.GetKeyDown(KeyCode.Joystick2Button0);
            Controller2.L1 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button4) : Input.GetKeyDown(KeyCode.Joystick2Button4);
            Controller2.R1 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button5) : Input.GetKeyDown(KeyCode.Joystick2Button5);
            Controller2.L2 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button6) : Input.GetKeyDown(KeyCode.Joystick2Button6);
            Controller2.R2 = IsChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button7) : Input.GetKeyDown(KeyCode.Joystick2Button7);
        }

        /// <summary>
        /// GetAxisをGetButtonDownのように入力したときだけ取得する
        /// </summary>
        /// <param name="axis">取得したいAxis</param>
        /// /// <param name="controller">対象コントローラー</param>
        /// <returns></returns>
        private void GetAxisDown(Axis axis, InputController controller = InputController.PlayerOne)
        {
            float input;
            switch (axis)
            {
                case Axis.PadVertical:
                    if(controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(joy1KeyName.PadVertical);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Pu)
                            {
                                firstAxisFlag.Pu = true;
                                _ = IsChangeController ? Controller2.UpKey = true : Controller1.UpKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.UpKey = false : Controller1.UpKey = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Pd)
                            {
                                firstAxisFlag.Pd = true;
                                _ = IsChangeController ? Controller2.DownKey = true : Controller1.DownKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.DownKey = false : Controller1.DownKey = false;
                            return;
                        }
                        else
                        {
                            if (firstAxisFlag.Pu) firstAxisFlag.Pu = false;
                            if (firstAxisFlag.Pd) firstAxisFlag.Pd = false;
                            return;
                        }
                    }
                    else
                    {
                        input = Input.GetAxis(joy2KeyName.PadVertical);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Pu)
                            {
                                secondAxisFlag.Pu = true;
                                _ = IsChangeController ? Controller1.UpKey = true : Controller2.UpKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.UpKey = false : Controller2.UpKey = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Pd)
                            {
                                secondAxisFlag.Pd = true;
                                _ = IsChangeController ? Controller1.DownKey = true : Controller2.DownKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.DownKey = false : Controller2.DownKey = false;
                            return;
                        }
                        else
                        {
                            if (secondAxisFlag.Pu) secondAxisFlag.Pu = false;
                            if (secondAxisFlag.Pd) secondAxisFlag.Pd = false;
                            return;
                        }
                    }
                case Axis.PadHorizontal:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(joy1KeyName.PadHorizontal);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Pr)
                            {
                                firstAxisFlag.Pr = true;
                                _ = IsChangeController ? Controller2.RightKey = true : Controller1.RightKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.RightKey = false : Controller1.RightKey = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Pl)
                            {
                                firstAxisFlag.Pl = true;
                                _ = IsChangeController ? Controller2.LeftKey = true : Controller1.LeftKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.LeftKey = false : Controller1.LeftKey = false;
                            return;
                        }
                        else
                        {
                            if (firstAxisFlag.Pr) firstAxisFlag.Pr = false;
                            if (firstAxisFlag.Pl) firstAxisFlag.Pl = false;
                            return;
                        }
                    }
                    else
                    {
                        input = Input.GetAxis(joy2KeyName.PadHorizontal);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Pr)
                            {
                                secondAxisFlag.Pr = true;
                                _ = IsChangeController ? Controller1.RightKey = true : Controller2.RightKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.RightKey = false : Controller2.RightKey = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Pl)
                            {
                                secondAxisFlag.Pl = true;
                                _ = IsChangeController ? Controller1.LeftKey = true : Controller2.LeftKey = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.LeftKey = false : Controller2.LeftKey = false;
                            return;
                        }
                        else
                        {
                            if (secondAxisFlag.Pr) secondAxisFlag.Pr = false;
                            if (secondAxisFlag.Pl) secondAxisFlag.Pl = false;
                            return;
                        }
                    }
                case Axis.LstickVertical:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(joy1KeyName.LstickVertical);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Lu)
                            {
                                firstAxisFlag.Lu = true;
                                _ = IsChangeController ? Controller2.LstickU = true : Controller1.LstickU = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.LstickU = false : Controller1.LstickU = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Ld)
                            {
                                firstAxisFlag.Ld = true;
                                _ = IsChangeController ? Controller2.LstickD = true : Controller1.LstickD = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.LstickD = false : Controller1.LstickD = false;
                            return;
                        }
                        else
                        {
                            if (firstAxisFlag.Lu) firstAxisFlag.Lu = false;
                            if (firstAxisFlag.Ld) firstAxisFlag.Ld = false;
                            return;
                        }
                    }
                    else
                    {
                        input = Input.GetAxis(joy2KeyName.LstickVertical);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Lu)
                            {
                                secondAxisFlag.Lu = true;
                                _ = IsChangeController ? Controller1.LstickU = true : Controller2.LstickU = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.LstickU = false : Controller2.LstickU = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Ld)
                            {
                                secondAxisFlag.Ld = true;
                                _ = IsChangeController ? Controller1.LstickD = true : Controller2.LstickD = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.LstickD = false : Controller2.LstickD = false;
                            return;
                        }
                        else
                        {
                            if (secondAxisFlag.Lu) secondAxisFlag.Lu = false;
                            if (secondAxisFlag.Ld) secondAxisFlag.Ld = false;
                            return;
                        }
                    }
                case Axis.LstickHorizontal:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(joy1KeyName.LstickHorizontal);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Lr)
                            {
                                firstAxisFlag.Lr = true;
                                _ = IsChangeController ? Controller2.LstickR = true : Controller1.LstickR = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.LstickR = false : Controller1.LstickR = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Ll)
                            {
                                firstAxisFlag.Ll = true;
                                _ = IsChangeController ? Controller2.LstickL = true : Controller1.LstickL = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.LstickL = false : Controller1.LstickL = false;
                            return;
                        }
                        else
                        {
                            if (firstAxisFlag.Lr) firstAxisFlag.Lr = false;
                            if (firstAxisFlag.Ll) firstAxisFlag.Ll = false;
                            return;
                        }
                    }
                    else
                    {
                        input = Input.GetAxis(joy2KeyName.LstickHorizontal);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Lr)
                            {
                                secondAxisFlag.Lr = true;
                                _ = IsChangeController ? Controller1.LstickR = true : Controller2.LstickR = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.LstickR = false : Controller2.LstickR = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Ll)
                            {
                                secondAxisFlag.Ll = true;
                                _ = IsChangeController ? Controller1.LstickL = true : Controller2.LstickL = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.LstickL = false : Controller2.LstickL = false;
                            return;
                        }
                        else
                        {
                            if (secondAxisFlag.Lr) secondAxisFlag.Lr = false;
                            if (secondAxisFlag.Ll) secondAxisFlag.Ll = false;
                            return;
                        }
                    }
                case Axis.RstickVertical:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(joy1KeyName.RstickVertical);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Ru)
                            {
                                firstAxisFlag.Ru = true;
                                _ = IsChangeController ? Controller2.RstickU = true : Controller1.RstickU = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.RstickU = false : Controller1.RstickU = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Rd)
                            {
                                firstAxisFlag.Rd = true;
                                _ = IsChangeController ? Controller2.RstickD = true : Controller1.RstickD = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.RstickD = false : Controller1.RstickD = false;
                            return;
                        }
                        else
                        {
                            if (firstAxisFlag.Ru) firstAxisFlag.Ru = false;
                            if (firstAxisFlag.Rd) firstAxisFlag.Rd = false;
                            return;
                        }
                    }
                    else
                    {
                        input = Input.GetAxis(joy2KeyName.RstickVertical);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Ru)
                            {
                                secondAxisFlag.Ru = true;
                                _ = IsChangeController ? Controller1.RstickU = true : Controller2.RstickU = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.RstickU = false : Controller2.RstickU = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Rd)
                            {
                                secondAxisFlag.Rd = true;
                                _ = IsChangeController ? Controller1.RstickD = true : Controller2.RstickD = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.RstickD = false : Controller2.RstickD = false;
                            return;
                        }
                        else
                        {
                            if (secondAxisFlag.Ru) secondAxisFlag.Ru = false;
                            if (secondAxisFlag.Rd) secondAxisFlag.Rd = false;
                            return;
                        }
                    }
                case Axis.RstickHorizontal:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(joy1KeyName.RstickHorizontal);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Rr)
                            {
                                firstAxisFlag.Rr = true;
                                _ = IsChangeController ? Controller2.RstickR = true : Controller1.RstickR = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.RstickR = true : Controller1.RstickR = true;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Rl)
                            {
                                firstAxisFlag.Rl = true;
                                _ = IsChangeController ? Controller2.RstickL = true : Controller1.RstickL = true;
                                return;
                            }
                            _ = IsChangeController ? Controller2.RstickL = true : Controller1.RstickL = true;
                            return;
                        }
                        else
                        {
                            if (firstAxisFlag.Rr) firstAxisFlag.Rr = false;
                            if (firstAxisFlag.Rl) firstAxisFlag.Rl = false;
                            return;
                        }
                    }
                    else
                    {
                        input = Input.GetAxis(joy2KeyName.RstickHorizontal);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Rr)
                            {
                                secondAxisFlag.Rr = true;
                                _ = IsChangeController ? Controller1.RstickR = true : Controller2.RstickR = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.RstickR = false : Controller2.RstickR = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Rl)
                            {
                                secondAxisFlag.Rl = true;
                                _ = IsChangeController ? Controller1.RstickL = true : Controller2.RstickL = true;
                                return;
                            }
                            _ = IsChangeController ? Controller1.RstickL = false : Controller2.RstickL = false;
                            return;
                        }
                        else
                        {
                            if (secondAxisFlag.Rr) secondAxisFlag.Rr = false;
                            if (secondAxisFlag.Rl) secondAxisFlag.Rl = false;
                            return;
                        }
                    }
                default:
                    return;
            }
        }

        private enum Axis
        {
            PadVertical,
            PadHorizontal,
            LstickVertical,
            LstickHorizontal,
            RstickVertical,
            RstickHorizontal
        }

        /// <summary>
        /// StandaloneInputModuleをPS4入力に対応させる
        /// </summary>
        /// <param name="input"></param>
        public void SetInputModule(InputController input)
        {
            // StandaloneInputModuleが無ければ処理を終了
            if (inputModule == null) return;
            
            // PS4入力に対応させる
            if(input == InputController.PlayerOne)
            {
                inputModule.submitButton = IsChangeController ? joy2KeyName.Submit : joy1KeyName.Submit;
                inputModule.cancelButton = IsChangeController ? joy2KeyName.Cancel : joy1KeyName.Cancel;
                inputModule.horizontalAxis = IsChangeController ? joy2KeyName.Horizontal : joy1KeyName.Horizontal;
                inputModule.verticalAxis = IsChangeController ? joy2KeyName.Vertical : joy1KeyName.Vertical;
            }
            else
            {
                inputModule.submitButton = IsChangeController ? joy1KeyName.Submit : joy2KeyName.Submit;
                inputModule.cancelButton = IsChangeController ? joy1KeyName.Cancel : joy2KeyName.Cancel;
                inputModule.horizontalAxis = IsChangeController ? joy1KeyName.Horizontal : joy2KeyName.Horizontal;
                inputModule.verticalAxis = IsChangeController ? joy1KeyName.Vertical : joy2KeyName.Vertical;
            }
        }
    }
}