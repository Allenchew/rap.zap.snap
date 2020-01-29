using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public ControllerNum currentplayer;
    public GameObject backgroundpic;
    public GameObject lyrics1;

    [SerializeField]
    public Color[] lyricsbg = new Color[2] { new Color(96, 15, 19, 255), new Color(13, 15, 13, 255) };

    private int roundCounter = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(Startup());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            backgroundpic.GetComponent<Image>().color = lyricsbg[0];
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            backgroundpic.GetComponent<Image>().color = lyricsbg[1];
        }
    }

    public void EndRun()
    {
        roundCounter++;
        currentplayer = 1 - currentplayer;
        if (roundCounter > 5)
        {
            BooingControl.Instance.BooingSystemOff();
            SceneManager.LoadScene(3);
            return;
        }
        StartCoroutine(SwitchPlayer());
    }
    IEnumerator SwitchPlayer()
    {
        int tmpindex = (int)GameData.Instance.GetCharacterData(currentplayer)-1;
        BooingControl.Instance.SetBooingPlayer(BooingControl.Instance.BooingPlayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1);
        for (float i = 0; i < 1.1f; i += 0.05f)
        {
            backgroundpic.GetComponent<Image>().color = Color.Lerp(lyricsbg[1-tmpindex], lyricsbg[tmpindex], i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1.0f);
        SpawnNotes.Instance.CallSpawnNotes();
    }
    IEnumerator Startup()
    {
        yield return new WaitForSeconds(1.0f);
        int tmpindex = (int)GameData.Instance.GetCharacterData(currentplayer) - 1;
        BooingControl.Instance.SetBooingPlayer(ControllerNum.P2);
        for (float i = 0; i < 1.1f; i += 0.05f)
        {
            backgroundpic.GetComponent<Image>().color = Color.Lerp(Color.white, lyricsbg[tmpindex], i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        SpawnNotes.Instance.CallSpawnNotes();
        lyrics1.SetActive(true);
    }
}
