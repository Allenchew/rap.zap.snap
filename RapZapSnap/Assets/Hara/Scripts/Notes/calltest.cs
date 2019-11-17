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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if(timer >= span)
        {
            //NotesControl.Instance.CallNotes(NotesType.CircleKey, new Vector3(-5, 0, 0), new Vector3(5, 0, 0), TargetPlayer.PlayerTwo);
            timer = 0;
        }
        

        if (Input.GetKeyDown(KeyCode.G))
        {
            NotesControl.Instance.CallNotes(NotesType.UpArrow, new Vector3(-6, 10, 0), new Vector3(-6, -3, 0));
            NotesControl.Instance.CallNotes(NotesType.UpArrow, new Vector3(6, -10, 0), new Vector3(6, 3, 0), InputController.PlayerTwo);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            NotesControl.Instance.CallNotes(NotesType.UpArrow, new Vector3(-600, 800, 0), new Vector3(-600, -300, 0));
            NotesControl.Instance.CallNotes(NotesType.UpArrow, new Vector3(600, -800, 0), new Vector3(600, 300, 0), InputController.PlayerTwo);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            NotesControl.Instance.CallNotes(new Vector3(-600, 800, 0), new Vector3(-600, -300, 0), 30, 0.75f, InputController.PlayerOne, 0.65f);
            NotesControl.Instance.CallNotes(new Vector3(600, -800, 0), new Vector3(600, 300, 0), 30, 0.75f, InputController.PlayerTwo, 0.65f);
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
