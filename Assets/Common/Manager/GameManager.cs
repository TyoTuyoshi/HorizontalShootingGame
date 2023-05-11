using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDontDestroy<GameManager>
{
    //操作できる艦隊
    //EditSceneで編隊できる
    public int exp = 0;
    public List<Ship> PlayAbleShip = new List<Ship>();
}
