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
            //FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
            //UIElementの操作が通らないためfalse
            //フェード時にtrue
            FadeImage.gameObject.SetActive(false);
        }

        protected override void Awake()
        {
        }
    }
}