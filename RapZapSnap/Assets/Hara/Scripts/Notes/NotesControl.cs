using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4;

public class NotesControl : MonoBehaviour
{
    public static NotesControl Instance { get; private set; } = null;

    [SerializeField, Tooltip("ノーツをUIとして生成するか")]
    private bool notesUIMode = true;

    // ノーツの最大生成数
    [SerializeField, Tooltip("ノーツの最大生成数"), Range(1, 20)]
    private int maxNotes = 5;

    // ノーツのプレファブオブジェクト
    [SerializeField, Tooltip("ノーツのPrefab")]
    private GameObject notesPrefab = null;
    [SerializeField, Tooltip("UIノーツのPrefab")]
    private GameObject notesUIPrefab = null;
    [SerializeField, Tooltip("UIノーツ用のCanvas")]
    private GameObject notesUICanvasPrefab = null;

    private Canvas canvasObj = null;

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
        public int Perfect, Good, Bad;

        // 初期化
        public void ResetDataBase()
        {
            notesCallCount = 0;
            notesCheckCount = 0;
            Perfect = 0;
            Good = 0;
            Bad = 0;
        }
    }
    private NotesDataBase dataBase1;
    private NotesDataBase dataBase2;

    // ノーツの準備が完了したかをチェックするフラグ
    private bool notesStartRady = false;

    [SerializeField, Tooltip("生成されるノーツのサイズ")]
    private float notesSize = 1.0f;

    [SerializeField, Tooltip("判定ノーツの透明度"), Range(0, 1.0f)]
    private float notesSpriteAlpha = 0.5f;

    [SerializeField, Tooltip("ノーツ判定距離:Perfect"), Range(0, 0.1f)]
    private float perfectLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Good"), Range(0, 0.1f)]
    private float goodLength = 0.05f;

    [SerializeField, Tooltip("ノーツの判定距離:Bad"), Range(0, 0.1f)]
    private float badLength = 0.05f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dataBase1.ResetDataBase();
            dataBase2.ResetDataBase();
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
        CreateNotes();
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
        if ((notesPrefab == null && !notesUIMode) || (notesUIPrefab == null && notesUIMode) || (notesUICanvasPrefab == null && notesUIMode))
        {
            Debug.LogError("プレファブが設定されていません");
            return;
        }

        // ノーツのプールを初期化
        dataBase1.NotesObjects = new NotesView[maxNotes];
        dataBase2.NotesObjects = new NotesView[maxNotes];

        // UIノーツ用のCanvasがあるかチェック
        if(canvasObj == null && notesUIMode)
        {
            canvasObj = Instantiate(notesUICanvasPrefab).GetComponent<Canvas>();
            DontDestroyOnLoad(canvasObj);
        }

        // ノーツのプールを作成し、格納する
        for (int i = 0; i < maxNotes; i++)
        {
            GameObject obj;
            if (dataBase1.NotesObjects[i] == null)
            {
                obj = notesUIMode ? Instantiate(notesUIPrefab, canvasObj.transform, false) : Instantiate(notesPrefab, gameObject.transform, false);
                obj.SetActive(false);
                dataBase1.NotesObjects[i] = obj.GetComponent<NotesView>();
                if (notesUIMode)
                {
                    dataBase1.NotesObjects[i].NotesUIMode = true;
                }
                else
                {
                    dataBase1.NotesObjects[i].NotesUIMode = false;
                }
                dataBase1.NotesObjects[i].MoveNotesObj = obj.transform.GetChild(1).gameObject;
                dataBase1.NotesObjects[i].GoalNotesObj = obj.transform.GetChild(0).gameObject;
            }
            if (dataBase2.NotesObjects[i] == null)
            {
                obj = notesUIMode ? Instantiate(notesUIPrefab, canvasObj.transform, false) : Instantiate(notesPrefab, gameObject.transform, false);
                obj.SetActive(false);
                dataBase2.NotesObjects[i] = obj.GetComponent<NotesView>();
                if (notesUIMode)
                {
                    dataBase2.NotesObjects[i].NotesUIMode = true;
                }
                else
                {
                    dataBase2.NotesObjects[i].NotesUIMode = false;
                }
                dataBase2.NotesObjects[i].MoveNotesObj = obj.transform.GetChild(1).gameObject;
                dataBase2.NotesObjects[i].GoalNotesObj = obj.transform.GetChild(0).gameObject;
            }
        }
        notesStartRady = true;
    }

    /// <summary>
    /// ノーツを再生する処理
    /// </summary>
    /// <param name="notesType">再生するノーツのタイプ <para>Example: NotesType.CircleKey → 〇ボタンノーツ</para></param>
    /// <param name="startPos">ノーツの再生開始座標</param>
    /// <param name="goalPos">ノーツの判定座標</param>
    /// <param name="input">このノーツを入力できるプレイヤー</param>
    /// <param name="duration">再生開始位置から判定位置まで移動するのにかかる時間[s]</param>
    public void CallNotes(NotesType notesType, Vector3 startPos, Vector3 goalPos, InputController input = InputController.PlayerOne, float duration = 1.0f)
    {
        // ノーツの生成準備が完了していなければ処理を終了
        if (!notesStartRady) return;

        if (input == InputController.PlayerOne)
        {
            // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
            if (dataBase1.NotesObjects[dataBase1.NotesCallCount].gameObject.activeSelf) return;

            // 第1ノーツプールからノーツを再生
            dataBase1.NotesObjects[dataBase1.NotesCallCount].SetNotesData(notesType, startPos, goalPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), notesSpriteAlpha);
            dataBase1.NotesCallCount++;
        }
        else
        {
            // 呼び出そうとしたノーツがすでに稼働中なら処理を終了
            if (dataBase2.NotesObjects[dataBase2.NotesCallCount].gameObject.activeSelf) return;

            // 第2ノーツプールからノーツを再生
            dataBase2.NotesObjects[dataBase2.NotesCallCount].SetNotesData(notesType, startPos, goalPos, duration, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), notesSpriteAlpha);
            dataBase2.NotesCallCount++;
        }
    }

    /// <summary>
    /// 指定した間隔で指定した数だけノーツを再生する処理
    /// </summary>
    /// <param name="start">ノーツの再生開始座標</param>
    /// <param name="end">ノーツの判定座標</param>
    /// <param name="time">ノーツを呼び出す回数</param>
    /// <param name="span">ノーツを呼び出す間隔[s]</param>
    /// <param name="player">ノーツの対象プレイヤー</param>
    /// <param name="duration">ノーツの到達所要時間</param>
    public void CallNotes(Vector3 start, Vector3 end, int time, float span, InputController player = InputController.PlayerOne, float duration = 1.0f)
    {
        ResetResult(player);
        StartCoroutine(CallTimeSpan(time, span, start, end, player, duration));
    }

    private IEnumerator CallTimeSpan(int time, float span, Vector3 start, Vector3 end, InputController player = InputController.PlayerOne, float duration = 1.0f)
    {
        int count = 0;
        while (count < time)
        {
            yield return new WaitForSeconds(span);
            CallNotes((NotesType)Random.Range(0, 6), start, end, player, duration);
            count++;
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
            if (inputPad.Circle)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.CircleKey), InputController.PlayerOne);
                return;
            }
            else if (inputPad.Cross)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.CrossKey), InputController.PlayerOne);
                return;
            }
            else if (inputPad.Triangle)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.TriangleKey), InputController.PlayerOne);
                return;
            }
            else if (inputPad.UpKey)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.UpArrow), InputController.PlayerOne);
                return;
            }
            else if (inputPad.DownKey)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.DownArrow), InputController.PlayerOne);
                return;
            }
            else if (inputPad.LeftKey)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.LeftArrow), InputController.PlayerOne);
                return;
            }
            else
            {
                if ((nowNotes1.NotesRate >= nowNotes1.MaxGood) || (nextNotes1.gameObject.activeSelf && Mathf.Abs(0.5f - nowNotes1.NotesRate) > Mathf.Abs(0.5f - nextNotes1.NotesRate)))
                {
                    nowNotes1.SecondMoveSet();
                    NotesResult();
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
            if (inputPad.Circle)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.CircleKey), InputController.PlayerTwo);
                return;
            }
            else if (inputPad.Cross)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.CrossKey), InputController.PlayerTwo);
                return;
            }
            else if (inputPad.Triangle)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.TriangleKey), InputController.PlayerTwo);
                return;
            }
            else if (inputPad.UpKey)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.UpArrow), InputController.PlayerTwo);
                return;
            }
            else if (inputPad.DownKey)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.DownArrow), InputController.PlayerTwo);
                return;
            }
            else if (inputPad.LeftKey)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.LeftArrow), InputController.PlayerTwo);
                return;
            }
            else
            {
                if ((nowNotes2.NotesRate >= nowNotes2.MaxGood) || (nextNotes2.gameObject.activeSelf && Mathf.Abs(0.5f - nowNotes2.NotesRate) > Mathf.Abs(0.5f - nextNotes2.NotesRate)))
                {
                    nowNotes2.SecondMoveSet();
                    NotesResult(input: InputController.PlayerTwo);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// ノーツの判定結果
    /// </summary>
    private void NotesResult(int resultNum = 0, InputController input = InputController.PlayerOne)
    {
        if (resultNum < 0 || resultNum > 2) return;

        bool inputFlag;
        if (input == InputController.PlayerOne)
        {
            dataBase1.NotesCheckCount++;
            inputFlag = true;
        }
        else
        {
            dataBase2.NotesCheckCount++;
            inputFlag = false;
        }

        switch (resultNum)
        {
            case 0:
                _ = inputFlag ? dataBase1.Bad++ : dataBase2.Bad++;
                break;
            case 1:
                _ = inputFlag ? dataBase1.Good++ : dataBase2.Good++;
                break;
            case 2:
                _ = inputFlag ? dataBase1.Perfect++ : dataBase2.Perfect++;
                break;
        }
        //Debug.Log(_ = inputFlag ? "プレイヤー１　　Perfect：" + dataBase1.Perfect + " Good：" + dataBase1.Good + " Bad：" + dataBase1.Bad : "プレイヤー２　　Perfect：" + dataBase2.Perfect + " Good：" + dataBase2.Good + " Bad：" + dataBase2.Bad);

        // 歌詞を流す処理（予定）

    }

    /// <summary>
    /// ノーツの結果を取得する
    /// </summary>
    /// <param name="resultNum">0:Badの数 1:Goodの数 2:Perfectの数 0～3以外:返り値0</param>
    /// <param name="playerNum">結果を取得する対象プレイヤー</param>
    /// <returns></returns>
    public int GetResult(int resultNum = 0, InputController playerNum = InputController.PlayerOne)
    {
        if (resultNum < 0 || resultNum > 2) return 0;

        NotesDataBase data = playerNum == InputController.PlayerOne ? dataBase1 : dataBase2;
        
        switch (resultNum)
        {
            case 0:
                return data.Bad;
            case 1:
                return data.Good;
            case 2:
                return data.Perfect;
            default:
                return 0;
        }
    }

    /// <summary>
    /// ノーツのリザルトを初期化する
    /// </summary>
    /// <param name="playerNum">初期化対象プレイヤー</param>
    public void ResetResult(InputController playerNum = InputController.PlayerOne)
    {
        if (playerNum == InputController.PlayerOne)
        {
            dataBase1.Perfect = 0;
            dataBase1.Good = 0;
            dataBase1.Bad = 0;
        }
        else
        {
            dataBase2.Perfect = 0;
            dataBase2.Good = 0;
            dataBase2.Bad = 0;
        }
    }
}
