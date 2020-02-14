using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesControl : SingletonMonoBehaviour<NotesControl>
{
    // ノーツの最大生成数
    [SerializeField, Tooltip("ノーツの最大生成数"), Range(1, 20), Header("ノーツの基本設定")]
    private int maxNotes = 5;

    // ノーツのプレファブオブジェクト
    [SerializeField, Tooltip("ノーツのPrefab")]
    private GameObject notesPrefab = null;

    [SerializeField, Tooltip("移動ノーツのSprite")]
    private Sprite[] moveNotesSprites = null;

    [SerializeField, Tooltip("判定ノーツのSprite")]
    private Sprite[] endNotesSprites = null;

    private bool clickFlag1 = true;    // 1P用の入力フラグ
    private bool clickFlag2 = true;    // 2P用の入力フラグ

    /// <summary>
    /// ノーツの管理用データベース
    /// </summary>
    private struct NotesDataBase
    {
        // ノーツのオブジェクトプール用の配列
        public NotesView[] NotesObjects;

        // ノーツの呼び出し番号
        private int notesCallCount;
        public int NotesCallCount { set { notesCallCount = value; if (notesCallCount >= NotesObjects.Length || notesCallCount < 0) notesCallCount = 0; } get { return notesCallCount; } }

        // 判定チェック中のノーツ番号
        private int notesCheckCount;
        public int NotesCheckCount { set { notesCheckCount = value; if (notesCheckCount >= NotesObjects.Length || notesCheckCount < 0) notesCheckCount = 0; } get { return notesCheckCount; } }

        // 初期化
        public void ResetDataBase()
        {
            notesCallCount = 0;
            notesCheckCount = 0;
        }
    }
    private NotesDataBase dataBase1;
    private NotesDataBase dataBase2;

    [SerializeField, Tooltip("生成されるノーツのサイズ")]
    private float notesSize = 1.0f;

    [SerializeField, Tooltip("判定ノーツの透明度"), Range(0, 1.0f)]
    private float notesSpriteAlpha = 0.5f;

    [SerializeField, Tooltip("ノーツ判定距離:Rap"), Range(0.0001f, 0.05f), Header("ノーツの判定域")]
    private float perfectLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Zap"), Range(0.0001f, 0.05f)]
    private float goodLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Snap"), Range(0.0001f, 0.05f)]
    private float badLength = 0.05f;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        InputNotesAction(ControllerNum.P1);
        InputNotesAction(ControllerNum.P2);
    }

    /// <summary>
    /// NotesControlの初期化
    /// </summary>
    private void Init()
    {
        dataBase1.ResetDataBase();
        dataBase2.ResetDataBase();
        CreateNotes(ControllerNum.P1);
        CreateNotes(ControllerNum.P2);
    }

    /// <summary>
    /// ノーツの生成
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    private void CreateNotes(ControllerNum id)
    {
        if (notesPrefab == null)
        {
            Debug.LogError("プレファブが設定されていません");
            return;
        }

        NotesDataBase notesData = id == ControllerNum.P1 ? dataBase1 : dataBase2;

        // NotesView配列の初期化
        notesData.NotesObjects = new NotesView[maxNotes];

        for (int i = 0; i < maxNotes; i++)
        {
            if(notesData.NotesObjects[i] == null)
            {
                GameObject obj = Instantiate(notesPrefab, gameObject.transform, false);
                notesData.NotesObjects[i] = obj.GetComponent<NotesView>();
                notesData.NotesObjects[i].SingleNotesData.NotesObject.SetActive(false);
                notesData.NotesObjects[i].DoubleNotesData.NotesObject.SetActive(false);
                notesData.NotesObjects[i].NotesResultAnime = notesData.NotesObjects[i].NotesResultObj.GetComponent<Animator>();
                notesData.NotesObjects[i].NotesResultObj.gameObject.SetActive(false);
            }
        }
        _ = id == ControllerNum.P1 ? dataBase1 = notesData : dataBase2 = notesData;
    }

    /// <summary>
    /// ノーツを1回再生する処理
    /// </summary>
    /// <param name="type">再生するノーツのタイプ</param>
    /// <param name="startPos">ノーツの再生開始座標</param>
    /// <param name="endPos">ノーツの判定座標</param>
    /// <param name="id">入力対象のコントローラ番号</param>
    /// <param name="duration">再生開始位置から判定位置まで移動するのにかかる時間[s]</param>
    public void PlayNotesOneShot(NotesType type, Vector3 startPos, Vector3 endPos, ControllerNum id, float duration = 1.0f)
    {
        if (startPos == endPos) { return; }
        NotesDataBase notesData = id == ControllerNum.P1 ? dataBase1 : dataBase2;

        // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
        if (notesData.NotesObjects[notesData.NotesCallCount].NotesCoroutine != null) { return; }

        // ノーツにデータをセットして再生する
        notesData.NotesObjects[notesData.NotesCallCount].Mode = NotesMode.Single;
        notesData.NotesObjects[notesData.NotesCallCount].SetSingleNotes(type, startPos, endPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), moveNotesSprites[(int)type], endNotesSprites[(int)type], notesSpriteAlpha);
        notesData.NotesCallCount++;

        _ = id == ControllerNum.P1 ? dataBase1.NotesCallCount = notesData.NotesCallCount : dataBase2.NotesCallCount = notesData.NotesCallCount;
    }

    /// <summary>
    /// ダブルノーツを1回再生する
    /// </summary>
    /// <param name="type1">再生するノーツのタイプ１</param>
    /// <param name="type2">再生するノーツのタイプ２</param>
    /// <param name="startPos">ノーツの再生開始座標</param>
    /// <param name="endPos">ノーツの判定座標</param>
    /// <param name="id">入力対象のコントローラ番号</param>
    /// <param name="duration">再生開始位置から判定位置まで移動するのにかかる時間[s]</param>
    public void PlayDoubleNotesOneShot(NotesType type1, NotesType type2, Vector3 startPos, Vector3 endPos, ControllerNum id, float duration = 1.0f)
    {
        if(startPos == endPos) { return; }
        NotesDataBase notesData = id == ControllerNum.P1 ? dataBase1 : dataBase2;

        // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
        if (notesData.NotesObjects[notesData.NotesCallCount].NotesCoroutine != null) { return; }

        // ノーツにデータをセットして再生する
        notesData.NotesObjects[notesData.NotesCallCount].Mode = NotesMode.Double;
        notesData.NotesObjects[notesData.NotesCallCount].SetDoubleNotes(type1, type2, startPos, endPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), moveNotesSprites[(int)type1], moveNotesSprites[(int)type2], endNotesSprites[(int)type1], endNotesSprites[(int)type2], notesSpriteAlpha);
        notesData.NotesCallCount++;

        _ = id == ControllerNum.P1 ? dataBase1.NotesCallCount = notesData.NotesCallCount : dataBase2.NotesCallCount = notesData.NotesCallCount;
    }

    /// <summary>
    /// 全てのノーツの再生を止める
    /// </summary>
    public void StopAllNotes()
    {
        for (int i = 0; i < maxNotes; i++)
        {
            dataBase1.NotesObjects[i].NotesEmergencyStop();
            dataBase2.NotesObjects[i].NotesEmergencyStop();
        }
        dataBase1.NotesCallCount = 0;
        dataBase1.NotesCheckCount = 0;
        dataBase2.NotesCallCount = 0;
        dataBase2.NotesCheckCount = 0;
    }

    /// <summary>
    /// ノーツの入力処理
    /// </summary>
    /// <param name="id">コントローラ番号</param>
    private void InputNotesAction(ControllerNum id)
    {
        NotesView nowNotes;
        NotesView nextNotes;

        if(id == ControllerNum.P1)
        {
            nowNotes = dataBase1.NotesObjects[dataBase1.NotesCheckCount];
            nextNotes = dataBase1.NotesObjects[dataBase1.NotesCheckCount + 1 >= dataBase1.NotesObjects.Length ? 0 : dataBase1.NotesCheckCount + 1];
        }
        else
        {
            nowNotes = dataBase2.NotesObjects[dataBase2.NotesCheckCount];
            nextNotes = dataBase2.NotesObjects[dataBase2.NotesCheckCount + 1 >= dataBase2.NotesObjects.Length ? 0 : dataBase2.NotesCheckCount + 1];
        }

        if(nowNotes.NotesCoroutine == null) { return; }

        if (nowNotes.Mode == NotesMode.Single)    // ノーツがシングルモードの時の入力
        {
            if (id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Circle == true : GamePadControl.Instance.GetKeyDown_2.Circle == true)
            {
                NotesCheck(nowNotes, 0, id);
                return;
            }

            if (id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Cross == true : GamePadControl.Instance.GetKeyDown_2.Cross == true)
            {
                NotesCheck(nowNotes, 1, id);
                return;
            }

            if (id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Triangle == true : GamePadControl.Instance.GetKeyDown_2.Triangle == true)
            {
                NotesCheck(nowNotes, 2, id);
                return;
            }

            if (id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Up == true : GamePadControl.Instance.GetKeyDown_2.Up == true)
            {
                NotesCheck(nowNotes, 3, id);
                return;
            }

            if (id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Down == true : GamePadControl.Instance.GetKeyDown_2.Down == true)
            {
                NotesCheck(nowNotes, 4, id);
                return;
            }

            if (id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Left == true : GamePadControl.Instance.GetKeyDown_2.Left == true)
            {
                NotesCheck(nowNotes, 5, id);
                return;
            }
        }
        else
        {
            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 0, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 1, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 2, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 3, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 4, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 5, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 6, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 6, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 7, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 8, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 9, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 10, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 11, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 12, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 13, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            if ((GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == true && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == true) && (id == ControllerNum.P1 ? (clickFlag1 == true) : (clickFlag2 == true)))
            {
                NotesCheck(nowNotes, 14, id);
                _ = id == ControllerNum.P1 ? clickFlag1 = false : clickFlag2 = false;
                return;
            }

            // キー入力が検知されなかったら入力を許可
            if (GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Circle) == false && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Cross) == false && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Triangle) == false && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Up) == false && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Down) == false && GamePadControl.Instance.GetDS4Key(id, DS4AllKeyType.Left) == false && (id == ControllerNum.P1 ? (clickFlag1 == false) : (clickFlag2 == false)))
            {
                _ = id == ControllerNum.P1 ? clickFlag1 = true : clickFlag2 = true;
                return;
            }
        }

        if ((nextNotes.NotesCoroutine != null && Mathf.Abs(0.5f - nowNotes.NotesRate) > Mathf.Abs(0.5f - nextNotes.NotesRate)) || nowNotes.NotesRate > nowNotes.MaxGood)
        {
            NotesResult(0, 0, id);
            return;
        }
    }

    /// <summary>
    /// ノーツの判定処理
    /// </summary>
    /// <param name="view">判定をチェックしたいノーツ</param>
    /// <param name="type">入力されたノーツタイプID</param>
    /// <param name="id">プレイヤー番号</param>
    private void NotesCheck(NotesView view, int type, ControllerNum id)
    {
        // ノーツの進行率をチェック
        var rate = view.NotesRate;
        if (rate < view.MinBad) { return; }

        // ノーツの判定をチェック
        int result;
        int score;
        if (type == view.NotesTypeNum)
        {
            if (rate >= view.MinPerfect && rate <= view.MaxPerfect)
            {
                result = 2;
                score = 800;
            }
            else if ((rate >= view.MinGood && rate < view.MinPerfect) || (rate <= view.MaxGood && rate > view.MaxPerfect))
            {
                result = 1;
                score = 400;
            }
            else
            {
                result = 0;
                score = 0;
            }
        }
        else
        {
            result = 0;
            score = 0;
        }

        // ノーツを非表示
        view.InputNotes(result);

        // 結果を算出
        NotesResult(result, score, id);
    }

    /// <summary>
    /// ノーツの判定結果
    /// </summary>
    /// <param name="result">ノーツの判定値</param>
    /// <param name="score">獲得スコア</param>
    /// <param name="id">プレイヤー番号</param>
    private void NotesResult(int result, int score, ControllerNum id)
    {
        _ = id == ControllerNum.P1 ? dataBase1.NotesCheckCount++ : dataBase2.NotesCheckCount++;

        switch (result)
        {
            case 0:
                GameData.Instance.PlusNotesResult(id, 0);
                break;
            case 1:
                GameData.Instance.PlusNotesResult(id, 1);
                break;
            case 2:
                GameData.Instance.PlusNotesResult(id, 2);
                break;
            default:
                return;
        }
        
        GameData.Instance.PlusTotalScore(id, score);
    }

    /// <summary>
    /// ノーツが再生中ならtrue, 停止中ならfalse
    /// </summary>
    /// <param name="id">チェック対象のプレイヤー番号</param>
    /// <returns></returns>
    public bool NotesIsPlaying(ControllerNum id)
    {
        NotesDataBase data = id == ControllerNum.P1 ? dataBase1 : dataBase2;
        for(int i = 0; i < data.NotesObjects.Length; i++)
        {
            if(data.NotesObjects[i].NotesCoroutine != null)
            {
                return true;
            }
        }
        return false;
    }
}
