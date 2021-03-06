﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択キャラクター
/// </summary>
public enum Character
{
    Mari,
    Tokiwa,
    Hajime
}

public class GameData : SingletonMonoBehaviour<GameData>
{
    [System.Serializable]
    private struct Data
    {
        // 各プレイヤーの選んだキャラクターの情報
        public Character SelectCharacter;

        // 各プレイヤーのトータルスコア
        public int TotalScore;

        // 各プレイヤーのノーツ判定の数
        public int Rap, Zap, Snap;
    }

    [SerializeField, Tooltip("1Pのデータ")] private Data data_P1;
    [SerializeField, Tooltip("2Pのデータ")] private Data data_P2;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// GameDataの初期化
    /// </summary>
    private void Init()
    {
        ResetScore(ControllerNum.P1);
        ResetScore(ControllerNum.P2);
    }

    /// <summary>
    /// 選択したキャラクター情報を保存する
    /// </summary>
    /// <param name="id">選んだプレイヤー</param>
    /// <param name="character">選んだキャラクター</param>
    public void SetCharacterData(ControllerNum id, Character character)
    {
        if(id == ControllerNum.P1)
        {
            data_P1.SelectCharacter = character;
        }
        else
        {
            data_P2.SelectCharacter = character;
        }
    }

    /// <summary>
    /// 各プレイヤーが選択したキャラクターの情報を取得
    /// </summary>
    /// <param name="id">キャラ情報を取得したいプレイヤー番号</param>
    /// <returns></returns>
    public Character GetCharacterData(ControllerNum id)
    {
        if(id == ControllerNum.P1)
        {
            return data_P1.SelectCharacter;
        }
        else
        {
            return data_P2.SelectCharacter;
        }
    }

    /// <summary>
    /// キャラクター情報の取得
    /// </summary>
    /// <param name="isWinner">true: 勝利プレイヤーのキャラクター情報, false: 敗北プレイヤーのキャラクター情報</param>
    /// <returns></returns>
    public Character GetCharacterData(bool isWinner)
    {
        if(data_P1.TotalScore > data_P2.TotalScore)
        {
            return isWinner == true ? data_P1.SelectCharacter : data_P2.SelectCharacter;
        }
        else if(data_P1.TotalScore < data_P2.TotalScore)
        {
            return isWinner == true ? data_P2.SelectCharacter : data_P1.SelectCharacter;
        }
        else
        {
            if(data_P1.Rap > data_P2.Rap)
            {
                return isWinner == true ? data_P1.SelectCharacter : data_P2.SelectCharacter;
            }
            else if(data_P1.Rap < data_P2.Rap)
            {
                return isWinner == true ? data_P2.SelectCharacter : data_P1.SelectCharacter;
            }
            else
            {
                if(data_P1.Rap + data_P1.Zap >= data_P2.Rap + data_P2.Zap)
                {
                    return isWinner == true ? data_P1.SelectCharacter : data_P2.SelectCharacter;
                }
                else
                {
                    return isWinner == true ? data_P2.SelectCharacter : data_P1.SelectCharacter;
                }
            }
        }
    }

    /// <summary>
    /// 勝利プレイヤー情報を取得
    /// </summary>
    /// <returns></returns>
    public ControllerNum GetWinnerPlayer()
    {
        if (data_P1.TotalScore > data_P2.TotalScore)
        {
            return ControllerNum.P1;
        }
        else if (data_P1.TotalScore < data_P2.TotalScore)
        {
            return ControllerNum.P2;
        }
        else
        {
            if (data_P1.Rap > data_P2.Rap)
            {
                return ControllerNum.P1;
            }
            else if (data_P1.Rap < data_P2.Rap)
            {
                return ControllerNum.P2;
            }
            else
            {
                if (data_P1.Rap + data_P1.Zap >= data_P2.Rap + data_P2.Zap)
                {
                    return ControllerNum.P1;
                }
                else
                {
                    return ControllerNum.P2;
                }
            }
        }
    }

    /// <summary>
    /// トータルスコアを加算する
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    /// <param name="increase">スコアの増加量</param>
    public void PlusTotalScore(ControllerNum id, int increase)
    {
        if(id == ControllerNum.P1)
        {
            data_P1.TotalScore += increase;
        }
        else
        {
            data_P2.TotalScore += increase;
        }
    }

    /// <summary>
    /// ノーツの判定結果を加算する
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    /// <param name="index">0: Snap, 1: Zap, 2: Rap, それ以外は無効</param>
    public void PlusNotesResult(ControllerNum id, int index)
    {
        switch (index)
        {
            case 0:
                _ = id == ControllerNum.P1 ? data_P1.Snap++ : data_P2.Snap++;
                return;
            case 1:
                _ = id == ControllerNum.P1 ? data_P1.Zap++ : data_P2.Zap++;
                return;
            case 2:
                _ = id == ControllerNum.P1 ? data_P1.Rap++ : data_P2.Rap++;
                return;
            default:
                return;
        }
    }

