using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS4;

public class NotesControl : MonoBehaviour
{
    public static NotesControl Instance { get; private set; } = null;
    
    // ノーツの最大生成数
    [SerializeField, Tooltip("ノーツの最大生成数"), Range(1, 10)]
    private int maxNotes = 5;

    // ノーツのプレファブオブジェクト
    [SerializeField, Tooltip("ノーツのPrefab")]
    private GameObject notesPrefab = null;

    // ノーツのプールを用意して使いまわす
    private NotesView[] notesViews;

    // ノーツプール用のカウンター
    private int callCount = 0;

    // ノーツのキー入力用のカウンター
    private int notesCount = 0;
    public int NotesCount { set { notesCount = value; if (notesCount >= notesViews.Length) notesCount = 0; } get { return notesCount; } }

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
        InputPadKey();    // キーの入力を検知
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
        notesViews = new NotesView[maxNotes];

        // ノーツのプールを作成し、格納する
        for(int i = 0; i < maxNotes; i++)
        {
            if(notesViews[i] == null)
            {
                var obj = Instantiate(notesPrefab);
                obj.SetActive(false);
                notesViews[i] = obj.GetComponent<NotesView>();
                obj.transform.SetParent(gameObject.transform);
            }
        }
    }

    public void CallNotes(NotesType notesType, Vector3 startPos, Vector3 goalPos, MoveMode moveMode = MoveMode.Pass, float duration = 0.75f)
    {
        if (notesViews[callCount].gameObject.activeSelf)
        {
            return;
        }

        // 呼び出されるノーツに情報を与える
        notesViews[callCount].NotesTypes = notesType;
        notesViews[callCount].NotesDuration = duration;
        notesViews[callCount].StartPos = startPos;
        notesViews[callCount].GoalPos = goalPos;
        notesViews[callCount].NotesMoveMode = moveMode;

        // 判定距離の情報
        notesViews[callCount].Perfect = perfectLength;
        notesViews[callCount].Good = goodLength;
        notesViews[callCount].Bad = badLength;

        // ノーツの調整用処理
        notesViews[callCount].transform.localScale = new Vector3(notesSize, notesSize, notesSize);
        notesViews[callCount].GoalSpriteAlpha = notesSpriteAlpha;

        // ノーツを表示
        notesViews[callCount].gameObject.SetActive(true);

        // ノーツプール用のカウンターを加算
        callCount = callCount >= notesViews.Length - 1 ? 0 : callCount += 1;
    }
    
    private void InputPadKey()
    {
        if (!notesViews[notesCount].gameObject.activeSelf)
        {
            return;
        }

        NotesType notesType;

        if (GamePadControl.Instance.Controller.Circle)
        {
            notesType = NotesType.CircleKey;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            notesType = NotesType.CrossKey;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            notesType = NotesType.TriangleKey;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            notesType = NotesType.UpArrow;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            notesType = NotesType.DownArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            notesType = NotesType.LeftArrow;
        }
        else
        {
            return;
        }

        if (notesViews[notesCount].ActionStartCheck(notesType))
        {
            NotesCount++;
        }
    }

    /// <summary>
    /// ノーツの判定結果
    /// </summary>
    public void NotesResult(ResultType result)
    {
        switch (result)
        {
            case ResultType.Perfect:
                Debug.Log("PERFECT");
                break;
            case ResultType.Good:
                Debug.Log("GOOD");
                break;
            case ResultType.Bad:
                Debug.Log("BAD");
                break;
        }
    }

    public enum ResultType
    {
        Perfect,
        Good,
        Bad
    }
}
