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
    private NotesView[] firstPlayerNotes;
    private NotesView[] secondPlayerNotes;

    // ノーツの準備が完了したかをチェックするフラグ
    private bool notesStartRady = false;

    // ノーツプール用のカウンター
    private int firstPlayerCallCount = 0;
    private int secondPlayerCallCount = 0;

    // ノーツのキー入力用のカウンター
    private int firstPlayerNotesCount = 0;
    public int FirstPlayerNotesCount { set { firstPlayerNotesCount = value; if (firstPlayerNotesCount >= firstPlayerNotes.Length) firstPlayerNotesCount = 0; } get { return firstPlayerNotesCount; } }
    private int secondPlayerNotesCount = 0;
    public int SecondPlayerNotesCount { set { secondPlayerNotesCount = value; if (secondPlayerNotesCount >= secondPlayerNotes.Length) secondPlayerNotesCount = 0; } get { return secondPlayerNotesCount; } }


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
        firstPlayerNotes = new NotesView[maxNotes];
        secondPlayerNotes = new NotesView[maxNotes];

        // ノーツのプールを作成し、格納する
        for(int i = 0; i < maxNotes; i++)
        {
            if(firstPlayerNotes[i] == null)
            {
                var obj = Instantiate(notesPrefab);
                obj.SetActive(false);
                firstPlayerNotes[i] = obj.GetComponent<NotesView>();
                obj.transform.SetParent(gameObject.transform);
            }
            if(secondPlayerNotes[i] == null)
            {
                var obj = Instantiate(notesPrefab);
                obj.SetActive(false);
                secondPlayerNotes[i] = obj.GetComponent<NotesView>();
                obj.transform.SetParent(gameObject.transform);
            }
        }

        notesStartRady = true;
    }

    public void CallNotes(NotesType notesType, Vector3 startPos, Vector3 goalPos, InputController targetPlayer = InputController.PlayerOne, float duration = 1.0f)
    {
        if (!notesStartRady) return;

        bool notesActive = targetPlayer == InputController.PlayerOne ? firstPlayerNotes[firstPlayerCallCount].gameObject.activeSelf : secondPlayerNotes[secondPlayerCallCount].gameObject.activeSelf;

        if (notesActive)
        {
            return;
        }

        if(targetPlayer == InputController.PlayerOne)
        {
            firstPlayerNotes[firstPlayerCallCount].SetNotesData(notesType, startPos, goalPos, targetPlayer, duration, MoveMode.Pass, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), notesSpriteAlpha);

            // ノーツプール用のカウンターを加算
            firstPlayerCallCount = firstPlayerCallCount >= firstPlayerNotes.Length - 1 ? 0 : firstPlayerCallCount += 1;
        }
        else
        {
            secondPlayerNotes[secondPlayerCallCount].SetNotesData(notesType, startPos, goalPos, targetPlayer, duration, MoveMode.Pass, perfectLength, goodLength, badLength, new Vector3(notesSize, notesSize, notesSize), notesSpriteAlpha);

            // ノーツプール用のカウンターを加算
            secondPlayerCallCount = secondPlayerCallCount >= secondPlayerNotes.Length - 1 ? 0 : secondPlayerCallCount += 1;
        }
    }
    
    private void InputPadKey()
    {
        if (!firstPlayerNotes[firstPlayerNotesCount].gameObject.activeSelf)
        {
            return;
        }

        NotesType notesType;

        if (firstPlayerNotes[firstPlayerNotesCount].InputPlayer == InputController.PlayerOne ? GamePadControl.Instance.Controller1.Circle : GamePadControl.Instance.Controller2.Circle)
        {
            notesType = NotesType.CircleKey;
        }
        else if (firstPlayerNotes[firstPlayerNotesCount].InputPlayer == InputController.PlayerOne ? GamePadControl.Instance.Controller1.Cross : GamePadControl.Instance.Controller2.Cross)
        {
            notesType = NotesType.CrossKey;
        }
        else if (firstPlayerNotes[firstPlayerNotesCount].InputPlayer == InputController.PlayerOne ? GamePadControl.Instance.Controller1.Triangle : GamePadControl.Instance.Controller2.Triangle)
        {
            notesType = NotesType.TriangleKey;
        }
        else if (firstPlayerNotes[firstPlayerNotesCount].InputPlayer == InputController.PlayerOne ? GamePadControl.Instance.Controller1.UpKey : GamePadControl.Instance.Controller2.UpKey)
        {
            notesType = NotesType.UpArrow;
        }
        else if (firstPlayerNotes[firstPlayerNotesCount].InputPlayer == InputController.PlayerOne ? GamePadControl.Instance.Controller1.DownKey : GamePadControl.Instance.Controller2.DownKey)
        {
            notesType = NotesType.DownArrow;
        }
        else if (firstPlayerNotes[firstPlayerNotesCount].InputPlayer == InputController.PlayerOne ? GamePadControl.Instance.Controller1.LeftKey : GamePadControl.Instance.Controller2.LeftKey)
        {
            notesType = NotesType.LeftArrow;
        }
        else
        {
            return;
        }

        if (firstPlayerNotes[firstPlayerNotesCount].ActionStartCheck(notesType))
        {
            FirstPlayerNotesCount++;
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
