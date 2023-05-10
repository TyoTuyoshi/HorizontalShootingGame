using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public sealed class FadeManager : Singleton<FadeManager>
    {
        /// <summary>
        /// シーンステート
        /// </summary>
        public enum SceneState
        {
            Title,
            Edit,
            Game,
        }

        //フェード用のスプライト
        public Image FadeImage;
        //SceneBaseプレハブから取得
        public Scene SceneObj = null;

        public void SetFadeImage()
        {
            FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        }

        protected override void Awake()
        {
        }
        /*
        [SerializeField] private List<Ship> myships;
        public List<Ship> PlayAbleShip
        {
            set
            {
                myships = value;
                Debug.Log($"{myships[0]} {myships.Count}");
            }
            /*
            get
            {
                if (myships is null)
                {
                    Debug.LogError("船がありません");
                }
                return myships;
            }
        }*/
    }
}