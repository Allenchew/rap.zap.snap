using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField, Tooltip("タイトルロゴ"), Header("タイトルのメインオブジェクト")] private Image titleLogo = null;
    private Animator titleLogoAnime = null;    // タイトルロゴ用のアニメーター
    [SerializeField, Tooltip("1PのタイトルImage")] private Image titleImage_P1 = null;
    [SerializeField, Tooltip("2PのタイトルImage")] private Image titleImage_P2 = null;
    private GameObject radyText_P1 = null;
    private GameObject radyText_P2 = null;

    [SerializeField, Tooltip("NowLoading用のSlider"), Header("ロード画面用のオブジェクト")] private Slider nowLoadingSlider = null;
    [SerializeField, Tooltip("NowLoadingのText")] private Text nowLoadingText = null;

    // コントローラの入力チェックフラグ
    private bool radyController1 = false;
    private bool radyController2 = false;

    private struct MovePos
    {
        public Vector3 Start;
        public Vector3 End;
    }
    private MovePos objP1;
    private MovePos objP2;

    private int step = 0;
    private bool stepEndFlag = false;
    private float deltaTime = 0f;
    private bool actionFlag = false;
    private AsyncOperation async = null;
    private Vector3 titleLogoScale;

    [SerializeField, Tooltip("シーン番号"), Header("キャラクター選択のシーン")] private int sceneNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        TitleInit();
    }

    // Update is called once per frame
    void Update()
    {
        TitleAction();
    }

    /// <summary>
    /// タイトルシーンの初期化
    /// </summary>
    private void TitleInit()
    {
        if(titleLogo == null || titleImage_P1 == null || titleImage_P2 == null)
        {
            Debug.LogError("メインオブジェクトが割り当てられていません");
            return;
        }

        if (nowLoadingSlider == null || nowLoadingText == null)
        {
            Debug.LogError("Loadingオブジェクトが割り当てられていません");
            return;
        }

        titleLogo.gameObject.SetActive(true);
        titleLogoScale = titleLogo.transform.localScale;
        titleLogoAnime = titleLogo.gameObject.GetComponent<Animator>();
        titleLogoAnime.enabled = true;

        titleImage_P1.gameObject.SetActive(true);
        titleImage_P2.gameObject.SetActive(true);

        radyText_P1 = titleImage_P1.transform.GetChild(0).gameObject;
        radyText_P1.SetActive(false);
        radyText_P2 = titleImage_P2.transform.GetChild(0).gameObject;
        radyText_P2.SetActive(false);

        nowLoadingSlider.value = 0;
        nowLoadingSlider.gameObject.SetActive(false);
        nowLoadingText.gameObject.SetActive(false);

        // 座標の初期化
        objP1.Start = titleImage_P1.transform.localPosition;
        objP2.Start = titleImage_P2.transform.localPosition;
        objP1.End = Vector3.left * titleImage_P1.rectTransform.rect.width + objP1.Start;
        objP2.End = Vector3.right * titleImage_P2.rectTransform.rect.width + objP2.Start;

        // スコアの初期化
        GameData.Instance.ResetScore(ControllerNum.P1);
        GameData.Instance.ResetScore(ControllerNum.P2);

        SoundManager.Instance.PlayBGM(BGMName.MainBGM);

        actionFlag = true;
    }

    /// <summary>
    /// タイトルシーンで行う処理
    /// </summary>
    private void TitleAction()
    {
        if(actionFlag == false) { return; }

        switch (step)
        {
            case 0:
                // コントローラの入力を待機
                if (GamePadControl.Instance.GetKeyDown_1.Circle == true && radyController1 == false)
                {
                    radyController1 = true;
                    SoundManager.Instance.PlaySE(SEName.InputSE, true);
                    radyText_P1.SetActive(true);
                }
                if (GamePadControl.Instance.GetKeyDown_2.Circle == true && radyController2 == false)
                {
                    radyController2 = true;
                    SoundManager.Instance.PlaySE(SEName.InputSE, true);
                    radyText_P2.SetActive(true);
                }
                if(radyController1 == true && radyController2 == true)
                {
                    stepEndFlag = true;
                    titleLogoAnime.enabled = false;
                    titleLogo.transform.localScale = titleLogoScale;
                }
                break;
            case 1:
                // 処理待機
                deltaTime += Time.deltaTime;
                if(deltaTime >= 1.0f)
                {
                    stepEndFlag = true;
                }
                break;
            case 2:
                // タイトルロゴなどを非表示
                titleLogo.gameObject.SetActive(false);
                radyText_P1.SetActive(false);
                radyText_P2.SetActive(false);
                stepEndFlag = true;
                break;
            case 3:
                // タイトルのメインオブジェクトを移動させる
                float span = 0.75f;
                var diff = deltaTime / span;
                titleImage_P1.transform.localPosition = Vector3.Lerp(objP1.Start, objP1.End, diff);
                titleImage_P2.transform.localPosition = Vector3.Lerp(objP2.Start, objP2.End, diff);
                deltaTime += Time.deltaTime;
                if(deltaTime >= span)
                {
                    titleImage_P1.transform.localPosition = objP1.End;
                    titleImage_P2.transform.localPosition = objP2.End;
                    titleImage_P1.gameObject.SetActive(false);
                    titleImage_P2.gameObject.SetActive(false);
                    stepEndFlag = true;
                }
                break;
            case 4:
                // NowLoading画面の表示
                nowLoadingText.gameObject.SetActive(true);
                nowLoadingSlider.gameObject.SetActive(true);
                stepEndFlag = true;
                break;
            case 5:
                // シーンのロードを開始
                StartCoroutine(SceneLoad());
                stepEndFlag = true;
                break;
            default:
                actionFlag = false;
                return;
        }

        if(stepEndFlag == true)
        {
            deltaTime = 0f;
            stepEndFlag = false;
            step++;
        }
    }

    /// <summary>
    /// シーンのロードを行う
    /// </summary>
    /// <returns></returns>
    private IEnumerator SceneLoad()
    {
        // シーンの読み込み
        async = SceneControl.Instance.LoadScene(sceneNum);

        async.allowSceneActivation = false;

        while(nowLoadingSlider.value < 1.0f || async.progress < 0.9f)
        {
            float increase = Random.Range(0.001f, 0.01f);
            nowLoadingSlider.value = nowLoadingSlider.value + increase >= 1.0f ? 1.0f : nowLoadingSlider.value += increase;
            yield return null;
        }

        async.allowSceneActivation = true;
    }
}
