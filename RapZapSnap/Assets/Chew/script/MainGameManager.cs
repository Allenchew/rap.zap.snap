using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public ControllerNum currentplayer;
    public GameObject ShowTurn;
    public GameObject VideoPlayertokiwa;
    public GameObject VideoPlayerhajime;
    public GameObject VideoPlayermari;
    public int[] character_sequal = new int[2];

    public bool f_ShowingTurn = false;

    private GameObject[] PvVideoPlayer;
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
        PvVideoPlayer = new GameObject[3] { VideoPlayertokiwa, VideoPlayerhajime, VideoPlayermari };
        character_sequal[0] = 0;
        character_sequal[1] = 0;
        StartCoroutine(Startup());
    }

    public void EndRun()
    {

        roundCounter++;

        MainGameManager.instance.character_sequal[(int)currentplayer]++;
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
        f_ShowingTurn = true;
        ShowTurn.GetComponent<changeturn>().StartMove(tmpindex, character_sequal[tmpindex]);
        yield return new WaitUntil(() => f_ShowingTurn == false);
        SpawnNotes.Instance.CallSpawnNotes();
        foreach(GameObject Child in PvVideoPlayer)
        {
            Child.SetActive(false);
        }
        PvVideoPlayer[tmpindex].SetActive(true);
        PvVideoPlayer[tmpindex].GetComponent<PvStorage>().SetThisVideo(character_sequal[tmpindex]);
    }
    IEnumerator Startup()
    {
        yield return new WaitForSeconds(1.0f);
        int tmpindex = (int)GameData.Instance.GetCharacterData(currentplayer) - 1;
        BooingControl.Instance.SetBooingPlayer(ControllerNum.P2);
        f_ShowingTurn = true;
        ShowTurn.GetComponent<changeturn>().StartMove(tmpindex,character_sequal[tmpindex]);
        yield return new WaitUntil(() => f_ShowingTurn == false);
        foreach (GameObject Child in PvVideoPlayer)
        {
            Child.SetActive(false);
        }
        PvVideoPlayer[tmpindex].SetActive(true);
        SpawnNotes.Instance.CallSpawnNotes();
        PvVideoPlayer[tmpindex].GetComponent<PvStorage>().SetThisVideo(character_sequal[tmpindex]);
    }
}
