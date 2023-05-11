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
        public Button btn;//ボタン
        public Ship ship;//選んだデータ
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


        //選択した追加ボタンの判別用フラグ
        public int index = 0;
        private void Start()
        {
            //シーンのフェードイン/アウト用Imageの設定
            FadeManager.Instance.SetFadeImage();
            
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
            Button btn_gogame = root.Query<Button>("btn_gogame");
            btn_gogame.clicked += () =>
            {
                //ゲームマネージャに登録
                SetShips();
                
            };
            //艦船登録

            //フェードイン
            //キャンバス非表示を解除
            //FadeManager.Instance.FadeImage.gameObject.SetActive(true);
            //FadeManager.Instance.SceneObj.SceneFadeIN("GameScene",1.0f);
        }
        /// <summary>
        /// マネージャーにshipを渡す
        /// </summary>
        public void SetShips()
        {
            GameManager.Instance.PlayAbleShip = ships;
        }
        /// <summary>
        /// データベースから艦船を取ってくる。
        /// </summary>
        public void GetSgips()
        {
            ships = ShipDataBase.Instance.ShipDB.GetRange(0, 3);
        }
    }
}