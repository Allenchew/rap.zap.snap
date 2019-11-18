2019/11/19 更新

---GamePadの導入方法---
1. シーン上にGamePadオブジェクトを配置します。
※シーン上にEventSystemオブジェクトが存在するとエラーが出る可能性がある為、Activeをfalseにするか削除してください。

2. GamePadのインスペクターからGamePadControlのUsePs4ControllerをtrueにするとPS4のコントローラー入力を検知できるようになります。（デフォルトでtrue）


---使い方---
1. スクリプトから呼び出す場合、スクリプトに「using DS4;」と記載してください。

2. コントローラーの入力を検知する。

　　　　GamePadControl.Instance.Controller1.Circle    1Pコントローラーの〇ボタンが押されたことをbool型で取得する（押された場合はtrue）

　　　　GamePadControl.Instance.Controller2.Cross     2Pコントローラーの×ボタンが押されたことをbool型で取得する

        ---取得できるキー---
		Circle      〇ボタン
		Cross       ×ボタン
		Triangle    △ボタン
        Square      □ボタン
        UpKey       十字キー ↑
        DownKey     十字キー ↓
        LeftKey     十字キー ←
        RightKey    十字キー →
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
        Option      Optionボタン
        Share       Shareボタン
        Home        PSボタン
        TrackPad    タッチパッド押し込み


		使用例 :

		if(GamePadControl.Instance.Controller1.Upley)
		{
		    // 1Pコントローラーの十字キー↑が押されたら実行したい処理

		}