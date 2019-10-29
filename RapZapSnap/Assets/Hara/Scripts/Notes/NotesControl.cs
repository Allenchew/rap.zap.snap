using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // ノーツチェック用のカウンター
    private int checkCount = 0;
    public int CheckCount { set { checkCount = value; if (checkCount >= notesViews.Length) checkCount = 0; } get { return checkCount; } }

    [SerializeField, Tooltip("生成されるノーツのサイズ")]
    private float notesSize = 1.0f;

    [SerializeField, Tooltip("判定ノーツの透明度"), Range(0, 1.0f)]
    private float notesSpriteAlpha = 0.5f;

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

    public void CallNotes(NotesModel.NotesType notesType, Vector3 startPos, Vector3 goalPos, float duration = 0.75f)
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
        // ノーツの調整用処理
        notesViews[callCount].transform.localScale = new Vector3(notesSize, notesSize, notesSize);
        notesViews[callCount].SpriteAlpha = notesSpriteAlpha;

        // ノーツを表示
        notesViews[callCount].gameObject.SetActive(true);

        // ノーツプール用のカウンターを加算
        callCount++;
        if(callCount >= notesViews.Length)
        {
            callCount = 0;
        }
    }

    public void CallNotes(NotesModel.NotesType notesType, Vector3 goalPos, float duration = 0.75f)
    {
        if (notesViews[callCount].gameObject.activeSelf)
        {
            return;
        }

        // 呼び出されるノーツに情報を与える
        notesViews[callCount].NotesTypes = notesType;
        notesViews[callCount].NotesDuration = duration;
        notesViews[callCount].GoalPos = goalPos;
        // ノーツの調整用処理
        notesViews[callCount].transform.localScale = new Vector3(notesSize, notesSize, notesSize);
        notesViews[callCount].SpriteAlpha = notesSpriteAlpha;

        // ノーツを表示
        notesViews[callCount].gameObject.SetActive(true);

        // ノーツプール用のカウンターを加算
        callCount++;
        if (callCount >= notesViews.Length)
        {
            callCount = 0;
        }
    }

    /// <summary>
    /// ノーツ用キー入力時の処理
    /// </summary>
    /// <param name="notesType"></param>
    public void InputKey(NotesModel.NotesType notesType)
    {
        if (notesViews[checkCount].gameObject.activeSelf)
        {
            notesViews[checkCount].NotesCheck(notesType);
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
