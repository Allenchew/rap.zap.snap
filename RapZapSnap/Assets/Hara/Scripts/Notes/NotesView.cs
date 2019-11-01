using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesView : NotesModel
{
    // ノーツが判定位置へ移動する所要時間
    public float NotesDuration { set; private get; } = 0.5f;

    // ノーツの判定位置の座標
    public Vector3 GoalPos { set; private get; }

    // ノーツが移動を開始する座標
    public Vector3 StartPos { set; private get; }

    // ノーツの最終目的地の座標
    private Vector3 moveEndPos;

    // ノーツの初速移動用のアニメーションカーブ
    [SerializeField]
    private AnimationCurve curve;

    private float startTime;

    // ノーツの判定位置までの進行率
    private float notesRate = 0;

    // ノーツの判定位置までの距離
    private float distance = 0;

    // ノーツの判定範囲
    public float Perfect { set; private get; } = 0;
    public float Good { set; private get; } = 0;
    public float Bad { set; private get; } = 0;

    private bool stopFlag = false;
    
    private SpriteRenderer moveNotesSprite = null;
    private SpriteRenderer goalNotesSprite = null;
    private GameObject moveNotesObj = null;
    private GameObject goalNotesObj = null;

    // スプライトの透明度を変更できるようにしておく
    private float spriteAlpha = 0.5f;
    public float SpriteAlpha { set { spriteAlpha = value; } }

    // 画面外を検知する用のRect
    private Rect rect = new Rect(0, 0, 1, 1);

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

        // durationが0秒以下ならノーツを非表示
        if(NotesDuration <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        startTime = Time.timeSinceLevelLoad;

        // ノーツの座標を初期化
        moveNotesObj.transform.position = StartPos;
        goalNotesObj.transform.position = GoalPos;
        Vector3 vec = (GoalPos - StartPos).normalized;
        moveEndPos = vec * Vector3.Distance(StartPos, GoalPos) + GoalPos; 

        // ノーツの画像を差し替え
        moveNotesSprite.sprite = NotesSprites[(int)NotesTypes];
        goalNotesSprite.sprite = NotesSprites[(int)NotesTypes];
        goalNotesSprite.color = new Color(1, 1, 1, spriteAlpha);    // 透明度を設定

        stopFlag = true;
    }

    /// <summary>
    /// ノーツの処理メソッド
    /// </summary>
    private void NotesAction()
    {
        // ノーツの移動開始からの経過時間
        var diff = Time.timeSinceLevelLoad - startTime;

        // 進行率を算出
        notesRate = diff / (NotesDuration * 2);
        var pos = curve.Evaluate(notesRate);

        // ノーツを移動する処理
        moveNotesObj.transform.position = Vector3.Lerp(StartPos, moveEndPos, pos);

        // ノーツが画面外に行ったかをチェック
        var viewport = Camera.main.WorldToViewportPoint(moveNotesObj.transform.position);    // MainCameraのviewportを取得
        if (!rect.Contains(viewport) || notesRate >= 1.0f)
        {
            // 画面外に行ったらノーツを非表示にする
            NotesControl.Instance.NotesResult(NotesControl.ResultType.Bad);
            ResetNotes(false);
        }
    }

    /// <summary>
    /// ノーツの判定処理
    /// </summary>
    /// <param name="notesType">入力したキー</param>
    /// <param name="rate">ノーツの判定値</param>
    private void NotesCheck(NotesType notesType, float rate)
    {
        // 入力したキーが合っているかチェック
        if(notesType == NotesTypes)
        {
            if(rate > (0.5f - Perfect) && rate < (0.5f + Perfect))
            {
                // prefect判定
                NotesControl.Instance.NotesResult(NotesControl.ResultType.Perfect);
            }
            else if((rate > (0.5f - Good) && rate <= (0.5f - Perfect)) || (rate >= (0.5f + Perfect) && rate < (0.5f + Good)))
            {
                // good判定
                NotesControl.Instance.NotesResult(NotesControl.ResultType.Good);
            }
            else
            {
                // bad判定
                NotesControl.Instance.NotesResult(NotesControl.ResultType.Bad);
            }

            ResetNotes(true);
        }
        else
        {
            NotesControl.Instance.NotesResult(NotesControl.ResultType.Bad);
            ResetNotes(true);
        }
    }

    /// <summary>
    /// ノーツの判定処理を実行するかをチェックする処理
    /// </summary>
    /// <param name="notesType">入力したキー</param>
    /// <returns></returns>
    public bool ActionStartCheck(NotesType notesType)
    {
        var rate = notesRate;
        
        // 入力判定内かチェック
        if(rate <= (0.5f - Bad))
        {
            return false;
        }
        else if (rate >= (0.5f + Bad))
        {
            return true;
        }
        else
        {
            NotesCheck(notesType, rate);
            return true;
        }
    }

    /// <summary>
    /// ノーツを初期化する
    /// </summary>
    private void ResetNotes(bool b)
    {
        stopFlag = false;
        notesRate = 0;
        if (b)
        {
            StartCoroutine(DelayMethod(0.15f, () => gameObject.SetActive(false)));
            return;
        }
        gameObject.SetActive(false);
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
