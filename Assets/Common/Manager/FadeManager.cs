using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public sealed class FadeManager : Singleton<FadeManager>
    {
        //フェード用のスプライト
        public Image FadeImage;
        //SceneBaseプレハブから取得
        public Scene SceneObj = null;

        /// <summary>
        /// フェードイメージの有効
        /// SetActive()のラッパ
        /// </summary>
        /// <param name="active">SetActive()</param>
        public void SetFadeImage(bool active)
        {
            //フェード時にtrue
            FadeImage.gameObject.SetActive(active);
        }
    }
}