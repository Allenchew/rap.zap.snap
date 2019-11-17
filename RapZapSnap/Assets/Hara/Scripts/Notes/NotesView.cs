using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesView : NotesModel
{
    // ノーツの移動をUIモードで行うか
    public bool NotesUIMode { set; private get; } = false;

    // ノーツのGameObject
    public GameObject MoveNotesObj { set; private get; } = null;
    public GameObject GoalNotesObj { set; private get; } = null;

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
    public float MinPerfect { private set; get; } = 0;
    public float MaxPerfect { private set; get; } = 0;
    public float MinGood { private set; get; } = 0;
    public float MaxGood { private set; get; } = 0;
    public float MinBad { private set; get; } = 0;

    private bool stopFlag = false;
    private bool notesClickFlag = false;
    private float firstMoveEndTime = 0;

    // ノーツのイメージデータ情報
    private SpriteRenderer moveNotesSprite = null;
    private SpriteRenderer goalNotesSprite = null;
    private UnityEngine.UI.Image moveNotesImage = null;
    private UnityEngine.UI.Image goalNotesImage = null;

    // Imageの透明度を変更できるようにしておく
    private float mainSpriteAlpha = 1.0f;
    private float goalSpriteAlpha = 0.5f;

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
        startTime = Time.timeSinceLevelLoad;
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
            var diff = notesClickFlag ? (Time.timeSinceLevelLoad - startTime) : (Time.timeSinceLevelLoad - firstMoveEndTime);

            // 進行率を算出
            NotesRate = firstMoveEnd ? (diff / moveDuration) : (diff / (moveDuration * 2));

            // ノーツを移動する処理
            _=NotesUIMode ? MoveNotesObj.gameObject.transform.localPosition = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate) : MoveNotesObj.gameObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);

            if (!notesClickFlag)
            {
                if (mainSpriteAlpha > 0)
                {
                    // 透明度を下げる
                    mainSpriteAlpha -= 0.05f;
                    _ = NotesUIMode ? moveNotesImage.color = new Color(1, 1, 1, mainSpriteAlpha) : moveNotesSprite.color = new Color(1, 1, 1, mainSpriteAlpha);
                }

                if (NotesRate >= 1.0f || mainSpriteAlpha <= 0)
                {
                    // ノーツが完全に透明になるか、目的地に着いたら非表示
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

        if (notesClickFlag && rate >= MinBad)
        {
            int num;

            // 入力したキーが合っているかチェック
            if (notesType == NotesTypes)
            {
                if (rate >= MinPerfect && rate <= MaxPerfect)
                {
                    // prefect判定
                    num = 2;
                }
                else if ((rate >= MinGood && rate < MinPerfect) || (rate >= MaxGood && rate < MaxPerfect))
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
        // durationが0秒以下または移動開始座標と判定座標が同じならreturnする
        if (duration <= 0 || start == end) return;

        // 共通変数の初期化
        NotesTypes = type;
        startPos = start;
        endPos = end;
        moveDuration = duration;
        goalSpriteAlpha = spriteAlpha;

        if (!GoalNotesObj.gameObject.activeSelf)
        {
            GoalNotesObj.gameObject.SetActive(true);
        }

        MinPerfect = 0.5f - perfect;
        MaxPerfect = 0.5f + perfect;
        MinGood = MinPerfect - good;
        MaxGood = MaxPerfect + good;
        MinBad = MinGood - bad;
        
        mainSpriteAlpha = 1.0f;
        
        MoveNotesObj.transform.localScale = scale;
        GoalNotesObj.transform.localScale = scale;

        moveEndPos = (endPos - startPos).normalized * Vector3.Distance(startPos, endPos) + endPos;
        moveStartPos = startPos;

        NotesRate = 0;
        notesClickFlag = true;
        firstMoveEnd = false;

        // UIモードと通常モードそれぞれで使う変数の初期化
        if (NotesUIMode)
        {
            if (moveNotesImage == null) moveNotesImage = MoveNotesObj.GetComponent<UnityEngine.UI.Image>();
            if (goalNotesImage == null) goalNotesImage = GoalNotesObj.GetComponent<UnityEngine.UI.Image>();

            moveNotesImage.sprite = NotesSprites[(int)NotesTypes];
            moveNotesImage.SetNativeSize();
            moveNotesImage.color = new Color(1, 1, 1, 1);
            goalNotesImage.sprite = NotesSprites[(int)NotesTypes];
            goalNotesImage.SetNativeSize();
            goalNotesImage.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定

            MoveNotesObj.transform.localPosition = startPos;
            GoalNotesObj.transform.localPosition = endPos;
        }
        else
        {
            if (moveNotesSprite == null) moveNotesSprite = MoveNotesObj.GetComponent<SpriteRenderer>();
            if (goalNotesSprite == null) goalNotesSprite = GoalNotesObj.GetComponent<SpriteRenderer>();

            moveNotesSprite.sprite = NotesSprites[(int)NotesTypes];
            moveNotesSprite.color = new Color(1, 1, 1, 1);
            goalNotesSprite.sprite = NotesSprites[(int)NotesTypes];
            goalNotesSprite.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定

            MoveNotesObj.transform.position = startPos;
            GoalNotesObj.transform.position = endPos;
        }

        // ノーツの再生
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ノーツの判定終了後の移動を設定する
    /// </summary>
    public void SecondMoveSet()
    {
        notesClickFlag = false;
        GoalNotesObj.gameObject.SetActive(false);

        // 第2移動の目的地を設定する
        var radius = Vector3.Distance(startPos, endPos);
        int direction = 8;    // 方向数
        float angleDiff = 360.0f / direction;    // 方向転換する角度の間隔
        var xPosDistance = Mathf.Abs(startPos.x - endPos.x);
        var yPosDistance = Mathf.Abs(startPos.y - endPos.y);
        int directionNum = 0;
        int rnd = Random.Range(0, 3);

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
        Vector3 newMoveStartPos = NotesUIMode ? MoveNotesObj.transform.localPosition : MoveNotesObj.transform.position;
        newMoveStartPos.x += radius * Mathf.Cos(angle);
        newMoveStartPos.y += radius * Mathf.Sin(angle);

        // 座標データを更新
        moveStartPos = NotesUIMode ? MoveNotesObj.gameObject.transform.localPosition : MoveNotesObj.transform.position;
        moveEndPos = newMoveStartPos;
        firstMoveEndTime = Time.timeSinceLevelLoad;

        firstMoveEnd = true;
    }
}
