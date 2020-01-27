using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField, Header("シーン内のキャンバススケーラー")] private CanvasScaler scaler = null;

    [System.Serializable]
    private struct ResultDataBase
    {
        [Header("リザルトに表示するキャラクターの画像リスト")] public Sprite[] CharacterSprites;
        [Header("リザルトに表示するキャラクター画像のオブジェクト")] public Image CharacterImage;
        [Header("トータルスコアのオブジェクト")] public GameObject TotalScoreObj;
        [Header("トータルスコアのテキスト")] public Text TotalScoreText;
        [Header("Rapのスコアテキスト")] public Text RapText;
        [Header("Zapのスコアテキスト")] public Text ZapText;
        [Header("Snapのスコアテキスト")] public Text SnapText;
        [Header("Winの画像データ")] public Sprite WinSprite;
    }

    [SerializeField, Header("1P")] private ResultDataBase player1 = new ResultDataBase();
    [SerializeField, Header("2P")] private ResultDataBase player2 = new ResultDataBase();

    [SerializeField, Header("スコアボードオブジェクト")] private GameObject scoreBoardObject = null;
    [SerializeField, Header("Winオブジェクト")] private Image winImageObj = null;

    [SerializeField, Header("リザルトシーン用のSE")] private AudioClip[] resultSE = null;
    private AudioSource resultAudio = null;

    private struct MoveObjPos
    {
        public Vector3 StartPos;
        public Vector3 EndPos;
    }

    private MoveObjPos objectMove_1 = new MoveObjPos();
    private MoveObjPos objectMove_2 = new MoveObjPos();
    private MoveObjPos scoreBoardMove;

    private float time = 0;
    private float span = 0;
    private int step = 0;
    private bool stepEndFlag = false;
    private bool actionFlag = false;
    [SerializeField, Tooltip("シーン番号"), Header("タイトルシーン")] private int sceneNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        ResultInit();
    }

    // Update is called once per frame
    void Update()
    {
        ResultAction();
    }

    /// <summary>
    /// リザルトシーンの初期化を行う
    /// </summary>
    private void ResultInit()
    {
        // 1P用の座標設定
        objectMove_1.EndPos = new Vector3(0, 0, 0);
        objectMove_1.StartPos = Vector3.left * scaler.referenceResolution.x + objectMove_1.EndPos;

        // 2P用の座標設定
        objectMove_2.EndPos = new Vector3(0, 0, 0);
        objectMove_2.StartPos = Vector3.right * scaler.referenceResolution.x + objectMove_1.EndPos;

        // ScoreBoardの初期化
        scoreBoardMove.EndPos = new Vector3(0, 0, 0);
        scoreBoardMove.StartPos = Vector3.up * scaler.referenceResolution.y + scoreBoardMove.EndPos;

        resultAudio = GetComponent<AudioSource>();

        actionFlag = true;
    }

    /// <summary>
    /// リザルトを表示する処理を実行
    /// </summary>
    private  void ResultAction()
    {
        if(actionFlag == false) { return; }

        switch (step)
        {
            case 0:
                // オブジェクトを初期値に配置及び初期化
                player1.CharacterImage.transform.localPosition = objectMove_1.StartPos;
                player1.CharacterImage.sprite = player1.CharacterSprites[(int)GameData.Instance.GetCharacterData(ControllerNum.P1)];
                player1.TotalScoreObj.transform.localPosition = objectMove_1.StartPos;
                player1.TotalScoreText.text = "0";
                player1.RapText.text = "";
                player1.ZapText.text = "";
                player1.SnapText.text = "";

                player2.CharacterImage.transform.localPosition = objectMove_2.StartPos;
                player2.CharacterImage.sprite = player2.CharacterSprites[(int)GameData.Instance.GetCharacterData(ControllerNum.P2)];
                player2.TotalScoreObj.transform.localPosition = objectMove_2.StartPos;
                player2.TotalScoreText.text = "0";
                player2.RapText.text = "";
                player2.ZapText.text = "";
                player2.SnapText.text = "";

                scoreBoardObject.transform.localPosition = scoreBoardMove.StartPos;

                winImageObj.gameObject.SetActive(false);

                stepEndFlag = true;
                break;
            case 1:
                // 1Pのキャラクターをスライドイン
                span = 0.75f;
                var diff = time / span;
                player1.CharacterImage.transform.localPosition = Vector3.Lerp(objectMove_1.StartPos, objectMove_1.EndPos, diff);
                time += Time.deltaTime;
                if(time >= span)
                {
                    player1.CharacterImage.transform.localPosition = objectMove_1.EndPos;
                    stepEndFlag = true;
                }
                break;
            case 2:
                // 2Pのキャラクターをスライドイン
                span = 0.75f;
                diff = time / span;
                player2.CharacterImage.transform.localPosition = Vector3.Lerp(objectMove_2.StartPos, objectMove_2.EndPos, diff);
                time += Time.deltaTime;
                if (time >= span)
                {
                    player2.CharacterImage.transform.localPosition = objectMove_2.EndPos;
                    stepEndFlag = true;
                }
                break;
            case 3:
                // スコアボードとトータルスコアをスライドイン
                span = 0.75f;
                diff = time / span;
                scoreBoardObject.transform.localPosition = Vector3.Lerp(scoreBoardMove.StartPos, scoreBoardMove.EndPos, diff);
                player1.TotalScoreObj.transform.localPosition = Vector3.Lerp(objectMove_1.StartPos, objectMove_1.EndPos, diff);
                player2.TotalScoreObj.transform.localPosition = Vector3.Lerp(objectMove_2.StartPos, objectMove_2.EndPos, diff);
                time += Time.deltaTime;
                if (time >= span)
                {
                    scoreBoardObject.transform.localPosition = scoreBoardMove.EndPos;
                    player1.TotalScoreObj.transform.localPosition = objectMove_1.EndPos;
                    player2.TotalScoreObj.transform.localPosition = objectMove_2.EndPos;
                    stepEndFlag = true;
                }
                break;
            case 4:
                // Rapのスコアを表示
                span = 0.75f;
                int i = 11 * Random.Range(1, 10);
                player1.RapText.text = i.ToString();
                player2.RapText.text = i.ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    player1.RapText.text = GameData.Instance.GetRapScore(ControllerNum.P1).ToString();
                    player2.RapText.text = GameData.Instance.GetRapScore(ControllerNum.P2).ToString();
                    stepEndFlag = true;
                }
                break;
            case 5:
                // Zapのスコアを表示
                span = 0.75f;
                i = 11 * Random.Range(1, 10);
                player1.ZapText.text = i.ToString();
                player2.ZapText.text = i.ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    player1.ZapText.text = GameData.Instance.GetZapScore(ControllerNum.P1).ToString();
                    player2.ZapText.text = GameData.Instance.GetZapScore(ControllerNum.P2).ToString();
                    stepEndFlag = true;
                }
                break;
            case 6:
                // Snapのスコアを表示
                span = 1.0f;
                i = 11 * Random.Range(1, 10);
                player1.SnapText.text = i.ToString();
                player2.SnapText.text = i.ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    player1.SnapText.text = GameData.Instance.GetSnapScore(ControllerNum.P1).ToString();
                    player2.SnapText.text = GameData.Instance.GetSnapScore(ControllerNum.P2).ToString();
                    stepEndFlag = true;
                }
                break;
            case 7:
                // トータルスコアを表示
                span = 1.0f;
                diff = time / span;
                player1.TotalScoreText.text = ((int)(GameData.Instance.GetTotalScore(ControllerNum.P1) * diff)).ToString();
                player2.TotalScoreText.text = ((int)(GameData.Instance.GetTotalScore(ControllerNum.P2) * diff)).ToString();
                time += Time.deltaTime;
                if (time >= span)
                {
                    player1.TotalScoreText.text = GameData.Instance.GetTotalScore(ControllerNum.P1).ToString();
                    player2.TotalScoreText.text = GameData.Instance.GetTotalScore(ControllerNum.P2).ToString();
                    stepEndFlag = true;
                }
                break;
            case 8:
                // Winオブジェクトを表示
                span = 0.5f;
                time += Time.deltaTime;
                if(time >= span)
                {
                    winImageObj.sprite = GameData.Instance.GetWinnerPlayer() == ControllerNum.P1 ? player1.WinSprite : player2.WinSprite;
                    winImageObj.gameObject.SetActive(true);
                    if(resultSE[0] != null) { resultAudio.PlayOneShot(resultSE[0]); }
                    stepEndFlag = true;
                }
                break;
            default:
                if(resultAudio.isPlaying == true) { return; }
                if ((GamePadControl.Instance.GetKeyDown_1.Circle == true || GamePadControl.Instance.GetKeyDown_2.Circle == true) && actionFlag == true)
                {
                    step = 0;
                    actionFlag = false;
                    if(resultSE[1] != null)
                    {
                        resultAudio.PlayOneShot(resultSE[1]);
                    }
                    SceneControl.Instance.LoadScene(sceneNum);
                }
                return;
        }

        if(stepEndFlag == true)
        {
            step++;
            stepEndFlag = false;
            time = 0;
        }

        // どちらかのプレイヤーが決定キーを入力したらアニメーションをスキップ
        if ((GamePadControl.Instance.GetKeyDown_1.Circle == true || GamePadControl.Instance.GetKeyDown_2.Circle == true) && actionFlag == true)
        {
            step = 9;
            stepEndFlag = false;
            time = 0;

            player1.CharacterImage.transform.localPosition = objectMove_1.EndPos;
            player2.CharacterImage.transform.localPosition = objectMove_2.EndPos;
            scoreBoardObject.transform.localPosition = scoreBoardMove.EndPos;
            player1.TotalScoreObj.transform.localPosition = objectMove_1.EndPos;
            player2.TotalScoreObj.transform.localPosition = objectMove_2.EndPos;
            player1.RapText.text = GameData.Instance.GetRapScore(ControllerNum.P1).ToString();
            player2.RapText.text = GameData.Instance.GetRapScore(ControllerNum.P2).ToString();
            player1.ZapText.text = GameData.Instance.GetZapScore(ControllerNum.P1).ToString();
            player2.ZapText.text = GameData.Instance.GetZapScore(ControllerNum.P2).ToString();
            player1.SnapText.text = GameData.Instance.GetSnapScore(ControllerNum.P1).ToString();
            player2.SnapText.text = GameData.Instance.GetSnapScore(ControllerNum.P2).ToString();
            player1.TotalScoreText.text = GameData.Instance.GetTotalScore(ControllerNum.P1).ToString();
            player2.TotalScoreText.text = GameData.Instance.GetTotalScore(ControllerNum.P2).ToString();
            winImageObj.sprite = GameData.Instance.GetWinnerPlayer() == ControllerNum.P1 ? player1.WinSprite : player2.WinSprite;
            winImageObj.gameObject.SetActive(true);
            if (resultSE[0] != null) { resultAudio.PlayOneShot(resultSE[0]); }
        }
    }
}
