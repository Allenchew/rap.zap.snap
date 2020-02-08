using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleControl : MonoBehaviour
{
    [SerializeField, Header("お邪魔用パーティクル")] private ParticleSystem[] booingParticle = null;
    [SerializeField, Header("パーティクルの描画用Image")] private RawImage rawImage = null;
    public int GetParticleIndex { get { return booingParticle.Length; } }

    /// <summary>
    /// お邪魔パーティクルの再生
    /// </summary>
    /// <param name="index">パーティクルの番号</param>
    /// <param name="size">パーティクルのサイズ</param>
    public void ParticlePlay(int index, float size)
    {
        if (index < 0 || index >= booingParticle.Length) { return; }

        ParticleSystem.MainModule mainModule = booingParticle[index].main;

        mainModule.startSize = size;

        booingParticle[index].Emit(1);
    }

    /// <summary>
    /// パーティクルの色を設定
    /// </summary>
    /// <param name="particleColor">色の情報</param>
    public void SetColor(Color particleColor)
    {
        rawImage.color = particleColor;
    }

    /// <summary>
    /// パーティクルの再生を止める
    /// </summary>
    public void StopParticle()
    {
        foreach (var particle in booingParticle)
        {
            if (particle.IsAlive(true) == true)
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    /// <summary>
    /// パーティクルの再生座標を取得
    /// </summary>
    /// <param name="index">パーティクル番号</param>
    /// <returns></returns>
    public Vector3 GetPlayPosition(int index)
    {
        return booingParticle[index].transform.position;
    }
}
