using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class calltest : MonoBehaviour
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private GameObject startObj = null;
    [SerializeField]
    private GameObject endObj = null;

    [SerializeField]
    private GameObject[] staroObjs = null;
    [SerializeField]
    private GameObject[] endObjs = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            NotesControl.Instance.ResetResult(ControllerNum.P1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            NotesControl.Instance.ResetResult(ControllerNum.P2);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NotesControl.Instance.PlayNotesOneShot(NotesType.CircleKey, new Vector3(-5, -5, 0), new Vector3(-5, 3, 0), duration: 0.75f);
            NotesControl.Instance.PlayNotesOneShot(NotesType.CrossKey, new Vector3(5, -5, 0), new Vector3(5, 3, 0), ControllerNum.P2, 0.75f);
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
            Debug.Log("ユーザー1  Perfect : " + NotesControl.Instance.GetResult(2, ControllerNum.P1) + "  Good : " + NotesControl.Instance.GetResult(1, ControllerNum.P1) + "  Bad : " + NotesControl.Instance.GetResult(0, ControllerNum.P1));
            Debug.Log("ユーザー2  Perfect : " + NotesControl.Instance.GetResult(2, ControllerNum.P2) + "  Good : " + NotesControl.Instance.GetResult(1, ControllerNum.P2) + "  Bad : " + NotesControl.Instance.GetResult(0, ControllerNum.P2));
            Debug.Log("ユーザー1  Total : " + NotesControl.Instance.GetResult(3, ControllerNum.P1));
            Debug.Log("ユーザー2  Total : " + NotesControl.Instance.GetResult(3, ControllerNum.P2));
        }
    }
}
