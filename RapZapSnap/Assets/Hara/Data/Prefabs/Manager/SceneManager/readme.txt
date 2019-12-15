---SceneManagerの導入方法---

シーン上にSceneManagerオブジェクトを配置してください

シーンのビルドインデックスの並び順を

0.タイトルシーン
1.キャラクター選択シーン
2.ゲームシーン
3.リザルトシーン

にしてください。


---主な機能---

SceneControl.Instance.LoadScene(遷移先のシーン) => シーンの遷移を行う処理
    
	SceneControl.Instance.LoadScene(SceneList.Title) => タイトルシーンに遷移

	※シーン遷移はビルドインデックスの順番を参照するので、シーンの並び順を注意してください。