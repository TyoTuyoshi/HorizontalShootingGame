using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//重巡洋艦クラス
public class HeavyCruiser : Ship
{
    //砲撃
    public override IEnumerator Bombardment()
    {
        yield return null;
    }
}
