using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesControl : MonoBehaviour
{
    // アクセス用の変数
    private static NotesControl notesControl = null;
    public static NotesControl Instance { get { return notesControl; } }

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

    [SerializeField, Tooltip("生成されるノーツのサイズ")]
    private float notesSize = 1.0f;

    [SerializeField, Tooltip("判定ノーツの透明度"), Range(0, 1.0f)]
    private float notesSpriteAlpha = 0.5f;

    [SerializeField, Tooltip("ノーツの移動開始位置のリスト")]
    private List<Vector3> startPos = new List<Vector3>();

    private void Awake()
    {
        if(notesControl == null)
        {
            notesControl = this;
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

    public void CallNotes(NotesModel.NotesType notesType, Vector3 endPosition, float duration = 0.5f)
    {
        // 呼び出されるノーツに情報を与える
        notesViews[callCount].transform.localScale = new Vector3(notesSize, notesSize, notesSize);
        notesViews[callCount].SpriteAlpha = notesSpriteAlpha;
        notesViews[callCount].NotesTypes = notesType;
        notesViews[callCount].NotesDuration = duration;
        notesViews[callCount].StartPos = startPos[Random.Range(0, startPos.Count)];
        notesViews[callCount].GoalPos = endPosition;

        // ノーツを表示
        notesViews[callCount].gameObject.SetActive(true);

        // ノーツプール用のカウンターを加算
        callCount++;
        if(callCount >= notesViews.Length)
        {
            callCount = 0;
        }
    }
}
