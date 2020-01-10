using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calltest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, (NotesType)Random.Range(1, 6), new Vector3(-7, 3, 0), new Vector3(7, 3, 0), ControllerNum.P1, 1.0f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NotesControl.Instance.PlayNotesOneShot((NotesType)Random.Range(0, 6), new Vector3(-7, 3, 0), new Vector3(7, 3, 0), ControllerNum.P1,1.0f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            BooingControl.Instance.SetBooingPlayer(ControllerNum.P1);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            BooingControl.Instance.SetBooingPlayer(ControllerNum.P2);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            BooingControl.Instance.BooingSystemOff();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneControl.Instance.LoadScene("Result");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            NotesControl.Instance.StopNotes();
        }
    }
}
