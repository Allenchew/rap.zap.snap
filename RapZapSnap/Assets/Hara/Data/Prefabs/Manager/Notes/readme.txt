---NotesObjectの導入方法---

シーン上にNotesObjectを配置してください
※同じくシーン上にGameDataObjectも配置してください。


---主な機能---

NotesControl.Instance.PlayNotesOneShot(再生するノーツの種類, ノーツの移動開始座標, ノーツの判定座標, プレイヤー番号, 移動の所要時間)

呼び出せるノーツの種類

NotesType.CircleKey      〇ボタンノーツ
NotesType.CrossKey       ×ボタンノーツ
NotesType.TriangleKey    △ボタンノーツ
NotesType.UpArrow        ↑ボタンノーツ
NotesType.DownArrow      ↓ボタンノーツ
NotesType.LeftArrow      ←ボタンノーツ

NotesControl.Instance.PlayNotesOneShot(NotesType.CircleKey, new Vector3(0, 0, 0), new Vector3(3, 3, 0), ControllerNum.P1, 2.0f)
    => 座標(0, 0, 0)から座標(3, 3, 0)を2秒間かけて〇ボタンノーツが移動する処理


NotesControl.Instance.StopNotes() => ノーツの再生を全て終了させる処理