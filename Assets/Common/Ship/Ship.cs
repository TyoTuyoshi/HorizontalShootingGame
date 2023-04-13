using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    public enum Type //艦船タイプ
    {
        Destroyer, //駆逐艦
        LCruiser, //軽巡洋艦
        HCruiser, //重巡洋艦
        BattleShip, //戦艦
        AirCarrier, //空母
        LAirCarrier //軽空母
    }

    public enum Condition //戦闘コンディション
    {
        Perfect, // 0%破損
        Normal, //50%未満破損
        Danger, //50%以上破損
    }

    public enum State //行動
    {
        Idle, //アイドル
        Move, //移動
        Battle, //戦闘
        Sunk //撃沈
    }

    [Header("耐久値"), SerializeField] private int durable; //耐久値

    public int Durable //耐久値プロパティ
    {
        set
        {
            durable = value;
            if (durable <= 0) //撃沈時の耐久値下限
            {
                durable = 0;
            }
        }
        get { return durable; }
    }

    [Header("装填"),SerializeField] private int charge; //装填

    public int Charge //装填プロパティ
    {
        set { charge = value; }
        get { return charge; }
    }

    [Header("火力"),SerializeField] private int power; //火力

    public int Power //火力プロパティ
    {
        set { power = value; }
        get { return power; }
    }

    [Header("回避"),SerializeField] private int avoidance; //回避
    public int Avoidance //回避プロパティ
    {
        set { avoidance = value; }
        get { return avoidance; }
    }

    [Header("艦名"), SerializeField] private string name; //名前
    public string Name //名前プロパティ
    {
        set { name = value; }
        get { return name; }
    }

    [Header("艦種"),SerializeField] private Type ship_type; //艦船タイプ
    public Type ShipType //艦船タイププロパティ
    {
        set { ship_type = value; }
        get { return ship_type; }
    }

    [Header("状態"), SerializeField] private State ship_state;//戦闘ステート
    public State ShipState //状態プロパティ
    {
        set { ship_state = value; }
        get { return ship_state; }
    }

    [System.NonSerialized] public GameObject KANSEN; //艦船
    [System.NonSerialized] public SpriteRenderer Renderer;

    //範囲制御
    [System.Serializable]
    public struct RangePos
    {
        public Vector2 MinPos;
        public Vector2 MaxPos;
    }
    private RangePos move_range;
    public RangePos Range
    {
        set { move_range = value; }
        get { return move_range; }
    }

    private Rigidbody2D rbody;//艦用物理コンポーネント
    
    private void Awake()
    {
        //オブジェクト、コンポーネント等の初期化取得
        KANSEN = this.gameObject;
        rbody = this.gameObject.GetComponent<Rigidbody2D>();
        Renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    //移動
    public void Move(Vector2 pos, float speed)
    {
        ship_state = State.Move;
        const int coefficient = 3; //速力係数
        //ship.gameObject.transform.position +=
        //    new Vector3(pos.x, pos.y, 0) * coefficient * speed * Time.deltaTime;
        //rbody.MovePosition(pos * coefficient * speed * Time.deltaTime);
        rbody.AddForce(pos * coefficient * speed * Time.deltaTime, ForceMode2D.Impulse);
        KANSEN.transform.position = PositionRange(KANSEN.transform.position, move_range.MinPos, move_range.MaxPos);
    }

    //追従移動
    public void MoveFollowing(Vector2 pos,float distance)
    {
        ship_state = State.Move;
        const float speed = 2.0f;
        //一定距離外であるときに追従
        if (Vector2.Distance(pos, KANSEN.transform.position) > distance)
        {
            float present_pos = (Time.deltaTime * speed) / distance;
            KANSEN.transform.position = Vector2.Lerp(KANSEN.transform.position, pos, present_pos);
        }
    }

    //攻撃時前面表示
    protected void SortDrawOrder(bool can_attack)
    {
        Renderer.sortingOrder = (can_attack) ? 1 : 0;
    }

    //座標クランプ
    private Vector2 PositionRange(Vector2 now_pos, Vector2 min, Vector2 max)
    {
        Vector2 pos = new Vector2();
        pos.x = Mathf.Clamp(now_pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(now_pos.y, min.y, max.y);
        return pos;
    }

    public virtual void Bombardment() { ship_state = State.Battle;}//基底　砲撃メソッド
    public virtual void TorpedoLaunch() { ship_state = State.Battle;}//基底　魚雷発射メソッド
    public virtual void AirStrike(){ ship_state = State.Move; }//基底　空爆メソッド

}

#if UNITY_EDITOR
[CustomEditor(typeof(Ship))]
public class Inspector: Editor
{
    public override void OnInspectorGUI()
    {
    }
}
#endif