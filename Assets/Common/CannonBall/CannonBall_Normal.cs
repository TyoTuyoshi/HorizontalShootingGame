using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall_Normal : CannonBall
{
    //物理コンポーネント更新
    private void FixedUpdate()
    {
        UpdateGame();
    }
    //更新
    private void UpdateGame()
    {
        FlyBom(new Vector2(6, 3).normalized, 10.0f);
    }
}
