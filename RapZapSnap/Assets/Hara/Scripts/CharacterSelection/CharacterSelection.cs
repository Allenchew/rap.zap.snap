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

    private bool doCoroutine = false;    // コルーチン処理が実行中の時true

    private ControllerNum selectPlayer = ControllerNum.P1;    // キャラクター選択をするプレイヤー

    private CanvasScaler scaler = null;

    private enum characterID
    {
        Tokiwa,
        Hajime,
        Mari
    }
    private characterID id = characterID.Tokiwa;

    public struct PlayerSelect
    {
        public int SelectedID;
        public bool IsSelected;
    }
    private PlayerSelect P1;
    private PlayerSelect P2;

    /// <summary>
    /// シーン内の初期化
    /// </summary>
    private void Init()
    {
        doCoroutine = false;
        selectPlayer = ControllerNum.P1;
        id = characterID.Tokiwa;
        P1.IsSelected = false;
        P2.IsSelected = false;

        // 画像差し替え
        backImageObject.sprite = backImage_1P;
        nowCharacterImageObj.sprite = characterImages_1P[(int)id];
        nowCharacterImageObj.transform.localPosition = new Vector3(0, 0, 0);
        nextCharacterImageObj.sprite = characterImages_1P[(int)id + 1];
        if(scaler == null) { scaler = GetComponent<CanvasScaler>(); }
        nextCharacterImageObj.transform.localPosition = new Vector3(scaler.referenceResolution.x * -1, 0, 0);
    }

    /// <summary>
    /// キャラクター選択の決定
    /// </summary>
    private void SelectCharacter()
    {
        if(doCoroutine == true) { return; }
        // キャラ情報の設定
        GameData.Instance.SetCharacterData(selectPlayer, id == characterID.Tokiwa ? Character.Tokiwa : id == characterID.Hajime ? Character.Hajime : Character.Mari);

        // 選択プレイヤーの切り替え
        if(selectPlayer == ControllerNum.P1)
        {
            selectPlayer = ControllerNum.P2;
        }
    }

    /// <summary>
    /// キャラクター切り替えの処理
    /// </summary>
    private void ChangeCharacter(bool direction)
    {
        if(doCoroutine == true) { return; }
        StartCoroutine(DoChange(direction));
    }

    /// <summary>
    /// キャラクター選択の切り替えのコルーチン処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoChange(bool direction)
    {
        // 処理開始
        doCoroutine = true;

        // 画像移動処理
        float time = 0;
        float duration = 1.0f;
        Vector3 moveStartPos;
        Vector3 moveEndPos;

        if(direction == true)
        {
            moveStartPos = new Vector3(scaler.referenceResolution.x * -1, 0, 0);
            moveEndPos = new Vector3(scaler.referenceResolution.x, 0, 0);

            if(id == characterID.Tokiwa)
            {
                id = characterID.Hajime;
            }
            else if(id == characterID.Hajime)
            {
                id = characterID.Mari;
            }
            else
            {
                id = characterID.Tokiwa;
            }
        }
        else
        {
            moveStartPos = new Vector3(scaler.referenceResolution.x, 0, 0);
            moveEndPos = new Vector3(scaler.referenceResolution.x * -1, 0, 0);

            if (id == characterID.Tokiwa)
            {
                id = characterID.Mari;
            }
            else if (id == characterID.Hajime)
            {
                id = characterID.Tokiwa;
            }
            else
            {
                id = characterID.Hajime;
            }
        }

        nextCharacterImageObj.transform.localPosition = moveStartPos;
        nextCharacterImageObj.sprite = selectPlayer == ControllerNum.P1 ? characterImages_1P[(int)id] : characterImages_2P[(int)id];

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
        nextCharacterImageObj.transform.localPosition = moveStartPos;

        // 処理終了
        doCoroutine = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
        SelectedCheck();
    }

    /// <summary>
    /// キャラクター選択が完了したら実行
    /// </summary>
    private void SelectedCheck()
    {
        if(P1.IsSelected == true && P2.IsSelected == true)
        {
            Debug.Log("OK");
            SceneControl.Instance.LoadScene(2);
        }
    }

    /// <summary>
    /// コントローラ入力
    /// </summary>
    private void InputController()
    {
        if(GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Circle) == true)
        {
            // 各プレイヤーの〇ボタンキーが押されたら実行
            SelectCharacter();
        }

        if(GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Left) == true)
        {
            // 各プレイヤーの左キーが押されたら実行
            ChangeCharacter(false);
        }

        if (GamePadControl.Instance.GetDS4Key(selectPlayer, DS4AllKeyType.Right) == true)
        {
            // 各プレイヤーの右キーが押されたら実行
            ChangeCharacter(true);
        }
    }
}
