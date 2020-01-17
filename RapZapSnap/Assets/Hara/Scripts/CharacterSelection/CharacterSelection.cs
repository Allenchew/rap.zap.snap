using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField, Header("背景イメージ"), Tooltip("1P用")] private Sprite backImage_1P = null;
    [SerializeField, Tooltip("2P用")] private Sprite backImage_2P = null;
    [SerializeField, Tooltip("背景オブジェクト")] private Image backImageObject = null;

    private bool moveDirection = true;    // キャラクター選択の切り替え時の移動方向

    private bool doCoroutine = false;    // コルーチン処理が実行中の時true

    private bool selectPlayer = true;    // trueの時1P、falseの時2P

    private ControllerNum player = ControllerNum.P1;

    private enum characterID
    {
        Tokiwa,
        Hajime,
        Mari
    }
    private characterID id = characterID.Tokiwa;

    /// <summary>
    /// キャラクター選択の決定
    /// </summary>
    private void SelectCharacter()
    {
        if(doCoroutine == true) { return; }
    }

    /// <summary>
    /// キャラクター選択の切り替えのコルーチン処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoMove()
    {
        // 処理開始
        doCoroutine = true;

        yield return null;

        // 処理終了
        doCoroutine = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selectPlayer == true)
        {
            player = ControllerNum.P1;
        }
        else
        {
            player = ControllerNum.P2;
        }

        
    }
}
