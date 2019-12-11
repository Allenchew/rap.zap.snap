﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesControl : SingletonMonoBehaviour<NotesControl>
{
    // ノーツの最大生成数
    [SerializeField, Tooltip("ノーツの最大生成数"), Range(1, 20)]
    private int maxNotes = 5;

    // ノーツのプレファブオブジェクト
    [SerializeField, Tooltip("ノーツのPrefab")]
    private GameObject notesPrefab = null;

    [SerializeField, Tooltip("ノーツのSpriteイメージデータ")]
    private Sprite[] notesSprites = null;

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

    [SerializeField, Tooltip("ノーツ判定距離:Perfect"), Range(0.0001f, 0.05f), Header("ノーツの判定域")]
    private float perfectLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Good"), Range(0.0001f, 0.05f)]
    private float goodLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Bad"), Range(0.0001f, 0.05f)]
    private float badLength = 0.05f;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

        for(int i = 0; i < maxNotes; i++)
        {
            if(notesData.NotesObjects[i] == null)
            {
                GameObject obj = Instantiate(notesPrefab, gameObject.transform, false);
                obj.SetActive(false);
                notesData.NotesObjects[i] = obj.GetComponent<NotesView>();
                notesData.NotesObjects[i].MoveNotesObj = obj.transform.GetChild(1).gameObject;
                notesData.NotesObjects[i].GoalNotesObj = obj.transform.GetChild(0).gameObject;
            }
        }

        _ = id == ControllerNum.P1 ? dataBase1 = notesData : dataBase2 = notesData;
    }

    /// <summary>
    /// ノーツを1回再生する処理
    /// </summary>
    /// <param name="type">再生するノーツのタイプ 
    /// <para>Example: NotesType.CircleKey → 〇ボタンノーツ</para>
    /// </param>
    /// <param name="startPos">ノーツの再生開始座標</param>
    /// <param name="endPos">ノーツの判定座標</param>
    /// <param name="id">入力対象のコントローラ番号</param>
    /// <param name="duration">再生開始位置から判定位置まで移動するのにかかる時間[s]</param>
    public void PlayNotesOneShot(NotesType type, Vector3 startPos, Vector3 endPos, ControllerNum id, float duration = 1.0f)
    {
        if (startPos == endPos) { return; }

        NotesDataBase notesData = id == ControllerNum.P1 ? dataBase1 : dataBase2;

        // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
        if (notesData.NotesObjects[notesData.NotesCallCount].gameObject.activeSelf == true) { return; }

        // ノーツにデータをセットして再生する
        notesData.NotesObjects[notesData.NotesCallCount].SetNotesData(type, startPos, endPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), notesSprites[(int)type], notesSpriteAlpha);
        notesData.NotesCallCount++;

        _ = id == ControllerNum.P1 ? dataBase1.NotesCallCount = notesData.NotesCallCount : dataBase2.NotesCallCount = notesData.NotesCallCount;
    }

    /// <summary>
    /// 全てのノーツの再生を止める
    /// </summary>
    public void StopNotes()
    {
        for(int i = 0; i < maxNotes; i++)
        {
            if(dataBase1.NotesObjects[i].gameObject.activeSelf == true)
            {
                dataBase1.NotesObjects[i].ResetNotes();
            }

            if(dataBase2.NotesObjects[i].gameObject.activeSelf == true)
            {
                dataBase2.NotesObjects[i].ResetNotes();
            }
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
        int nextNotesNum;
        NotesView nextNotes;

        if(id == ControllerNum.P1)
        {
            nowNotes = dataBase1.NotesObjects[dataBase1.NotesCheckCount];
            nextNotesNum = dataBase1.NotesCheckCount + 1 >= dataBase1.NotesObjects.Length ? 0 : dataBase1.NotesCheckCount + 1;
            nextNotes = dataBase1.NotesObjects[nextNotesNum];
        }
        else
        {
            nowNotes = dataBase2.NotesObjects[dataBase2.NotesCheckCount];
            nextNotesNum = dataBase2.NotesCheckCount + 1 >= dataBase2.NotesObjects.Length ? 0 : dataBase2.NotesCheckCount + 1;
            nextNotes = dataBase2.NotesObjects[nextNotesNum];
        }

        if(nowNotes.gameObject.activeSelf == false) { return; }

        DS4InputKey input = id == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1 : GamePadControl.Instance.GetKeyDown_2;

        if(input.Circle == true || (_ = id == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.A) == true : Input.GetKeyDown(KeyCode.J) == true))
        {
            NotesCheck(nowNotes, NotesType.CircleKey, id);
            return;
        }

        if (input.Cross == true || (_ = id == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.S) == true : Input.GetKeyDown(KeyCode.K) == true))
        {
            NotesCheck(nowNotes, NotesType.CrossKey, id);
            return;
        }

        if (input.Triangle == true || (_ = id == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.D) == true : Input.GetKeyDown(KeyCode.L) == true))
        {
            NotesCheck(nowNotes, NotesType.TriangleKey, id);
            return;
        }

        if (input.Up == true || (_ = id == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.UpArrow) == true : Input.GetKeyDown(KeyCode.Keypad8) == true))
        {
            NotesCheck(nowNotes, NotesType.UpArrow, id);
            return;
        }

        if (input.Down == true || (_ = id == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.DownArrow) == true : Input.GetKeyDown(KeyCode.Keypad2) == true))
        {
            NotesCheck(nowNotes, NotesType.DownArrow, id);
            return;
        }

        if (input.Left == true || (_ = id == ControllerNum.P1 ? Input.GetKeyDown(KeyCode.LeftArrow) == true : Input.GetKeyDown(KeyCode.Keypad4) == true))
        {
            NotesCheck(nowNotes, NotesType.LeftArrow, id);
            return;
        }

        if ((nowNotes.NotesRate >= nowNotes.MaxGood) || (nextNotes.gameObject.activeSelf && Mathf.Abs(0.5f - nowNotes.NotesRate) > Mathf.Abs(0.5f - nextNotes.NotesRate)))
        {
            nowNotes.SecondMoveSet();
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
    private void NotesCheck(NotesView view,  NotesType type, ControllerNum id)
    {
        // ノーツの進行率をチェック
        var rate = view.NotesRate;
        if(rate < view.MinBad) { return; }

        // ノーツを非表示
        view.ResetNotes();

        // ノーツの判定をチェック
        int result;
        int score;
        if (type == view.NotesTypes)
        {
            if(rate >= view.MinPerfect && rate <= view.MaxPerfect)
            {
                result = 2;
                score = 800;
            }
            else if((rate >= view.MinGood && rate < view.MinPerfect) || (rate <= view.MaxGood && rate > view.MaxPerfect))
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
        NotesDataBase notesData = id == ControllerNum.P1 ? dataBase1 : dataBase2;

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

        notesData.NotesCheckCount++;
        GameData.Instance.PlusTotalScore(id, score);

        _ = id == ControllerNum.P1 ? dataBase1 = notesData : dataBase2 = notesData;

        // 歌詞を流す処理（予定）

    }
}
