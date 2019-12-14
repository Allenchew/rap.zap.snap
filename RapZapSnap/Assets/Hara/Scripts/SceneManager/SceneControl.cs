using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneList
{
    Title = 0,
    CharacterSelect = 1,
    GameMain = 2,
    Result = 3
}

public class SceneControl : SingletonMonoBehaviour<SceneControl>
{
    [SerializeField, Tooltip("シーン名"), Header("現在のシーン情報")] private SceneList scene = SceneList.Title;

    private int[] sceneNumbers = null;

    private int sceneListCount = 0;    // 登録されているシーン数

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        int index = Enum.GetValues(typeof(SceneList)).Length;
        sceneListCount = SceneManager.sceneCountInBuildSettings;
        sceneNumbers = new int[index];
        for(int i = 0; i < sceneNumbers.Length; i++)
        {
            sceneNumbers[i] = i >= sceneListCount ? sceneListCount - 1 : i;
        }
    }

    /// <summary>
    /// シーンの遷移
    /// </summary>
    public AsyncOperation LoadScene()
    {
        int sceneNumber;

        switch (scene)
        {
            case SceneList.Title:
                sceneNumber = sceneNumbers[(int)SceneList.CharacterSelect];
                break;
            case SceneList.CharacterSelect:
                sceneNumber = sceneNumbers[(int)SceneList.GameMain];
                break;
            case SceneList.GameMain:
                sceneNumber = sceneNumbers[(int)SceneList.Result];
                break;
            case SceneList.Result:
                sceneNumber = sceneNumbers[(int)SceneList.Title];
                break;
            default:
                return null;
        }

        // シーンの読み込み
        return SceneManager.LoadSceneAsync(sceneNumber);
    }
}
