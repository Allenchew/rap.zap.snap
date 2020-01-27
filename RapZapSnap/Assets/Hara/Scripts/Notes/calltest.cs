using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calltest : MonoBehaviour
{

    [SerializeField, Header("ノーツの再生回数")] private int callCount = 0;
    [SerializeField, Header("ノーツの呼び出し頻度")] private float callSpan = 1.0f;
    private bool callFlag = true;

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
            NotesControl.Instance.PlayNotesOneShot((NotesType)Random.Range(0, 6), new Vector3(-7, 0, 0), new Vector3(5, 0, 0), ControllerNum.P1,1.0f);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("TestStart");
            TestCallStart();
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
            if(callFlag == false) { return; }
            SceneControl.Instance.LoadScene("Result");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            NotesControl.Instance.StopNotes();
        }
    }

    private void TestCallStart()
    {
        if(callFlag == false) { return; }
        StartCoroutine(enumerator(callCount, callSpan));
    }

    private IEnumerator enumerator(int call, float span)
    {
        callFlag = false;
        int i = 0;
        while(i < callCount)
        {
            i++;
            int type;
            int notesType = Random.Range(0, 2);

            if(notesType < 1)
            {
                type = Random.Range(0, 6);
                NotesControl.Instance.PlayNotesOneShot((NotesType)type, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                NotesControl.Instance.PlayNotesOneShot((NotesType)type, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
            }
            else
            {
                type = Random.Range(0, 12);
                switch (type)
                {
                    case 0:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.CrossKey, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.CrossKey, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 1:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.TriangleKey, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.TriangleKey, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 2:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.UpArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.UpArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 3:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.DownArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.DownArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 4:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.LeftArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CircleKey, NotesType.LeftArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 5:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.TriangleKey, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.TriangleKey, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 6:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.UpArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.UpArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 7:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.DownArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.DownArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 8:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.LeftArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.CrossKey, NotesType.LeftArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 9:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.TriangleKey, NotesType.UpArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.TriangleKey, NotesType.UpArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    case 10:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.TriangleKey, NotesType.DownArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.TriangleKey, NotesType.DownArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                    default:
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.TriangleKey, NotesType.LeftArrow, new Vector3(-10, 3, 0), new Vector3(6, 3, 0), ControllerNum.P1);
                        NotesControl.Instance.PlayDoubleNotesOneShot(NotesType.TriangleKey, NotesType.LeftArrow, new Vector3(-10, -3, 0), new Vector3(6, -3, 0), ControllerNum.P2);
                        break;
                }
            }
            yield return new WaitForSeconds(span);
        }
        callFlag = true;
        Debug.Log("TestEnd");
    }
}
