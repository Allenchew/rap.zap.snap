using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleControl : MonoBehaviour
{
    [SerializeField, Header("カメラオブジェクト")] private Camera particleCamera = null;

    [SerializeField, Header("お邪魔用パーティクル")] private ParticleSystem[] booingParticle = null;
    [SerializeField, Header("パーティクル用のRawImage")] private RawImage rawImage = null;

    [SerializeField, Header("投げつけるアイテムのオブジェクト")] private Image throwObject = null;
    [SerializeField, Header("投げつけるアイテムのSpriteデータ")] private Sprite[] throwSprites = null;
    [SerializeField, Header("割れたアイテムのSpriteデータ")] private Sprite[] brokeSprites = null;

    private Coroutine throwCoroutine = null;

    public int GetParticleIndex { get { return booingParticle.Length; } }
    public bool GetDoingThrowCoroutine() { if(throwCoroutine == null) { return false; } return true; }

    public void ThrowObjectActive(bool activeState)
    {
        throwObject.gameObject.SetActive(activeState);
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
    /// パーティクルの色を設定
    /// </summary>
    /// <param name="particleColor">色の情報</param>
    public void SetColor(Color particleColor)
    {
        rawImage.color = particleColor;
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
    /// モノを投げる＆パーティクルの再生
    /// </summary>
    /// <param name="duration">投げる速度</param>
    /// <param name="itemIndex">投げるモノの種類</param>
    /// <param name="particleIndex">投げる場所</param>
    /// <param name="particleSize">再生するパーティクルの大きさ</param>
    /// <param name="action">実行したい処理</param>
    public void StartThrowItem(float duration, int itemIndex, int particleIndex, float particleSize, System.Action action = null)
    {
        if(duration < 0 || itemIndex < 0 || itemIndex >= throwSprites.Length || particleIndex < 0 || particleIndex >= GetParticleIndex || throwCoroutine != null) { return; }
        throwCoroutine = StartCoroutine(DoThrowItem(duration, itemIndex, particleIndex, particleSize, action));
    }

    private IEnumerator DoThrowItem(float duration, int itemIndex, int particleIndex, float particleSize, System.Action action)
    {
        float deltaTime = 0;
        float direction = -360;
        float angle = direction / duration;
        Vector3 offset = new Vector3(-0.75f, 0, -0.75f);
        Vector3 basePos = booingParticle[particleIndex].transform.position;

        // オブジェクトの初期化
        Vector3 end = RectTransformUtility.WorldToScreenPoint(particleCamera, basePos);
        Vector3 start = RectTransformUtility.WorldToScreenPoint(particleCamera, basePos + offset);
        throwObject.transform.position = start;
        throwObject.transform.rotation = Quaternion.Euler(throwObject.transform.eulerAngles.x, throwObject.transform.eulerAngles.y, 0);
        throwObject.sprite = throwSprites[itemIndex];
        ThrowObjectActive(true);

        while (deltaTime < duration)
        {
            // 移動処理
            throwObject.transform.position = Vector3.Lerp(start, end, deltaTime / duration);

            // 回転処理
            throwObject.transform.Rotate(throwObject.transform.eulerAngles.x, throwObject.transform.eulerAngles.y, angle * Time.deltaTime);

            deltaTime += Time.deltaTime;
            yield return null;
        }

        // 座標等の補正
        throwObject.transform.position = new Vector3(end.x, end.y, 0);
        throwObject.transform.rotation = Quaternion.Euler(throwObject.transform.eulerAngles.x, throwObject.transform.eulerAngles.y, direction);

        // Spriteを割れたアイテムに差し替え
        throwObject.sprite = brokeSprites[itemIndex];

        action?.Invoke();

        // パーティクルの再生
        ParticlePlay(particleIndex, particleSize);

        // 遅延処理
        deltaTime = 0;
        while (deltaTime < 0.3f)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }

        // 投げたオブジェクトを非表示
        ThrowObjectActive(false);

        StopThrowItem(false);
    }

    public void StopThrowItem(bool flag)
    {
        if(throwCoroutine == null) { return; }

        if(flag == true)
        {
            StopCoroutine(throwCoroutine);
            StopParticle();
            ThrowObjectActive(false);
        }
        throwCoroutine = null;
    }
}
