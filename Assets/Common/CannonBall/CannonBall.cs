using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CannonBall : MonoBehaviour
{
    //砲弾の攻撃スペック
    [System.Serializable]
    public struct Attack
    {
        public int contisous;   //連続回数
        public int power;       //火力
    }

    public static int EXP;
    //攻撃対象のベクトル
    protected Vector2 target_vec;

    //距離減衰
    private int power_attenuation = 0;
    
    //連続砲撃スペック
    public Attack attack;

    //味方の弾か敵の弾か?
    //trueなら敵,falseなら味方
    private bool is_enemy = false;
    
    //砲弾用物理コンポーネント
    protected Rigidbody2D rbody;

    //外装変更用
    private SpriteRenderer renderer;

    //パーティクル変更用
    private bool invisible = true;
    
    public SpriteRenderer Renderer
    {
        set { renderer = value; }
        get { return renderer; }
    }
    //進行方向
    private Vector2 vec = Vector2.right; 
    public Vector2 Vec
    {
        set { vec = value; }
        get { return vec; }
    }
    //進行速度(プレハブごとに指定する用)
    [SerializeField] protected float velocity = 1.0f;
   
    //砲弾の作成(艦船から変更する用)
    public virtual void Create(Vector2 vec,float velocity)
    {
        this.vec = vec;
        this.velocity = velocity;
    }
    
    //継承先からのthis.gameObject省略用
    protected GameObject MyBom;

    private void Awake()
    {
        MyBom = this.gameObject;
        renderer = MyBom.GetComponent<SpriteRenderer>();
        rbody = MyBom.GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        //描画されているか否か
        if (!invisible)
        {
            Destroy(MyBom);
            //Bom.SetActive(false);
        }
    }

    //public virtual void SetTarget(Vector3 target_pos) { }
    //public virtual void SetTarget(GameObject target_obj,GameObject attacker) { }

    /// <summary>
    /// 攻撃対象のベクトルを設定
    /// </summary>
    /// <param name="target_pos"></param>
    public void SetTarget(Vector3 vec)
    {
        target_vec = vec.normalized;
    }

    /// <summary>
    /// 引数をゲームオブジェクトにしたオバロ
    /// </summary>
    /// <param name="target_obj">攻撃対象</param>
    /// <param name="attacker">攻撃する側</param>
    public void SetTarget(GameObject target,GameObject attacker)
    {
        //途中でどちらかが消えてしまった中断
        if (target == null || attacker == null) return;
        //弾の火力に、発射元の火力を加算
        attack.power += attacker.gameObject.GetComponent<Ship>().Power / 100;
        //目標を設定
        target_vec = (target.transform.position - attacker.transform.position).normalized;
    }
    
    /// <summary>
    /// 発射する座標の設定
    /// </summary>
    /// <param name="pos">配置する座標</param>
    /// <param name="parent">親オブジェクト</param>
    public void SetPosition(Vector3 pos,GameObject parent = null)
    {
        MyBom.transform.position = pos;
        //親子関係を持たせたい時だけ
        if (parent != null) MyBom.transform.parent = parent.transform;
    }

    /// <summary>
    /// 敵か味方かの判別用レイヤー割り当てメソッド
    /// </summary>
    /// <param name="is_enemy">falseなら味方,trueなら敵</param>
    public void SetPositionLayer(bool is_enemy)
    {
        string layer_name = (is_enemy) ? "Enemy" : "Player";
        this.is_enemy = is_enemy;
        MyBom.gameObject.layer = LayerMask.NameToLayer(layer_name);
    }

    //砲撃向き設定
    public void SetDirection(Vector2 direction)
    {
        
    }

    /// <summary>
    /// 発射元からの火力を加算
    /// </summary>
    /// <param name="power"></param>
    public void AddPower(int power)
    {
        
    }

    private void OnBecameInvisible()
    {
        invisible = false;
    }

    //追跡型魚雷
    public virtual void TrackingTorpedo(Vector2 target_pos)
    {
        
    }
}
