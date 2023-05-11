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
        [System.NonSerialized] public List<Button> AddButtons = new List<Button>(); 

        //選択した追加ボタンの判別用フラグ
        public int index = 0;
        private void Start()
        {
            //シーンのフェードイン/アウト用Imageの設定
            FadeManager.Instance.SetFadeImage();
            
            var root = uiDocument.rootVisualElement;
            //追加ボタンの取得
            AddButtons.Add(root.Query<Button>("btn_add1"));
            AddButtons.Add(root.Query<Button>("btn_add2"));
            AddButtons.Add(root.Query<Button>("btn_add3"));

            foreach (var btn in AddButtons.Select((v, i) => (v, i )))
            {
                btn.v.clicked += () =>
                {
                    ShipViewer.SetActive(true);
                    //押されたボタン番号
                    index = btn.i;
                };
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