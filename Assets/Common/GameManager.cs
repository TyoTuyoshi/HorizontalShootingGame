using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public sealed class GameManager : Singleton<GameManager>
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
        public GameObject FadeObj;
        public Image FadeImage;
        public Scene scene;

        public void SetFadeImage()
        {
            FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        }
        /*
        public void CreateImage()
        {
            GameObject image;
            if (!GameObject.Find("FadeImage"))
            {
                Debug.Log("すでに存在");
            }
            else
            {
                image = Instantiate(FadeObj) as GameObject;
                image.name = "FadeImage";
                image.transform.parent = GameObject.Find("Canvas").transform;
            }
        }*/
        
        

        //操作できる艦隊
        //EditSceneで編隊できる
        private List<Ship> myships;
        public List<Ship> PlayAbleShip
        {
            set { myships = value; }
            get
            {
                if (myships is null)
                {
                    Debug.LogError("船がありません");
                }
                return myships;
            }
        }
    }
}