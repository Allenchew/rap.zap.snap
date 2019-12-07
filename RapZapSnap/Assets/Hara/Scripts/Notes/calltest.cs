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
    [SerializeField] private int sceneNumber = 0;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NotesControl.Instance.PlayNotesOneShot(NotesType.CircleKey, new Vector3(-5, -5, 0), new Vector3(-5, 3, 0), ControllerNum.P1,0.75f);
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
            SceneManager.LoadScene(sceneNumber);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            NotesControl.Instance.StopNotes();
        }
    }
}
