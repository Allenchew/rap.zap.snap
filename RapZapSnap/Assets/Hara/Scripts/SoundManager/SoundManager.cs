using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


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

public enum VoiceName
{
    Select_TOKIWA,
    Select_HAJIME,
    Select_MARI,

    Vs_TOKIWA,
    Vs_HAJIME,
    Vs_MARI,

    Win_TOKIWA,
    Win_HAJIME,
    Win_MARI,

    Lose_TOKIWA_Win_HAJIME,
    Lose_TOKIWA_Win_MARI,
    Lose_HAJIME_Win_TOKIWA,
    Lose_HAJIME_Win_MARI,
    Lose_MARI_Win_TOKIWA,
    Lose_MARI_Win_HAJIME,

    Vibration_TOKIWA,
    Vibration_HAJIME,
    Vibration_MARI,

    Paint_TOKIWA,
    Paint_HAJIME,
    Paint_MARI,

    Shake_TOKIWA,
    Shake_HAJIME,
    Shake_MARI
}

public enum SourceType
{
    BGM,
    SE,
    OneShotSE,
    Voice
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource[] audioSources = null;

    [SerializeField, Header("BGMのリスト")] private AudioClip[] bgmClips = null;
    [SerializeField, Header("SEのリスト")] private AudioClip[] seClips = null;
    [SerializeField, Header("キャラクターボイスのリスト")] private AudioClip[] voiceClips = null;

    private Coroutine bgmFadeCoroutine = null;
    private Coroutine seFadeCoroutine = null;
    private Coroutine voiceActionCoroutine = null;
    private Coroutine seActionCoroutine = null;

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
        int sourceID = (int)SourceType.BGM;

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
        
        if (index < 0 || index >= seClips.Length)
        {
            Debug.LogError("指定したSEは存在しません!　検出したID => " + index);
            return;
        }

        int sourceID = oneShot == true ? (int)SourceType.OneShotSE : (int)SourceType.SE;
        audioSources[sourceID].volume = 1;

