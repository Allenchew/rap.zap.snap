using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotesMode
{
    Single,    // シングル
    Double,    // ダブル
    Mash       // 連打
}

public class NotesView : NotesModel
{
    public struct Notes
    {
        public GameObject NotesParentObject;
        public GameObject MoveNotesObject;
        public GameObject EndNotesObject;

        public SpriteRenderer MoveNotesSprite1;
        public SpriteRenderer MoveNotesSprite2;
        public SpriteRenderer EndNotesSprite1;
        public SpriteRenderer EndNotesSprite2;
    }

    public Notes SingleNotes { set; get; }
    public Notes DoubleNotes { set; get; }
    public Notes MashNotes { set; get; }

    // ノーツのモード
    public NotesMode Mode { set; get; } = NotesMode.Single;

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

    // Imageの透明度を変更できるようにしておく
    private float mainSpriteAlpha = 1.0f;
    private float goalSpriteAlpha = 0.5f;

    private Coroutine coroutine = null;

    public bool NotesClickFlag { private set; get; } = true;    // クリックの有効フラグ

    /// <summary>
    /// ノーツのSprite情報の取得
    /// </summary>
    /// <param name="notesObject">ノーツの親オブジェクト</param>
    public void GetNotesSprite(GameObject notesObject)
    {
        Notes data;

        // シングルノーツの情報取得
        data = SingleNotes;
        data.NotesParentObject = notesObject.transform.GetChild(0).gameObject;
        data.MoveNotesObject = data.NotesParentObject.transform.GetChild(1).gameObject;
        data.EndNotesObject = data.NotesParentObject.transform.GetChild(0).gameObject;
        data.MoveNotesSprite1 = data.MoveNotesObject.GetComponent<SpriteRenderer>();
        data.EndNotesSprite1 = data.EndNotesObject.GetComponent<SpriteRenderer>();
        SingleNotes = data;

        // ダブルノーツの情報取得
        data = DoubleNotes;
        data.NotesParentObject = notesObject.transform.GetChild(1).gameObject;
        data.MoveNotesObject = data.NotesParentObject.transform.GetChild(1).gameObject;
        data.EndNotesObject = data.NotesParentObject.transform.GetChild(0).gameObject;
        data.MoveNotesSprite1 = data.MoveNotesObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        data.MoveNotesSprite2 = data.MoveNotesObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        data.EndNotesSprite1 = data.EndNotesObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        data.EndNotesSprite2 = data.EndNotesObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        DoubleNotes = data;
    }

    /// <summary>
    /// ノーツの移動コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoNotesMove()
    {
        var time = 0f;

        // 判定に向かう移動
        while(NotesRate < 1.0f && mainSpriteAlpha > 0)
        {
            // 進行率を算出
            NotesRate = time / (moveDuration * 2);

            // ノーツを移動する処理
            if(Mode == NotesMode.Single)
            {
                SingleNotes.MoveNotesObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);
            }
            else if(Mode == NotesMode.Double)
            {
                DoubleNotes.MoveNotesObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);
            }

            time += Time.deltaTime;

            if(NotesRate > MaxGood && NotesClickFlag == true)
            {
                // 判定ノーツを非表示にする
                if (Mode == NotesMode.Single)
                {
                    SingleNotes.EndNotesObject.SetActive(false);
                }
                else if (Mode == NotesMode.Double)
                {
                    DoubleNotes.EndNotesObject.SetActive(false);
                }
                NotesClickFlag = false;
            }

            if(NotesClickFlag == false)
            {
                // 透明度を下げる
                mainSpriteAlpha -= 0.15f;
                if (Mode == NotesMode.Single)
                {
                    SingleNotes.MoveNotesSprite1.color = new Color(1, 1, 1, mainSpriteAlpha);
                }
                else if (Mode == NotesMode.Double)
                {
                    DoubleNotes.MoveNotesSprite1.color = new Color(1, 1, 1, mainSpriteAlpha);
                    DoubleNotes.MoveNotesSprite2.color = new Color(1, 1, 1, mainSpriteAlpha);
                }
            }

