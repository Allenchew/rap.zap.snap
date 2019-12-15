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
    /// <param name="scene">遷移先のシーン</param>
    /// <returns></returns>
    public AsyncOperation LoadScene(SceneList scene)
    {
        int sceneNumber = (int)scene;

        if(sceneNumber >= sceneListCount)
        {
            Debug.LogError("指定された「シーン番号：" + sceneNumber + "(" + scene + ")」のシーンは存在しなかった為、代わりのシーンを読み込みました。");
        }

        // シーンの読み込み
        return SceneManager.LoadSceneAsync(sceneNumbers[sceneNumber]);
    }
}
