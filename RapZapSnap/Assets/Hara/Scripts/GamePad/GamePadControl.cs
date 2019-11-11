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

        [SerializeField, Tooltip("PS4コントローラーを使う")]
        private bool usePs4Controller = true;

        [SerializeField, Tooltip("LスティックでUI操作を行う")]
        private bool useLstickForUI = false;

        private UnityEngine.EventSystems.StandaloneInputModule inputModule;

        // 現在、操作しているプレイヤー
        public InputController NowControlPlayer { set; private get; } = InputController.PlayerOne;

        // PS4コントローラーの主要なキー名
        private readonly string submit_1 = "DS4circle_1";
        private readonly string cancel_1 = "DS4cross_1";
        private readonly string padVertical_1 = "DS4verticalPad_1";
        private readonly string padHorizontal_1 = "DS4horizontalPad_1";
        private readonly string leftVertical_1 = "DS4verticalLstick_1";
        private readonly string leftHorizontal_1 = "DS4horizontalLstick_1";
        private readonly string rightVertical_1 = "DS4verticalRstick_1";
        private readonly string rightHorizontal_1 = "DS4horizontalRstick_1";
        private readonly string submit_2 = "DS4circle_2";
        private readonly string cancel_2 = "DS4cross_2";
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
                //UnityEngine.SceneManagement.SceneManager.sceneLoaded += InputModuleCheck;
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += InputModuleCheck;
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
                isChangeController = false;
                oneShotCall = false;
                Debug.Log("1P → 1P");
                SetInputModule(InputController.PlayerOne);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                isChangeController = true;
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
            Controller1.Circle = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button2) : Input.GetKeyDown(KeyCode.Joystick1Button2);
            Controller1.Cross = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button1) : Input.GetKeyDown(KeyCode.Joystick1Button1);
            Controller1.Triangle = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button3) : Input.GetKeyDown(KeyCode.Joystick1Button3);
            Controller1.Square = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button0) : Input.GetKeyDown(KeyCode.Joystick1Button0);
            Controller1.L1 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button4) : Input.GetKeyDown(KeyCode.Joystick1Button4);
            Controller1.R1 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button5) : Input.GetKeyDown(KeyCode.Joystick1Button5);
            Controller1.L2 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button6) : Input.GetKeyDown(KeyCode.Joystick1Button6);
            Controller1.R2 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick2Button7) : Input.GetKeyDown(KeyCode.Joystick1Button7);

            // コントローラ２の入力
            GetAxisDown(Axis.PadVertical, InputController.PlayerTwo);
            GetAxisDown(Axis.PadHorizontal, InputController.PlayerTwo);
            GetAxisDown(Axis.LstickVertical, InputController.PlayerTwo);
            GetAxisDown(Axis.LstickHorizontal, InputController.PlayerTwo);
            Controller2.Circle = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button2) : Input.GetKeyDown(KeyCode.Joystick2Button2);
            Controller2.Cross = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button1) : Input.GetKeyDown(KeyCode.Joystick2Button1);
            Controller2.Triangle = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button3) : Input.GetKeyDown(KeyCode.Joystick2Button3);
            Controller2.Square = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button0) : Input.GetKeyDown(KeyCode.Joystick2Button0);
            Controller2.L1 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button4) : Input.GetKeyDown(KeyCode.Joystick2Button4);
            Controller2.R1 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button5) : Input.GetKeyDown(KeyCode.Joystick2Button5);
            Controller2.L2 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button6) : Input.GetKeyDown(KeyCode.Joystick2Button6);
            Controller2.R2 = isChangeController ? Input.GetKeyDown(KeyCode.Joystick1Button7) : Input.GetKeyDown(KeyCode.Joystick2Button7);
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
                        input = Input.GetAxis(padVertical_1);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Pu)
                            {
                                firstAxisFlag.Pu = true;
                                _ = isChangeController ? Controller2.UpKey = true : Controller1.UpKey = true;
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
                    }
                    else
                    {
                        input = Input.GetAxis(padVertical_2);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Pu)
                            {
                                secondAxisFlag.Pu = true;
                                _ = isChangeController ? Controller1.UpKey = true : Controller2.UpKey = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.UpKey = false : Controller2.UpKey = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Pd)
                            {
                                secondAxisFlag.Pd = true;
                                _ = isChangeController ? Controller1.DownKey = true : Controller2.DownKey = true;
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
                    }
                case Axis.PadHorizontal:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(padHorizontal_1);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Pr)
                            {
                                firstAxisFlag.Pr = true;
                                _ = isChangeController ? Controller2.RightKey = true : Controller1.RightKey = true;
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
                    }
                    else
                    {
                        input = Input.GetAxis(padHorizontal_2);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Pr)
                            {
                                secondAxisFlag.Pr = true;
                                _ = isChangeController ? Controller1.RightKey = true : Controller2.RightKey = true;
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
                    }
                case Axis.LstickVertical:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(leftVertical_1);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Lu)
                            {
                                firstAxisFlag.Lu = true;
                                _ = isChangeController ? Controller2.LstickU = true : Controller1.LstickU = true;
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
                    }
                    else
                    {
                        input = Input.GetAxis(leftVertical_2);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Lu)
                            {
                                secondAxisFlag.Lu = true;
                                _ = isChangeController ? Controller1.LstickU = true : Controller2.LstickU = true;
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
                    }
                case Axis.LstickHorizontal:
                    if (controller == InputController.PlayerOne)
                    {
                        input = Input.GetAxis(leftHorizontal_1);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Lr)
                            {
                                firstAxisFlag.Lr = true;
                                _ = isChangeController ? Controller2.LstickR = true : Controller1.LstickR = true;
                                return;
                            }
                            _ = isChangeController ? Controller2.LstickR = false : Controller1.LstickR = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Ll)
                            {
                                firstAxisFlag.Ll = true;
                                _ = isChangeController ? Controller2.LstickL = true : Controller1.LstickL = true;
                                return;
                            }
                            _ = isChangeController ? Controller2.LstickL = false : Controller1.LstickL = false;
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
                        input = Input.GetAxis(leftHorizontal_2);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Lr)
                            {
                                secondAxisFlag.Lr = true;
                                _ = isChangeController ? Controller1.LstickR = true : Controller2.LstickR = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.LstickR = false : Controller2.LstickR = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Ll)
                            {
                                secondAxisFlag.Ll = true;
                                _ = isChangeController ? Controller1.LstickL = true : Controller2.LstickL = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.LstickL = false : Controller2.LstickL = false;
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
                        input = Input.GetAxis(rightVertical_1);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Ru)
                            {
                                firstAxisFlag.Ru = true;
                                _ = isChangeController ? Controller2.RstickU = true : Controller1.RstickU = true;
                                return;
                            }
                            _ = isChangeController ? Controller2.RstickU = false : Controller1.RstickU = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Rd)
                            {
                                firstAxisFlag.Rd = true;
                                _ = isChangeController ? Controller2.RstickD = true : Controller1.RstickD = true;
                                return;
                            }
                            _ = isChangeController ? Controller2.RstickD = false : Controller1.RstickD = false;
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
                        input = Input.GetAxis(rightVertical_2);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Ru)
                            {
                                secondAxisFlag.Ru = true;
                                _ = isChangeController ? Controller1.RstickU = true : Controller2.RstickU = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.RstickU = false : Controller2.RstickU = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Rd)
                            {
                                secondAxisFlag.Rd = true;
                                _ = isChangeController ? Controller1.RstickD = true : Controller2.RstickD = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.RstickD = false : Controller2.RstickD = false;
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
                        input = Input.GetAxis(rightHorizontal_1);
                        if (input > 0)
                        {
                            if (!firstAxisFlag.Rr)
                            {
                                firstAxisFlag.Rr = true;
                                _ = isChangeController ? Controller2.RstickR = true : Controller1.RstickR = true;
                                return;
                            }
                            _ = isChangeController ? Controller2.RstickR = true : Controller1.RstickR = true;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!firstAxisFlag.Rl)
                            {
                                firstAxisFlag.Rl = true;
                                _ = isChangeController ? Controller2.RstickL = true : Controller1.RstickL = true;
                                return;
                            }
                            _ = isChangeController ? Controller2.RstickL = true : Controller1.RstickL = true;
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
                        input = Input.GetAxis(rightHorizontal_2);
                        if (input > 0)
                        {
                            if (!secondAxisFlag.Rr)
                            {
                                secondAxisFlag.Rr = true;
                                _ = isChangeController ? Controller1.RstickR = true : Controller2.RstickR = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.RstickR = false : Controller2.RstickR = false;
                            return;
                        }
                        else if (input < 0)
                        {
                            if (!secondAxisFlag.Rl)
                            {
                                secondAxisFlag.Rl = true;
                                _ = isChangeController ? Controller1.RstickL = true : Controller2.RstickL = true;
                                return;
                            }
                            _ = isChangeController ? Controller1.RstickL = false : Controller2.RstickL = false;
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
        /// シーンが遷移したらそのシーンのStandaloneInputModuleを探す
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void InputModuleCheck(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if(inputModule == null)
            {
                // InputModuleを探す
                inputModule = FindObjectOfType<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            SetInputModule(NowControlPlayer);
        }

        /// <summary>
        /// アクティブシーンが切り替わったらそのシーンのStandaloneInputModuleを探す
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void InputModuleCheck(UnityEngine.SceneManagement.Scene thisScene, UnityEngine.SceneManagement.Scene nextScene)
        {
            if (inputModule == null)
            {
                // InputModuleを探す
                inputModule = FindObjectOfType<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            SetInputModule(NowControlPlayer);
        }

        /// <summary>
        /// StandaloneInputModuleをPS4入力に対応させる
        /// </summary>
        /// <param name="input"></param>
        public void SetInputModule(InputController input)
        {
            // シーン内にStandaloneInputModuleが無ければ処理を終了
            if (inputModule == null) return;

            // PS4入力に対応させる
            if(input == InputController.PlayerOne)
            {
                inputModule.submitButton = isChangeController ? submit_2 : submit_1;
                inputModule.cancelButton = isChangeController ? cancel_2 : cancel_1;
                if (useLstickForUI)
                {
                    inputModule.verticalAxis = isChangeController ? leftVertical_2 : leftVertical_1;
                    inputModule.horizontalAxis = isChangeController ? leftHorizontal_2 : leftHorizontal_1;
                }
                else
                {
                    inputModule.verticalAxis = isChangeController ? padVertical_2 : padVertical_1;
                    inputModule.horizontalAxis = isChangeController ? padHorizontal_2 : padHorizontal_1;
                }
            }
            else
            {
                inputModule.submitButton = isChangeController ? submit_1 : submit_2;
                inputModule.cancelButton = isChangeController ? cancel_1 : cancel_2;
                if (useLstickForUI)
                {
                    inputModule.verticalAxis = isChangeController ? leftVertical_1 : leftVertical_2;
                    inputModule.horizontalAxis = isChangeController ? leftHorizontal_1 : leftHorizontal_2;
                }
                else
                {
                    inputModule.verticalAxis = isChangeController ? padVertical_1 : padVertical_2;
                    inputModule.horizontalAxis = isChangeController ? padHorizontal_1 : padHorizontal_2;
                }
            }
        }
    }
}