using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotesMode
{
    Single,    // シングル
    Double,    // ダブル
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
    [SerializeField, Header("シングルノーツ")] private SingleNotes singleNotes = new SingleNotes();
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
    [SerializeField, Header("ダブルノーツ")] private DoubleNotes doubleNotes = new DoubleNotes();
    public DoubleNotes DoubleNotesData { get { return doubleNotes; } }

    // ノーツのモード
    public NotesMode Mode { set; get; } = NotesMode.Single;

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

    public Coroutine NotesCoroutine { private set; get; } = null;
    private Coroutine notesResultCoroutine = null;

    [SerializeField, Header("ノーツの判定結果"), Tooltip("判定結果アイコン")] private Sprite[] notesResultSprites = null;
    [SerializeField, Tooltip("判定オブジェクト")] private SpriteRenderer notesResultObj = null;
    public SpriteRenderer NotesResultObj { get { return notesResultObj; } }
    public Animator NotesResultAnime { set; private get; } = null;

    [SerializeField, Header("ノーツの消滅時のエフェクト")] private ParticleSystem[] notesParticles = null;
    

    /// <summary>
    /// ノーツの移動コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoNotesMove(float moveTime)
    {
        float deltaTime = 0;

        float spriteAlpha = 1.0f;
        float endTime = 0;
        float duration = 0.2f;
        bool checkFlag = true;
        Color spriteColor;

        // 判定に向かう移動
        while(spriteAlpha > 0)
        {
            // 進行率を算出
            NotesRate = deltaTime / (moveTime * 2);

            // ノーツを移動する処理
            if(Mode == NotesMode.Single)
            {
                singleNotes.MoveObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);
            }
            else
            {
                doubleNotes.MoveObject.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, NotesRate);
            }

            // ノーツが判定域を超えたら
            if(NotesRate > MaxGood)
            {
                // 一度だけ実行する処理
                if (checkFlag == true)
                {
                    // 判定ノーツを非表示
                    if(Mode == NotesMode.Single)
                    {
                        singleNotes.EndObject.SetActive(false);
                    }
                    else
                    {
                        doubleNotes.EndObject.SetActive(false);
                    }

                    // SNAPを表示
                    NotesResult(0);

                    checkFlag = false;
                }

                // ノーツをゆっくり透明にしていく
                spriteAlpha = 1.0f - (endTime / duration);
                spriteColor = new Color(1, 1, 1, spriteAlpha);
                if(Mode == NotesMode.Single)
                {
                    singleNotes.MoveSprite.color = spriteColor;
                }
                else
                {
                    doubleNotes.MoveSprite1.color = spriteColor;
                }
                endTime += Time.deltaTime;
            }

            deltaTime += Time.deltaTime;
            yield return null;
        }

        // ノーツを非表示にする
        StopNotes(false);
    }

    /// <summary>
    /// ノーツのコルーチン処理を停止
    /// </summary>
    private void StopNotes(bool flag)
    {
        if(NotesCoroutine == null) { return; }

        // ノーツを非表示
        if(flag == true) { StopCoroutine(NotesCoroutine); }
        NotesCoroutine = null;
        if (Mode == NotesMode.Single)
        {
            singleNotes.NotesObject.SetActive(false);
        }
        else
        {
            doubleNotes.NotesObject.SetActive(false);
        }
    }

    /// <summary>
    /// ノーツの入力を検知した時に実行する処理
    /// </summary>
    /// <param name="result">リザルト番号</param>
    public void InputNotes(int result)
    {
        StopNotes(true);
        NotesResult(result);
    }

    /// <summary>
    /// ノーツ再生を強制的に停止させる
    /// </summary>
    public void NotesEmergencyStop()
    {
        StopNotes(true);
        StopNotesRsult(true);
    }

    /// <summary>
    /// ノーツのリザルトを表示
    /// </summary>
    private void NotesResult(int result)
    {
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
                PlayNotesParticle(1);
                break;
            case 2:
                // Rap
                notesResultObj.sprite = notesResultSprites[2];
                PlayNotesParticle(0);
                break;
            default:
                return;
        }

        if (Mode == NotesMode.Single)
        {
            notesResultObj.gameObject.transform.position = singleNotes.EndObject.transform.position;
        }
        else
        {
            notesResultObj.gameObject.transform.position = doubleNotes.EndObject.transform.position;
        }
        
        notesResultCoroutine = StartCoroutine(DoResultAnime());
    }

    /// <summary>
    /// ノーツのリザルト表示コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoResultAnime()
    {
        // 判定アイコンの表示
        notesResultObj.gameObject.SetActive(true);

        while(NotesResultAnime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        StopNotesRsult(false);
    }

    /// <summary>
    /// ノーツのリザルト表示コルーチンを止める
    /// </summary>
    private void StopNotesRsult(bool flag)
    {
        if(notesResultCoroutine == null) { return; }

        if(flag == true) { StopCoroutine(notesResultCoroutine); }
        notesResultObj.gameObject.SetActive(false);
        notesResultCoroutine = null;
    }

    /// <summary>
    /// ノーツの消滅時のエフェクトを再生する処理
    /// </summary>
    /// <param name="particleIndex">パーティクル番号</param>
    private void PlayNotesParticle(int particleIndex)
    {

        if(particleIndex < 0 || particleIndex >= notesParticles.Length) { return; }

        // パーティクルの位置を判定位置にセット
        if(Mode == NotesMode.Single)
        {
            notesParticles[particleIndex].gameObject.transform.position = singleNotes.EndObject.transform.position;
        }
        else
        {
            notesParticles[particleIndex].gameObject.transform.position = doubleNotes.EndObject.transform.position;
        }

        // パーティクルの再生
        notesParticles[particleIndex].Play();
    }

    /// <summary>
    /// Singleモードのノーツのデータ初期化
    /// </summary>
    public void SetSingleNotes(NotesType type, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, Sprite moveSprite, Sprite endSprite, float spriteAlpha)
    {
        // durationが0秒以下または移動開始座標と判定座標が同じならreturnする
        if (duration <= 0 || start == end) { return; }

        // 判定IDの設定
        if(type == NotesType.CircleKey)
        {
            NotesTypeNum = 0;
        }
        else if(type == NotesType.CrossKey)
        {
            NotesTypeNum = 1;
        }
        else if (type == NotesType.TriangleKey)
        {
            NotesTypeNum = 2;
        }
        else if (type == NotesType.UpArrow)
        {
            NotesTypeNum = 3;
        }
        else if (type == NotesType.DownArrow)
        {
            NotesTypeNum = 4;
        }
        else
        {
            NotesTypeNum = 5;
        }

        // ノーツのSprite情報を初期化
        singleNotes.MoveSprite.sprite = moveSprite;
        singleNotes.MoveSprite.color = new Color(1, 1, 1, 1);
        singleNotes.EndSprite.sprite = endSprite;
        singleNotes.EndSprite.color = new Color(1, 1, 1, spriteAlpha);    // 透明度を設定

        // 共通初期化を実行し、ノーツを再生する
        CommonInit(start, end, duration, perfect, good, bad, scale, singleNotes.NotesObject, singleNotes.MoveObject, singleNotes.EndObject);
    }

    /// <summary>
    /// Doubleモードのノーツのデータ初期化
    /// </summary>
    public void SetDoubleNotes(NotesType type1, NotesType type2, Vector3 start, Vector3 end, float duration, float perfect, float good, float bad, Vector3 scale, Sprite moveSprite1, Sprite moveSprite2, Sprite endSprite1, Sprite endSprite2, float spriteAlpha)
    {
        // durationが0秒以下、移動開始座標と判定座標が同じまたはノーツタイプが同じならreturnする
        if (duration <= 0 || start == end || (type1 == type2)) { return; }

        // 判定IDの設定
        if((type1 == NotesType.CircleKey && type2 == NotesType.CrossKey) || (type1 == NotesType.CrossKey && type2 == NotesType.CircleKey))
        {
            NotesTypeNum = 0;
        }
        else if((type1 == NotesType.CircleKey && type2 == NotesType.TriangleKey) || (type1 == NotesType.TriangleKey && type2 == NotesType.CircleKey))
        {
            NotesTypeNum = 1;
        }
        else if ((type1 == NotesType.CircleKey && type2 == NotesType.UpArrow) || (type1 == NotesType.UpArrow && type2 == NotesType.CircleKey))
        {
            NotesTypeNum = 2;
        }
        else if ((type1 == NotesType.CircleKey && type2 == NotesType.DownArrow) || (type1 == NotesType.DownArrow && type2 == NotesType.CircleKey))
        {
            NotesTypeNum = 3;
        }
        else if ((type1 == NotesType.CircleKey && type2 == NotesType.LeftArrow) || (type1 == NotesType.LeftArrow && type2 == NotesType.CircleKey))
        {
            NotesTypeNum = 4;
        }
        else if ((type1 == NotesType.CrossKey && type2 == NotesType.TriangleKey) || (type1 == NotesType.TriangleKey && type2 == NotesType.CrossKey))
        {
            NotesTypeNum = 5;
        }
        else if ((type1 == NotesType.CrossKey && type2 == NotesType.UpArrow) || (type1 == NotesType.UpArrow && type2 == NotesType.CrossKey))
        {
            NotesTypeNum = 6;
        }
        else if ((type1 == NotesType.CrossKey && type2 == NotesType.DownArrow) || (type1 == NotesType.DownArrow && type2 == NotesType.CrossKey))
        {
            NotesTypeNum = 7;
        }
        else if ((type1 == NotesType.CrossKey && type2 == NotesType.LeftArrow) || (type1 == NotesType.LeftArrow && type2 == NotesType.CrossKey))
        {
            NotesTypeNum = 8;
        }
        else if ((type1 == NotesType.TriangleKey && type2 == NotesType.UpArrow) || (type1 == NotesType.UpArrow && type2 == NotesType.TriangleKey))
        {
            NotesTypeNum = 9;
        }
        else if ((type1 == NotesType.TriangleKey && type2 == NotesType.DownArrow) || (type1 == NotesType.DownArrow && type2 == NotesType.TriangleKey))
        {
            NotesTypeNum = 10;
        }
        else if ((type1 == NotesType.TriangleKey && type2 == NotesType.LeftArrow) || (type1 == NotesType.LeftArrow && type2 == NotesType.TriangleKey))
        {
            NotesTypeNum = 11;
        }
        else if ((type1 == NotesType.UpArrow && type2 == NotesType.DownArrow) || (type1 == NotesType.DownArrow && type2 == NotesType.UpArrow))
        {
            NotesTypeNum = 12;
        }
        else if ((type1 == NotesType.UpArrow && type2 == NotesType.LeftArrow) || (type1 == NotesType.LeftArrow && type2 == NotesType.UpArrow))
        {
            NotesTypeNum = 13;
        }
        else
        {
            NotesTypeNum = 14;
        }

        // ノーツのSprite情報を初期化
        doubleNotes.MoveSprite1.sprite = moveSprite1;
        doubleNotes.MoveSprite1.color = new Color(1, 1, 1, 1);
        doubleNotes.MoveSprite2.sprite = moveSprite2;
        doubleNotes.MoveSprite2.color = new Color(1, 1, 1, 1);
        doubleNotes.EndSprite1.sprite = endSprite1;
        doubleNotes.EndSprite1.color = new Color(1, 1, 1, spriteAlpha);    // 透明度を設定
        doubleNotes.EndSprite2.sprite = endSprite2;
        doubleNotes.EndSprite2.color = new Color(1, 1, 1, spriteAlpha);    // 透明度を設定

        // 共通初期化を実行し、ノーツを再生する
        CommonInit(start, end, duration, perfect, good, bad, scale, doubleNotes.NotesObject, doubleNotes.MoveObject, doubleNotes.EndObject);
    }

    /// <summary>
    /// 共通のノーツ初期化処理
    /// </summary>
    private void CommonInit(Vector3 startPos, Vector3 endPos, float duration, float perfect, float good, float bad, Vector3 scale, GameObject notesObj, GameObject moveObj, GameObject endObj)
    {
        MinPerfect = 0.5f - perfect;
        MaxPerfect = 0.5f + perfect;
        MinGood = MinPerfect - good;
        MaxGood = MaxPerfect + good;
        MinBad = MinGood - bad;
        moveEndPos = (endPos - startPos).normalized * Vector3.Distance(startPos, endPos) + endPos;
        moveStartPos = startPos;

        notesObj.transform.localScale = scale;
        moveObj.transform.position = startPos;
        endObj.transform.position = endPos;

        if(endObj.activeSelf == false)
        {
            endObj.SetActive(true);
        }

        NotesRate = 0;

        notesObj.SetActive(true);
        NotesCoroutine = StartCoroutine(DoNotesMove(duration));
    }
}