        if (oneShot == true)
        {
            audioSources[sourceID].PlayOneShot(seClips[index]);
        }
        else
        {
            audioSources[sourceID].clip = seClips[index];
            audioSources[sourceID].Play();
        }
    }

    /// <summary>
    /// SE再生後に特定の処理を実行
    /// </summary>
    /// <param name="name">ボイス名</param>
    /// <param name="action">実行したい処理</param>
    public void PlaySEAndAction(SEName name, Action action = null)
    {
        if (voiceActionCoroutine != null) { return; }
        seActionCoroutine = StartCoroutine(DoSECroutine(name, action));
    }

    /// <summary>
    /// SEのコルーチン
    /// </summary>
    /// <param name="name">ボイス名</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DoSECroutine(SEName name, Action action)
    {
        PlaySE(name, true);

        while (IsPlayingAudio(SourceType.OneShotSE) == true)
        {
            yield return null;
        }

        StopSECoroutine(false);

        action?.Invoke();
    }

    /// <summary>
    /// SEのコルーチンを停止
    /// </summary>
    public void StopSECoroutine(bool flag)
    {
        if (seActionCoroutine == null) { return; }

        if (flag == true)
        {
            StopCoroutine(seActionCoroutine);
            StopAudio(SourceType.OneShotSE);
        }
        seActionCoroutine = null;
    }

    /// <summary>
    /// キャラクターボイスの再生
    /// </summary>
    /// <param name="name">ボイス名</param>
    public void PlayVoice(VoiceName name)
    {
        int index = (int)name;
        int sourceID = (int)SourceType.Voice;

        if (index < 0 || index >= voiceClips.Length)
        {
            Debug.LogError("指定したボイスは存在しません!　検出したID => " + index);
            return;
        }

        audioSources[sourceID].PlayOneShot(voiceClips[index]);
    }

    /// <summary>
    /// ボイス再生後に特定の処理を実行
    /// </summary>
    /// <param name="name">ボイス名</param>
    /// <param name="action">実行したい処理</param>
    public void PlayVoiceAndAction(VoiceName name, Action action = null)
    {
        if(voiceActionCoroutine != null) { return; }
        voiceActionCoroutine = StartCoroutine(DoVoiceCoroutine(name, action));
    }

    /// <summary>
    /// ボイスのコルーチン
    /// </summary>
    /// <param name="name">ボイス名</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DoVoiceCoroutine(VoiceName name, Action action)
    {
        PlayVoice(name);

        while (IsPlayingAudio(SourceType.Voice) == true)
        {
            yield return null;
        }

        StopVoiceCoroutine(false);

        action?.Invoke();
    }

    /// <summary>
    /// ボイスのコルーチンを停止
    /// </summary>
    public void StopVoiceCoroutine(bool flag)
    {
        if(voiceActionCoroutine == null) { return; }

        if(flag == true)
        {
            StopCoroutine(voiceActionCoroutine);
            StopAudio(SourceType.Voice);
        }
        voiceActionCoroutine = null;
    }

    /// <summary>
    /// Audioの再生を止める
    /// </summary>
    /// <param name="type">AudioSourceのタイプ</param>
    public void StopAudio(SourceType type)
    {
        int sourceID = (int)type;

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
        if(bgmFadeCoroutine != null || duration < 0) { return; }
        bgmFadeCoroutine = StartCoroutine(DoFadeBGM(duration, action));
    }

    /// <summary>
    /// SEをフェードアウトさせて止める
    /// </summary>
    /// <param name="duration">フェードアウト完了までの時間</param>
    /// <param name="action">フェードアウト完了時に実行したい処理</param>
    public void FadeOutSE(float duration, Action action = null)
    {
        if (seFadeCoroutine != null || duration < 0) { return; }
        seFadeCoroutine = StartCoroutine(DoFadeSE(duration, action));
    }

    /// <summary>
    /// BGMをフェードアウトして止めるコルーチン
    /// </summary>
    /// <param name="duration">フェードアウト完了までの時間</param>
    /// <param name="action">フェードアウト完了時に実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DoFadeBGM(float duration, Action action)
    {
        SourceType type = SourceType.BGM;

        if(IsPlayingAudio(type) == false)
        {
            StopBGMFade(true);
        }

        float deltaTime = 0;

        while(deltaTime < duration)
        {
            audioSources[(int)type].volume = 1.0f - (deltaTime / duration);
            deltaTime += Time.deltaTime;
            yield return null;
        }

        StopAudio(type);

        StopBGMFade(false);

        action?.Invoke();
    }

    /// <summary>
    /// SEをフェードアウトして止めるコルーチン
    /// </summary>
    /// <param name="duration">フェードアウト完了までの時間</param>
    /// <param name="action">フェードアウト完了時に実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DoFadeSE(float duration, Action action)
    {
        SourceType type = SourceType.SE;

        if (IsPlayingAudio(type) == false)
        {
            StopSEFade(true);
        }

        float deltaTime = 0;

        while (deltaTime < duration)
        {
            audioSources[(int)type].volume = 1.0f - (deltaTime / duration);
            deltaTime += Time.deltaTime;
            yield return null;
        }

        StopAudio(type);

        StopSEFade(false);

        action?.Invoke();
    }

    /// <summary>
    /// BGMフェードコルーチンを終了させる
    /// </summary>
    public void StopBGMFade(bool flag)
    {
        if(bgmFadeCoroutine == null) { return; }

        if(flag == true)
        {
            StopCoroutine(bgmFadeCoroutine);
            StopAudio(SourceType.BGM);
        }
        bgmFadeCoroutine = null;
    }

    /// <summary>
    /// SEフェードコルーチンを終了させる
    /// </summary>
    public void StopSEFade(bool flag)
    {
        if(seFadeCoroutine == null) { return; }

        if (flag == true)
        {
            StopCoroutine(seFadeCoroutine);
            StopAudio(SourceType.SE);
        }
        seFadeCoroutine = null;
    }

    /// <summary>
    /// Audioが再生されているかを取得
    /// </summary>
    /// <param name="type">AudioSourceのタイプ</param>
    /// <returns></returns>
    public bool IsPlayingAudio(SourceType type)
    {
        int sourceID = (int)type;
        return audioSources[sourceID].isPlaying;
    }
}
