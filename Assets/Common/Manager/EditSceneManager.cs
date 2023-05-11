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
        private const int size = 3;
        private VisualElement[] panel;
        private Button[] btn_add;

        [SerializeField] private GameObject ShipViewer = null;

        public List<Ship> ships = new List<Ship>();
        private void Start()
        {
            //シーンのフェードイン/アウト用Imageの設定
            FadeManager.Instance.SetFadeImage();
            
            GetSgips();

            var root = uiDocument.rootVisualElement;
            //btn_add[0] = root.Q<Button>("btn_add1");
            //btn_add[1] = root.Q<Button>("btn_add2");
            //btn_add[2] = root.Q<Button>("btn_add3");

            //追加ボタンの取得
            
            Button[] buttons = 
            {
                root.Query<Button>("btn_add1"),
                root.Query<Button>("btn_add2"),
                root.Query<Button>("btn_add3"),
            };

            //buttons.Select(e => e.clicked += () =>
            //{
            //    ShipViewer.SetActive(true);
            //}).ToArray();
            
            foreach (var btn in buttons.Select((v, i) => (v, i )))
            {
                btn.v.clicked += () =>
                {
                    ShipViewer.SetActive(true);
                };
            }

            //艦船登録
            SetShips();
            //btn_add[0] = uiDocument.rootVisualElement.Query<Button>("btn_add1");
            //Scene.SceneFadeIN("GameScene", 1.0f);

            //フェードイン
            //キャンバス非表示を解除
            //FadeManager.Instance.FadeImage.gameObject.SetActive(true);
            //FadeManager.Instance.SceneObj.SceneFadeIN("GameScene",1.0f);
        }

        /// <summary>
        /// 艦船リストを開く
        /// </summary>
        private void OpenShipViewer()
        {
            ShipViewer.SetActive(true);
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