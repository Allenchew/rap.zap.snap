using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calltest : MonoBehaviour
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private List<Vector3> startPos = new List<Vector3>();

    [SerializeField]
    private float span = 0.0f;
    private float timer = 0.0f;
    [SerializeField]
    private MoveMode moveMode = MoveMode.Arrival;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer += Time.deltaTime;
        if(timer >= span)
        {
            NotesControl.Instance.CallNotes(NotesType.CircleKey, new Vector3(-5, 0, 0), new Vector3(5, 0, 0), duration);

        }
        */

        if (Input.GetKeyDown(KeyCode.G))
        {
            NotesControl.Instance.CallNotes(NotesType.CircleKey, new Vector3(-5, 0, 0), new Vector3(5, 0, 0), moveMode, duration);
        }

    }
}
