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
            NotesControl.Instance.ResetResult(InputController.PlayerOne);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            NotesControl.Instance.ResetResult(InputController.PlayerTwo);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NotesControl.Instance.PlayNotesOneShot(NotesType.CircleKey, new Vector3(-5, -5, 0), new Vector3(-5, 3, 0), duration: 0.75f);
            NotesControl.Instance.PlayNotesOneShot(NotesType.CrossKey, new Vector3(5, -5, 0), new Vector3(5, 3, 0), InputController.PlayerTwo, 0.75f);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("ユーザー1  Perfect : " + NotesControl.Instance.GetResult(2, InputController.PlayerOne) + "  Good : " + NotesControl.Instance.GetResult(1, InputController.PlayerOne) + "  Bad : " + NotesControl.Instance.GetResult(0, InputController.PlayerOne));
            Debug.Log("ユーザー2  Perfect : " + NotesControl.Instance.GetResult(2, InputController.PlayerTwo) + "  Good : " + NotesControl.Instance.GetResult(1, InputController.PlayerTwo) + "  Bad : " + NotesControl.Instance.GetResult(0, InputController.PlayerTwo));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //SceneManager.LoadScene(1);
        }
    }
}
