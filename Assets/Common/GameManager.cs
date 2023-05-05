using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager>
{
    /// <summary>
    ///戦績評価
    ///＊勝利条件＊
    /// 1.制限時間内に敵艦隊を撃破
    /// 2.味方が一隻も撃沈されない
    /// 3.敵艦隊を撃破
    /// </summary>
    public enum BattleScore
    {
        S,//勝利条件を全て達成
        A,//勝利条件が一つ欠ける
        B,//勝利条件が二つ欠ける
        C,//全滅
    }
    
    [System.NonSerialized] public BattleScore Result;
    [System.NonSerialized] public int TotalEXP;
    private void Update()
    {
        
    }
}
