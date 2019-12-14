﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNotes : MonoBehaviour
{ 
    [SerializeField] NotesData notesdata;
    [SerializeField] NotesData2 notesdata2;

    private bool playeroneturn = true;

    int character_one_sequal = 0;
    int character_two_sequal = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnout());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CallSpawnNotes()
    {
        StartCoroutine(spawnout());
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
            NotesControl.Instance.PlayNotesOneShot(tmpdata.ntype,tmpstart, tmpend, ControllerNum.P1,tmpdata.speed);
        }
        if (playeroneturn) character_one_sequal++;
        else character_two_sequal++;
    }
    public MstNotesEntity GetDatabyId(int id)
    {
        if (playeroneturn)
        {
            switch (character_one_sequal)
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
        else
        {
            switch (character_two_sequal)
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
    }
}