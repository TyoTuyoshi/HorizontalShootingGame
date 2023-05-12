using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Manager
{

    /// <summary>
    /// 追加ボタンクラス
    /// </summary>
    public class AddShipButton
    {
        public Button btn;  //ボタン
        public Ship ship;   //選んだ艦船
    }

    /// <summary>
    /// EditSceneのGUIなどのマネージャー
    /// </summary>
    public class EditSceneManager : Singleton<EditSceneManager>
    {
        [SerializeField] private UIDocument uiDocument = null;
        //編隊最大数
        private const int size = 3;
        //艦船選択パネル
        public GameObject ShipViewer = null;
        //登録済み艦船リスト
        public List<Ship> ships = new List<Ship>();

        //艦船追加ボタン
        [System.NonSerialized] public List<AddShipButton> AddButtons = new List<AddShipButton>();

        //戻るボタン
        private Button btn_gogame;

        //選択した追加ボタンの判別用フラグ
        public int index = 0;
        private void Start()
        {
            //シーンのフェードイン/アウト用Imageの設定
            FadeManager.Instance.SetFadeImage(true);
            //遷移時間
            const float fade_time = 0.5f;
            FadeManager.Instance.SceneObj.SceneFadeOUT(fade_time);
            //フェードアウト後にfalse
            Invoke("SetFadeImage",fade_time);

            var root = uiDocument.rootVisualElement;
            //追加ボタンの取得
            {
                for (int i = 0; i < 3; i++)
                {
                    AddShipButton asb = new AddShipButton();
                    asb.btn = root.Query<Button>($"btn_add{i + 1}");
                    asb.ship = null;
                    AddButtons.Add(asb);
                }
                //ボタンイベント(艦船選択パネル表示)
                foreach (var asb in AddButtons.Select((v, i) => (v, i)))
                {
                    asb.v.btn.clicked += () =>
                    {
                        ShipViewer.SetActive(true);
                        //どこのボタンが押されたかのフラグ用
                        index = asb.i;
                    };
                }
            }

            //出撃ボタン
            btn_gogame = root.Query<Button>("btn_gogame");
            btn_gogame.clicked += () =>
            {
                if (ships.Count > 0)
                {
                    //ゲームマネージャに艦船を登録
                    SetGMShips();
                    //フェードイン
                    //キャンバス非表示を解除
                    FadeManager.Instance.FadeImage.gameObject.SetActive(true);
                    FadeManager.Instance.SceneObj.SceneFadeIN("GameScene", 1.0f);
                }
            };

            //タイトルに戻る(バックボタン)
            Button btn_back = root.Query<Button>("btn_back");
            btn_back.clicked += () =>
            {
                ResetShips();
                FadeManager.Instance.FadeImage.gameObject.SetActive(true);
                FadeManager.Instance.SceneObj.SceneFadeIN("TitleScene", 1.0f);
            };
        }

        private void Update()
        {
            //出撃ボタン用の監視メッセージ
            btn_gogame.text = (ships.Count > 0) ? "出撃" : "一隻以上必要です";
        }

        /// <summary>
        ///Invoke()呼び出し用ラッパ
        /// </summary>
        private void SetFadeImage()
        {
            FadeManager.Instance.SetFadeImage(false);
        }

        /// <summary>
        /// ゲームマネージャーにshipを渡す
        /// </summary>
        public void SetGMShips()
        {
            GameManager.Instance.PlayAbleShip = ships;
        }

        /// <summary>
        /// 登録した艦船を初期化
        /// </summary>
        public void ResetShips()
        {
            GameManager.Instance.PlayAbleShip = new List<Ship>();
            ships = new List<Ship>();
        }

        /// <summary>
        /// データベースから艦船を取ってくる。
        /// </summary>
        public void GetDBSgips()
        {
            ships = ShipDataBase.Instance.ShipDB.GetRange(0, 3);
        }
    }
}