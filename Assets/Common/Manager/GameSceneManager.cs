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
        [Header("制限時間(秒)"), SerializeField] private float lim_time = 120.0f;

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

        //敵用の耐久値表示バー
        public ProgressBar DurableBar;
        
        private void Start()
        {
            //遷移時間
            const float fade_time = 0.5f;
            //シーンのフェードアウト
            FadeManager.Instance.SceneObj.SceneFadeOUT(fade_time);
            InitState();
            
            //フェードアウト後にfalse
            Invoke("SetFadeImage",fade_time);
            //GameManager.Instance.SceneObj.SceneFadeIN("EditScene", 1.0f);
        }
        
        /// <summary>
        ///Invoke()呼び出し用ラッパ
        /// </summary>
        private void SetFadeImage()
        {
            FadeManager.Instance.SetFadeImage(false);
        }

        //チャージ技を発動するときのフラグ
        public bool[] IsCharge = { false, false, false };

        //評価用パネル
        private VisualElement elm_result;
        //評価用ラベル
        private Label lb_result;
        //カウントダウンラベル
        private Label lb_timer;

        //退却用パネル
        private VisualElement elm_cencel;
        //退却ボタン1
        private Button btn_cancel;
        
        /// <summary>
        ///ステータス初期化
        /// </summary>
        private void InitState()
        {
            //出撃時の艦隊規模(艦船の数)
            first_cnt = player.MyShips.Count;
            Debug.Log($"Result = {Result.GetHashCode()}");

            var root = uiDocument.rootVisualElement;

            //敵の耐久値バー登録
            DurableBar = root.Query<ProgressBar>("bar_enemy");
            lb_timer = root.Query<Label>("lb_timer");
            
            //評価のパネル
            elm_result = root.Query<VisualElement>("elm_result");
            //編隊シーンへ戻るボタン
            Button btn_back = elm_result.Query<Button>("btn_back");
            btn_back.clicked += () =>
            {
                //フェードイン
                //キャンバス非表示を解除
                FadeManager.Instance.FadeImage.gameObject.SetActive(true);
                FadeManager.Instance.SceneObj.SceneFadeIN("EditScene", 1.0f);
            };
            lb_result = elm_result.Query<Label>("lb_result");
            //バトル終了まで表示させない
            elm_result.visible = false;
            
            //退却パネル
            elm_cencel = root.Query<VisualElement>("elm_cancel");
            //退却確認ボタン(はい/いいえ)
            Button btn_yes = elm_cencel.Query<Button>("btn_yes");
            Button btn_no = elm_cencel.Query<Button>("btn_no");
            //はい
            btn_yes.clicked += () =>
            {
                //編隊シーンへ戻る
                FadeManager.Instance.FadeImage.gameObject.SetActive(true);
                FadeManager.Instance.SceneObj.SceneFadeIN("EditScene", 1.0f);
            };
            //いいえ
            btn_no.clicked += () =>
            {
                elm_cencel.visible = false;
            };
            
            //退却ボタン
            btn_cancel = root.Query<Button>("btn_cancel");
            btn_cancel.clicked += () =>
            {
                //退却パネル表示
                elm_cencel.visible = true;
            };

            //ボタンが押されるまで非表示
            elm_cencel.visible = false;
            //string[] btn_names = { "btn_torpedo", "btn_cannon", "btn_air" };
            //Button[] buttons = 
            //{
            //    uiDocument.rootVisualElement.Query<Button>(btn_names[0]),
            //    uiDocument.rootVisualElement.Query<Button>(btn_names[1]),
            //    uiDocument.rootVisualElement.Query<Button>(btn_names[2]),
            //};
            //foreach (var btn in buttons.Select((v, i) => (v, i )))
            //{
            //    btn.v.clicked += () =>
            //    {
            //        IsCharge[btn.i] = (!IsCharge[btn.i]) ? true : false;
            //    };
            //}
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
            battle_state = (enemy.Annihilation || player.Annihilation) ? BattleState.Finish : BattleState.Fighing;

            //戦闘後の評価
            if (battle_state == BattleState.Finish)
            {
                //退却ボタンを非表示に
                btn_cancel.visible = false;
                elm_cencel.visible = false;
                //結果表示
                elm_result.visible = true;
                char[] score = new[] { 'S', 'A', 'B', 'C' };
                //結果表示
                lb_result.text = $"戦闘評価 : {score[(int)Result]}";
            }

            //戦闘中は時間を加算
            if (battle_state == BattleState.Fighing)
            {
                time += Time.deltaTime;

                //タイマーメッセージ
                string mes_timer = "";
                //時間内か判定
                if ((int)(lim_time - time) < 0) mes_timer = "タイムオーバー";
                else mes_timer = $"残り：{(int)(lim_time - time)}秒";
                //メッセージ表示
                lb_timer.text = mes_timer;
            }
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
        }
        
        /// <summary>
        /// ボタンで指示できるチャージ技
        /// </summary>
        enum ChargeAttack
        {
            Torpedo,
            Battleship,
            AirCarrier,
        }

        /// <summary>
        /// ボタンアクションの設定
        /// </summary>
        /// <param name="num"></param>
        private void ButtonAction(int num)
        {
            switch ((ChargeAttack)num)
            {
                case ChargeAttack.Torpedo:
                    break;
                case ChargeAttack.Battleship:
                    break;
                case ChargeAttack.AirCarrier:
                    break;
            }
        }

    }
}