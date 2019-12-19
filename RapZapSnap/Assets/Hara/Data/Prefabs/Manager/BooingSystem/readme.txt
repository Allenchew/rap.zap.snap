---BooingSystemの導入方法---

シーン上にBooingSystemオブジェクトを配置してください
※同じくシーン上にGameDataObjectを必ず配置してください。


---主な機能---

BooingControl.Instance.SetBooingPlayer(プレイヤー番号)
    => ブーイングシステムを使うプレイヤーを設定する処理

	BooingControl.Instance.SetBooingPlayer(ControllerNum.P1) => 1Pがブーイングシステムを使えるようになる

	ゲーム開始時に、1Pが先攻なら2Pのブーイングシステムを使えるようにしてください。
	ターンが切り替わったら、先攻プレイヤーのブーイングシステムを使えるようにしてください。


BooingControl.Instance.BooingSystemOff() => ブーイングシステムを使えないようにする処理
    
	ゲームが終了したら、この処理を実行してください。