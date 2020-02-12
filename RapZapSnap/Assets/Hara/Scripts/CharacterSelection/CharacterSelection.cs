using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [System.Serializable]
    private struct DataBase
    {
        // キャラクター選択時に使用するオブジェクト
        [Header("背景イメージ")] public Sprite BackSprite;
        [Header("キャラクター選択用の画像リスト")] public Sprite[] CharacterSprites;
        [Header("選択済み用の文字カラー")] public Color SelectedColor;

        // キャラクター決定後に使用するオブジェクト
        [Header("VS用のキャラクター画像リスト")] public Sprite[] VsCharacterSprites;
        [Header("VS用のキャラクターイメージオブジェクト")] public Image VsCharacterImage;
        [Header("VS用のキャラクター名画像リスト")] public Sprite[] VsCharacterNameSprites;
        [Header("VS用のキャラクター名イメージオブジェクト")] public Image VsCharacterNameImage;
    }
    [SerializeField, Header("1P")] private DataBase player1 = new DataBase();
    [SerializeField, Header("2P")] private DataBase player2 = new DataBase();

    [SerializeField, Header("背景オブジェクト")] private Image backImageObject = null;

    [SerializeField, Header("選択中のイメージオブジェクト")] private Image nowCharacterImageObj = null;
    [SerializeField, Header("切り替え時のイメージオブジェクト")] private Image nextCharacterImageObj = null;

    [SerializeField, Header("選択済みテキスト"), Tooltip("選択中のImage用")] private Text nowCharacterImageText = null;
    [SerializeField, Tooltip("切り替えImage用")] private Text nextCharacterImageText = null;

    private bool inputFlag = true;    // 入力を許可するフラグ

    private ControllerNum selectPlayer = ControllerNum.P1;    // キャラクター選択をするプレイヤー

    private CanvasScaler scaler = null;

    [SerializeField, Header("選択済みカラー")] private Color selectedColor = new Color(100f / 255f, 100f / 255f, 100f / 255f, 1);
    private Color unSelectedColor = new Color(1, 1, 1, 1);

    [SerializeField, Header("VSオブジェクト")] private GameObject vsObject = null;
    [SerializeField, Header("VSアイコン")] private Image vsIcon = null;
    [SerializeField, Header("ぶっかませ！オブジェクト")] private GameObject battleStartObj = null;

    private readonly string selectedText_P1 = "1P SELECTED!!";
    private readonly string selectedText_P2 = "2P SELECTED!!";

    [SerializeField, Header("シーン遷移番号")] private int nextSceneID = 0;

    private enum CharacterID
    {
        Tokiwa,
        Hajime,
        Mari
    }
    private CharacterID id = CharacterID.Tokiwa;

    private CharacterID selectedID_P1 = CharacterID.Tokiwa;
    private CharacterID selectedID_P2 = CharacterID.Tokiwa;

    /// <summary>
    /// シーン内の初期化
    /// </summary>
    private void Init()
    {
        inputFlag = true;
        selectPlayer = ControllerNum.P1;
        id = CharacterID.Tokiwa;
        nowCharacterImageText.text = "";
        nextCharacterImageText.text = "";

        // 画像差し替え
        backImageObject.sprite = player1.BackSprite;
        nowCharacterImageObj.sprite = player1.CharacterSprites[(int)id];
        nowCharacterImageObj.transform.localPosition = new Vector3(0, 0, 0);
        nowCharacterImageObj.color = unSelectedColor;
        nextCharacterImageObj.sprite = player1.CharacterSprites[(int)id + 1];
        if(scaler == null) { scaler = GetComponent<CanvasScaler>(); }
        nextCharacterImageObj.transform.localPosition = new Vector3(scaler.referenceResolution.x * -1, 0, 0);
        nextCharacterImageObj.color = unSelectedColor;

        // VSオブジェクト非表示
        vsObject.SetActive(false);
    }

    /// <summary>
    /// キャラクター選択の決定
    /// </summary>
    private void SelectCharacter()
    {
        if(selectPlayer == ControllerNum.P2 && selectedID_P1 == id) { return; }

        // キャラ情報の設定
        GameData.Instance.SetCharacterData(selectPlayer, id == CharacterID.Tokiwa ? Character.Tokiwa : id == CharacterID.Hajime ? Character.Hajime : Character.Mari);
        _ = selectPlayer == ControllerNum.P1 ? selectedID_P1 = id : selectedID_P2 = id;

        // 決定SE再生
        SoundManager.Instance.PlaySE(SEName.InputSE, true);

        // ボイスの再生
        StartCoroutine(DoSelectePlayerChange());
    }

    private IEnumerator DoSelectePlayerChange()
    {
        inputFlag = false;

        nowCharacterImageObj.color = selectedColor;
        nowCharacterImageText.text = selectPlayer == ControllerNum.P1 ? selectedText_P1 : selectedText_P2;
        nowCharacterImageText.color = selectPlayer == ControllerNum.P1 ? player1.SelectedColor : player2.SelectedColor;

        // ボイスの再生
        VoiceName voice = id == CharacterID.Tokiwa ? VoiceName.Select_TOKIWA : id == CharacterID.Hajime ? VoiceName.Select_HAJIME : VoiceName.Select_MARI;
        SoundManager.Instance.PlayVoice(voice);

        while(SoundManager.Instance.IsPlayingAudio(SourceType.Voice) == true)
        {
            yield return null;
        }

        // 選択プレイヤーの切り替え
        if (selectPlayer == ControllerNum.P1)
        {
            selectPlayer = ControllerNum.P2;
            backImageObject.sprite = player2.BackSprite;
            ChangeCharacter(true);
        }
        else
        {
            // 両プレイヤーの選択が終了したら実行
            SelectCompleted();
        }
    }

    /// <summary>
    /// キャラクター切り替えの処理
    /// </summary>
    private void ChangeCharacter(bool direction)
    {
        StartCoroutine(DoChange(direction));
        SoundManager.Instance.PlaySE(SEName.SelectChange, true);
    }

    /// <summary>
    /// キャラクター選択の切り替えのコルーチン処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoChange(bool direction)
    {
        // 処理開始
        if(inputFlag == true) { inputFlag = false; }

        // 画像移動処理
        float time = 0;
        float duration = 0.75f;
        Vector3 moveStartPos;
        Vector3 moveEndPos;

        if(direction == true)
        {
            moveStartPos = new Vector3(scaler.referenceResolution.x * -1, 0, 0);
            moveEndPos = new Vector3(scaler.referenceResolution.x, 0, 0);

            if(id == CharacterID.Tokiwa)
            {
                id = CharacterID.Hajime;
            }
            else if(id == CharacterID.Hajime)
            {
                id = CharacterID.Mari;
            }
            else
            {
                id = CharacterID.Tokiwa;
            }
        }
        else
        {
            moveStartPos = new Vector3(scaler.referenceResolution.x, 0, 0);
            moveEndPos = new Vector3(scaler.referenceResolution.x * -1, 0, 0);

            if (id == CharacterID.Tokiwa)
            {
                id = CharacterID.Mari;
            }
            else if (id == CharacterID.Hajime)
            {
                id = CharacterID.Tokiwa;
            }
            else
            {
                id = CharacterID.Hajime;
            }
        }

        nextCharacterImageObj.transform.localPosition = moveStartPos;
        nextCharacterImageObj.sprite = selectPlayer == ControllerNum.P1 ? player1.CharacterSprites[(int)id] : player2.CharacterSprites[(int)id];
        if (selectPlayer == ControllerNum.P2)
        {
            if(selectedID_P1 == id)
            {
                nextCharacterImageObj.color = selectedColor;
                nextCharacterImageText.text = selectedText_P1;
                nextCharacterImageText.color = player1.SelectedColor;
            }
            else
            {
                nextCharacterImageObj.color = unSelectedColor;
                nextCharacterImageText.text = "";
            }
        }

        while (time < duration)
        {
            float diff = time / duration;

            nowCharacterImageObj.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), moveEndPos, diff);
            nextCharacterImageObj.transform.localPosition = Vector3.Lerp(moveStartPos, new Vector3(0, 0, 0), diff);

            time += Time.deltaTime;

            yield return null;
        }
        
        nowCharacterImageObj.transform.localPosition = new Vector3(0, 0, 0);
        nowCharacterImageObj.sprite = nextCharacterImageObj.sprite;
        if (selectPlayer == ControllerNum.P2)
        {
            if (selectedID_P1 == id)
            {
                nowCharacterImageObj.color = selectedColor;
                nowCharacterImageText.text = selectedText_P1;
                nowCharacterImageText.color = player1.SelectedColor;
            }
            else
            {
                nowCharacterImageObj.color = unSelectedColor;
                nowCharacterImageText.text = "";
            }
        }
        nextCharacterImageObj.transform.localPosition = moveStartPos;

        // 処理終了
        if(inputFlag == false) { inputFlag = true; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        InputController();    // 入力取得
    }

    /// <summary>
    /// キャラクター選択が完了したら実行
    /// </summary>
    private void SelectCompleted()
    {
        inputFlag = false;
        StartCoroutine(DoVSAction());
    }

    private IEnumerator DoVSAction()
    {
        // VSオブジェクトの初期化
        vsIcon.enabled = false;
        player1.VsCharacterImage.sprite = player1.VsCharacterSprites[(int)selectedID_P1];
        player1.VsCharacterNameImage.sprite = player1.VsCharacterNameSprites[(int)selectedID_P1];
        player1.VsCharacterImage.transform.localPosition = Vector3.left * scaler.referenceResolution.x;
        player1.VsCharacterNameImage.transform.localPosition = Vector3.left * scaler.referenceResolution.x;
        player2.VsCharacterImage.sprite = player2.VsCharacterSprites[(int)selectedID_P2];
        player2.VsCharacterNameImage.sprite = player2.VsCharacterNameSprites[(int)selectedID_P2];
        player2.VsCharacterImage.transform.localPosition = Vector3.right * scaler.referenceResolution.x;
        player2.VsCharacterNameImage.transform.localPosition = Vector3.right * scaler.referenceResolution.x;
        battleStartObj.transform.localPosition = Vector3.right * scaler.referenceResolution.x + Vector3.up * scaler.referenceResolution.y;
        vsObject.SetActive(true);

        float time;
        float duration;
        float diff;

        // VS用のキャラクターイメージをスライドイン
        time = 0;
        duration = 0.75f;
        while(time < duration)
        {
            diff = time / duration;
            player1.VsCharacterImage.transform.localPosition = Vector3.Lerp(player1.VsCharacterImage.transform.localPosition, new Vector3(0, 0, 0), diff);
            player2.VsCharacterImage.transform.localPosition = Vector3.Lerp(player2.VsCharacterImage.transform.localPosition, new Vector3(0, 0, 0), diff);
            time += Time.deltaTime;
            yield return null;
        }
        player1.VsCharacterImage.transform.localPosition = new Vector3(0, 0, 0);
        player2.VsCharacterImage.transform.localPosition = new Vector3(0, 0, 0);
        vsIcon.enabled = true;

        // VSボイス再生
        for(int i = 0; i < System.Enum.GetValues(typeof(ControllerNum)).Length; i++)
        {
            Character character = GameData.Instance.GetCharacterData((ControllerNum)i);
            VoiceName voice;
            if(character == Character.Tokiwa)
            {
                voice = VoiceName.Vs_TOKIWA;
            }
            else if(character == Character.Hajime)
            {
                voice = VoiceName.Vs_HAJIME;
            }
            else
            {
                voice = VoiceName.Vs_MARI;
            }
            SoundManager.Instance.PlayVoice(voice);
        }

        // 遅延処理
        time = 0;
        duration = 0.5f;
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // VS用のキャラクター名イメージをスライドイン
        time = 0;
        duration = 0.75f;
        while(time < duration)
        {
            diff = time / duration;
            player1.VsCharacterNameImage.transform.localPosition = Vector3.Lerp(player1.VsCharacterNameImage.transform.localPosition, new Vector3(0, 0, 0), diff);
            player2.VsCharacterNameImage.transform.localPosition = Vector3.Lerp(player2.VsCharacterNameImage.transform.localPosition, new Vector3(0, 0, 0), diff);
            time += Time.deltaTime;
            yield return null;
        }
        player1.VsCharacterNameImage.transform.localPosition = new Vector3(0, 0, 0);
        player2.VsCharacterNameImage.transform.localPosition = new Vector3(0, 0, 0);

        // VS用のキャラクター名イメージを揺らす
        time = 0;
        duration = 1.5f;
        Vector3 pos1 = player1.VsCharacterNameImage.transform.localPosition;
        Vector3 pos2 = player2.VsCharacterNameImage.transform.localPosition;
        while(time < duration)
        {
            float magnitude = 10;
            player1.VsCharacterNameImage.transform.localPosition = new Vector3(pos1.x + Random.Range(-1f, 1f) * magnitude, pos1.y + Random.Range(-1f, 1f) * magnitude, 0);
            player2.VsCharacterNameImage.transform.localPosition = new Vector3(pos2.x + Random.Range(-1f, 1f) * magnitude, pos2.y + Random.Range(-1f, 1f) * magnitude, 0);
            time += Time.deltaTime;
            yield return null;
        }
        player1.VsCharacterNameImage.transform.localPosition = pos1;
        player2.VsCharacterNameImage.transform.localPosition = pos2;

        // ぶっかませを表示
        time = 0;
        duration = 1.0f;
        SoundManager.Instance.PlaySE(SEName.TurnChange, true);
        while (time < duration)
        {
            diff = time / duration;
            battleStartObj.transform.localPosition = Vector3.Lerp(backImageObject.transform.localPosition, new Vector3(0, 0, 0), diff);
            time += Time.deltaTime;
            yield return null;
        }
        battleStartObj.transform.localPosition = new Vector3(0, 0, 0);

        // 遅延処理
        time = 0;
        duration = 1.0f;
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // BGMを止めてシーン切り替え
        SoundManager.Instance.FadeOutBGM(0.3f, () => { SceneControl.Instance.LoadScene(nextSceneID); });
    }

    /// <summary>
    /// コントローラ入力
    /// </summary>
    private void InputController()
    {

        if(inputFlag == false) { return; }

        if(selectPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Circle == true : GamePadControl.Instance.GetKeyDown_2.Circle == true)
        {
            // 各プレイヤーの〇ボタンキーが押されたら実行
            SelectCharacter();
        }

        if(selectPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Left == true : GamePadControl.Instance.GetKeyDown_2.Left == true)
        {
            // 各プレイヤーの左キーが押されたら実行
            ChangeCharacter(false);
        }

        if (selectPlayer == ControllerNum.P1 ? GamePadControl.Instance.GetKeyDown_1.Right == true : GamePadControl.Instance.GetKeyDown_2.Right == true)
        {
            // 各プレイヤーの右キーが押されたら実行
            ChangeCharacter(true);
        }
    }
}
