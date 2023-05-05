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
        FlyBom(target_vec.normalized, velocity);
    }
    
    //砲弾の運動(移動)
    private void FlyBom(Vector2 vec,float velocity)
    {
        vec = vec.normalized;
        rbody.MovePosition(transform.position + (Vector3)vec * velocity * Time.deltaTime);
    }
}
