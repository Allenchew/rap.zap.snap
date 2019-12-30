using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノーツの入力要求キー
/// </summary>
public enum NotesType
{
    CircleKey = 0,      // PS4コントローラー[○ボタン]
    CrossKey = 1,       // PS4コントローラー[×ボタン]
    TriangleKey = 2,    // PS4コントローラー[△ボタン]
    UpArrow = 3,        // PS4コントローラー[↑ボタン]
    DownArrow = 4,      // PS4コントローラー[↓ボタン]
    LeftArrow = 5,      // PS4コントローラー[←ボタン]
}

public class NotesModel : MonoBehaviour
{
    // ノーツのタイプ
    public NotesType NotesType1 { set; get; } = NotesType.CircleKey;
    public NotesType NotesType2 { set; get; } = NotesType.CircleKey;

    // Doubleノーツ用の判定ID
    public int NotesTypeNum { set; get; } = 0;
}
