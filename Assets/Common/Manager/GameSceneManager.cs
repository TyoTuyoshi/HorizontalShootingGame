using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        /// <summary>
        ///戦績評価
        ///＊勝利条件＊
        /// 1.制限時間内に敵艦隊を撃破
        /// 2.味方が一隻も撃沈されない
        /// 3.敵艦隊を撃破
        /// </summary>
        enum BattleScore
        {
            S = 0, //勝利条件を全て達成
            A, //勝利条件が一つ欠ける
            B, //勝利条件が二つ欠ける
            C, //全滅
        }

        //バトルの状態
        public enum BattleState
        {
            Start = 0,
            Fighing,
            Finish
        }

        //戦績評価
        //最高評価からの減点方式
        private BattleScore Result = BattleScore.S;

        //総計経験値
        private int TotalEXP;

        //プレイヤー側
        [SerializeField] private Commander player;

        //戦闘開始時のプレイヤーの艦船数
        private int first_cnt;

        //敵側
        [SerializeField] private EnemyCommander enemy;

        //制限時間二分以内
        private const float lim_time = 10.0f;

        //進行時間
        private float time = 0.0f;

        //バトルステート
        [SerializeField] private BattleState battle_state = BattleState.Start;
        public BattleState State
        {
            get { return battle_state; }
        }


        //バトルスコア判定用フラグ(Updateの足止め用)
        private bool[] score_flag = { false, false, false };

        private void Start()
        {
            InitState();
        }

        /// <summary>
        ///ステータス初期化
        /// </summary>
        private void InitState()
        {
            first_cnt = player.KANTAI.Count;
            Debug.Log($"Result = {Result.GetHashCode()}");
        }

        private void Update()
        {
            UpdateGame();
        }

        /// <summary>
        /// ゲーム更新
        /// </summary>
        private void UpdateGame()
        {
            //敵を全滅しない間は戦闘中　全滅したら終了
            battle_state = (enemy.Annihilation) ? BattleState.Finish : BattleState.Fighing;
            
            //戦闘中は時間を加算
            if (battle_state == BattleState.Fighing) time += Time.deltaTime;
            //制限時間外になると評価低下
            if (time > lim_time && !score_flag[0])
            {
                Result++;
                score_flag[0] = true;
            }

            //プレイヤー側が撃沈されると評価低下
            if (player.KANTAI.Count < first_cnt && !score_flag[1])
            {
                Result++;
                score_flag[1] = true;
            }

            //全滅すると最低評価
            if (player.Annihilation) Result = BattleScore.C;

            Debug.Log($"Result = {Result.GetHashCode()}");
        }
        
    }
}