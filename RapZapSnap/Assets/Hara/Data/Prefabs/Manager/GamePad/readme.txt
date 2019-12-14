---GamePadの導入方法---

シーン上にGamePadオブジェクトを配置してください
※シーン上にEventSystemオブジェクト(StandaloneInputModule等)が存在するとエラーが出る可能性がある為、Activeをfalseにするか削除してください。


---主な機能---

GamePadControl.Instance.GetKeyDown_1 => 1Pのコントローラー入力を取得 (入力したタイミングを取得)
GamePadControl.Instance.GetKeyDown_2 => 2Pのコントローラー入力を取得 (入力したタイミングを取得)

GamePadControl.Instance.GetKayUp_1 => 1Pのコントローラー入力を取得 (入力し終えたタイミングを取得)
GamePadControl.Instance.GetKayUp_2 => 2Pのコントローラー入力を取得 (入力し終えたタイミングを取得)

if(GamePadControl.Instance.GetKeyDown_1.Circle)
{
    // 1Pコントローラーの〇ボタンが押されたら処理を実行
}

取得可能キーの一覧

Circle      〇ボタン
Cross       ×ボタン
Triangle    △ボタン
Square      □ボタン
Up          十字キー ↑
Down        十字キー ↓
Left        十字キー ←
Right       十字キー →
L1　　　　　L1ボタン
L3　　　　　Lスティック押し込み
R1          R1ボタン
R3          Rスティック押し込み
OPTION      Optionボタン
SHARE       Shareボタン

LeftStick_Up        左スティック ↑
LeftStick_Down      左スティック ↓
LeftStick_Left      左スティック ←
LeftStick_Right     左スティック →
RightStick_Up       右スティック ↑
RightStick_Down     右スティック ↓
RightStick_Left     右スティック ←
RightStick_Right    右スティック →
L2　　　　　        L2ボタン
R2　　　　　        R2ボタン
