using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノーツの入力要求キー
/// </summary>
public enum NotesType
{
    CircleKey,      // PS4コントローラー[○ボタン]
    CrossKey,       // PS4コントローラー[×ボタン]
    TriangleKey,    // PS4コントローラー[△ボタン]
    SquareKey,      // PS4コントローラー[□ボタン]
    UpArrow,        // PS4コントローラー[↑ボタン]
    DownArrow,      // PS4コントローラー[↓ボタン]
    LeftArrow,      // PS4コントローラー[←ボタン]
    RightArrow,     // PS4コントローラー[→ボタン]
}

public class NotesModel : MonoBehaviour
{
    // ノーツの判定ID
    [SerializeField, Tooltip("ノーツの入力要求キー")]
    private NotesType notesTypes;
    public NotesType NotesTypes { set { notesTypes = value; } get { return notesTypes; } }

    

    // ノーツのスプライト画像リスト
    [SerializeField, Tooltip("ノーツの画像リスト")]
    private List<Sprite> notesSprites = new List<Sprite>();
    public List<Sprite> NotesSprites { get { return notesSprites; } }
}
