using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField, Header("背景イメージ"), Tooltip("1P用")] private Sprite backImage_1P = null;
    [SerializeField, Tooltip("2P用")] private Sprite backImage_2P = null;
    [SerializeField, Tooltip("背景オブジェクト")] private Image backImageObject = null;

    [SerializeField, Header("キャラクターイメージ"), Tooltip("1P用")] private Sprite[] characterImages_1P = null;
    [SerializeField, Tooltip("2P用")] private Sprite[] characterImages_2P = null;
    [SerializeField, Tooltip("キャラクターイメージオブジェクト")] private Image nowCharacterImageObj = null;
    [SerializeField, Tooltip("切り替え用のイメージオブジェクト")] private Image nextCharacterImageObj = null;

    [SerializeField, Header("選択済みテキスト"), Tooltip("選択中のImage用")] private Text nowCharacterImageText = null;
    [SerializeField, Tooltip("切り替えImage用")] private Text nextCharacterImageText = null;

    private bool inputFlag = true;    // 入力を許可するフラグ
    private bool singleInput = false;

    private ControllerNum selectPlayer = ControllerNum.P1;    // キャラクター選択をするプレイヤー

    private CanvasScaler scaler = null;

    [SerializeField, Header("選択済みカラー")] private Color selectedColor = new Color(100f / 255f, 100f / 255f, 100f / 255f, 1);
    private Color unSelectedColor = new Color(1, 1, 1, 1);
    [SerializeField, Header("選択済みテキストカラー"), Tooltip("1P用")] private Color selectedTextColor_P1 = new Color(0, 0, 0, 1);
    [SerializeField, Tooltip("2P用")] private Color selectedTextColor_P2 = new Color(0, 0, 0, 1);

    private readonly string selectedText_P1 = "1P Selected!!";
    private readonly string selectedText_P2 = "2P Selected!!";

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
        backImageObject.sprite = backImage_1P;
        nowCharacterImageObj.sprite = characterImages_1P[(int)id];
        nowCharacterImageObj.transform.localPosition = new Vector3(0, 0, 0);
        nowCharacterImageObj.color = unSelectedColor;
        nextCharacterImageObj.sprite = characterImages_1P[(int)id + 1];
        if(scaler == null) { scaler = GetComponent<CanvasScaler>(); }
        nextCharacterImageObj.transform.localPosition = new Vector3(scaler.referenceResolution.x * -1, 0, 0);
        nextCharacterImageObj.color = unSelectedColor;
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

        StartCoroutine(DoSelectePlayerChange());
    }

    private IEnumerator DoSelectePlayerChange()
    {
        inputFlag = false;
        float time = 0;
        float duration = 0.75f;

        nowCharacterImageObj.color = selectedColor;
        nowCharacterImageText.text = selectPlayer == ControllerNum.P1 ? selectedText_P1 : selectedText_P2;
        nowCharacterImageText.color = selectPlayer == ControllerNum.P1 ? selectedTextColor_P1 : selectedTextColor_P2;

        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 選択プレイヤーの切り替え
        if (selectPlayer == ControllerNum.P1)
        {
            selectPlayer = ControllerNum.P2;
            backImageObject.sprite = backImage_2P;
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
        nextCharacterImageObj.sprite = selectPlayer == ControllerNum.P1 ? characterImages_1P[(int)id] : characterImages_2P[(int)id];
        if (selectPlayer == ControllerNum.P2)
        {
            if(selectedID_P1 == id)
            {
                nextCharacterImageObj.color = selectedColor;
                nextCharacterImageText.text = selectedText_P1;
                nextCharacterImageText.color = selectedTextColor_P1;
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
                nowCharacterImageText.color = selectedTextColor_P1;
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
        SceneControl.Instance.LoadScene(2);
    }

    /// <summary>
    /// コントローラ入力
    /// </summary>
    private void InputController()
    {

        if(inputFlag == false) { return; }

        if(GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Circle) == true && singleInput == true)
        {
            // 各プレイヤーの〇ボタンキーが押されたら実行
            singleInput = false;
            SelectCharacter();
        }

        if(GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Left) == true && singleInput == true)
        {
            // 各プレイヤーの左キーが押されたら実行
            singleInput = false;
            ChangeCharacter(false);
        }

        if (GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Right) == true && singleInput == true)
        {
            // 各プレイヤーの右キーが押されたら実行
            singleInput = false;
            ChangeCharacter(true);
        }

        if(GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Circle) == false && GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Left) == false && GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Right) == false && singleInput == false)
        {
            singleInput = true;
        }
    }
}
