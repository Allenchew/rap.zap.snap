using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BGMName
{
    MainBGM
}

public enum SEName
{
    InputSE,
    WinSE,
    BooingSE,
    InkSE,
    ShakeSE,
    SelectChange,
    TurnChange
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource[] audioSources = null;

    [SerializeField, Header("BGMのリスト")] private AudioClip[] bgmClips = null;
    [SerializeField, Header("SEのリスト")] private AudioClip[] seClips = null;

    private Coroutine bgmCoroutine = null;
    private Coroutine seCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        audioSources = GetComponents<AudioSource>();
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="name">BGMの名前</param>
    public void PlayBGM(BGMName name)
    {
        int index = (int)name;
        int sourceID = 0;

        if(index < 0 || index >= bgmClips.Length)
        {
            Debug.LogError("指定したBGMは存在しません!　検出したID => " + index);
            return;
        }
        audioSources[sourceID].volume = 1;
        audioSources[sourceID].clip = bgmClips[index];
        audioSources[sourceID].Play();
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="name">SEの名前</param>
    /// <param name="oneShot">OneShotで再生するか</param>
    public void PlaySE(SEName name, bool oneShot)
    {
        int index = (int)name;
        int sourceID;

        if (index < 0 || index >= seClips.Length)
        {
            Debug.LogError("指定したSEは存在しません!　検出したID => " + index);
            return;
        }

        if(oneShot == true)
        {
            sourceID = 2;
            audioSources[sourceID].PlayOneShot(seClips[index]);
        }
        else
        {
            sourceID = 1;
            audioSources[sourceID].volume = 1;
            audioSources[sourceID].clip = seClips[index];
            audioSources[sourceID].Play();
        }
    }

    /// <summary>
    /// Audioの再生を止める
    /// </summary>
    /// <param name="isBGM">true → BGMを停止 / false → SEを停止</param>
    public void StopAudio(bool isBGM)
    {
        int sourceID = isBGM == true ? 0 : 1;

        if (audioSources[sourceID].isPlaying == true)
        {
            audioSources[sourceID].Stop();
        }
    }

    /// <summary>
    /// BGMをフェードアウトさせて止める
    /// </summary>
    /// <param name="duration">フェードアウト完了までの時間</param>
    /// <param name="action">フェードアウト完了時に実行したい処理</param>
    public void FadeOutBGM(float duration, Action action = null)
    {
        if(bgmCoroutine != null || duration < 0) { return; }
        bgmCoroutine = StartCoroutine(DoFadeBGMorSE(duration, action, true));
    }

    /// <summary>
    /// BGMをフェードアウトさせて止める
    /// </summary>
    /// <param name="duration">フェードアウト完了までの時間</param>
    /// <param name="action">フェードアウト完了時に実行したい処理</param>
    public void FadeOutSE(float duration, Action action = null)
    {
        if (seCoroutine != null || duration < 0) { return; }
        seCoroutine = StartCoroutine(DoFadeBGMorSE(duration, action, false));
    }

    /// <summary>
    /// Audioをフェードアウトして止めるコルーチン
    /// </summary>
    /// <param name="duration">フェードアウト完了までの時間</param>
    /// <param name="action">フェードアウト完了時に実行したい処理</param>
    /// <param name="isBGM">true → BGMをフェードアウト / false → SEをフェードアウト</param>
    /// <returns></returns>
    private IEnumerator DoFadeBGMorSE(float duration, Action action, bool isBGM)
    {
        int source = isBGM == true ? 0 : 1;

        if(audioSources[source].isPlaying == false)
        {
            _ = isBGM == true ? bgmCoroutine = null : seCoroutine = null;
            yield break;
        }

        float deltaTime = 0;

        while(deltaTime < duration)
        {
            audioSources[source].volume = 1.0f - (deltaTime / duration);
            deltaTime += Time.deltaTime;
            yield return null;
        }

        audioSources[source].Stop();

        action?.Invoke();

        _ = isBGM == true ? bgmCoroutine = null : seCoroutine = null;
    }

    /// <summary>
    /// コルーチン処理を強制的に終了させる
    /// </summary>
    /// <param name="isBGM">true → BGMのコルーチン処理を停止 / false → SEのコルーチン処理を停止</param>
    public void StopFadeCoroutine(bool isBGM)
    {
        if(isBGM == true)
        {
            StopCoroutine(bgmCoroutine);
            bgmCoroutine = null;
            StopAudio(true);
        }
        else
        {
            StopCoroutine(seCoroutine);
            seCoroutine = null;
            StopAudio(false);
        }
    }

    /// <summary>
    /// BGMが再生中かを取得
    /// </summary>
    /// <returns></returns>
    public bool IsPlayingBGM()
    {
        int sourceID = 0;
        return audioSources[sourceID].isPlaying;
    }

    /// <summary>
    /// SEが再生中かを取得
    /// </summary>
    /// <param name="oneShot">OneShotで再生したSEが再生中かを取得する</param>
    /// <returns></returns>
    public bool IsPlayingSE(bool oneShot)
    {
        int sourceID = oneShot == true ? 2 : 1;
        return audioSources[sourceID].isPlaying;
    }
}
