using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotesMode
{
    Single,    // シングル
    Double,    // ダブル
    Long       // 連打
}

public class NotesView : NotesModel
{
    [System.Serializable]
    public struct SingleNotes
    {
        public GameObject NotesObject;
        public GameObject MoveObject;
        public GameObject EndObject;
        public SpriteRenderer MoveSprite;
        public SpriteRenderer EndSprite;
    }
    [SerializeField, Header("シングルノーツ")] private SingleNotes singleNotes;
    public SingleNotes SingleNotesData { get { return singleNotes; } }

    [System.Serializable]
    public struct DoubleNotes
    {
        public GameObject NotesObject;
        public GameObject MoveObject;
        public GameObject EndObject;
        public SpriteRenderer MoveSprite1;
        public SpriteRenderer MoveSprite2;
        public SpriteRenderer EndSprite1;
        public SpriteRenderer EndSprite2;
    }
    [SerializeField, Header("ダブルノーツ")] private DoubleNotes doubleNotes;
    public DoubleNotes DoubleNotesData { get { return doubleNotes; } }

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

    public Coroutine NotesCoroutine { private set; get; } = null;

    public bool NotesClickFlag { private set; get; } = true;    // クリックの有効フラグ

    [SerializeField, Header("ノーツの判定結果"), Tooltip("判定結果アイコン")] private Sprite[] notesResultSprites = null;
    [SerializeField, Tooltip("判定オブジェクト")] private SpriteRenderer notesResultObj = null;
    public SpriteRenderer NotesResultObj { get { return notesResultObj; } }
    public Animator NotesResultAnime { set; private get; } = null;
    

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
                singleNotes.MoveObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);
            }
            else if(Mode == NotesMode.Double)
            {
                doubleNotes.MoveObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);
            }

            time += Time.deltaTime;

            if(NotesRate > MaxGood && NotesClickFlag == true)
            {
                // 判定ノーツを非表示にする
                if (Mode == NotesMode.Single)
                {
                    singleNotes.EndObject.SetActive(false);
                }
                else if (Mode == NotesMode.Double)
                {
                    doubleNotes.EndObject.SetActive(false);
                }
                NotesClickFlag = false;
            }

            if(NotesClickFlag == false)
            {
                // 透明度を下げる
                mainSpriteAlpha -= 0.15f;
                if (Mode == NotesMode.Single)
                {
                    singleNotes.MoveSprite.color = new Color(1, 1, 1, mainSpriteAlpha);
                }
                else if (Mode == NotesMode.Double)
                {
                    doubleNotes.MoveSprite1.color = new Color(1, 1, 1, mainSpriteAlpha);
                    doubleNotes.MoveSprite2.color = new Color(1, 1, 1, mainSpriteAlpha);
                }
            }

            yield return null;
        }

        ResultNotes(0);

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
    /// ノーツのリザルトを表示
    /// </summary>
    public void ResultNotes(int result)
    {
        // ノーツを非表示
        StopCoroutine(NotesCoroutine);
        NotesCoroutine = null;
        if(Mode == NotesMode.Single)
        {
            singleNotes.NotesObject.SetActive(false);
        }
        else if (Mode == NotesMode.Double)
        {
            doubleNotes.NotesObject.SetActive(false);
        }

        // ノーツの判定を表示
        switch (result)
        {
            case 0:
                // Zap
                notesResultObj.sprite = notesResultSprites[0];
                break;
            case 1:
                // Snap
                notesResultObj.sprite = notesResultSprites[1];
                break;
            case 2:
                // Rap
                notesResultObj.sprite = notesResultSprites[2];
                break;
            default:
                return;
        }
        notesResultObj.gameObject.transform.position = endPos;
        StartCoroutine(DoResultAnime());
    }

    private IEnumerator DoResultAnime()
    {
        notesResultObj.gameObject.SetActive(true);

        while(NotesResultAnime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        notesResultObj.gameObject.SetActive(false);
    }

    /// <summary>
    /// Singleモードのノーツのデータ初期化
    /// </summary>
    public void SetSingleNotes(NotesType type, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, Sprite moveSprite, Sprite endSprite, float spriteAlpha)
    {
        // durationが0秒以下または移動開始座標と判定座標が同じならreturnする
        if (duration <= 0 || start == end) { return; }

        // ノーツのデータを初期化
        NotesType1 = type;
        startPos = start;
        endPos = end;
        moveDuration = duration;
        goalSpriteAlpha = spriteAlpha;

        if (singleNotes.EndObject.activeSelf == false)
        {
            singleNotes.EndObject.SetActive(true);
        }

        // 判定IDの設定
        if(NotesType1 == NotesType.CircleKey)
        {
            NotesTypeNum = 0;
        }
        else if(NotesType1 == NotesType.CrossKey)
        {
            NotesTypeNum = 1;
        }
        else if (NotesType1 == NotesType.TriangleKey)
        {
            NotesTypeNum = 2;
        }
        else if (NotesType1 == NotesType.UpArrow)
        {
            NotesTypeNum = 3;
        }
        else if (NotesType1 == NotesType.DownArrow)
        {
            NotesTypeNum = 4;
        }
        else
        {
            NotesTypeNum = 5;
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
        singleNotes.NotesObject.transform.localScale = scale;
        singleNotes.MoveObject.transform.position = startPos;
        singleNotes.EndObject.transform.position = endPos;

        // 進行率と入力フラグの初期化
        NotesRate = 0;
        NotesClickFlag = true;
        
        // ノーツのSprite情報を初期化
        singleNotes.MoveSprite.sprite = moveSprite;
        singleNotes.MoveSprite.color = new Color(1, 1, 1, 1);
        singleNotes.EndSprite.sprite = endSprite;
        singleNotes.EndSprite.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定
        mainSpriteAlpha = 1.0f;

        // ノーツの再生
        singleNotes.NotesObject.SetActive(true);
        NotesCoroutine = StartCoroutine(DoNotesMove());
    }

    /// <summary>
    /// Doubleモードのノーツのデータ初期化
    /// </summary>
    public void SetDoubleNotes(NotesType type1, NotesType type2, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, Sprite moveSprite1, Sprite moveSprite2, Sprite endSprite1, Sprite endSprite2, float spriteAlpha)
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

        if (doubleNotes.EndObject.activeSelf == false)
        {
            doubleNotes.EndObject.SetActive(true);
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
        doubleNotes.NotesObject.transform.localScale = scale;
        doubleNotes.MoveObject.transform.position = startPos;
        doubleNotes.EndObject.transform.position = endPos;

        // 進行率と入力フラグの初期化
        NotesRate = 0;
        NotesClickFlag = true;

        // ノーツのSprite情報を初期化
        doubleNotes.MoveSprite1.sprite = moveSprite1;
        doubleNotes.MoveSprite1.color = new Color(1, 1, 1, 1);
        doubleNotes.MoveSprite2.sprite = moveSprite2;
        doubleNotes.MoveSprite2.color = new Color(1, 1, 1, 1);
        doubleNotes.EndSprite1.sprite = endSprite1;
        doubleNotes.EndSprite1.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定
        doubleNotes.EndSprite2.sprite = endSprite2;
        doubleNotes.EndSprite2.color = new Color(1, 1, 1, goalSpriteAlpha);    // 透明度を設定
        mainSpriteAlpha = 1.0f;

        // ノーツの再生
        doubleNotes.NotesObject.SetActive(true);
        NotesCoroutine = StartCoroutine(DoNotesMove());
    }
}
