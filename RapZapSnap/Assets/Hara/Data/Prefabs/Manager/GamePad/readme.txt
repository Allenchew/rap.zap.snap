---GamePadの導入方法---

1. シーン上にGamePadオブジェクトを配置します。
※シーン上にEventSystemオブジェクトが存在するとエラーが出る可能性がある為、Activeをfalseにするか削除してください。

2. GamePadのインスペクターからGamePadControlのUsePs4ControllerをtrueにするとPS4のコントローラー入力を検知できるようになります。（デフォルトでtrue）

3. AxisValueの値を変えると、コントローラーのスティックの有効入力感度を調節できます。 


---使い方---

1. コントローラーのボタン入力を検知する。

　　　　GamePadControl.Instance.GetButtonDown(ControllerNum.P1, DS4ButtonKey.Circle)    1Pコントローラーの〇ボタンが押されたことをbool型で取得する（押された場合はtrue）

        ---DS4ButtonKey一覧---
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


		使用例 :

		if(GamePadControl.Instance.GetButtonDown(ControllerNum.P1, DS4ButtonKey.Up) == true)
		{
		    // 1Pコントローラーの十字キー↑が押されたら実行したい処理

		}



2. コントローラーのスティック入力を検知する。

　　　　GamePadControl.Instance.GetAxisDown(ControllerNum.P1, DS4AxisKey.L2)    1PコントローラーのL2ボタンが入力されたことをbool型で取得する（押された場合はtrue）
        
		入力感度はAxisValueで調節可能

        ---DS4AxisKey一覧---
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



3. StandaloneInputModule(UI操作用のモジュール)を使う。
        
		StandaloneInputModuleを使う場合、コントローラを予め有線接続しておく必要があるので注意。

		GamePadControl.Instance.SetInputModule(ControllerNum.P1)    1PのコントローラーでuGIのボタン入力等の操作が可能になります。