    /// <summary>
    /// トータルスコアの取得
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    /// <returns></returns>
    public int GetTotalScore(ControllerNum id)
    {
        if(id == ControllerNum.P1)
        {
            return data_P1.TotalScore;
        }
        else
        {
            return data_P2.TotalScore;
        }
    }

    /// <summary>
    /// トータルスコアの取得
    /// </summary>
    /// <param name="isWinner">true: 勝利プレイヤーのトータルスコア, false: 敗北プレイヤーのトータルスコア</param>
    /// <returns></returns>
    public int GetTotalScore(bool isWinner)
    {
        if (data_P1.TotalScore > data_P2.TotalScore)
        {
            return isWinner == true ? data_P1.TotalScore : data_P2.TotalScore;
        }
        else if (data_P1.TotalScore < data_P2.TotalScore)
        {
            return isWinner == true ? data_P2.TotalScore : data_P1.TotalScore;
        }
        else
        {
            if (data_P1.Rap > data_P2.Rap)
            {
                return isWinner == true ? data_P1.TotalScore : data_P2.TotalScore;
            }
            else if (data_P1.Rap < data_P2.Rap)
            {
                return isWinner == true ? data_P2.TotalScore : data_P1.TotalScore;
            }
            else
            {
                if (data_P1.Rap + data_P1.Zap >= data_P2.Rap + data_P2.Zap)
                {
                    return isWinner == true ? data_P1.TotalScore : data_P2.TotalScore;
                }
                else
                {
                    return isWinner == true ? data_P2.TotalScore : data_P1.TotalScore;
                }
            }
        }
    }

    /// <summary>
    /// ノーツの判定結果の取得
    /// </summary>
    /// <param name="isWinner">true: 勝利プレイヤーのノーツ判定, false: 敗北プレイヤーのノーツ判定</param>
    /// <param name="index">0: Snap, 1: Zap, 2: Rap, それ以外は無効</param>
    /// <returns></returns>
    public int GetNotesResult(bool isWinner, int index)
    {
        Data data;

        if (data_P1.TotalScore > data_P2.TotalScore)
        {
            data = isWinner == true ? data_P1 : data_P2;
        }
        else if (data_P1.TotalScore < data_P2.TotalScore)
        {
            data = isWinner == true ? data_P2 : data_P1;
        }
        else
        {
            if (data_P1.Rap > data_P2.Rap)
            {
                data = isWinner == true ? data_P1 : data_P2;
            }
            else if (data_P1.Rap < data_P2.Rap)
            {
                data = isWinner == true ? data_P2 : data_P1;
            }
            else
            {
                if (data_P1.Zap >= data_P2.Zap)
                {
                    data = isWinner == true ? data_P1 : data_P2;
                }
                else
                {
                    data = isWinner == true ? data_P2 : data_P1;
                }
            }
        }

        switch (index)
        {
            case 0:
                return data.Snap;
            case 1:
                return data.Zap;
            case 2:
                return data.Rap;
            default:
                return 0;
        }
    }

    /// <summary>
    /// 指定したプレイヤーのRap(「良」判定)スコアを取得
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    /// <returns></returns>
    public int GetRapScore(ControllerNum id)
    {
        if(id == ControllerNum.P1)
        {
            return data_P1.Rap;
        }
        else
        {
            return data_P2.Rap;
        }
    }

    /// <summary>
    /// 指定したプレイヤーのZap(「可」判定)スコアを取得
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    /// <returns></returns>
    public int GetZapScore(ControllerNum id)
    {
        if (id == ControllerNum.P1)
        {
            return data_P1.Zap;
        }
        else
        {
            return data_P2.Zap;
        }
    }

    /// <summary>
    /// 指定したプレイヤーのSnap(「不可」判定)スコアを取得
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    /// <returns></returns>
    public int GetSnapScore(ControllerNum id)
    {
        if (id == ControllerNum.P1)
        {
            return data_P1.Snap;
        }
        else
        {
            return data_P2.Snap;
        }
    }

    /// <summary>
    /// スコアのリセット
    /// </summary>
    /// <param name="id">プレイヤー番号</param>
    public void ResetScore(ControllerNum id)
    {
        if(id == ControllerNum.P1)
        {
            data_P1.TotalScore = 0;
            data_P1.Rap = 0;
            data_P1.Zap = 0;
            data_P1.Snap = 0;
        }
        else
        {
            data_P2.TotalScore = 0;
            data_P2.Rap = 0;
            data_P2.Zap = 0;
            data_P2.Snap = 0;
        }
    }
}
