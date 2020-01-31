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
    public GameObject backgroundpic;
    public GameObject VideoPlayertokiwa;
    public GameObject VideoPlayerhajime;
    public int[] character_sequal = new int[2];
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
        character_sequal[0] = 0;
        character_sequal[1] = 0;
        StartCoroutine(Startup());
    }
    
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
        for (float i = 0; i < 1.1f; i += 0.05f)
        {
            backgroundpic.GetComponent<Image>().color = Color.Lerp(lyricsbg[1-tmpindex], lyricsbg[tmpindex], i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1.0f);
        SpawnNotes.Instance.CallSpawnNotes();
        if (tmpindex == 0)
        {
            VideoPlayerhajime.SetActive(false);
            VideoPlayertokiwa.SetActive(true);
            VideoPlayertokiwa.GetComponent<VideoPlayer>().Play();
        }
        else
        {
            VideoPlayerhajime.SetActive(true);
            VideoPlayertokiwa.SetActive(false);
            VideoPlayerhajime.GetComponent<VideoPlayer>().Play();
        }
        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            backgroundpic.GetComponent<Image>().color = Color.Lerp(lyricsbg[tmpindex], Color.clear, i);
            yield return new WaitForSeconds(0.01f);
        }
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
        if (tmpindex == 0)
        {
            VideoPlayerhajime.SetActive(false);
            VideoPlayertokiwa.SetActive(true);
            VideoPlayertokiwa.GetComponent<VideoPlayer>().Play();
        }
        else
        {
            VideoPlayerhajime.SetActive(true);
            VideoPlayertokiwa.SetActive(false);
            VideoPlayerhajime.GetComponent<VideoPlayer>().Play();
        }

        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            backgroundpic.GetComponent<Image>().color = Color.Lerp(lyricsbg[tmpindex],Color.clear, i);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
