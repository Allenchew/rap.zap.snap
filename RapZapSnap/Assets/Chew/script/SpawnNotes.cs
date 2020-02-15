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
    private int[] localsequal = new int[2];

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        localsequal = MainGameManager.instance.character_sequal;
    }
    public void CallSpawnNotes()
    {
        if (!runningnotes)
        {
            runningnotes = true;
            currentplayer = MainGameManager.instance.currentplayer;
            currentcharacter = GameData.Instance.GetCharacterData(currentplayer);
            int tmpbgmIndex = ((int)(GameData.Instance.GetCharacterData(currentplayer)))*3;
            BgmManager.Instance.StartPlay(tmpbgmIndex+MainGameManager.instance.character_sequal[(int)currentplayer]);
            
            Instantiate(LyricsPrefabs[tmpbgmIndex + MainGameManager.instance.character_sequal[(int)currentplayer]]);

            // お邪魔のシステムを起動
            BooingControl.Instance.SetBooingPlayer(currentplayer == ControllerNum.P1 ? ControllerNum.P2 : ControllerNum.P1);

            StartCoroutine(spawnout());
        }
    }
    IEnumerator spawnout()
    {
        localsequal = MainGameManager.instance.character_sequal;
        int ulfaindex;
        ulfaindex = currentcharacter == Character.Mari? 2:0;
        for (int i = 0;i< 18+ulfaindex; i++)
        {
              MstNotesEntity tmpdata;
              tmpdata = GetDatabyId(i+1);
              Vector3 tmpstart = new Vector3(11, 4, 0);
              Vector3 tmpend = new Vector3(tmpdata.endposX, 4, 0);
              yield return new WaitForSeconds(tmpdata.delaytime);
              NotesControl.Instance.PlayNotesOneShot(tmpdata.ntype,tmpstart, tmpend, currentplayer,tmpdata.speed);
        }

        runningnotes = false;
    }
    public MstNotesEntity GetDatabyId(int id)
    {
       if (currentcharacter == Character.Tokiwa)
        {
            switch (localsequal[(int)currentplayer])
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
            switch (localsequal[(int)currentplayer])
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
        else if (currentcharacter == Character.Mari)
        {
            switch (localsequal[(int)currentplayer])
            {
                case 0:
                    return notesdata.mari1.Find(entity => entity.id == id);
                case 1:
                    return notesdata.mari2.Find(entity => entity.id == id);
                case 2:
                    return notesdata.mari3.Find(entity => entity.id == id);
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
