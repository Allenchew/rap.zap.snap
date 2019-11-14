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
    UpArrow,        // PS4コントローラー[↑ボタン]
    DownArrow,      // PS4コントローラー[↓ボタン]
    LeftArrow,      // PS4コントローラー[←ボタン]
}

public class NotesModel : MonoBehaviour
{
    public NotesType NotesTypes { set; get; }

    // ノーツのスプライト画像リスト
    [SerializeField, Tooltip("ノーツの画像リスト")]
    private List<Sprite> notesSprites = new List<Sprite>();
    public List<Sprite> NotesSprites { get { return notesSprites; } }
}