            yield return null;
        }

        ResetNotes();

        /*
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
        Vector3 newEndPos = MoveNotesObj.transform.position;
        newEndPos.x += radius * Mathf.Cos(angle);
        newEndPos.y += radius * Mathf.Sin(angle);

        // 座標データを更新
        moveStartPos = MoveNotesObj.transform.position;
        moveEndPos = newEndPos;

        time = 0;

        // 判定後の移動
        while(mainSpriteAlpha > 0)
        {
            // 透明度を下げる
            mainSpriteAlpha -= 0.05f;
            if(Mode == NotesMode.Single)
            {
                SingleNotes.MoveNotesSprite1.color = new Color(1, 1, 1, mainSpriteAlpha);
            }
            else if(Mode == NotesMode.Double)
            {
                DoubleNotes.MoveNotesSprite1.color = new Color(1, 1, 1, mainSpriteAlpha);
                DoubleNotes.MoveNotesSprite2.color = new Color(1, 1, 1, mainSpriteAlpha);
            }

            // 進行率を算出
            NotesRate = time / moveDuration;

            // ノーツを移動する処理
            MoveNotesObj.gameObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);

            time += Time.deltaTime;

            yield return null;
        }*/
    }

    /// <summary>
    /// ノーツの判定結果を返す＆ノーツの初期化処理
    /// </summary>
    public void ResetNotes()
    {
        StopCoroutine(coroutine);
        if(Mode == NotesMode.Single)
        {
            SingleNotes.NotesParentObject.SetActive(false);
        }
        else if (Mode == NotesMode.Double)
        {
            DoubleNotes.NotesParentObject.SetActive(false);
        }
    }

    /// <summary>
    /// Singleモードのノーツのデータ初期化
    /// </summary>
    public void SingleNotesData(NotesType type, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, Sprite moveSprite, Sprite endSprite, float spriteAlpha)
    {
        // durationが0秒以下または移動開始座標と判定座標が同じならreturnする
        if (duration <= 0 || start == end) { return; }

        // ノーツのデータを初期化
        NotesType1 = type;
        startPos = start;
        endPos = end;
        moveDuration = duration;
        goalSpriteAlpha = spriteAlpha;

        if (SingleNotes.EndNotesObject.activeSelf == false)
        {
            SingleNotes.EndNotesObject.SetActive(true);
        }

        // 判定域の設定
        MinPerfect = 0.5f - perfect;
        MaxPerfect = 0.5f + perfect;
        MinGood = MinPerfect - good;
        MaxGood = MaxPerfect + good;
        MinBad = MinGood - bad;
        moveEndPos = (endPos - startPos).normalized * Vector3.Distance(startPos, endPos) + endPos;
        moveStartPos = startPos;

        // ノーツの表示サイズ&座標を初期化
        SingleNotes.NotesParentObject.transform.localScale = scale;
        SingleNotes.MoveNotesObject.transform.position = startPos;
        SingleNotes.EndNotesObject.transform.position = endPos;

        // 進行率と入力フラグの初期化
        NotesRate = 0;
        NotesClickFlag = true;
        
        // ノーツのSprite情報を初期化
        SingleNotes.MoveNotesSprite1.sprite = moveSprite;
        SingleNotes.MoveNotesSprite1.color = new Color(1, 1, 1, 1);
        SingleNotes.EndNotesSprite1.sprite = endSprite;
        SingleNotes.EndNotesSprite1.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定
        mainSpriteAlpha = 1.0f;

        // ノーツの再生
        SingleNotes.NotesParentObject.SetActive(true);
        coroutine = StartCoroutine(DoNotesMove());
    }

    /// <summary>
    /// Doubleモードのノーツのデータ初期化
    /// </summary>
    public void DoubleNotesData(NotesType type1, NotesType type2, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, Sprite moveSprite1, Sprite moveSprite2, Sprite endSprite1, Sprite endSprite2, float spriteAlpha)
    {
        // durationが0秒以下、移動開始座標と判定座標が同じまたはノーツタイプが同じならreturnする
        if (duration <= 0 || start == end || (type1 == type2)) { return; }

        // ノーツのデータを初期化
        NotesType1 = type1;
        NotesType2 = type2;
        startPos = start;
        endPos = end;
        moveDuration = duration;
        goalSpriteAlpha = spriteAlpha;

        if (DoubleNotes.EndNotesObject.activeSelf == false)
        {
            DoubleNotes.EndNotesObject.SetActive(true);
        }

        // 判定IDの設定
        if((NotesType1 == NotesType.CircleKey && NotesType2 == NotesType.CrossKey) || (NotesType1 == NotesType.CrossKey && NotesType2 == NotesType.CircleKey))
        {
            NotesTypeNum = 0;
        }
        else if((NotesType1 == NotesType.CircleKey && NotesType2 == NotesType.TriangleKey) || (NotesType1 == NotesType.TriangleKey && NotesType2 == NotesType.CircleKey))
        {
            NotesTypeNum = 1;
        }
        else if ((NotesType1 == NotesType.CircleKey && NotesType2 == NotesType.UpArrow) || (NotesType1 == NotesType.UpArrow && NotesType2 == NotesType.CircleKey))
        {
            NotesTypeNum = 2;
        }
        else if ((NotesType1 == NotesType.CircleKey && NotesType2 == NotesType.DownArrow) || (NotesType1 == NotesType.DownArrow && NotesType2 == NotesType.CircleKey))
        {
            NotesTypeNum = 3;
        }
        else if ((NotesType1 == NotesType.CircleKey && NotesType2 == NotesType.LeftArrow) || (NotesType1 == NotesType.LeftArrow && NotesType2 == NotesType.CircleKey))
        {
            NotesTypeNum = 4;
        }
        else if ((NotesType1 == NotesType.CrossKey && NotesType2 == NotesType.TriangleKey) || (NotesType1 == NotesType.TriangleKey && NotesType2 == NotesType.CrossKey))
        {
            NotesTypeNum = 5;
        }
        else if ((NotesType1 == NotesType.CrossKey && NotesType2 == NotesType.UpArrow) || (NotesType1 == NotesType.UpArrow && NotesType2 == NotesType.CrossKey))
        {
            NotesTypeNum = 6;
        }
        else if ((NotesType1 == NotesType.CrossKey && NotesType2 == NotesType.DownArrow) || (NotesType1 == NotesType.DownArrow && NotesType2 == NotesType.CrossKey))
        {
            NotesTypeNum = 7;
        }
        else if ((NotesType1 == NotesType.CrossKey && NotesType2 == NotesType.LeftArrow) || (NotesType1 == NotesType.LeftArrow && NotesType2 == NotesType.CrossKey))
        {
            NotesTypeNum = 8;
        }
        else if ((NotesType1 == NotesType.TriangleKey && NotesType2 == NotesType.UpArrow) || (NotesType1 == NotesType.UpArrow && NotesType2 == NotesType.TriangleKey))
        {
            NotesTypeNum = 9;
        }
        else if ((NotesType1 == NotesType.TriangleKey && NotesType2 == NotesType.DownArrow) || (NotesType1 == NotesType.DownArrow && NotesType2 == NotesType.TriangleKey))
        {
            NotesTypeNum = 10;
        }
        else if ((NotesType1 == NotesType.TriangleKey && NotesType2 == NotesType.LeftArrow) || (NotesType1 == NotesType.LeftArrow && NotesType2 == NotesType.TriangleKey))
        {
            NotesTypeNum = 11;
        }
        else if ((NotesType1 == NotesType.UpArrow && NotesType2 == NotesType.DownArrow) || (NotesType1 == NotesType.DownArrow && NotesType2 == NotesType.UpArrow))
        {
            NotesTypeNum = 12;
        }
        else if ((NotesType1 == NotesType.UpArrow && NotesType2 == NotesType.LeftArrow) || (NotesType1 == NotesType.LeftArrow && NotesType2 == NotesType.UpArrow))
        {
            NotesTypeNum = 13;
        }
        else
        {
            NotesTypeNum = 14;
        }

        // 判定域の設定
        MinPerfect = 0.5f - perfect;
        MaxPerfect = 0.5f + perfect;
        MinGood = MinPerfect - good;
        MaxGood = MaxPerfect + good;
        MinBad = MinGood - bad;
        moveEndPos = (endPos - startPos).normalized * Vector3.Distance(startPos, endPos) + endPos;
        moveStartPos = startPos;

        // ノーツの表示サイズ&座標を初期化
        DoubleNotes.NotesParentObject.transform.localScale = scale;
        DoubleNotes.MoveNotesObject.transform.position = startPos;
        DoubleNotes.EndNotesObject.transform.position = endPos;

        // 進行率と入力フラグの初期化
        NotesRate = 0;
        NotesClickFlag = true;

        // ノーツのSprite情報を初期化
        DoubleNotes.MoveNotesSprite1.sprite = moveSprite1;
        DoubleNotes.MoveNotesSprite1.color = new Color(1, 1, 1, 1);
        DoubleNotes.MoveNotesSprite2.sprite = moveSprite2;
        DoubleNotes.MoveNotesSprite2.color = new Color(1, 1, 1, 1);
        DoubleNotes.EndNotesSprite1.sprite = endSprite1;
        DoubleNotes.EndNotesSprite1.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定
        DoubleNotes.EndNotesSprite2.sprite = endSprite2;
        DoubleNotes.EndNotesSprite2.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定
        mainSpriteAlpha = 1.0f;

        // ノーツの再生
        DoubleNotes.NotesParentObject.SetActive(true);
        coroutine = StartCoroutine(DoNotesMove());
    }
}
