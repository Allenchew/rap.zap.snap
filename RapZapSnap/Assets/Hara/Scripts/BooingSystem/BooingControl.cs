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
        if (booingFlag == false) { return; }

        ControllerNum target = booingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1;
        GamePadControl pad = GamePadControl.Instance;
        DS4InputDownKey input = booingPlayer == ControllerNum.P1 ? pad.Input_1 : pad.Input_2;

        // 〇ボタンでSEのみ再生
        if (input.Circle == true)
        {
            PlaySE(0);
        }

        // ×ボタンでSE再生とバイブレーションを実行
        if (input.Cross == true)
        {
            PlaySE(0);
            pad.SetVibration(target, 255f, 2.0f);
        }

        // □ボタンでSE再生と画面の揺れを実行
        if (input.Square == true)
        {
            PlaySE(0);
            ShakeCameraAction(1.0f, 0.5f);
        }
    }

    /// <summary>
    /// ブーイングシステムを使うプレイヤーを決める
    /// </summary>
    /// <param name="player">ブーイングシステムを使うプレイヤー</param>
    public void SetBooingPlayer(ControllerNum player)
    {
        booingPlayer = player;
        if (booingFlag == false) { booingFlag = true; }
    }

    /// <summary>
    /// ブーイングシステムを無効にする
    /// </summary>
    public void BooingSystemOff()
    {
        if (booingFlag == false) { return; }
        booingFlag = false;
    }
}
