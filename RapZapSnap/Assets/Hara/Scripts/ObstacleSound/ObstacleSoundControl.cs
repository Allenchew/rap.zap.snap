using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4;

public class ObstacleSoundControl : MonoBehaviour
{
    public static ObstacleSoundControl Instance { private set; get; } = null;

    public InputController InputPlayer { set; private get; } = InputController.PlayerTwo;

    private AudioSource audioSource;

    [SerializeField, Tooltip("ノーツ入力を邪魔する用のSE音源リスト")]
    private List<AudioClip> audioClips = new List<AudioClip>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
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
        InputKey();
    }

    /// <summary>
    /// お邪魔SEを再生する
    /// </summary>
    /// <param name="seNumber">再生するSE番号</param>
    private void PlaySE(int seNumber)
    {
        if(seNumber < 0|| seNumber >= audioClips.Count || audioClips[seNumber] == null)
        {
            Debug.LogError("指定した番号にSEが登録されていません");
            return;
        }
        audioSource.PlayOneShot(audioClips[seNumber]);
    }

    private void InputKey()
    {
        GamePadControl.DS4InputKeyType key;
        if(InputPlayer == InputController.PlayerOne)
        {
            key = GamePadControl.Instance.Controller1;
        }
        else
        {
            key = GamePadControl.Instance.Controller2;
        }

        if (key.Circle)
        {
            PlaySE(0);
        }
    }
}
