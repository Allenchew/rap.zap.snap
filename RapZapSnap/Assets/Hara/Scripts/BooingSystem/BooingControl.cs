using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooingControl : SingletonMonoBehaviour<BooingControl>
{
    [SerializeField, Tooltip("ブーイングシステムを使うか")]
    private bool booingFlag = false;

    [SerializeField, Tooltip("ブーイングができるプレイヤー")]
    private ControllerNum booingPlayer = ControllerNum.P2;

    private AudioSource audioSource;

    [SerializeField, Tooltip("SE音源リスト"), Header("お邪魔用のサウンド")]
    private List<AudioClip> audioClips = null;

    [SerializeField, Tooltip("振動の強さ"), Header("コントローラーを振動させるお邪魔システム")]
    private byte vibrationPower = 255;
    [SerializeField, Tooltip("再生時間"), Range(0, 5.0f)] private float vibDuration = 2.0f;
    private bool isRunningVibration = false;
    private Coroutine vibrationCoroutine = null;

    [SerializeField, Tooltip("揺れの強さ"), Range(0f, 2.0f), Header("画面を揺らすお邪魔システム")]
    private float shakeMagnitude = 0.5f;
    [SerializeField, Tooltip("再生時間"), Range(0f, 5.0f)]
    private float shakeDuration = 1.0f;
    private GameObject mainCamera = null;
    private Vector3 mainCameraPos;
    private bool isRunningShake = false;
    private Coroutine shakeCoroutine = null;

    [SerializeField, Tooltip("インクパーティクルオブジェクト"), Header("画面を汚すお邪魔システム")]
    private GameObject particleCameraObject = null;
    private ParticleSystem particle = null;
    private UnityEngine.UI.RawImage particleRawImage = null;
    [SerializeField, Tooltip("再生回数"), Range(0, 5)] private int particleCallTime = 1;
    [SerializeField, Tooltip("再生間隔"), Range(0f, 2.0f)] private float particleCallSpan = 1.0f;
    [SerializeField, Tooltip("パーティクルのカラー")] private Color particleColor = Color.black;
    [SerializeField, Tooltip("パーティクルのサイズ"), Range(0f, 3.0f)] private float particleSize = 1.0f;
    private bool isRunningParticle = false;
    private Coroutine particleCoroutine = null;

    private int booingPlayCount = 3;
    private bool playVibration = true;
    private bool playShake = true;
    private bool playPaint = true;

    protected override void Awake()
    {
        base.Awake();
        Init();
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
    /// BooingControlの初期化
    /// </summary>
    private void Init()
    {
        audioSource = GetComponent<AudioSource>();

        if (particleCameraObject != null)
        {
            var particleCamera = Instantiate(particleCameraObject, gameObject.transform, false);
            particleCamera.transform.position = Camera.main.transform.position + Vector3.back * 10;
            particle = particleCamera.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleRawImage = particleCamera.transform.GetChild(2).transform.GetChild(0).GetComponent<UnityEngine.UI.RawImage>();
        }
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
    /// コントローラーの振動
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="vibration">振動値</param>
    /// <param name="duration">振動時間</param>
    private void StartVibration(ControllerNum id, byte vibration, float duration)
    {
        if (isRunningVibration == true) { return; }
        vibrationCoroutine = StartCoroutine(DoVibration(id, vibration, duration));
    }

    /// <summary>
    /// コントローラーの振動コルーチン
    /// </summary>
    /// <param name="id">コントローラー番号</param>
    /// <param name="vibration">振動値</param>
    /// <param name="duration">振動時間</param>
    /// <returns></returns>
    private IEnumerator DoVibration(ControllerNum id, byte vibration, float duration)
    {
        isRunningVibration = true;

        float time = 0f;

        // 振動開始
        GamePadControl.Instance.SetVibration(id, vibration);

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 振動停止
        GamePadControl.Instance.StopVibration(id);

        isRunningVibration = false;
    }

    /// <summary>
    /// カメラを揺らす（画面を揺らす）処理
    /// </summary>
    /// <param name="duration">揺らす時間</param>
    /// <param name="magnitude">揺れの強さ</param>
    private void ShakeAction(float duration, float magnitude)
    {
        if(duration <= 0f || magnitude <= 0f || isRunningShake == true) { return; }
        shakeCoroutine = StartCoroutine(DoShake(duration, magnitude));
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

        // MainCameraを取得
        if (mainCamera == null) { mainCamera = Camera.main.gameObject; }
        
        mainCameraPos = mainCamera.transform.localPosition;
        var time = 0f;
        
        while(time < duration)
        {
            var x = mainCameraPos.x + Random.Range(-1f, 1f) * magnitude;
            var y = mainCameraPos.y + Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(x, y, mainCameraPos.z);
            time += Time.deltaTime;

            yield return null;
        }
        
        mainCamera.transform.localPosition = mainCameraPos;

        isRunningShake = false;
    }

    /// <summary>
    /// パーティクル再生
    /// </summary>
    /// <param name="time">再生回数</param>
    /// <param name="span">再生間隔</param>
    private void PlayParticle(int time, float span)
    {
        if(isRunningParticle == true) { return; }

        particleCoroutine = StartCoroutine(DoParticle(time, span));
    }

    /// <summary>
    /// パーティクル再生用のコルーチン
    /// </summary>
    /// <param name="time">再生回数</param>
    /// <param name="span">再生間隔</param>
    /// <returns></returns>
    private IEnumerator DoParticle(int time, float span)
    {
        isRunningParticle = true;

        // パーティクルの色(RawImageの色)を設定
        particleRawImage.color = particleColor;

        // パーティクルのMainModuleを取得
        ParticleSystem.MainModule mainModule = particle.main;

        float deltaTime;

        for (int i = 0; i < time; i++)
        {
            // 時間の初期化
            deltaTime = 0f;

            // パーティクルの大きさの設定
            mainModule.startSize = particleSize;

            // パーティクルの再生
            particle.Emit(1);

            // 待機
            while(deltaTime < span)
            {
                deltaTime += Time.deltaTime;
                yield return null;
            }
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
            StartVibration(target, vibrationPower, vibDuration);
            booingPlayCount--;
            playVibration = false;
        }

        // △ボタンでSE再生と画面の揺れを実行
        if(input.Triangle == true || (_ = booingPlayer == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.D) == true : Input.GetKeyDown(KeyCode.L) == true))
        {
            if(playShake == false) { return; }
            PlaySE(0);
            ShakeAction(shakeDuration, shakeMagnitude);
            booingPlayCount--;
            playShake = false;
        }

        // □ボタンで画面の邪魔を表示
        if (input.Square == true || (_ = booingPlayer == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.W) == true : Input.GetKeyDown(KeyCode.I) == true))
        {
            if(playPaint == false) { return; }
            PlaySE(0);
            PlayParticle(particleCallTime, particleCallSpan);
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

        if(isRunningVibration == true)
        {
            StopCoroutine(vibrationCoroutine);
            GamePadControl.Instance.StopVibration(_ = booingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1);
            isRunningVibration = false;
        }

        if(isRunningShake == true)
        {
            StopCoroutine(shakeCoroutine);
            mainCamera.transform.localPosition = mainCameraPos;
            isRunningShake = false;
        }

        if(isRunningParticle == true)
        {
            StopCoroutine(particleCoroutine);

            if(particle.IsAlive(true) == true)
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            isRunningParticle = false;
        }

        booingFlag = false;
    }
}
