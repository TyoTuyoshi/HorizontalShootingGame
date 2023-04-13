using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CannonBall : MonoBehaviour
{
    //連続砲撃用
    [System.Serializable]
    public struct ContinuousAttack
    {
        public int contisous;   //連続回数
        public int power;       //火力
    }
    //距離減衰
    private int power_attenuation = 0;
    //連続砲撃可能スペック判断
    public bool CanContinous = false;
    //連続砲撃スペック
    public ContinuousAttack ContinuousCanon;

    //砲弾用物理コンポーネント
    private Rigidbody2D rbody;
    private SpriteRenderer renderer;
    
    private void Awake()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
        rbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!renderer.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        UpdateGame();
    }

    //更新
    private void UpdateGame()
    {
        rbody.MovePosition(transform.position + new Vector3(0.025f, 0, 0) * Time.fixedTime);
    }

    //砲撃向き設定
    public void SetDirection(Vector2 direction)
    {
        
    }

    //追跡型魚雷
    public virtual void TrackingTorpedo(Vector2 target_pos)
    {
        
    }
}
