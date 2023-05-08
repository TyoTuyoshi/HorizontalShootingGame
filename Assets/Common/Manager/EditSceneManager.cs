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
        
        private void Start()
        {
            //panel[0] = uiDocument.rootVisualElement.Query<VisualElement>("");
        }

        public void SetShips(int i)
        {
            
        }
    }
}