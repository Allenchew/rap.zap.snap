using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4;

public class NotesControl : MonoBehaviour
{
    public static NotesControl Instance { get; private set; } = null;
    
    // ノーツの最大生成数
    [SerializeField, Tooltip("ノーツの最大生成数"), Range(1, 20)]
    private int maxNotes = 5;

    // ノーツのプレファブオブジェクト
    [SerializeField, Tooltip("ノーツのPrefab")]
    private GameObject notesPrefab = null;
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
        if(Instance == null)
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
        InputNotesAction();
    }

    /// <summary>
    /// ノーツを必要数生成する
    /// </summary>
    private void CreateNotes()
    {
        if(notesPrefab == null)
        {
            Debug.LogError("ノーツのプレファブが設定されていません");
            return;
        }

        // ノーツのプールを初期化
        dataBase1.NotesObjects = new NotesView[maxNotes];
        dataBase2.NotesObjects = new NotesView[maxNotes];

        // ノーツのプールを作成し、格納する
        for(int i = 0; i < maxNotes; i++)
        {
            if(dataBase1.NotesObjects[i] == null)
            {
                var obj = Instantiate(notesPrefab);
                obj.SetActive(false);
                dataBase1.NotesObjects[i] = obj.GetComponent<NotesView>();
                obj.transform.SetParent(gameObject.transform);
            }
            if(dataBase2.NotesObjects[i] == null)
            {
                var obj = Instantiate(notesPrefab);
                obj.SetActive(false);
                dataBase2.NotesObjects[i] = obj.GetComponent<NotesView>();
                obj.transform.SetParent(gameObject.transform);
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

        if((input == InputController.PlayerOne && !GamePadControl.Instance.IsChangeController) || (input == InputController.PlayerTwo && GamePadControl.Instance.IsChangeController))
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
    /// ノーツ入力判定処理
    /// </summary>
    private void InputNotesAction()
    {
        var nowNotes1 = dataBase1.NotesObjects[dataBase1.NotesCheckCount];
        var nowNotes2 = dataBase2.NotesObjects[dataBase2.NotesCheckCount];
        int nextNotesNum1 = dataBase1.NotesCheckCount + 1 >= dataBase1.NotesObjects.Length ? 0 : dataBase1.NotesCheckCount + 1;
        int nextNotesNum2 = dataBase2.NotesCheckCount + 1 >= dataBase2.NotesObjects.Length ? 0 : dataBase2.NotesCheckCount + 1;
        var nextNotes1 = dataBase1.NotesObjects[nextNotesNum1];
        var nextNotes2 = dataBase2.NotesObjects[nextNotesNum2];

        if (nowNotes1.gameObject.activeSelf)
        {
            var inputPad = GamePadControl.Instance.IsChangeController ? GamePadControl.Instance.Controller2 : GamePadControl.Instance.Controller1;
            if (inputPad.Circle)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.CircleKey), InputController.PlayerOne);
            }
            else if (inputPad.Cross)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.CrossKey), InputController.PlayerOne);
            }
            else if (inputPad.Triangle)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.TriangleKey), InputController.PlayerOne);
            }
            else if (inputPad.UpKey)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.UpArrow), InputController.PlayerOne);
            }
            else if (inputPad.DownKey)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.DownArrow), InputController.PlayerOne);
            }
            else if (inputPad.LeftKey)
            {
                NotesResult(nowNotes1.NotesCheck(NotesType.LeftArrow), InputController.PlayerOne);
            }
            else if (!nowNotes1.NotesClickFlag)
            {
                NotesResult(0, InputController.PlayerOne);
            }
        }

        if (nowNotes2.gameObject.activeSelf)
        {
            var inputPad = GamePadControl.Instance.IsChangeController ? GamePadControl.Instance.Controller1 : GamePadControl.Instance.Controller2;
            if (inputPad.Circle)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.CircleKey), InputController.PlayerTwo);
            }
            else if (inputPad.Cross)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.CrossKey), InputController.PlayerTwo);
            }
            else if (inputPad.Triangle)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.TriangleKey), InputController.PlayerTwo);
            }
            else if (inputPad.UpKey)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.UpArrow), InputController.PlayerTwo);
            }
            else if (inputPad.DownKey)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.DownArrow), InputController.PlayerTwo);
            }
            else if (inputPad.LeftKey)
            {
                NotesResult(nowNotes2.NotesCheck(NotesType.LeftArrow), InputController.PlayerTwo);
            }
            else if(!nowNotes2.NotesClickFlag)
            {
                NotesResult(0, InputController.PlayerTwo);
            }
        }
    }

    /// <summary>
    /// ノーツの判定結果
    /// </summary>
    private void NotesResult(int resultNum, InputController input)
    {
        if((input == InputController.PlayerOne && !GamePadControl.Instance.IsChangeController) || (input == InputController.PlayerTwo && GamePadControl.Instance.IsChangeController))
        {
            dataBase1.NotesCheckCount++;
        }
        else
        {
            dataBase2.NotesCheckCount++;
        }

        switch (resultNum)
        {
            case 0:
                Debug.Log("BAD");
                break;
            case 1:
                Debug.Log("GOOD");
                break;
            case 2:
                Debug.Log("PERFECT");
                break;
            default:
                break;
        }

        // 歌詞を流す処理（予定）

    }
}
