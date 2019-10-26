using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calltest : MonoBehaviour
{
    [SerializeField]
    private NotesModel.NotesType notesType;
    [SerializeField]
    private float duration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NotesControl.Instance.CallNotes(notesType, new Vector3(5, 5, 0), duration);
        }
        if (Input.GetKeyDown(KeyCode.A)) notesType = NotesModel.NotesType.CircleKey;
        if (Input.GetKeyDown(KeyCode.S)) notesType = NotesModel.NotesType.CrossKey;
        if (Input.GetKeyDown(KeyCode.D)) notesType = NotesModel.NotesType.TriangleKey;
        if (Input.GetKeyDown(KeyCode.W)) notesType = NotesModel.NotesType.SquareKey;

    }
}
