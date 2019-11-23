using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesControl : MonoBehaviour
{
    public static NotesControl Instance { get; private set; } = null;

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

        // Perfect, Good, Badそれぞれの総数
        public int Perfect, Good, Bad, TotalScore;

        // 初期化
        public void ResetDataBase()
        {
            notesCallCount = 0;
            notesCheckCount = 0;
            Perfect = 0;
            Good = 0;
            Bad = 0;
            TotalScore = 0;
        }
    }
    private NotesDataBase dataBase1;
    private NotesDataBase dataBase2;

    [SerializeField, Tooltip("生成されるノーツのサイズ")]
    private float notesSize = 1.0f;

    [SerializeField, Tooltip("判定ノーツの透明度"), Range(0, 1.0f)]
    private float notesSpriteAlpha = 0.5f;

    [SerializeField, Tooltip("ノーツ判定距離:Perfect"), Range(0.0001f, 0.05f)]
    private float perfectLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Good"), Range(0.0001f, 0.05f)]
    private float goodLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Bad"), Range(0.0001f, 0.05f)]
    private float badLength = 0.05f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dataBase1.ResetDataBase();
            dataBase2.ResetDataBase();
            CreateNotes();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputNotesAction1();
        InputNotesAction2();
    }

    /// <summary>
    /// ノーツを必要数生成する
    /// </summary>
    private void CreateNotes()
    {
        if (notesPrefab == null)
        {
            Debug.LogError("プレファブが設定されていません");
            return;
        }

        // ノーツのプールを初期化
        dataBase1.NotesObjects = new NotesView[maxNotes];
        dataBase2.NotesObjects = new NotesView[maxNotes];

        // ノーツのプールを作成し、格納する
        for (int i = 0; i < maxNotes; i++)
        {
            GameObject obj;
            if (dataBase1.NotesObjects[i] == null)
            {
                obj = Instantiate(notesPrefab, gameObject.transform, false);
                obj.SetActive(false);
                dataBase1.NotesObjects[i] = obj.GetComponent<NotesView>();
                dataBase1.NotesObjects[i].MoveNotesObj = obj.transform.GetChild(1).gameObject;
                dataBase1.NotesObjects[i].GoalNotesObj = obj.transform.GetChild(0).gameObject;
            }
            if (dataBase2.NotesObjects[i] == null)
            {
                obj = Instantiate(notesPrefab, gameObject.transform, false);
                obj.SetActive(false);
                dataBase2.NotesObjects[i] = obj.GetComponent<NotesView>();
                dataBase2.NotesObjects[i].MoveNotesObj = obj.transform.GetChild(1).gameObject;
                dataBase2.NotesObjects[i].GoalNotesObj = obj.transform.GetChild(0).gameObject;
            }
        }
    }

    /// <summary>
    /// ノーツを1回再生する処理
    /// </summary>
    /// <param name="type">再生するノーツのタイプ 
    /// <para>Example: NotesType.CircleKey → 〇ボタンノーツ</para>
    /// <para>int型で宣言しても可  0:〇ボタン 1:×ボタン 2:△ボタン 3:↑ボタン 4:↓ボタン 5:←ボタン</para>
    /// </param>
    /// <param name="startPos">ノーツの再生開始座標</param>
    /// <param name="endPos">ノーツの判定座標</param>
    /// <param name="player">このノーツを入力できるプレイヤー</param>
    /// <param name="duration">再生開始位置から判定位置まで移動するのにかかる時間[s]</param>
    public void PlayNotesOneShot(NotesType type, Vector3 startPos, Vector3 endPos, InputPlayer player = InputPlayer.P1, float duration = 1.0f)
    {
        if (startPos == endPos) return;

        Sprite sprite = notesSprites[(int)type];

        if (player == InputPlayer.P1)
        {
            // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
            if (dataBase1.NotesObjects[dataBase1.NotesCallCount].gameObject.activeSelf) return;

            // 第1ノーツプールからノーツを再生
            dataBase1.NotesObjects[dataBase1.NotesCallCount].SetNotesData(type, startPos, endPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), sprite, notesSpriteAlpha);
            dataBase1.NotesCallCount++;
        }
        else
        {
            // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
            if (dataBase2.NotesObjects[dataBase2.NotesCallCount].gameObject.activeSelf) return;

            // 第2ノーツプールからノーツを再生
            dataBase2.NotesObjects[dataBase2.NotesCallCount].SetNotesData(type, startPos, endPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), sprite, notesSpriteAlpha);
            dataBase2.NotesCallCount++;
        }
    }

    /// <summary>
    /// プレイヤー１のノーツ入力判定処理
    /// </summary>
    private void InputNotesAction1()
    {
        var nowNotes1 = dataBase1.NotesObjects[dataBase1.NotesCheckCount];
        int nextNotesNum1 = dataBase1.NotesCheckCount + 1 >= dataBase1.NotesObjects.Length ? 0 : dataBase1.NotesCheckCount + 1;
        var nextNotes1 = dataBase1.NotesObjects[nextNotesNum1];

        if (nowNotes1.gameObject.activeSelf)
        {
            var inputPad = GamePadControl.Instance.Controller1;
            if (inputPad.Circle || Input.GetKeyDown(KeyCode.A))
            {
                NotesCheck(nowNotes1, NotesType.CircleKey, InputPlayer.P1);
                return;
            }
            else if (inputPad.Cross || Input.GetKeyDown(KeyCode.S))
            {
                NotesCheck(nowNotes1, NotesType.CrossKey, InputPlayer.P1);
                return;
            }
            else if (inputPad.Triangle || Input.GetKeyDown(KeyCode.D))
            {
                NotesCheck(nowNotes1, NotesType.TriangleKey, InputPlayer.P1);
                return;
            }
            else if (inputPad.UpKey || Input.GetKeyDown(KeyCode.UpArrow))
            {
                NotesCheck(nowNotes1, NotesType.UpArrow, InputPlayer.P1);
                return;
            }
            else if (inputPad.DownKey || Input.GetKeyDown(KeyCode.DownArrow))
            {
                NotesCheck(nowNotes1, NotesType.DownArrow, InputPlayer.P1);
                return;
            }
            else if (inputPad.LeftKey || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                NotesCheck(nowNotes1, NotesType.LeftArrow, InputPlayer.P1);
                return;
            }
            else
            {
                if ((nowNotes1.NotesRate >= nowNotes1.MaxGood) || (nextNotes1.gameObject.activeSelf && Mathf.Abs(0.5f - nowNotes1.NotesRate) > Mathf.Abs(0.5f - nextNotes1.NotesRate)))
                {
                    nowNotes1.SecondMoveSet();
                    NotesResult(0, 0, InputPlayer.P1);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// プレイヤー２のノーツ入力判定処理
    /// </summary>
    private void InputNotesAction2()
    {
        var nowNotes2 = dataBase2.NotesObjects[dataBase2.NotesCheckCount];
        int nextNotesNum2 = dataBase2.NotesCheckCount + 1 >= dataBase2.NotesObjects.Length ? 0 : dataBase2.NotesCheckCount + 1;
        var nextNotes2 = dataBase2.NotesObjects[nextNotesNum2];

        if (nowNotes2.gameObject.activeSelf)
        {
            var inputPad = GamePadControl.Instance.Controller2;
            if (inputPad.Circle || Input.GetKeyDown(KeyCode.J))
            {
                NotesCheck(nowNotes2, NotesType.CircleKey, InputPlayer.P2);
                return;
            }
            else if (inputPad.Cross || Input.GetKeyDown(KeyCode.K))
            {
                NotesCheck(nowNotes2, NotesType.CrossKey, InputPlayer.P2);
                return;
            }
            else if (inputPad.Triangle || Input.GetKeyDown(KeyCode.L))
            {
                NotesCheck(nowNotes2, NotesType.TriangleKey, InputPlayer.P2);
                return;
            }
            else if (inputPad.UpKey || Input.GetKeyDown(KeyCode.Keypad8))
            {
                NotesCheck(nowNotes2, NotesType.UpArrow, InputPlayer.P2);
                return;
            }
            else if (inputPad.DownKey || Input.GetKeyDown(KeyCode.Keypad2))
            {
                NotesCheck(nowNotes2, NotesType.DownArrow, InputPlayer.P2);
                return;
            }
            else if (inputPad.LeftKey || Input.GetKeyDown(KeyCode.Keypad4))
            {
                NotesCheck(nowNotes2, NotesType.LeftArrow, InputPlayer.P2);
                return;
            }
            else
            {
                if ((nowNotes2.NotesRate >= nowNotes2.MaxGood) || (nextNotes2.gameObject.activeSelf && Mathf.Abs(0.5f - nowNotes2.NotesRate) > Mathf.Abs(0.5f - nextNotes2.NotesRate)))
                {
                    nowNotes2.SecondMoveSet();
                    NotesResult(0, 0, InputPlayer.P2);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// ノーツの判定処理
    /// </summary>
    private void NotesCheck(NotesView view,  NotesType type, InputPlayer player = InputPlayer.P1)
    {
        // ノーツの進行率をチェック
        var rate = view.NotesRate;
        if(rate < view.MinBad || !view.NotesClickFlag) { return; }

        // ノーツの判定をチェック
        int result;
        int score;
        if (type == view.NotesTypes)
        {
            if(rate >= view.MinPerfect && rate <= view.MaxPerfect)
            {
                result = 2;
                score = 200;
            }
            else if((rate >= view.MinGood && rate < view.MinPerfect) || (rate >= view.MaxGood && rate < view.MaxPerfect))
            {
                result = 1;
                score = 100;
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
        NotesResult(result, score, player);

        // ノーツを非表示
        view.ResetNotes();
    }

    /// <summary>
    /// ノーツの判定結果
    /// </summary>
    /// <param name="result">ノーツの判定値</param>
    /// <param name="score">獲得スコア</param>
    /// <param name="player">プレイヤー番号</param>
    private void NotesResult(int result, int score, InputPlayer player = InputPlayer.P1)
    {
        if (player == InputPlayer.P1)
        {
            dataBase1.NotesCheckCount++;
            dataBase1.TotalScore += score;
            switch (result)
            {
                case 0:
                    dataBase1.Bad++;
                    break;
                case 1:
                    dataBase1.Good++;
                    break;
                case 2:
                    dataBase1.Perfect++;
                    break;
            }
        }
        else
        {
            dataBase2.NotesCheckCount++;
            dataBase2.TotalScore += score;
            switch (result)
            {
                case 0:
                    dataBase2.Bad++;
                    break;
                case 1:
                    dataBase2.Good++;
                    break;
                case 2:
                    dataBase2.Perfect++;
                    break;
            }
        }

        // 歌詞を流す処理（予定）

    }

    /// <summary>
    /// ノーツの結果を取得する
    /// </summary>
    /// <param name="resultNum">0:Badの数 1:Goodの数 2:Perfectの数 3:トータルスコア 0～3以外:返り値0</param>
    /// <param name="playerNum">結果を取得する対象プレイヤー</param>
    /// <returns></returns>
    public int GetResult(int resultNum, InputPlayer playerNum = InputPlayer.P1)
    {
        if (resultNum < 0 || resultNum > 3) return 0;

        NotesDataBase data = playerNum == InputPlayer.P1 ? dataBase1 : dataBase2;
        
        switch (resultNum)
        {
            case 0:
                return data.Bad;
            case 1:
                return data.Good;
            case 2:
                return data.Perfect;
            case 3:
                return data.TotalScore;
            default:
                return 0;
        }
    }

    /// <summary>
    /// ノーツのリザルトを初期化する
    /// </summary>
    /// <param name="playerNum">初期化対象プレイヤー</param>
    public void ResetResult(InputPlayer playerNum = InputPlayer.P1)
    {
        if (playerNum == InputPlayer.P1)
        {
            dataBase1.Perfect = 0;
            dataBase1.Good = 0;
            dataBase1.Bad = 0;
            dataBase1.TotalScore = 0;
        }
        else
        {
            dataBase2.Perfect = 0;
            dataBase2.Good = 0;
            dataBase2.Bad = 0;
            dataBase2.TotalScore = 0;
        }
    }
}
