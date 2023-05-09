using System;
using System.Collections;
using System.Collections.Generic;
using Shooting;
using UnityEngine;
using UnityEngine.UIElements;

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
            btn_add[0] = uiDocument.rootVisualElement.Query<Button>("btn_add1");
            
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