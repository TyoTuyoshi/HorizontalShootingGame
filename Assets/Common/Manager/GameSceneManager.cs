using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Manager
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        //ゲームシーンのUI
        [SerializeField] private UIDocument uiDocument = null;
        
        /// <summary>
        ///戦績評価
        ///＊勝利条件＊
        /// 1.制限時間内に敵艦隊を撃破
        /// 2.味方が一隻も撃沈されない
        /// 3.敵艦隊を撃破
        /// </summary>
        enum BattleScore
        {
            S , //勝利条件を全て達成
            A, //勝利条件が一つ欠ける
            B, //勝利条件が二つ欠ける
            C, //全滅
        }

        //バトルの状態
        public enum BattleState
        {
            Start,
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
        private const float lim_time = 120.0f;

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
            //シーンのフェードアウト
            FadeManager.Instance.SceneObj.SceneFadeOUT(1.0f);
            InitState();
            //GameManager.Instance.SceneObj.SceneFadeIN("EditScene", 1.0f);
        }

        //チャージ技を発動するときのフラグ
        public bool[] IsCharge = { false, false, false };

        /// <summary>
        ///ステータス初期化
        /// </summary>
        private void InitState()
        {
            //出撃時の艦隊規模(艦船の数)
            first_cnt = player.MyShips.Count;
            Debug.Log($"Result = {Result.GetHashCode()}");

            string[] btn_names = { "btn_torpedo", "btn_cannon", "btn_air" };
            Button[] buttons = 
            {
                uiDocument.rootVisualElement.Query<Button>(btn_names[0]),
                uiDocument.rootVisualElement.Query<Button>(btn_names[1]),
                uiDocument.rootVisualElement.Query<Button>(btn_names[2]),
            };
            foreach (var btn in buttons.Select((v, i) => (v, i )))
            {
                btn.v.clicked += () =>
                {
                    IsCharge[btn.i] = (!IsCharge[btn.i]) ? true : false;
                };
            }
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

            //Debug.Log($"{IsCharge[0]} {IsCharge[1]} {IsCharge[2]}");
            
            //戦闘中は時間を加算
            if (battle_state == BattleState.Fighing) time += Time.deltaTime;
            //制限時間外になると評価低下
            if (time > lim_time && !score_flag[0])
            {
                Result++;
                score_flag[0] = true;
            }

            //プレイヤー側が撃沈されると評価低下
            if (player.MyShips.Count < first_cnt && !score_flag[1])
            {
                Result++;
                score_flag[1] = true;
            }

            //全滅すると最低評価
            if (player.Annihilation) Result = BattleScore.C;

            //Debug.Log($"Result = {Result.GetHashCode()}");
        }
        
        /// <summary>
        /// ボタンで指示できるチャージ技
        /// </summary>
        enum ChargeAttack
        {
            Torpedo,
            Battleship,
            Aircarrier,
        }

        //ボタンアクションの設定
        private void ButtonAction(int num)
        {
            switch ((ChargeAttack)num)
            {
                case ChargeAttack.Torpedo:
                    break;
                case ChargeAttack.Battleship:
                    break;
                case ChargeAttack.Aircarrier:
                    break;
            }
        }

    }
}