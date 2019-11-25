---NotesObject導入方法---

1. シーン上にNotesObjectを配置する

2. インスペクターで設定可能な項目
    
	MaxNotes              1度に画面上に表示できるノーツの最大数

    NotesSize    　       生成されるノーツのScale

	NotesSpriteAlpha      判定ノーツの透明度

	PerfectLength         ノーツのPerfect判定域（値が小さいほど判定がシビアになります）

	GoodLength            ノーツのGood判定域（値が小さいほど判定がシビアになります）

	BadLength             ノーツのBad判定域（値が小さいほど判定がシビアになります）


---使い方---
1. スクリプトから呼び出す場合、NotesControl.Instance.呼び出したい関数名 で呼び出せます

2. 呼び出せる関数一覧
    
	PlayNotesOneShot()        ノーツを指定した座標に１回再生する
	　　　　　　　　　　　　　※ NotesControl.Instance.PlayNotesOneShot(ノーツの開始座標, ノーツの判定座標, 1Pか2Pか, 判定位置まで移動に要する時間)


    GetResult()              ノーツの判定結果を取得できる
	                         ※ GetResult(0, InputController.PlayerOne) 1PのBadの数を取得する


	ResetResult()            ノーツの判定結果をリセットする
	                         ※ ResetResult(InputController.PlayerTwo) 2Pの判定結果をリセットする



---ノーツの入力（テストプレイ用）---

1P操作
    〇ボタン == Aキー
    ×ボタン == Sキー
    △ボタン == Dキー
    十字キー == 矢印キー

2P操作
    〇ボタン == Jキー
    ×ボタン == Kキー
    △ボタン == Lキー
    十字キー == テンキーの矢印キー
							  　 