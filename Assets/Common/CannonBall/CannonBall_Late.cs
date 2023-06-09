using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//遅延設置発射弾
public class CannonBall_Late : CannonBall
{
    [SerializeField] private float wait_time = 2.5f;
    //発射フラグ
    private bool flag = false;

    private void Start()
    {
        Invoke("FlagON", wait_time);
    }

    private void FlagON()
    {
        flag = true;
    }

    //物理コンポーネント更新
    private void FixedUpdate()
    {
        UpdateGame();
    }

    //更新
    private void UpdateGame()
    {
        if (flag)
        {
            //親子解除
            this.gameObject.transform.parent = null ;
            LateBom(target_vec, 25.0f);
        }
    }

    //砲弾の移動
    private void LateBom(Vector2 vec, float velocity)
    {
        vec = vec.normalized;
        rbody.MovePosition(MyBom.transform.position + (Vector3)vec * velocity * Time.deltaTime);
    }
}