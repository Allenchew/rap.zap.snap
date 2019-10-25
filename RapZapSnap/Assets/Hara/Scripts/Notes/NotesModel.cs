using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesModel : MonoBehaviour
{
    // ノーツの判定ID
    [SerializeField, Range(0, 3)]
    private int notesID = 0;
    public int NotesID { set { notesID = value; } get { return notesID; } }
}
