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

    private Vector2 vec = Vector2.right; //進行方向
    private float velocity = 1.0f;      //進行速度

    private GameObject Bom;  //自身
    
    //砲弾の作成
    public void Create(Vector2 vec,float velocity)
    {
        this.vec = vec;
        this.velocity = velocity;
    }

    private void Awake()
    {
        Bom = this.gameObject;
        renderer = Bom.GetComponent<SpriteRenderer>();
        rbody = Bom.GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        //描画されているか否か
        if (!renderer.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    //物理コンポーネント更新
    private void FixedUpdate()
    {
        UpdateGame();
    }

    //更新
    private void UpdateGame()
    {
        rbody.MovePosition(transform.position + velocity * (Vector3)vec * Time.deltaTime);
        //rbody.AddForce(vec * velocity);
    }

    //砲弾の運動(移動)
    public void FlyBom(Vector2 vec,float velocity)
    {
        vec = vec.normalized;
        //rbody.MovePosition(transform.position + new Vector3(0.025f, 0, 0) * Time.fixedTime);
        //rbody.AddForce(vec * velocity);
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
