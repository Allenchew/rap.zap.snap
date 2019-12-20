using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaGameManager : SingletonMonoBehaviour<AlphaGameManager>
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) == true)
        {
            SceneControl.Instance.LoadScene(0);
            NotesControl.Instance.StopNotes();
            BooingControl.Instance.BooingSystemOff();
        }
    }
}
