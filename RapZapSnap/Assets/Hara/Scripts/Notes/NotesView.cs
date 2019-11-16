﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesView : NotesModel
{
    // ノーツが判定位置へ移動する所要時間
    private float moveDuration = 0.5f;

    // ノーツの判定位置の座標
    private Vector3 endPos;

    // ノーツが移動を開始する座標
    private Vector3 startPos;

    private Vector3 moveStartPos;
    private Vector3 moveEndPos;

    private float startTime;

    // ノーツの判定位置までの進行率
    public float NotesRate { private set; get; } = 0;

    // ノーツの判定範囲
    private float minPerfect = 0;
    private float maxPerfect = 0;
    private float minGood = 0;
    private float maxGood = 0;
    private float minBad = 0;

    private bool stopFlag = false;
    public bool NotesClickFlag { private set; get; } = false;
    private float firstMoveEndTime = 0;
    private float time = 0;
    
    private SpriteRenderer moveNotesSprite = null;
    private SpriteRenderer goalNotesSprite = null;
    private GameObject moveNotesObj = null;
    private GameObject goalNotesObj = null;

    // スプライトの透明度を変更できるようにしておく
    private float mainSpriteAlpha = 1.0f;
    private float goalSpriteAlpha = 0.5f;

    // 画面外を検知する用のRect
    private Rect rect = new Rect(0, 0, 1, 1);

    // ノーツの最初の移動が終了したことを検知するフラグ
    private bool firstMoveEnd = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        NotesAction();
    }

    private void OnEnable()
    {
        // ノーツのスプライトレンダラーを取得
        if(moveNotesObj == null)
        {
            moveNotesObj = gameObject.transform.GetChild(1).gameObject;
            moveNotesSprite = moveNotesObj.GetComponent<SpriteRenderer>();
        }

        if(goalNotesObj == null)
        {
            goalNotesObj = gameObject.transform.GetChild(0).gameObject;
            goalNotesSprite = goalNotesObj.GetComponent<SpriteRenderer>();
        }

        // durationが0秒以下または移動開始座標と判定座標が同じならreturnする
        if(moveDuration <= 0 || startPos == endPos)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!goalNotesObj.activeSelf)
        {
            goalNotesObj.SetActive(true);
        }

        startTime = Time.timeSinceLevelLoad;

        // ノーツの座標を初期化
        moveNotesObj.transform.position = startPos;
        goalNotesObj.transform.position = endPos;
        moveEndPos = (endPos - startPos).normalized * Vector3.Distance(startPos, endPos) + endPos;
        moveStartPos = startPos;

        // ノーツの画像を差し替え
        moveNotesSprite.sprite = NotesSprites[(int)NotesTypes];
        moveNotesSprite.color = new Color(1, 1, 1, 1);
        mainSpriteAlpha = 1.0f;
        goalNotesSprite.sprite = NotesSprites[(int)NotesTypes];
        goalNotesSprite.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定

        stopFlag = true;    // 処理開始
    }

    /// <summary>
    /// ノーツの処理メソッド
    /// </summary>
    private void NotesAction()
    {
        if (stopFlag)
        {
            // ノーツの移動開始からの経過時間
            var diff = NotesClickFlag ? (Time.timeSinceLevelLoad - startTime) : (Time.timeSinceLevelLoad - firstMoveEndTime);
            
            // 進行率を算出
            NotesRate = firstMoveEnd ? (diff / moveDuration) : (diff / (moveDuration * 2));

            // ノーツを移動する処理
            moveNotesObj.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);

            if (NotesClickFlag)
            {
                if (NotesRate >= maxGood)
                {
                    NotesClickFlag = false;
                    goalNotesObj.SetActive(false);

                    // 第2移動の目的地を設定する
                    var radius = Vector3.Distance(startPos, endPos);
                    int direction = 8;    // 方向数
                    float angleDiff = 360.0f / direction;    // 方向転換する角度の間隔
                    var xPosDistance = Mathf.Abs(startPos.x - endPos.x);
                    var yPosDistance = Mathf.Abs(startPos.y - endPos.y);
                    int directionNum = 0;
                    int rnd = UnityEngine.Random.Range(0, 3);

                    if ((startPos.x - endPos.x) > 0 && xPosDistance >= yPosDistance)
                    {
                        // ノーツの移動方向が右→左のとき
                        switch (rnd)
                        {
                            case 0:
                                directionNum = 5;
                                break;
                            case 1:
                                directionNum = 6;
                                break;
                            case 2:
                                directionNum = 7;
                                break;
                        }
                    }
                    else if ((startPos.x - endPos.x) < 0 && xPosDistance >= yPosDistance)
                    {
                        // ノーツの移動方向が左→右のとき
                        switch (rnd)
                        {
                            case 0:
                                directionNum = 1;
                                break;
                            case 1:
                                directionNum = 2;
                                break;
                            case 2:
                                directionNum = 3;
                                break;
                        }
                    }
                    else if ((startPos.y - endPos.y) > 0 && xPosDistance <= yPosDistance)
                    {
                        // ノーツの移動方向が上→下のとき
                        switch (rnd)
                        {
                            case 0:
                                directionNum = 3;
                                break;
                            case 1:
                                directionNum = 4;
                                break;
                            case 2:
                                directionNum = 5;
                                break;
                        }
                    }
                    else
                    {
                        // ノーツの移動方向が下→上のとき
                        switch (rnd)
                        {
                            case 0:
                                directionNum = 7;
                                break;
                            case 1:
                                directionNum = 8;
                                break;
                            case 2:
                                directionNum = 1;
                                break;
                        }
                    }

                    float angle = (90 - angleDiff * directionNum) * Mathf.Deg2Rad;
                    Vector3 newMoveStartPos = moveNotesObj.transform.position;
                    newMoveStartPos.x += radius * Mathf.Cos(angle);
                    newMoveStartPos.y += radius * Mathf.Sin(angle);

                    // 座標データを更新
                    moveStartPos = moveNotesObj.transform.position;
                    moveEndPos = newMoveStartPos;
                    firstMoveEndTime = Time.timeSinceLevelLoad;

                    firstMoveEnd = true;
                }
            }
            else
            {
                if (mainSpriteAlpha > 0)
                {
                    // 透明度を下げる
                    mainSpriteAlpha -= 0.05f;
                    moveNotesSprite.color = new Color(1, 1, 1, mainSpriteAlpha);
                }

                // ノーツが画面外に行ったかをチェック
                var viewport = Camera.main.WorldToViewportPoint(moveNotesObj.transform.position);    // MainCameraのviewportを取得
                if (!rect.Contains(viewport) || NotesRate >= 1.0f || mainSpriteAlpha <= 0)
                {
                    // ノーツが完全に透明になるか、画面外へ移動したか、目的地に着いたら非表示
                    ResetNotes();
                }
            }
        }
    }

    /// <summary>
    /// ノーツの判定処理
    /// <para>返り値 2:Perfect 1:Good 0:Bad -1:判定外</para>
    /// </summary>
    /// <param name="notesType">入力したキー</param>
    /// <param name="rate">ノーツの判定値</param>
    public int NotesCheck(NotesType notesType)
    {
        var rate = NotesRate;

        if(NotesClickFlag && rate >= minBad)
        {
            int num = 0;

            // 入力したキーが合っているかチェック
            if (notesType == NotesTypes)
            {
                if (rate >= minPerfect && rate <= maxPerfect)
                {
                    // prefect判定
                    num = 2;
                }
                else if ((rate >= minGood && rate < minPerfect) || (rate >= maxGood && rate < maxPerfect))
                {
                    // good判定
                    num = 1;
                }
                else
                {
                    // bad判定
                    num = 0;
                }
            }
            else
            {
                num = 0;
            }

            ResetNotes();
            return num;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// ノーツの判定結果を返す＆ノーツの初期化処理
    /// </summary>
    private void ResetNotes()
    {
        stopFlag = false;
        gameObject.SetActive(false);
    }

    public void SetNotesData(NotesType type, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, float spriteAlpha)
    {
        // ノーツ情報
        NotesTypes = type;
        startPos = start;
        endPos = end;
        moveDuration = duration;

        // ノーツの判定域
        minPerfect = 0.5f - perfect;
        maxPerfect = 0.5f + perfect;
        minGood = minPerfect - good;
        maxGood = maxPerfect + good;
        minBad = minGood - bad;

        // ノーツのサイズ
        transform.localScale = scale;

        // 判定ノーツのalpha値
        goalSpriteAlpha = spriteAlpha;

        // ノーツを表示する前に初期化するフラグなど
        NotesRate = 0;
        NotesClickFlag = true;
        firstMoveEnd = false;

        // ノーツの再生
        gameObject.SetActive(true);
    }
}
