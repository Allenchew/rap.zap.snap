using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooingControl : SingletonMonoBehaviour<BooingControl>
{
    [SerializeField, Tooltip("ブーイングシステムを使うか")]
    private bool booingFlag = false;

    [SerializeField, Tooltip("ブーイングができるプレイヤー")]
    private ControllerNum booingPlayer = ControllerNum.P2;
    public ControllerNum BooingPlayer { get { return booingPlayer; } }

    [SerializeField, Tooltip("振動の強さ"), Header("コントローラーを振動させるお邪魔システム")]
    private byte vibrationPower = 255;
    [SerializeField, Tooltip("再生時間"), Range(0, 5.0f)] private float vibDuration = 2.0f;
    private bool isRunningVibration = false;

    [SerializeField, Tooltip("揺れの強さ"), Range(0f, 2.0f), Header("画面を揺らすお邪魔システム")]
    private float shakeMagnitude = 0.5f;
    [SerializeField, Tooltip("再生時間"), Range(0f, 5.0f)]
    private float shakeDuration = 1.0f;
    private GameObject mainCamera = null;
    private Vector3 mainCameraPos;
    private bool isRunningShake = false;
    private Coroutine shakeCoroutine = null;

    [SerializeField, Tooltip("パーティクル用のカメラ"), Header("画面を汚すお邪魔システム")]
    private GameObject particleCameraObject = null;
    [SerializeField, Tooltip("再生回数"), Range(0, 5)] private int particleCallTime = 1;
    [SerializeField, Tooltip("再生間隔"), Range(0f, 2.0f)] private float particleCallSpan = 1.0f;
    [SerializeField, Tooltip("パーティクルのサイズ"), Range(0f, 3.0f)] private float particleSize = 1.0f;
    private ParticleControl particle = null;
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
        if (particleCameraObject != null)
        {
            var particleCamera = Instantiate(particleCameraObject, gameObject.transform, false);
            particleCamera.transform.position = Camera.main.transform.position + Vector3.back * 10;
            particle = particleCamera.GetComponent<ParticleControl>();
        }
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

        isRunningVibration = true;

        // SEの再生
        SoundManager.Instance.PlaySE(SEName.BooingSE, false);

        // 振動開始
        GamePadControl.Instance.SetVibration(id, vibration);

        // SEと振動の停止
        SoundManager.Instance.FadeOutSE(duration, () => { GamePadControl.Instance.StopVibration(id); isRunningVibration = false; });

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

        // SEの再生
        SoundManager.Instance.PlaySE(SEName.ShakeSE, true);

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
    /// <param name="id">プレイヤー番号</param>
    private void PlayParticle(int time, float span, ControllerNum id)
    {
        if(isRunningParticle == true || particle == null) { return; }

        particleCoroutine = StartCoroutine(DoParticle(time, span, id));
    }

    /// <summary>
    /// パーティクル再生用のコルーチン
    /// </summary>
    /// <param name="time">再生回数</param>
    /// <param name="span">再生間隔</param>
    /// /// <param name="id">プレイヤー番号</param>
    /// <returns></returns>
    private IEnumerator DoParticle(int time, float span, ControllerNum id)
    {
        isRunningParticle = true;
        int colorType = Random.Range(0, 3);
        Color particleColor;
        int index;

        // パーティクルの色(RawImageの色)を設定
        switch(colorType)
        {
            case 0:
                particleColor = new Color(96f / 255f, 14f / 255f, 18f / 255f, 1);    // 常盤カラー
                break;
            case 1:
                particleColor = new Color(10f / 255f, 25f / 255f, 50f / 255f, 1);    // 創カラー
                break;
            case 2:
                particleColor = new Color(234f / 255f, 162f / 255f, 23f / 255f, 1);    // 茉莉カラー
                break;
            default:
                yield break;
        }
        particle.SetColor(particleColor);

        float deltaTime;

        for (int i = 0; i < time; i++)
        {
            // 時間の初期化
            deltaTime = 0f;

            index = Random.Range(0, particle.GetParticleIndex);

            // パーティクルの再生
            particle.ParticlePlay(index, particleSize);

            // SEの再生
            SoundManager.Instance.PlaySE(SEName.InkSE, true);

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

        // 〇ボタンでSE再生とバイブレーションを実行
        if (booingPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Circle == true : GamePadControl.Instance.GetKeyDown_2.Circle == true)
        {
            if(playVibration == false) { return; }
            StartVibration(target, vibrationPower, vibDuration);
            booingPlayCount--;
            playVibration = false;
        }

        // △ボタンでSE再生と画面の揺れを実行
        if(booingPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Triangle == true : GamePadControl.Instance.GetKeyDown_2.Triangle == true)
        {
            if(playShake == false) { return; }
            ShakeAction(shakeDuration, shakeMagnitude);
            booingPlayCount--;
            playShake = false;
        }

        // □ボタンで画面の邪魔を表示
        if (booingPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Square == true : GamePadControl.Instance.GetKeyDown_2.Square == true)
        {
            if(playPaint == false) { return; }
            PlayParticle(particleCallTime, particleCallSpan, booingPlayer);
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
            SoundManager.Instance.StopFadeCoroutine(false);
            GamePadControl.Instance.StopVibration(_ = booingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1);
            isRunningVibration = false;
        }

        if(isRunningShake == true)
        {
            StopCoroutine(shakeCoroutine);
            mainCamera.transform.localPosition = mainCameraPos;
            isRunningShake = false;
        }

        if(isRunningParticle == true && particle != null)
        {
            StopCoroutine(particleCoroutine);

            particle.StopParticle();

            isRunningParticle = false;
        }

        booingFlag = false;
    }
}
