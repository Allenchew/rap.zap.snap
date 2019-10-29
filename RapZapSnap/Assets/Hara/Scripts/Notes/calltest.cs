using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calltest : MonoBehaviour
{
    [SerializeField]
    private NotesModel.NotesType notesType;
    [SerializeField]
    private float duration;
    [SerializeField, Tooltip("ノーツの移動開始位置のリスト")]
    private List<Vector3> startPos = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) NotesControl.Instance.CallNotes(notesType, new Vector3(-5, 0, 0), new Vector3(5, 0, 0), duration);

        if (Input.GetKeyDown(KeyCode.I)) NotesControl.Instance.InputKey(NotesModel.NotesType.CircleKey);

        if (Input.GetKeyDown(KeyCode.A)) notesType = NotesModel.NotesType.CircleKey;
        if (Input.GetKeyDown(KeyCode.S)) notesType = NotesModel.NotesType.CrossKey;
        if (Input.GetKeyDown(KeyCode.D)) notesType = NotesModel.NotesType.TriangleKey;
        if (Input.GetKeyDown(KeyCode.W)) notesType = NotesModel.NotesType.SquareKey;

    }
}
