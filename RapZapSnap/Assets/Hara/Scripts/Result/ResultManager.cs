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

    [SerializeField, Tooltip("1Pのトータルスコアテキスト")] private Text totalScore_P1 = null;
    [SerializeField, Tooltip("2Pのトータルスコアテキスト")] private Text totalScore_P2 = null;

    /// <summary>
    /// 各プレイヤーのスコア情報
    /// </summary>
    private struct Data
    {
        public enum Character
        {
            Mari,
            Tokiwa,
            Hajime
        }

        public Character CharacterData;
        public int RapScore;
        public int ZapScore;
        public int SnapScore;
        public int TotalScore;
    }

    private Data data_P1;
    private Data data_P2;

    private float time = 0;
    private float span = 0;
    private bool isStart = false;


    // Start is called before the first frame update
    void Start()
    {
        SetScoreData();
        SetScoreObject();
    }

    // Update is called once per frame
    void Update()
    {
        ResultAction();
    }

    /// <summary>
    /// スコア情報を取得してセットする
    /// </summary>
    private void SetScoreData()
    {
        // P1の結果を取得
        data_P1.RapScore = NotesControl.Instance.GetResult(2, ControllerNum.P1);
        data_P1.ZapScore = NotesControl.Instance.GetResult(1, ControllerNum.P1);
        data_P1.SnapScore = NotesControl.Instance.GetResult(0, ControllerNum.P1);
        data_P1.TotalScore = NotesControl.Instance.GetResult(3, ControllerNum.P1);    // ルーレットのスコアは後に加算処理を追加

        // P2の結果を取得
        data_P2.RapScore = NotesControl.Instance.GetResult(2, ControllerNum.P2);
        data_P2.ZapScore = NotesControl.Instance.GetResult(1, ControllerNum.P2);
        data_P2.SnapScore = NotesControl.Instance.GetResult(0, ControllerNum.P2);
        data_P2.TotalScore = NotesControl.Instance.GetResult(3, ControllerNum.P2);    // ルーレットのスコアは後に加算処理を追加
    }

    /// <summary>
    /// スコアを表示するオブジェクトの初期化を行う
    /// </summary>
    private void SetScoreObject()
    {

    }

    /// <summary>
    /// リザルトを表示する処理を実行
    /// </summary>
    private  void ResultAction()
    {

    }
}
