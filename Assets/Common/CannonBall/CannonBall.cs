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

    //味方の弾か敵の弾か?
    private bool is_enemy = false;
    
    //砲弾用物理コンポーネント
    protected Rigidbody2D rbody;

    //外装変更用
    private SpriteRenderer renderer;
    public SpriteRenderer Renderer
    {
        set { renderer = value; }
        get { return renderer; }
    }

    private Vector2 vec = Vector2.right; //進行方向
    public Vector2 Vec
    {
        set { vec = value; }
        get { return vec; }
    }
    private float velocity = 1.0f;      //進行速度

    public float Velocity
    {
        set { velocity = value; }
        get { return velocity; }
    }

    private GameObject Bom;  //自身
    //砲弾の作成
    public virtual void Create(Vector2 vec,float velocity)
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
        /*
        if (!renderer.isVisible)
        {
            Destroy(this.gameObject);
        }*/
    }

    /// <summary>
    /// 敵か味方かの判別用レイヤー割り当てメソッド
    /// </summary>
    /// <param name="is_enemy">falseなら味方,trueなら敵</param>
    public void SetPositionLayer(bool is_enemy)
    {
        string layer_name = (is_enemy) ? "Enemy" : "Player";
        this.is_enemy = is_enemy;
        Bom.gameObject.layer = LayerMask.NameToLayer(layer_name);
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
