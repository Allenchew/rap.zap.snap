using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BooingControl : SingletonMonoBehaviour<BooingControl>
{
    [SerializeField, Tooltip("ブーイングシステムを使うか"), Header("ブーイングシステム基本設定")]
    private bool booingFlag = false;

    [SerializeField, Tooltip("ブーイングができるプレイヤー")]
    private ControllerNum booingPlayer = ControllerNum.P2;
    public ControllerNum BooingPlayer { get { return booingPlayer; } }

    [SerializeField, Tooltip("お邪魔実行時間"), Range(0f, 5.0f)] private float booingDuration = 1.0f;
    [SerializeField, Tooltip("お邪魔UI")] private Image booingUI = null;
    [SerializeField, Tooltip("お邪魔UI用Spriteデータ1")] private Sprite[] booingUISprites1 = null;
    [SerializeField, Tooltip("お邪魔UI用Spriteデータ2")] private Sprite[] booingUISprites2 = null;
    private Coroutine booingUICoroutine = null;
    private enum AnimationType
    {
        Type1,
        Type2,
        Type3
    }
    [SerializeField, Tooltip("お邪魔UIの再生パターン")] private AnimationType booingUIType = AnimationType.Type1;

    [SerializeField, Tooltip("振動の強さ"), Header("コントローラーを振動させるお邪魔システム")]
    private byte vibrationPower = 255;
    private bool isRunningVibration = false;

    [SerializeField, Tooltip("揺れの強さ"), Range(0f, 2.0f), Header("画面を揺らすお邪魔システム")] private float shakeMagnitude = 0.5f;
    private GameObject mainCamera = null;
    private Vector3 mainCameraPos;
    private Coroutine shakeCoroutine = null;

    [SerializeField, Tooltip("パーティクル用のカメラ"), Header("画面を汚すお邪魔システム")] private Camera booingCamera = null;
    [SerializeField, Tooltip("お邪魔用パーティクル")] private ParticleSystem[] booingParticle = null;
    [SerializeField, Tooltip("パーティクル用のRawImage")] private RawImage booingRawImage = null;
    [SerializeField, Tooltip("投げつけるアイテムのオブジェクト")] private Image throwObject = null;
    [SerializeField, Tooltip("投げつけるアイテムのSpriteデータ")] private Sprite[] throwSprites = null;
    [SerializeField, Tooltip("割れたアイテムのSpriteデータ")] private Sprite[] brokeSprites = null;

    [SerializeField, Tooltip("再生回数"), Range(0, 5)] private int particleCallTime = 1;
    [SerializeField, Tooltip("パーティクルのサイズ"), Range(0f, 3.0f)] private float particleSize = 1.0f;
    private Coroutine throwCoroutine = null;

    private bool playVibration = true;
    private bool playShake = true;
    private bool playThrow = true;

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
        booingCamera.transform.position = Camera.main.transform.position + Vector3.back * 10;
        throwObject.gameObject.SetActive(false);
        booingUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// お邪魔UIを再生
    /// </summary>
    /// <param name="duration">実行時間</param>
    /// <param name="id">プレイヤー番号</param>
    /// <param name="type">再生パターン</param>
    private void StartBooingUI(float duration, ControllerNum id, AnimationType type)
    {
        if(booingUICoroutine != null || duration < 0) { return; }
        booingUICoroutine = StartCoroutine(DoBooingUI(duration, id, type));
    }

    /// <summary>
    /// お邪魔UIコルーチン
    /// </summary>
    /// <param name="duration">実行時間</param>
    /// <param name="id">プレイヤー番号</param>
    /// <param name="type">再生パターン</param>
    /// <returns></returns>
    private IEnumerator DoBooingUI(float duration, ControllerNum id, AnimationType type)
    {
        float deltaTime;
        float span;
        bool switchSprite = true;
        int spriteIndex;
        int callTime;
        Character character = GameData.Instance.GetCharacterData(id);

        if(character == Character.Tokiwa)
        {
            spriteIndex = 0;
        }
        else if(character == Character.Hajime)
        {
            spriteIndex = 1;
        }
        else
        {
            spriteIndex = 2;
        }
        booingUI.sprite = booingUISprites1[spriteIndex];

        //　アニメーション開始
        if (type == AnimationType.Type1)
        {
            booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, 1);
            booingUI.gameObject.SetActive(true);
            callTime = 6;
            span = duration / callTime;

            for(int i = 0; i < callTime; i++)
            {
                deltaTime = 0;
                while(deltaTime < span)
                {
                    deltaTime += Time.deltaTime;
                    yield return null;
                }

                if(switchSprite == true)
                {
                    booingUI.sprite = booingUISprites2[spriteIndex];
                    switchSprite = false;
                }
                else
                {
                    booingUI.sprite = booingUISprites1[spriteIndex];
                    switchSprite = true;
                }
            }
            booingUI.gameObject.SetActive(false);
        }
        else if(type == AnimationType.Type2)
        {
            booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, 1);
            callTime = 3;
            span = duration / callTime;

            for (int i = 0; i < callTime; i++)
            {
                // 表示
                booingUI.gameObject.SetActive(true);
                deltaTime = 0;
                while (deltaTime < span / 2)
                {
                    deltaTime += Time.deltaTime;
                    yield return null;
                }

                // 非表示
                booingUI.gameObject.SetActive(false);
                deltaTime = 0;
                while (deltaTime < span / 2)
                {
                    deltaTime += Time.deltaTime;
                    yield return null;
                }

                if (switchSprite == true)
                {
                    booingUI.sprite = booingUISprites2[spriteIndex];
                    switchSprite = false;
                }
                else
                {
                    booingUI.sprite = booingUISprites1[spriteIndex];
                    switchSprite = true;
                }
            }
        }
        else
        {
            booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, 0);
            booingUI.gameObject.SetActive(true);
            callTime = 2;
            span = duration / callTime;

            for(int i = 0; i < callTime; i++)
            {
                // フェードイン
                deltaTime = 0;
                while(deltaTime < span / 2)
                {
                    booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, deltaTime / span * 2);
                    deltaTime += Time.deltaTime;
                    yield return null;
                }
                booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, 1);

                // フェードアウト
                deltaTime = 0;
                while (deltaTime < span / 2)
                {
                    booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, 1 - deltaTime / span * 2);
                    deltaTime += Time.deltaTime;
                    yield return null;
                }
                booingUI.color = new Color(booingUI.color.r, booingUI.color.g, booingUI.color.b, 0);

                if (switchSprite == true)
                {
                    booingUI.sprite = booingUISprites2[spriteIndex];
                    switchSprite = false;
                }
                else
                {
                    booingUI.sprite = booingUISprites1[spriteIndex];
                    switchSprite = true;
                }
            }
            booingUI.gameObject.SetActive(false);
        }

        // 遅延処理
        deltaTime = 0;
        while(deltaTime < 0.2f)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }

        // 処理終了
        StopBooingUI(false);
    }

    /// <summary>
    /// お邪魔UIを停止
    /// </summary>
    private void StopBooingUI(bool flag)
    {
        if(booingUICoroutine == null) { return; }

        if(flag == true)
        {
            StopCoroutine(booingUICoroutine);
            booingUI.gameObject.SetActive(false);
        }
        booingUICoroutine = null;
    }

    /// <summary>
    /// コントローラーの振動
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    /// <param name="vibration">振動値</param>
    /// <param name="duration">振動時間</param>
    private void VibrationAction(ControllerNum id, byte vibration, float duration)
    {
        if (duration < 0 || isRunningVibration == true || playVibration == false || booingUICoroutine != null) { return; }

        playVibration = false;

        isRunningVibration = true;

        // お邪魔UIの再生
        StartBooingUI(duration, booingPlayer, booingUIType);

        // SEの再生
        SoundManager.Instance.PlaySE(SEName.BooingSE, false);

        // 振動開始
        GamePadControl.Instance.SetVibration(id, vibration);

        // SEと振動の停止
        SoundManager.Instance.FadeOutSE(duration, () => StopVibrationAction(id, false));

    }

    /// <summary>
    /// コントローラー振動のお邪魔システムを停止
    /// </summary>
    private void StopVibrationAction(ControllerNum id, bool flag)
    {
        if(isRunningVibration == false) { return; }

        if(flag == true)
        {
            SoundManager.Instance.StopFadeCoroutine(false);
        }

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
        if(duration <= 0f || magnitude <= 0f || shakeCoroutine != null || playShake == false || booingUICoroutine != null) { return; }
        shakeCoroutine = StartCoroutine(DoShake(duration, magnitude));
        StartBooingUI(duration, booingPlayer, booingUIType);
        playShake = false;
    }

    /// <summary>
    /// カメラを揺らす（画面を揺らす）コルーチン処理
    /// </summary>
    /// <param name="duration">揺らす時間</param>
    /// <param name="magnitude">揺れの強さ</param>
    /// <returns></returns>
    private IEnumerator DoShake(float duration, float magnitude)
    {
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

        StopShakeAction(false);
    }

    /// <summary>
    /// カメラを揺らす（画面を揺らす）処理を停止
    /// </summary>
    private void StopShakeAction(bool flag)
    {
        if(shakeCoroutine == null) { return; }

        if(flag == true)
        {
            StopCoroutine(shakeCoroutine);
            mainCamera.transform.localPosition = mainCameraPos;
        }
        shakeCoroutine = null;
    }

    /// <summary>
    /// 画面に物を投げるお邪魔システム
    /// </summary>
    /// <param name="time">再生回数</param>
    /// <param name="duration">実行時間</param>
    private void ThrowToScreenAction(int time, float duration)
    {
        if(time < 0 || duration < 0 || throwCoroutine != null || playThrow == false || booingUICoroutine != null) { return; }
        throwCoroutine = StartCoroutine(DoThrow(time, duration));
        StartBooingUI(duration, booingPlayer, booingUIType);
        playThrow = false;
    }

    /// <summary>
    /// 画面に物を投げるお邪魔システムのコルーチン
    /// </summary>
    /// <param name="time">再生回数</param>
    ///  <param name="duration">実行時間</param>
    /// <returns></returns>
    private IEnumerator DoThrow(int time, float duration)
    {
        int itemIndex = Random.Range(0, 3);
        Color particleColor;
        int particleIndex;

        float wait = duration / 10;
        float span = (duration - wait) / time;
        float deltaTime;

        float direction = -360;
        float angle = direction / span;
        Vector3 offset = new Vector3(-0.75f, 0, -0.75f);
        Vector3 basePos;
        Vector3 start;
        Vector3 end;

        // パーティクルの色(RawImageの色)を設定
        switch (itemIndex)
        {
            case 0:
                particleColor = new Color(96f / 255f, 14f / 255f, 18f / 255f, 1);    // トマト
                break;
            case 1:
                particleColor = new Color(10f / 255f, 25f / 255f, 50f / 255f, 1);    // ボトル
                break;
            case 2:
                particleColor = new Color(234f / 255f, 162f / 255f, 23f / 255f, 1);    // たまご
                break;
            default:
                yield break;
        }

        booingRawImage.color = particleColor;

        for (int i = 0; i < time; i++)
        {
            particleIndex = Random.Range(0, booingParticle.Length);
            deltaTime = 0;
            basePos = booingParticle[particleIndex].transform.position;

            // 投げるアイテムを初期値に配置
            end = RectTransformUtility.WorldToScreenPoint(booingCamera, basePos);
            start = RectTransformUtility.WorldToScreenPoint(booingCamera, basePos + offset);
            throwObject.transform.position = start;
            throwObject.transform.rotation = Quaternion.Euler(throwObject.transform.eulerAngles.x, throwObject.transform.eulerAngles.y, 0);
            throwObject.sprite = throwSprites[itemIndex];
            throwObject.gameObject.SetActive(true);

            // 投げるアニメーション開始
            while (deltaTime < span)
            {
                // 移動処理
                throwObject.transform.position = Vector3.Lerp(start, end, deltaTime / span);

                // 回転処理
                throwObject.transform.Rotate(throwObject.transform.eulerAngles.x, throwObject.transform.eulerAngles.y, angle * Time.deltaTime);

                deltaTime += Time.deltaTime;
                yield return null;
            }
            throwObject.transform.position = new Vector3(end.x, end.y, 0);
            throwObject.transform.rotation = Quaternion.Euler(throwObject.transform.eulerAngles.x, throwObject.transform.eulerAngles.y, direction);
            throwObject.sprite = brokeSprites[itemIndex];

            // SEの再生
            SoundManager.Instance.PlaySE(SEName.InkSE, true);

            // パーティクルの再生
            ParticlePlay(particleIndex, particleSize);

            // 待機
            deltaTime = 0;
            while (deltaTime < wait)
            {
                deltaTime += Time.deltaTime;
                yield return null;
            }

            // 投げたオブジェクトを非表示
            throwObject.gameObject.SetActive(false);
        }

        StopThrowToScreenAction(false);
    }

    /// <summary>
    /// 画面に物を投げるお邪魔システムを停止
    /// </summary>
    private void StopThrowToScreenAction(bool flag)
    {
        if(throwCoroutine == null) { return; }

        if(flag == true)
        {
            StopCoroutine(throwCoroutine);
            StopParticle();
            throwObject.gameObject.SetActive(false);
        }
        throwCoroutine = null;
    }

    /// <summary>
    /// お邪魔パーティクルの再生
    /// </summary>
    /// <param name="index">パーティクルの番号</param>
    /// <param name="size">パーティクルのサイズ</param>
    /// <param name="color">パーティクルの色</param>
    private void ParticlePlay(int index, float size)
    {
        if (index < 0 || index >= booingParticle.Length) { return; }

        ParticleSystem.MainModule mainModule = booingParticle[index].main;

        mainModule.startSize = size;

        booingParticle[index].Emit(1);
    }

    /// <summary>
    /// パーティクルの再生を止める
    /// </summary>
    private void StopParticle()
    {
        foreach (var particle in booingParticle)
        {
            if (particle.IsAlive(true) == true)
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
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
            VibrationAction(target, vibrationPower, booingDuration);
        }

        // △ボタンでSE再生と画面の揺れを実行
        if(booingPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Triangle == true : GamePadControl.Instance.GetKeyDown_2.Triangle == true)
        {
            ShakeAction(booingDuration, shakeMagnitude);
        }

        // □ボタンで画面の邪魔を表示
        if (booingPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Square == true : GamePadControl.Instance.GetKeyDown_2.Square == true)
        {
            ThrowToScreenAction(particleCallTime, booingDuration);
        }

        if(playVibration == false && playShake == false && playThrow == false) { booingFlag = false; }
    }

    /// <summary>
    /// ブーイングシステムを使うプレイヤーを決める
    /// </summary>
    /// <param name="player">ブーイングシステムを使うプレイヤー</param>
    public void SetBooingPlayer(ControllerNum player)
    {
        // 再生中のお邪魔システムを停止
        BooingSystemOff();

        // フラグを初期化
        booingPlayer = player;
        playVibration = true;
        playShake = true;
        playThrow = true;
        booingFlag = true;
    }

    /// <summary>
    /// ブーイングシステムを無効にする
    /// </summary>
    public void BooingSystemOff()
    {
        if (booingFlag == false) { return; }

        StopVibrationAction(_ = booingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1, true);

        StopShakeAction(true);

        StopThrowToScreenAction(true);

        StopBooingUI(true);

        booingFlag = false;
    }
}
