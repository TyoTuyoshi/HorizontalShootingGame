using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanonBallSet
{
    public sealed class Shape
    {
        private float pi = Mathf.PI;
        //円
        public void Round(ref Vector2 center, int wait_time, int n, int size, int radius, int offset = 0)
        {
           
        }

        //四角
        public void Square(Vector2 center, int cnt = 1)
        {
            
        }
        //星
        public void Star(Vector2 center, int cnt = 1)
        {

        }
        //円弧
        public void Arc(Vector2 center, float target_angle, float angle, int cnt = 1)
        {

        }
        //通常配置
        public void Normal(Vector2 center)
        {
            
        }
    }
}