using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : SingletonMonoBehaviour<SceneControl>
{
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
        sceneListCount = SceneManager.sceneCountInBuildSettings;
    }

    /// <summary>
    /// シーンの遷移
    /// </summary>
    /// <param name="sceneNumber">遷移先のシーン番号</param>
    /// <returns></returns>
    public AsyncOperation LoadScene(int sceneNumber)
    {

        if(sceneNumber >= sceneListCount || sceneNumber < 0)
        {
            Debug.LogError("指定された「シーン番号：" + sceneNumber + "」のシーンは存在しません！");
            return null;
        }

        // シーンの読み込み
        return SceneManager.LoadSceneAsync(sceneNumber);
    }

    /// <summary>
    /// シーンの遷移
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    /// <returns></returns>
    public AsyncOperation LoadScene(string sceneName)
    {
        // シーンの読み込み
        return SceneManager.LoadSceneAsync(sceneName);
    }
}
