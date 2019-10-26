using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesModel : MonoBehaviour
{
    // ノーツの判定ID
    [SerializeField, Tooltip("ノーツの入力要求キー")]
    private NotesType notesTypes;
    public NotesType NotesTypes { set { notesTypes = value; } get { return notesTypes; } }

    public enum NotesType
    {
        CircleKey,      // PS4コントローラー[○ボタン]
        CrossKey,       // PS4コントローラー[×ボタン]
        TriangleKey,    // PS4コントローラー[△ボタン]
        SquareKey,      // PS4コントローラー[□ボタン]
    }

    // ノーツのスプライト画像リスト
    [SerializeField, Tooltip("ノーツの画像リスト")]
    private List<Sprite> notesSprites = new List<Sprite>();
    public List<Sprite> NotesSprites { get { return notesSprites; } }
}
