using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour <T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError(typeof(T).ToString() + "がありません");
            }
            return instance;
        }
    }

    [SerializeField, Tooltip("シングルトンにするか")] protected bool singletonMode = true;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = GetComponent<T>();
        if(singletonMode == true) { DontDestroyOnLoad(gameObject); }
    }
}
