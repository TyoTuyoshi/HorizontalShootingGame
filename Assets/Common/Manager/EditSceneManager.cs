using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Manager
{
    public class EditSceneManager : Singleton<EditSceneManager>
    {
        [SerializeField] private UIDocument uiDocument = null;
        private const int size = 3;
        private VisualElement[] panel;
        private Button[] btn_add;
        
        private void Start()
        {
            //シーンのフェードイン/アウト用Imageの設定
            GameManager.Instance.SetFadeImage();
            //btn_add[0] = uiDocument.rootVisualElement.Query<Button>("btn_add1");
            //Scene.SceneFadeIN("GameScene", 1.0f);
            GameManager.Instance.scene.SceneFadeIN("GameScene",1.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetShips()
        {
            GameManager.Instance.PlayAbleShip = new List<Ship>();
        }
    }
}