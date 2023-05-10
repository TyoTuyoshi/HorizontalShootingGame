using System;
using System.Collections;
using System.Collections.Generic;
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

        private List<Ship> ships = new List<Ship>();
        private void Start()
        {
            //シーンのフェードイン/アウト用Imageの設定
            FadeManager.Instance.SetFadeImage();
            GetSgips();
            SetShips();
            //btn_add[0] = uiDocument.rootVisualElement.Query<Button>("btn_add1");
            //Scene.SceneFadeIN("GameScene", 1.0f);
            FadeManager.Instance.SceneObj.SceneFadeIN("GameScene",1.0f);
        }
        
        /// <summary>
        /// マネージャーにshipを渡す。
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