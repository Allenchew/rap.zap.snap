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
    [SerializeField] NotesData2 notesdata2;

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
        for(int i = 0;i< notesdata.Phrase1.Count; i++)
        {
            MstNotesEntity tmpdata;
            tmpdata = GetDatabyId(i+1);

            if (tmpdata.startposX == tmpdata.endposX && tmpdata.startposY == tmpdata.endposY)
            {
                yield return new WaitForSeconds(tmpdata.delaytime);
                continue;
            }
            Vector3 tmpstart = new Vector3(tmpdata.startposX, tmpdata.startposY, 0);
            Vector3 tmpend = new Vector3(tmpdata.endposX, tmpdata.endposY, 0);
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
                    return notesdata.Phrase1.Find(entity => entity.id == id);
                case 1:
                    return notesdata.Phrase2.Find(entity => entity.id == id);
                case 2:
                    return notesdata.Phrase3.Find(entity => entity.id == id);
                default:
                    return null;
            }
        }
        else if(currentcharacter == Character.Hajime)
        {
            switch (character_sequal[(int)currentplayer])
            {
                case 0:
                    return notesdata2.Phrase1.Find(entity => entity.id == id);
                case 1:
                    return notesdata2.Phrase2.Find(entity => entity.id == id);
                case 2:
                    return notesdata2.Phrase3.Find(entity => entity.id == id);
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
