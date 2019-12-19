---SceneManagerの導入方法---

シーン上にSceneManagerオブジェクトを配置してください

シーンのビルドインデックスの並び順を

0.タイトルシーン
1.キャラクター選択シーン
2.ゲームシーン
3.リザルトシーン

にしてください。


---主な機能---

SceneControl.Instance.LoadScene(遷移先のシーン番号(int型)) => シーンの遷移を行う処理
    
	SceneControl.Instance.LoadScene(0) => ビルドインデックスに登録した0番のシーンに遷移

	※ビルドインデックスの順番を参照するので、シーンの並び順を注意してください。


SceneControl.Instance.LoadScene(遷移先のシーン名(string型)) => シーンの遷移を行う処理
    
	SceneControl.Instance.LoadScene(Title) => 「Title」という名前で登録されたシーンに遷移