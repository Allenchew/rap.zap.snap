using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private GameObject startObj = null;
    [SerializeField]
    private GameObject endObj = null;

    [SerializeField]
    private List<GameObject> staroObjs = new List<GameObject>();
    [SerializeField]
    private List<GameObject> endObjs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            NotesControl.Instance.CallNotes(new Vector3(-600, 800, 0), new Vector3(-600, -300, 0), InputController.PlayerOne, 0.6f, 30, 0.75f);
            //NotesControl.Instance.CallNotes(new Vector3(600, -800, 0), new Vector3(600, 300, 0), InputController.PlayerTwo, 0.6f, 30, 0.75f);
        }

        if (Input.GetKeyDown(KeyCode.R))
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
