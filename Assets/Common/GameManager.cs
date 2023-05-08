using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting
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
        //操作できる艦隊
        //EditSceneで編隊
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