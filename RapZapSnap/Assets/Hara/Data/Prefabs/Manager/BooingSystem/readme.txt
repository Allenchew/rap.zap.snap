---BooingSystemの導入方法---

シーン上にBooingSystemオブジェクトを配置してください
※同じくシーン上にGameDataObjectを必ず配置してください。


---主な機能---

BooingControl.Instance.SetBooingPlayer(プレイヤー番号)
    => ブーイングシステムを使うプレイヤーを設定する処理

	BooingControl.Instance.SetBooingPlayer(ControllerNum.P1) => 1Pがブーイングシステムを使えるようになる


BooingControl.Instance.BooingSystemOff() => ブーイングシステムを使えないようにする処理