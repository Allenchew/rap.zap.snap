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

    [SerializeField, Tooltip("揺らしたいオブジェクト")]
    private GameObject shakeObject = null;
    public GameObject ShakeObject { set { shakeObject = value; } }

    [SerializeField, Tooltip("インクパーティクルオブジェクト")]
    private GameObject particleCameraObject = null;
    private ParticleSystem particleType1 = null;    // 1回だけインクを表示するパーティクル
    private ParticleSystem particleType2 = null;    // 指定した秒数の間何度でもインクを表示するパーティクル
    [SerializeField, Tooltip("パーティクルモード")] private bool particleMode = false;

    private int booingPlayCount = 3;
    private bool playVibration = true;
    private bool playShake = true;
    private bool playPaint = true;

    private bool isRunningShake = false;
    private bool isRunningParticle = false;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            if(particleCameraObject != null)
            {
                var particleCamera = Instantiate(particleCameraObject, gameObject.transform, false);
                particleCamera.transform.position = Camera.main.transform.position + Vector3.back * 10;
                particleType1 = particleCamera.transform.GetChild(0).GetComponent<ParticleSystem>();
                particleType2 = particleCamera.transform.GetChild(1).GetComponent<ParticleSystem>();
            }
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
    private void ShakeAction(float duration, float magnitude)
    {
        if(duration <= 0f || magnitude <= 0f || isRunningShake == true) { return; }
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
        isRunningShake = true;

        // 設定されていないならMainCameraを使う
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

        isRunningShake = false;
    }

    /// <summary>
    /// パーティクルの再生
    /// </summary>
    /// <param name="duration">再生時間</param>
    private void PlayParticle(float duration)
    {
        if(particleMode == true)
        {
            StartCoroutine(DoParticle(duration, false));
        }
        else
        {
            StartCoroutine(DoParticle(duration, true));
        }
    }

    /// <summary>
    /// パーティクルの再生用コルーチン
    /// </summary>
    /// <param name="duration">再生時間</param>
    /// <param name="mode">再生モード</param>
    /// <returns></returns>
    private IEnumerator DoParticle(float duration, bool mode)
    {
        isRunningParticle = true;

        float time = 0;

        ParticleSystem particle = mode == true ? particleType1 : particleType2;

        particle.Play();
        
        if(mode == true)
        {
            while (time < 1.8f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            particle.Pause();

            time = 0;
        }

        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if(mode == true)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        isRunningParticle = false;
    }

    /// <summary>
    /// ブーイングシステムを実行
    /// </summary>
    private void PlayBooing()
    {
        if (booingFlag == false) { return; }

        ControllerNum target = booingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1;
        DS4InputKey input = booingPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1 : GamePadControl.Instance.GetKeyDown_2;

        // 〇ボタンでSE再生とバイブレーションを実行
        if(input.Circle == true || (_ = booingPlayer == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.A) == true : Input.GetKeyDown(KeyCode.J) == true))
        {
            if(playVibration == false) { return; }
            PlaySE(0);
            GamePadControl.Instance.SetVibration(target, 255f, 2.0f);
            booingPlayCount--;
            playVibration = false;
        }

        // △ボタンでSE再生と画面の揺れを実行
        if(input.Triangle == true || (_ = booingPlayer == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.D) == true : Input.GetKeyDown(KeyCode.L) == true))
        {
            if(playShake == false) { return; }
            PlaySE(0);
            ShakeAction(2.0f, 0.5f);
            booingPlayCount--;
            playShake = false;
        }

        // □ボタンで画面の邪魔を表示
        if (input.Square == true || (_ = booingPlayer == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.W) == true : Input.GetKeyDown(KeyCode.I) == true))
        {
            if(playPaint == false) { return; }
            PlaySE(0);
            PlayParticle(5.0f);
            booingPlayCount--;
            playPaint = false;
        }

        if(booingPlayCount <= 0) { booingFlag = false; }
    }

    /// <summary>
    /// ブーイングシステムを使うプレイヤーを決める
    /// </summary>
    /// <param name="player">ブーイングシステムを使うプレイヤー</param>
    public void SetBooingPlayer(ControllerNum player)
    {
        booingPlayer = player;
        booingPlayCount = 3;
        playVibration = true;
        playShake = true;
        playPaint = true;
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
