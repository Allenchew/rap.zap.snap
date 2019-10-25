﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesView : NotesModel
{
    // ノーツが判定位置へ移動する所要時間
    [SerializeField]
    private float duration = 1.0f;
    public float NotesDuration { set { duration = value; } }

    // ノーツの判定位置の座標
    [SerializeField]
    private Vector3 goalPos;
    public Vector3 GoalPos { set { goalPos = value; } }

    // ノーツが移動を開始する座標
    [SerializeField]
    private Vector3 startPos;
    public Vector3 StartPos { set { startPos = value; } }

    // ノーツの初速移動用のアニメーションカーブ
    [SerializeField]
    private AnimationCurve curve;

    private float startTime;

    // ノーツの判定位置までの進行率
    private float rate;
    public float NotesRate { get { return rate; } }

    // ノーツの処理を止めるフラグ
    private bool stopFlag = false;

    private SpriteRenderer notesSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stopFlag)
        {
            NotesAction();
        }
    }

    private void OnEnable()
    {
        // ノーツのスプライトレンダラーを取得
        if(notesSprite == null)
        {
            notesSprite = GetComponent<SpriteRenderer>();
        }

        // durationが0秒以下ならノーツを非表示
        if(duration <= 0)
        {
            transform.localPosition = goalPos;
            gameObject.SetActive(false);
            return;
        }

        startTime = Time.timeSinceLevelLoad;

        // ノーツの座標を初期化
        transform.localPosition = startPos;

        // ノーツの画像を差し替え
        

        stopFlag = true;
    }

    /// <summary>
    /// ノーツの処理メソッド
    /// </summary>
    private void NotesAction()
    {
        // ノーツの移動開始からの経過時間
        var diff = Time.timeSinceLevelLoad - startTime;

        // 経過時間が所要時間を超えた場合、ノーツを非表示にする
        if(diff > duration)
        {
            transform.localPosition = goalPos;
            StartCoroutine(DelayMethod(5, () => gameObject.SetActive(false)));
            stopFlag = false;
        }

        // 進行率を算出
        rate = diff / duration;
        var pos = curve.Evaluate(rate);

        // ノーツを移動する処理
        transform.localPosition = Vector3.Lerp(startPos, goalPos, pos);
    }

    /// <summary>
    /// n秒後に処理を実行
    /// </summary>
    /// <param name="waitTime">遅延時間[s]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    /// <summary>
    /// nフレーム後に処理を実行
    /// </summary>
    /// <param name="delayFrameCount">遅延フレーム数</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for(int i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
}
