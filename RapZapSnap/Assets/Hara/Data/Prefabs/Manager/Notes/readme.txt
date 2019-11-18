2019/11/19 更新

---NotesObject導入方法---
1. シーン上にNotesObjectを配置する

2. インスペクターで設定可能な項目
    
	NotesUIMode    true:  ノーツをUIとして生成
	               false: ノーツをSpriteとして生成

    NotesSize    　       生成されるノーツのScale

	NotesSpriteAlpha      判定ノーツの透明度

	PerfectLength         ノーツのPerfect判定域（値が小さいほど判定がシビアになります）

	GoodLength            ノーツのGood判定域（値が小さいほど判定がシビアになります）

	BadLength             ノーツのBad判定域（値が小さいほど判定がシビアになります）


---使い方---
1. スクリプトから呼び出す場合、NotesControl.Instance.呼び出したい関数名 で呼び出せます

2. 呼び出せる関数一覧
    
	PlayNotesOneShot()        ノーツを指定した座標に１回再生する
	　　　　　　　　　　　　　※ 引数をGameObjectにすることで、そのオブジェクトの座標にノーツを再生することも可能


	PlayNotes()               ノーツを指定した座標に指定した回数だけ再生する
	                          ※ NotesControl.Instance.PlayNotes(new Vector3(-5, -5, 0), new Vector3(5, 5, 0), InputController.PlayerOne, 1.5f, 15, 1.0f)
							     ノーツを(-5, -5, 0)から(5, 5, 0)までを1.5秒間で移動する処理を1秒ごとで再生し、15回繰り返す処理
							  ※ 引数にVector3[]や、GameObject[]を宣言することも可能
							     GameObject[]として宣言する場合、シーン上に適当なGameObjectを配置しておき配列データに格納しておくと、その配列データ
								 を宣言した際にそのGameObjectの座標にノーツが再生されるようにすることもできる


    GetResult()              ノーツの判定結果を取得できる
	                         ※ GetResult(0, InputController.PlayerOne) 1PのBadの数を取得する


	ResetResult()            ノーツの判定結果をリセットする
	                         ※ ResetResult(InputController.PlayerTwo) 2Pの判定結果をリセットする



---ノーツの入力（テストプレイ用）---

〇ボタン == Aキー
×ボタン == Sキー
△ボタン == Dキー
十字キー == 矢印キー
							  　 