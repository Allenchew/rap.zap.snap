using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField, Tooltip("リザルトに表示するキャラクター画像")]
    private List<Sprite> characterSprites = new List<Sprite>();

    [SerializeField, Tooltip("勝利プレイヤー用の背景画像")]
    private List<Sprite> winnerBackSprites = new List<Sprite>();

    [SerializeField, Tooltip("敗北プレイヤー用の背景画像")]
    private List<Sprite> loserBackSprites = new List<Sprite>();


    // リザルトシーンのオブジェクト
    [SerializeField, Tooltip("Winnerオブジェクト")] private GameObject winnerObj = null;
    [SerializeField, Tooltip("Loserオブジェクト")] private GameObject loserObj = null;
    [SerializeField, Tooltip("ノーツのスコアボードオブジェクト")] private GameObject notesScoreBoard = null;
    [SerializeField, Tooltip("勝利プレイヤー用のトータルスコアテキスト")] private Text totalScore_Win = null;
    [SerializeField, Tooltip("敗北プレイヤー用のトータルスコアテキスト")] private Text totalScore_Lose = null;
    [SerializeField, Tooltip("勝利プレイヤー用のrapのスコアテキスト")] private Text rapScore_Win = null;
    [SerializeField, Tooltip("敗北プレイヤー用のrapのスコアテキスト")] private Text rapScore_Lose = null;
    [SerializeField, Tooltip("勝利プレイヤー用のzapのスコアテキスト")] private Text zapScore_Win = null;
    [SerializeField, Tooltip("敗北プレイヤー用のzapのスコアテキスト")] private Text zapScore_Lose = null;
    [SerializeField, Tooltip("勝利プレイヤー用のsnapのスコアテキスト")] private Text snapScore_Win = null;
    [SerializeField, Tooltip("敗北プレイヤー用のsnapのスコアテキスト")] private Text snapScore_Lose = null;

    private struct MoveObjPos
    {
        public Vector3 StartPos;
        public Vector3 EndPos;
    }

    private Image winnerBackImage = null;
    private Image winnerCharacterImage = null;
    private Image loserBackImage = null;
    private Image loserCharacterImage = null;
    private Vector3 scoreBoardStartPos;
    private Vector3 scoreBoardEndPos;
    private MoveObjPos winnerObjMove;
    private MoveObjPos loserObjMove;


    private float time = 0;
    private float span = 0;
    private int step = 0;
    private bool stepEndFlag = false;
    [SerializeField] private bool startFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        SetScoreObject();
    }

    // Update is called once per frame
    void Update()
    {
        ResultAction();
    }

    /// <summary>
    /// スコアを表示するオブジェクトの初期化を行う
    /// </summary>
    private void SetScoreObject()
    {
        // Winnerオブジェクトの初期化
        winnerObjMove.EndPos = winnerObj.transform.localPosition;
        winnerObjMove.StartPos = Vector3.left * 1200 + winnerObjMove.EndPos;
        winnerObj.transform.localPosition = winnerObjMove.StartPos;
        winnerBackImage = winnerObj.transform.GetChild(0).GetComponent<Image>();
        winnerCharacterImage = winnerObj.transform.GetChild(1).GetComponent<Image>();
        winnerBackImage.sprite = winnerBackSprites[(int)GameData.Instance.GetCharacterData(true)];
        winnerCharacterImage.sprite = characterSprites[(int)GameData.Instance.GetCharacterData(true)];
        totalScore_Win.text = "0";

        // Loserオブジェクトの初期化
        loserObjMove.EndPos = loserObj.transform.localPosition;
        loserObjMove.StartPos = Vector3.right * 1200 + loserObjMove.EndPos;
        loserObj.transform.localPosition = loserObjMove.StartPos;
        loserBackImage = loserObj.transform.GetChild(0).GetComponent<Image>();
        loserCharacterImage = loserObj.transform.GetChild(1).GetComponent<Image>();
        loserBackImage.sprite = loserBackSprites[(int)GameData.Instance.GetCharacterData(false)];
        loserCharacterImage.sprite = characterSprites[(int)GameData.Instance.GetCharacterData(false)];
        totalScore_Lose.text = "0";

        // ScoreBoardの初期化
        scoreBoardEndPos = notesScoreBoard.transform.localPosition;
        scoreBoardStartPos = Vector3.up * 1200 + scoreBoardEndPos;
        notesScoreBoard.transform.localPosition = scoreBoardStartPos;
        rapScore_Win.text = "";
        rapScore_Lose.text = "";
        zapScore_Win.text = "";
        zapScore_Lose.text = "";
        snapScore_Win.text = "";
        snapScore_Lose.text = "";

        startFlag = true;
    }

    /// <summary>
    /// リザルトを表示する処理を実行
    /// </summary>
    private  void ResultAction()
    {
        if(startFlag == false) { return; }

        switch (step)
        {
            case 0:
                // 勝利プレイヤーのキャラクターとスコアを表示(スライドイン)させる
                span = 0.75f;
                var diff = time / span;
                winnerObj.transform.localPosition = Vector3.Lerp(winnerObjMove.StartPos, winnerObjMove.EndPos, diff);
                time += Time.deltaTime;
                if(time >= span)
                {
                    winnerObj.transform.localPosition = winnerObjMove.EndPos;
                    stepEndFlag = true;
                }
                break;
            case 1:
                // 敗北プレイヤーのキャラクターとスコアを表示(スライドイン)させる
                span = 0.75f;
                diff = time / span;
                loserObj.transform.localPosition = Vector3.Lerp(loserObjMove.StartPos, loserObjMove.EndPos, diff);
                time += Time.deltaTime;
                if (time >= span)
                {
                    loserObj.transform.localPosition = loserObjMove.EndPos;
                    stepEndFlag = true;
                }
                break;
            case 2:
                // ノーツのスコアボードを表示(スライドイン)させる
                span = 1.0f;
                diff = time / span;
                notesScoreBoard.transform.localPosition = Vector3.Lerp(scoreBoardStartPos, scoreBoardEndPos, diff);
                time += Time.deltaTime;
                if (time >= span)
                {
                    notesScoreBoard.transform.localPosition = scoreBoardEndPos;
                    stepEndFlag = true;
                }
                break;
            case 3:
                // Rapのスコアを表示
                span = 1.0f;
                int i = 11 * Random.Range(1, 10);
                rapScore_Win.text = i.ToString();
                rapScore_Lose.text = i.ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    rapScore_Win.text = GameData.Instance.GetNotesResult(true, 2).ToString();
                    rapScore_Lose.text = GameData.Instance.GetNotesResult(false, 2).ToString();
                    stepEndFlag = true;
                }
                break;
            case 4:
                // Zapのスコアを表示
                span = 1.0f;
                i = 11 * Random.Range(1, 10);
                zapScore_Win.text = i.ToString();
                zapScore_Lose.text = i.ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    zapScore_Win.text = GameData.Instance.GetNotesResult(true, 1).ToString();
                    zapScore_Lose.text = GameData.Instance.GetNotesResult(false, 1).ToString();
                    stepEndFlag = true;
                }
                break;
            case 5:
                // Snapのスコアを表示
                span = 1.0f;
                i = 11 * Random.Range(1, 10);
                snapScore_Win.text = i.ToString();
                snapScore_Lose.text = i.ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    snapScore_Win.text = GameData.Instance.GetNotesResult(true, 0).ToString();
                    snapScore_Lose.text = GameData.Instance.GetNotesResult(false, 0).ToString();
                    stepEndFlag = true;
                }
                break;
            case 6:
                // トータルスコアを表示
                span = 1.5f;
                diff = time / span;
                totalScore_Win.text = ((int)(GameData.Instance.GetTotalScore(true) * diff)).ToString();
                totalScore_Lose.text = ((int)(GameData.Instance.GetTotalScore(false) * diff)).ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    totalScore_Win.text = GameData.Instance.GetTotalScore(true).ToString();
                    totalScore_Lose.text = GameData.Instance.GetTotalScore(false).ToString();
                    stepEndFlag = true;
                }
                break;
            default:
                startFlag = false;
                return;
        }

        if(stepEndFlag == true)
        {
            step++;
            stepEndFlag = false;
            time = 0;
        }
    }
}
