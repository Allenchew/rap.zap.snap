using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS4Input
{
    public class SingletonMonoBehaviour <T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    Debug.Log(typeof(T).ToString() + "がありません");
                }
                return instance;
            }
        }


        protected virtual void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = GetComponent<T>();
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
}


