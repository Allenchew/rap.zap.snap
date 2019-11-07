using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS4
{
    public class GamePadControl : MonoBehaviour
    {
        public static GamePadControl Instance { private set; get; } = null;

        public int i = 0;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
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
            if (Input.GetButton("DS4circle_1"))
            {
                Debug.Log("○ : コントローラー番号 1");
            }

            if (Input.GetButton("DS4circle_2"))
            {
                Debug.Log("○ : コントローラー番号 2");
            }

            if (Input.GetButton("DS4cross_1"))
            {
                Debug.Log("× : コントローラー番号 1");
            }

            if (Input.GetButton("DS4cross_2"))
            {
                Debug.Log("× : コントローラー番号 2");
            }
        }
    }
}