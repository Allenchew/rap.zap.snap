---GameDataObjectの導入方法---

シーン上にGameDataObjectを配置してください
※シーン上にNotesObjectや、BooingSystemオブジェクトが配置されている場合は必ず配置してください。


---主な機能---

1. GameData.Instance.SetCharacterData(プレイヤー番号, 選択したキャラクター)
    => キャラクター選択で各プレイヤーがどのキャラクターを選択したかをデータとして保存しておく処理

	1Pが常盤壮太を選んだ場合 => GameData.Instance.SetCharacterData(ControllerNum.P1, Character.Tokiwa)
	2Pが先崎茉莉を選んだ場合 => GameData.Instance.SetCharacterData(ControllerNum.P2, Character.Mari)



2. GameData.Instance.GetCharacterData(プレイヤー番号) => 各プレイヤーが選択したキャラクターの情報を取得
   
	GameData.Instance.GetCharacterData(ControllerNum.P1) => 1Pが選択したキャラクターの情報を取得



3. GameData.Instance.PlusTotalScore(プレイヤー番号, 増加量) => 各プレイヤーのトータルスコアを加算する処理

	GameData.Instance.PlusTotalScore(ControllerNum.P1, 1000) => 1Pのトータルスコアを1000加算



4. GameData.Instance.GetTotalScore(プレイヤー番号) => 各プレイヤーのトータルスコアを取得

	GameData.Instance.GetTotalScore(ControllerNum.P1) => 1Pのトータルスコアを取得



5. GameData.Instance.GetRapScore(プレイヤー番号) => 各プレイヤーのRapスコアを取得

	GameData.Instance.GetRapScore(ControllerNum.P1) => 1PのRapスコアを取得



6. GameData.Instance.GetZapScore(プレイヤー番号) => 各プレイヤーのZapスコアを取得

	GameData.Instance.GetZapScore(ControllerNum.P1) => 1PのZapスコアを取得



7. GameData.Instance.GetSnapScore(プレイヤー番号) => 各プレイヤーのSnapスコアを取得

	GameData.Instance.GetSnapScore(ControllerNum.P1) => 1PのSnapスコアを取得



8. GameData.Instance.ResetScore(プレイヤー番号) => 各プレイヤーのスコア情報を初期化する処理

	GameData.Instance.ResetScore(ControllerNum.P1) => 1Pのスコア情報を初期化