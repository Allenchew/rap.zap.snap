using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMng : MonoBehaviour
{
    public static CharacterMng Instance;

    public List<int> SelectedCharacter = new List<int>();
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
    }
    
    void Update()
    {
    }
   
}
