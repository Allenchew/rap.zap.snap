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
    public GameObject SEplayer;
    public GameObject ShowTurn;
    public GameObject TutorialPic;
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
        PvVideoPlayer = new GameObject[3] { VideoPlayermari, VideoPlayertokiwa, VideoPlayerhajime };
        character_sequal[0] = 0;
        character_sequal[1] = 0;
        StartCoroutine(ShowTurorial());
    }

    private IEnumerator DoEndCoroutine()
    {
        // ノーツの再生が完了するまで待つ
        while(NotesControl.Instance.NotesIsPlaying(currentplayer) == true)
        {
            yield return null;
        }

        roundCounter++;

        // お邪魔の再生を止める
        BooingControl.Instance.BooingSystemOff();

        // ノーツの再生を止める
        NotesControl.Instance.StopAllNotes();

        MainGameManager.instance.character_sequal[(int)currentplayer]++;
        //currentplayer = 1 - currentplayer;
        currentplayer = currentplayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1;
        if (roundCounter > 5)
        {
            SceneManager.LoadScene(3);
            yield break;
        }

        StartCoroutine(SwitchPlayer());
    }

    /// <summary>
    /// ターン終了時に実行
    /// </summary>
    public void EndAction()
    {
        StartCoroutine(DoEndCoroutine());
    }

    IEnumerator SwitchPlayer()
    {
        int tmpindex = (int)GameData.Instance.GetCharacterData(currentplayer);
        f_ShowingTurn = true;
        Debug.Log(tmpindex);
        ShowTurn.GetComponent<changeturn>().StartMove(tmpindex, character_sequal[(int)currentplayer]);
        yield return new WaitUntil(() => f_ShowingTurn == false);
        SpawnNotes.Instance.CallSpawnNotes();
        foreach(GameObject Child in PvVideoPlayer)
        {
            Child.SetActive(false);
        }
        PvVideoPlayer[tmpindex].SetActive(true);
        PvVideoPlayer[tmpindex].GetComponent<PvStorage>().SetThisVideo(character_sequal[(int)currentplayer]);
    }
    IEnumerator Startup()
    {
        yield return new WaitForSeconds(1.0f);
        int tmpindex = (int)GameData.Instance.GetCharacterData(currentplayer);
        f_ShowingTurn = true;
        Debug.Log(tmpindex);
        ShowTurn.GetComponent<changeturn>().StartMove(tmpindex,character_sequal[(int)currentplayer]);
        yield return new WaitUntil(() => f_ShowingTurn == false);
        foreach (GameObject Child in PvVideoPlayer)
        {
            Child.SetActive(false);
        }
        PvVideoPlayer[tmpindex].SetActive(true);
        SpawnNotes.Instance.CallSpawnNotes();
        PvVideoPlayer[tmpindex].GetComponent<PvStorage>().SetThisVideo(character_sequal[(int)currentplayer]);
    }
    IEnumerator ShowTurorial()
    {
        SEplayer.GetComponent<SEContainer>().SetThisSe(0);
        for(float i = 0; i<1.01f;i+= 0.05f)
        {
            TutorialPic.transform.localPosition = Vector3.Lerp(new Vector3(0, 1080, 0), new Vector3(0, 0, 0), i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitUntil(() => Input.anyKey);
        SEplayer.GetComponent<SEContainer>().SetThisSe(0);
        for (float i = 0; i < 1.01f; i += 0.05f)
        {
            TutorialPic.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -1080, 0), i);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(Startup());
    }
}
