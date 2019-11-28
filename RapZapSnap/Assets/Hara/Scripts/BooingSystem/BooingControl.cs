using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooingControl : MonoBehaviour
{
    public static BooingControl Instance { private set; get; } = null;

    [SerializeField, Tooltip("ブーイングができるプレイヤー")]
    private ControllerNum booingPlayer = ControllerNum.P2;

    [SerializeField, Tooltip("ブーイングシステムを使うか")]
    private bool booingFlag = false;

    [SerializeField, Tooltip("ブーイングを実行できる回数")]
    private int booingPlayCount = 3;
    private int booingDefaultCount = 0;

    private struct BooingSystem
    {
        public bool PlaySE;
        public bool PlaySE_Vibration;
        public bool PlaySE_ShakeCamera;
        public void ResetFlag()
        {
            PlaySE = true;
            PlaySE_Vibration = true;
            PlaySE_ShakeCamera = true;
        }
    }
    private BooingSystem booingDataBase;

    private AudioSource audioSource;

    [SerializeField, Tooltip("ノーツ入力を邪魔する用のSE音源リスト")]
    private List<AudioClip> audioClips = null;

    [SerializeField, Tooltip("画面を揺らす用のカメラ")]
    private GameObject shakeObject = null;
    public GameObject ShakeObject { set { shakeObject = value; } }

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            booingDataBase.ResetFlag();
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
        booingDefaultCount = booingPlayCount;
    }

    // Update is called once per frame
    void Update()
    {
        PlayBooing();
    }

    /// <summary>
    /// お邪魔SEを再生する
    /// </summary>
    /// <param name="seNumber">再生するSE番号</param>
    private void PlaySE(int seNumber)
    {
        if (seNumber < 0 || seNumber >= audioClips.Count || audioClips[seNumber] == null)
        {
            Debug.LogError("指定した番号にSEが登録されていません");
            return;
        }
        audioSource.PlayOneShot(audioClips[seNumber]);
    }

    /// <summary>
    /// カメラを揺らす（画面を揺らす）処理
    /// </summary>
    /// <param name="duration">揺らす時間</param>
    /// <param name="magnitude">揺れの強さ</param>
    private void ShakeCameraAction(float duration, float magnitude)
    {
        if(duration <= 0f || magnitude <= 0f) { return; }
        StartCoroutine(DoShake(duration, magnitude));
    }

    /// <summary>
    /// カメラを揺らす（画面を揺らす）コルーチン処理
    /// </summary>
    /// <param name="duration">揺らす時間</param>
    /// <param name="magnitude">揺れの強さ</param>
    /// <returns></returns>
    private IEnumerator DoShake(float duration, float magnitude)
    {
        // カメラが設定されていないならMainCameraを使う
        if(shakeObject == null) { shakeObject = Camera.main.gameObject; }

        var pos = shakeObject.transform.localPosition;
        var time = 0f;
        
        while(time < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            shakeObject.transform.localPosition = new Vector3(x, y, pos.z);
            time += Time.deltaTime;

            yield return null;
        }
        shakeObject.transform.localPosition = pos;
    }

    /// <summary>
    /// ブーイングシステムを実行
    /// </summary>
    private void PlayBooing()
    {
        if (booingFlag == false || booingPlayCount <= 0) { return; }

        ControllerNum target = booingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1;
        GamePadControl pad = GamePadControl.Instance;

        // 〇ボタンでSEのみ再生
        if (pad.GetButtonDown(booingPlayer, DS4ButtonKey.Circle) == true)
        {
            if (booingDataBase.PlaySE == false) { return; }
            PlaySE(0);
            booingDataBase.PlaySE = false;
            booingPlayCount--;
        }

        // ×ボタンでSE再生とバイブレーションを実行
        if (pad.GetButtonDown(booingPlayer, DS4ButtonKey.Cross) == true)
        {
            if (booingDataBase.PlaySE_Vibration == false) { return; }
            PlaySE(0);
            pad.SetVibration(target, 255f, 2.0f);
            booingDataBase.PlaySE_Vibration = false;
            booingPlayCount--;
        }

        // □ボタンでSE再生と画面の揺れを実行
        if (pad.GetButtonDown(booingPlayer, DS4ButtonKey.Square) == true)
        {
            if (booingDataBase.PlaySE_ShakeCamera == false) { return; }
            PlaySE(0);
            ShakeCameraAction(1.0f, 0.5f);
            booingDataBase.PlaySE_ShakeCamera = false;
            booingPlayCount--;
        }
    }

    /// <summary>
    /// ブーイングシステムの初期化
    /// </summary>
    private void ResetBooingSystem()
    {
        booingDataBase.ResetFlag();
        booingPlayCount = booingDefaultCount;
    }

    /// <summary>
    /// ブーイングシステムを使うプレイヤーを決める
    /// </summary>
    /// <param name="player">ブーイングシステムを使うプレイヤー</param>
    public void SetBooingPlayer(ControllerNum player)
    {
        ResetBooingSystem();
        booingPlayer = player;
        if (booingFlag == false) { booingFlag = true; }
    }

    /// <summary>
    /// ブーイングシステムを無効にする
    /// </summary>
    public void BooingSystemOff()
    {
        if (booingFlag == false) { return; }
        ResetBooingSystem();
        booingFlag = false;
    }
}
