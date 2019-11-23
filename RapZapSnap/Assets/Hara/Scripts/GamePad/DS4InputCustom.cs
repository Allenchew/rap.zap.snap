using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.InteropServices;

namespace DS4InputCustom
{
    public struct DS4Color
    {
        public byte red;
        public byte green;
        public byte blue;

        public DS4Color(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public static DS4Color Black => new DS4Color(13, 13, 13);
        public static DS4Color White => new DS4Color(255, 255, 255);
        public static DS4Color Blue => new DS4Color(70, 70, 217);
        public static DS4Color Red => new DS4Color(204, 17, 17);
        public static DS4Color Yellow => new DS4Color(235, 235, 0);
        public static DS4Color Green => new DS4Color(29, 140, 31);
        public static DS4Color Purple => new DS4Color(152, 6, 200);
        public static DS4Color Orange => new DS4Color(205, 115, 48);
    }

    public struct DS4Vibration
    {
        public byte right;
        public byte left;

        public DS4Vibration(byte right, byte left)
        {
            this.right = right;
            this.left = left;
        }

        public static DS4Vibration Max { get { return new DS4Vibration(255, 255); } }
        public static DS4Vibration Min { get { return new DS4Vibration(0, 0); } }
        public static DS4Vibration RightMax { get { return new DS4Vibration(255, 0); } }
        public static DS4Vibration LeftMax { get { return new DS4Vibration(0, 255); } }
    }

    public class DS4InputCustom : MonoBehaviour
    {
        [DllImport("DS4Input")]
        private static extern bool StartDS4();
        [DllImport("DS4Input")]
        private static extern bool GetController();
        [DllImport("DS4Input")]
        private static extern bool ControllerChack(int id);
        [DllImport("DS4Input")]
        private static extern bool ChangeColor(int id, byte r, byte g, byte b);
        [DllImport("DS4Input")]
        private static extern void ChangeVibration(int id, byte right, byte left);
        [DllImport("DS4Input")]
        private static extern void SendOutput(int id);
        [DllImport("DS4Input")]
        private static extern bool UpdateInputReport();
        [DllImport("DS4Input")]
        private static extern bool GetButton(int id, DS4ButtonType keyType);
        [DllImport("DS4Input")]
        private static extern bool GetButtonDown(int id, DS4ButtonType keyType);
        [DllImport("DS4Input")]
        private static extern bool GetButtonUp(int id, DS4ButtonType keyType);
        [DllImport("DS4Input")]
        private static extern float GetAxis(int id, DS4AxisType keyType);
        [DllImport("DS4Input")]
        private static extern bool EndDS4();

        [SerializeField] private bool multithreadUpdate = true;
        private bool isInputUpdate = false;
        private Task task = null;
        private bool isStart = false;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                GetController();
            }
            if (multithreadUpdate) { return; }
            if (isInputUpdate == false) { return; }
            UpdateInputReport();
        }

        private void OnDestroy()
        {
            Delete();
        }

        /// <summary>
        /// DS4Inputの初期化
        /// </summary>
        public void Init()
        {
            isStart = StartDS4();
            GetController();
            isInputUpdate = true;
            if (multithreadUpdate)
            {
                task = new Task(InputUpdate);
                task.Start();
            }
        }

        /// <summary>
        /// コントローラーの取得
        /// </summary>
        public void UpdateGetController()
        {
            if (isStart == false)
            {
                StartDS4();
            }
            GetController();
        }

        /// <summary>
        /// DS4Inputの破棄
        /// </summary>
        public void Delete()
        {
            isInputUpdate = false;
            if (task != null)
            {
                task.Wait();
                task.Dispose();
            }

            for (int i = 0; i < (int)DS4ControllerType.P4 + 1; i++)
            {
                if (IsController((DS4ControllerType)i) == true)
                {
                    SetColorAndVibration((DS4ControllerType)i, DS4Color.Black, DS4Vibration.Min);
                }
            }
            EndDS4();
        }

        public bool IsController(DS4ControllerType id)
        {
            return ControllerChack((int)id);
        }

        /// <summary>
        /// 指定したコントローラの指定のボタンが押されているか(非同期アップデートを使用する場合はこの関数を使用)
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="keyType">ボタンのタイプ</param>
        /// <returns></returns>
        public bool IsButton(DS4ControllerType id, DS4ButtonType keyType)
        {
            return GetButton((int)id, keyType);
        }

        /// <summary>
        /// 指定したコントローラの指定のボタンが押したか(非同期のアップデートだと上手く取れない)
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="keyType">ボタンのタイプ</param>
        /// <returns></returns>
        public bool IsButtonDown(DS4ControllerType id, DS4ButtonType keyType)
        {
            return GetButtonDown((int)id, keyType);
        }

        /// <summary>
        /// 指定したコントローラの指定のボタンを離したか(非同期のアップデートだと上手く取れない)
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="keyType">ボタンのタイプ</param>
        /// <returns></returns>
        public bool IsButtonUp(DS4ControllerType id, DS4ButtonType keyType)
        {
            return GetButtonUp((int)id, keyType);
        }

        /// <summary>
        /// 指定したコントローラの指定のスティックの値を取得
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="axisType">スティックのタイプ</param>
        /// <returns></returns>
        public float IsAxis(DS4ControllerType id, DS4AxisType axisType)
        {
            float v = GetAxis((int)id, axisType);
            if (Mathf.Abs(v) < 0.1) { v = 0; }
            if (Mathf.Abs(v) > 1) { v = 1; }
            return v;
        }

        /// <summary>
        /// 指定したコントローラに振動をさせる
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="vibration">振動値</param>
        public void SetVibration(DS4ControllerType id, DS4Vibration vibration)
        {
            if (!GetController()) return;
            ChangeVibration((int)id, vibration.right, vibration.left);
            SendOutput((int)id);
        }

        /// <summary>
        /// 指定したコントローラのバックライトをを変更する
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="color">変化後の色</param>
        public void SetColor(DS4ControllerType id, DS4Color color)
        {
            ChangeColor((int)id, color.red, color.green, color.blue);
            SendOutput((int)id);
        }

        /// <summary>
        /// 指定したコントローラに振動とバックライトの色を送る
        /// </summary>
        /// <param name="id">コントローラの番号</param>
        /// <param name="color">変化後の色</param>
        /// <param name="vibration">振動値</param>
        public void SetColorAndVibration(DS4ControllerType id, DS4Color color, DS4Vibration vibration)
        {
            ChangeColor((int)id, color.red, color.green, color.blue);
            ChangeVibration((int)id, vibration.right, vibration.left);
            SendOutput((int)id);
        }

        /// <summary>
        /// 非同期で更新する場合
        /// </summary>
        private void InputUpdate()
        {
            while (isInputUpdate == true)
            {
                UpdateInputReport();
            }
        }
    }
}
