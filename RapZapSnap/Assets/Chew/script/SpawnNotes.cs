using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MaskData
{
    public int RunCount;
    public bool OnceEffect;
    public bool StartEffect;
    public bool EndEffect;
    public int EffectIndex;
    public float EffectDelay;
    public void Set(int runcount ,bool SE, bool EE,  int Eindex, bool onceEf, float effectdelay)
    {
        RunCount = runcount;
        StartEffect = SE;
        EndEffect = EE;
        EffectIndex = Eindex;
        OnceEffect = onceEf;
        EffectDelay = effectdelay;
    }
}


public class SpawnNotes : MonoBehaviour
{
    public static SpawnNotes Instance;
    
    public GameObject[] LyricsPrefabs;

    [SerializeField] NotesData notesdata;

    private bool runningnotes = false;
    private ControllerNum currentplayer;
    private Character currentcharacter;
    int[] character_sequal = new int[2];

    public GameObject tmplyrics;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        character_sequal[0] = 0;
        character_sequal[1] = 2;
    }
    public void CallSpawnNotes()
    {
        if (!runningnotes)
        {
            runningnotes = true;
            currentplayer = MainGameManager.instance.currentplayer;
            currentcharacter = GameData.Instance.GetCharacterData(currentplayer);
            int tmpbgmIndex = ((int)(GameData.Instance.GetCharacterData(currentplayer))-1)*3;
            BgmManager.Instance.StartPlay(tmpbgmIndex+character_sequal[(int)currentplayer]);
            tmplyrics.SetActive(true);
            //Instantiate(LyricsPrefabs[tmpbgmIndex + character_sequal[(int)currentplayer]]);
            StartCoroutine(spawnout());
        }
    }
    IEnumerator spawnout()
    {
          for(int i = 0;i< 18; i++)
          {
              MstNotesEntity tmpdata;
              tmpdata = GetDatabyId(i+1);
              Vector3 tmpstart = new Vector3(11, 4, 0);
              Vector3 tmpend = new Vector3(tmpdata.endposX, 4, 0);
              yield return new WaitForSeconds(tmpdata.delaytime);
              NotesControl.Instance.PlayNotesOneShot(tmpdata.ntype,tmpstart, tmpend, currentplayer,tmpdata.speed);
          }
          character_sequal[(int)currentplayer]++;
          BgmManager.Instance.StopPlay();
          runningnotes = false;
    }
    public MstNotesEntity GetDatabyId(int id)
    {
       if (currentcharacter == Character.Tokiwa)
        {
            switch (character_sequal[(int)currentplayer])
            {
                case 0:
                    return notesdata.tokiwa1.Find(entity => entity.id == id);
                case 1:
                    return notesdata.tokiwa2.Find(entity => entity.id == id);
                case 2:
                    return notesdata.tokiwa3.Find(entity => entity.id == id);
                default:
                    return null;
            }
        }
        else if(currentcharacter == Character.Hajime)
        {
            switch (character_sequal[(int)currentplayer])
            {
                case 0:
                    return notesdata.hajime1.Find(entity => entity.id == id);
                case 1:
                    return notesdata.hajime2.Find(entity => entity.id == id);
                case 2:
                    return notesdata.hajime3.Find(entity => entity.id == id);
                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    }
   
}
