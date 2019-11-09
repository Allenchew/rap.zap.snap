using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS4
{
    public class GamePadControl : MonoBehaviour
    {
        public static GamePadControl Instance { private set; get; } = null;

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

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                Controller1.ResetKey();
                Controller2.ResetKey();
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
            InputDS4key();
        }

        /// <summary>
        /// DS4の入力を検知する
        /// </summary>
        private void InputDS4key()
        {
            if (Input.GetButtonDown("DS4circle_1"))
            {
                Debug.Log("○ : コントローラー番号 1");
            }

            Controller1.Circle = Input.GetButtonDown("DS4circle_1");

            if (Input.GetButton("DS4circle_2"))
            {
                Debug.Log("○ : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4cross_1"))
            {
                Debug.Log("× : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4cross_2"))
            {
                Debug.Log("× : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4triangle_1"))
            {
                Debug.Log("△ : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4triangle_2"))
            {
                Debug.Log("△ : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4square_1"))
            {
                Debug.Log("□ : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4square_2"))
            {
                Debug.Log("□ : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4horizontalPad_1") > 0)
            {
                Debug.Log("十字キー 右 : コントローラー番号 1");
            }
            else if (Input.GetAxis("DS4horizontalPad_1") < 0)
            {
                Debug.Log("十字キー 左 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4horizontalPad_2") > 0)
            {
                Debug.Log("十字キー 右 : コントローラー番号 2");
            }
            else if (Input.GetAxis("DS4horizontalPad_2") < 0)
            {
                Debug.Log("十字キー 左 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4verticalPad_1") > 0)
            {
                Debug.Log("十字キー 上 : コントローラー番号 1");
            }
            else if (Input.GetAxis("DS4verticalPad_1") < 0)
            {
                Debug.Log("十字キー 下 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4verticalPad_2") > 0)
            {
                Debug.Log("十字キー 上 : コントローラー番号 2");
            }
            else if (Input.GetAxis("DS4verticalPad_2") < 0)
            {
                Debug.Log("十字キー 下 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4L1_1"))
            {
                Debug.Log("L1 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4L1_2"))
            {
                Debug.Log("L1 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4L2_1") > -1)
            {
                Debug.Log("L2 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4L2_2") > -1)
            {
                Debug.Log("L2 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4L3_1"))
            {
                Debug.Log("L3 押し込み : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4L3_2"))
            {
                Debug.Log("L3 押し込み : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4horizontalLstick_1") > 0)
            {
                Debug.Log("Lスティック 右 : コントローラー番号 1");
            }
            else if (Input.GetAxis("DS4horizontalLstick_1") < 0)
            {
                Debug.Log("Lスティック 左 : コントローラー番号 1");
            }
            else
            {
                
            }

            if (Input.GetAxis("DS4horizontalLstick_2") > 0)
            {
                Debug.Log("Lスティック 右 : コントローラー番号 2");
            }
            else if (Input.GetAxis("DS4horizontalLstick_2") < 0)
            {
                Debug.Log("Lスティック 左 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4verticalLstick_1") > 0)
            {
                Debug.Log("Lスティック 上 : コントローラー番号 1");
            }
            else if (Input.GetAxis("DS4verticalLstick_1") < 0)
            {
                Debug.Log("Lスティック 下 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4verticalLstick_2") > 0)
            {
                Debug.Log("Lスティック 上 : コントローラー番号 2");
            }
            else if (Input.GetAxis("DS4verticalLstick_2") < 0)
            {
                Debug.Log("Lスティック 下 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4R1_1"))
            {
                Debug.Log("R1 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4R1_2"))
            {
                Debug.Log("R1 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4R2_1") > -1)
            {
                Debug.Log("R2 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4R2_2") > -1)
            {
                Debug.Log("R2 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4R3_1"))
            {
                Debug.Log("R3 押し込み : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4R3_2"))
            {
                Debug.Log("R3 押し込み : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4horizontalRstick_1") > 0)
            {
                Debug.Log("Rスティック 右 : コントローラー番号 1");
            }
            else if (Input.GetAxis("DS4horizontalRstick_1") < 0)
            {
                Debug.Log("Rスティック 左 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4horizontalRstick_2") > 0)
            {
                Debug.Log("Rスティック 右 : コントローラー番号 2");
            }
            else if (Input.GetAxis("DS4horizontalRstick_2") < 0)
            {
                Debug.Log("Rスティック 左 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetAxis("DS4verticalRstick_1") > 0)
            {
                Debug.Log("Rスティック 上 : コントローラー番号 1");
            }
            else if (Input.GetAxis("DS4verticalRstick_1") < 0)
            {
                Debug.Log("Rスティック 下 : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetAxis("DS4verticalRstick_2") > 0)
            {
                Debug.Log("Rスティック 上 : コントローラー番号 2");
            }
            else if (Input.GetAxis("DS4verticalRstick_2") < 0)
            {
                Debug.Log("Lスティック 下 : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4option_1"))
            {
                Debug.Log("OPTION : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4option_2"))
            {
                Debug.Log("OPTION : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4share_1"))
            {
                Debug.Log("SHARE : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4share_2"))
            {
                Debug.Log("SHARE : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4home_1"))
            {
                Debug.Log("PSボタン : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4home_2"))
            {
                Debug.Log("PSボタン : コントローラー番号 2");
            }
            else
            {

            }

            if (Input.GetButton("DS4trackpad_1"))
            {
                Debug.Log("タッチパッド 押し込み : コントローラー番号 1");
            }
            else
            {

            }

            if (Input.GetButton("DS4trackpad_2"))
            {
                Debug.Log("タッチパッド 押し込み : コントローラー番号 2");
            }
            else
            {

            }
        }
    }
}