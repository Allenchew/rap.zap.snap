---GamePadの導入方法---

1. シーン上にGamePadオブジェクトを配置します。
※シーン上にEventSystemオブジェクトが存在するとエラーが出る可能性がある為、Activeをfalseにするか削除してください。

2. GamePadのインスペクターからGamePadControlのUsePs4ControllerをtrueにするとPS4のコントローラー入力を検知できるようになります。（デフォルトでtrue）

3. AxisValueの値を変えると、コントローラーのスティックの有効入力感度を調節できます。 


---使い方---

1. コントローラーの入力を検知する。

　　　　GamePadControl.Instance.Controller1.Circle    1Pコントローラーの〇ボタンが押されたことをbool型で取得する（押された場合はtrue）

　　　　GamePadControl.Instance.Controller2.Cross     2Pコントローラーの×ボタンが押されたことをbool型で取得する

        ---取得できるキー---
		Circle      〇ボタン
		Cross       ×ボタン
		Triangle    △ボタン
        Square      □ボタン
        Up          十字キー ↑
        Down        十字キー ↓
        Left        十字キー ←
        Right       十字キー →
        L1　　　　　L1ボタン
        L2　　　　　L2ボタン
        L3　　　　　Lスティック押し込み
        LstickU     Lスティック ↑
        LstickD     Lスティック ↓
        LstickL     Lスティック ←
        LstickR     Lスティック →
        R1          R1ボタン
        R2          R2ボタン
        R3          Rスティック押し込み
        RstickU     Rスティック ↑
        RstickD     Rスティック ↓
        RstickL     Rスティック ←
        RstickR     Rスティック →
        OPTION      Optionボタン
        SHARE       Shareボタン


		使用例 :

		if(GamePadControl.Instance.Controller1.Upley)
		{
		    // 1Pコントローラーの十字キー↑が押されたら実行したい処理

		}

2. StandaloneInputModule(UI操作用のモジュール)を使う。
        
		StandaloneInputModuleを使う場合、コントローラを予め有線接続しておく必要があるので注意。

		GamePadControl.Instance.SetInputModule(入力対象コントローラー)    指定したコントローラーでuGIのボタン入力等の操作が可能になります。