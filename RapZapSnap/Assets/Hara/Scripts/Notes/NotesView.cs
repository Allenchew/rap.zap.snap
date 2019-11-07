using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode
{
    Arrival,      // ノーツが判定で止まる
    Pass          // ノーツが判定を通過する
}

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

    private float notesPerfectBorder
    {
        get
        {
            float num = 0;
            switch (NotesMoveMode)
            {
                case MoveMode.Arrival:
                    num = 1.0f;
                    break;
                case MoveMode.Pass:
                    num = 0.5f;
                    break;
            }
            return num;
        }
    }

    // ノーツの判定範囲
    private float minPerfect = 0;
    private float maxPerfect = 0;
    public float Perfect { set { minPerfect = notesPerfectBorder - value; maxPerfect = notesPerfectBorder + value; } }

    private float minGood = 0;
    private float maxGood = 0;
    public float Good { set { minGood = minPerfect - value; maxGood = maxPerfect + value; } }

    private float minBad = 0;
    private float maxBad = 0;
    public float Bad { set { minBad = minGood - value; maxBad = maxGood + value; } }

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

    // ノーツの移動タイプの設定
    public MoveMode NotesMoveMode { set; private get; } = MoveMode.Arrival;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        NotesAction();
    }

    public void StartNotes()
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
        moveEndPos = NotesMoveMode == MoveMode.Arrival ? GoalPos : ((GoalPos - StartPos).normalized * Vector3.Distance(StartPos, GoalPos) + GoalPos);

        // ノーツの画像を差し替え
        moveNotesSprite.sprite = NotesSprites[(int)NotesTypes];
        goalNotesSprite.sprite = NotesSprites[(int)NotesTypes];
        goalNotesSprite.color = new Color(1, 1, 1, spriteAlpha);    // 透明度を設定

        notesRate = 0;

        gameObject.SetActive(true);    // ノーツ表示
        stopFlag = true;               // 処理開始
    }

    /// <summary>
    /// ノーツの処理メソッド
    /// </summary>
    private void NotesAction()
    {
        if (stopFlag)
        {
            // ノーツの移動開始からの経過時間
            var diff = Time.timeSinceLevelLoad - startTime;

            // 進行率を算出
            notesRate = NotesMoveMode == MoveMode.Arrival ? (diff / NotesDuration) : (diff / (NotesDuration * 2));
            var pos = curve.Evaluate(notesRate);

            // ノーツを移動する処理
            moveNotesObj.transform.position = Vector3.Lerp(StartPos, moveEndPos, pos);

            switch (NotesMoveMode)
            {
                case MoveMode.Arrival:
                    // ノーツが判定位置に到達したかチェック
                    if(diff > NotesDuration)
                    {
                        moveNotesObj.transform.position = GoalPos;
                        stopFlag = false;
                        StartCoroutine(DelayMethod(0.1f, () =>
                        {
                            NotesControl.Instance.NotesCount++;
                            ReturnResult(NotesControl.ResultType.Bad);
                        }));
                    }
                    break;
                case MoveMode.Pass:
                    // ノーツの入力判定が有効範囲内かチェック
                    if(notesRate >= maxGood) NotesControl.Instance.NotesCount++;
                    // ノーツが画面外に行ったかをチェック
                    var viewport = Camera.main.WorldToViewportPoint(moveNotesObj.transform.position);    // MainCameraのviewportを取得
                    if (!rect.Contains(viewport) || notesRate >= 1.0f)
                    {
                        // 画面外に行ったらノーツを非表示にする
                        stopFlag = false;
                        ReturnResult(NotesControl.ResultType.Bad);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// ノーツの判定処理
    /// </summary>
    /// <param name="notesType">入力したキー</param>
    /// <param name="rate">ノーツの判定値</param>
    private void NotesCheck(NotesType notesType, float rate)
    {
        var result = NotesControl.ResultType.Bad;
        // 入力したキーが合っているかチェック
        if(notesType == NotesTypes)
        {
            if(rate >= minPerfect && rate <= maxPerfect)
            {
                // prefect判定
                result = NotesControl.ResultType.Perfect;
            }
            else if((rate >= minGood && rate < minPerfect) || (rate >= maxGood && rate < maxPerfect))
            {
                // good判定
                result = NotesControl.ResultType.Good;
            }
            else
            {
                // bad判定
                result = NotesControl.ResultType.Bad;
            }
        }
        else
        {
            result = NotesControl.ResultType.Bad;
        }

        ReturnResult(result);
    }

    /// <summary>
    /// ノーツの判定結果を返す＆ノーツの初期化処理
    /// </summary>
    /// <param name="resultType">判定の結果</param>
    private void ReturnResult(NotesControl.ResultType resultType)
    {
        NotesControl.Instance.NotesResult(resultType);
        gameObject.SetActive(false);
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
        if (rate < minBad)
        {
            return false;
        }
        else
        {
            NotesCheck(notesType, rate);
            return true;
        }
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
