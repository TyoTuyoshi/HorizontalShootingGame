using System;
using System.Collections;
using Manager;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ship : MonoBehaviour
{
    [Header("撃てる弾")] public CannonBall[] CannonBalls;
    //砲撃間隔カウンタ
    //{通常砲撃チャージタイム,固有弾幕チャージタイム,仮}
    protected float[] bom_time = {0, 0, 0};
    
    //砲撃するターゲット
    private Ship target;
    //ターゲットのプロパティ
    public Ship Target
    {
        set { target = value; }
        get { return target; }
    }

    [SerializeField] private static int level = 1;
    public int Level
    {
        get { return level; }
    }

    //敵扱いフラグ
    //falseなら味方　trueなら敵
    [Header("敵フラグ"),SerializeField] private bool is_enemy = false;
    public bool IsEnemy //敵扱いフラグプロパティ
    {
        set { is_enemy = value; }
        get { return is_enemy; }
    }

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
    
    [Header("固有弾幕時間手動有効"), SerializeField] private bool is_unique = false;
    [Header("固有弾幕装填時間"), SerializeField] private int unique_charge = 0;
    public int UniqueCharge//固有弾幕装填時間
    {
        set { unique_charge = value; }
        get { return unique_charge; }
    }

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

    protected GameObject MyShip; //継承先からのthis.gameObject省略用

    [SerializeField] protected Slider DurableBar;//耐久表示バー

    //開始時の最大耐久値(初期値)
    protected int MaxDurable;
    
    //演算用円周率
    protected float pi = Mathf.PI;
    private void Awake()
    {
        //オブジェクト、コンポーネント等の初期化取得
        MyShip = this.gameObject;
        rbody = this.gameObject.GetComponent<Rigidbody2D>();
        Renderer = this.gameObject.GetComponent<SpriteRenderer>();
        MaxDurable = durable;
        Debug.Log($"max = {MaxDurable}");

        //敵の場合1.2倍の大きさ
        if (is_enemy)
        {
            MyShip.transform.localScale *= 1.2f;
        }

        //固有弾幕待ち時間の設定
        if (!is_unique)
        {
            UniqueCharge *= 5;
        }
    }

    /// <summary>
    ///　上下左右移動 
    /// </summary>
    /// <param name="vec">移動先ベクトル</param>
    /// <param name="speed">速度</param>
    public void Move(Vector2 vec, float speed)
    {
        //ship_state = State.Move;
        const float coefficient = 1.5f; //速力係数
        rbody.MovePosition(MyShip.transform.position + (Vector3)vec * coefficient * speed * Time.fixedDeltaTime);
        //rbody.AddForce(pos * coefficient * speed * Time.deltaTime, ForceMode2D.Impulse);
        MyShip.transform.position = PositionRange(MyShip.transform.position, move_range.MinPos, move_range.MaxPos);
    }

    /// <summary>
    ///　追従移動 
    /// </summary>
    /// <param name="pos">移動先座標</param>
    /// <param name="distance">間隔</param>
    public void MoveFollowing(Vector2 pos,float distance)
    {
        const float speed = 1.5f;
        //一定距離外であるときに追従
        if (Vector2.Distance(pos, MyShip.transform.position) > distance)
        {
            float present_pos = (Time.deltaTime * speed) / distance;
            MyShip.transform.position = Vector2.Lerp(MyShip.transform.position, pos, present_pos);
        }
    }

    /// <summary>
    ///　破棄 
    /// </summary>
    public void Destory()
    {
        //味方が敵を倒したとき
        //if (is_enemy) GameManager.Instance.Result = GameManager.BattleScore.S;
        UnityEngine.Object.Destroy(this.gameObject);
    }

    /// <summary>
    ///攻撃時前面表示
    /// </summary>
    /// <param name="can_attack">攻撃フラグ</param>
    protected void SortDrawOrder(bool can_attack)
    {
        Renderer.sortingOrder = (can_attack) ? 1 : 0;
    }

    /// <summary>
    /// 移動範囲制限関数
    /// </summary>
    /// <param name="now_pos">現在の座標</param>
    /// <param name="min">左下最小座標</param>
    /// <param name="max">右上最大座標</param>
    /// <returns>修正座標</returns>
    private Vector2 PositionRange(Vector2 now_pos, Vector2 min, Vector2 max)
    {
        Vector2 pos = new Vector2();
        pos.x = Mathf.Clamp(now_pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(now_pos.y, min.y, max.y);
        return pos;
    }

    /// <summary>
    ///通常砲撃メソッド(連続砲撃)
    /// </summary>
    /// <param name="cannon_ball">発射する弾</param>
    /// <param name="n">連続発射回数</param>
    /// <param name="interval">発射周期(待ち時間)</param>
    public IEnumerator Bombardment(CannonBall cannon_ball, int n, float interval)
    {
        //Debug.Log(Name + ":発射!");

        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSecondsRealtime(interval);
            //敵か味方かのレイヤー指定
            var ball = Instantiate(cannon_ball) as CannonBall_Normal;
            ball.SetPositionLayer(is_enemy);
            //発射先ターゲットのベクトルを指定
            var vec = (target != null) ? target.transform.position - MyShip.transform.position : Vector3.left;
            ball.SetTarget(vec);
            //配置
            ball.SetPosition(MyShip.transform.position);
        }
    }

    /// <summary>
    /// 拡散型通常弾幕(カスタム)
    /// </summary>
    /// <param name="cannon_ball">発射する弾</param>
    /// <param name="n">連続発射回数</param>
    /// <param name="radius">半径</param>
    /// <param name="phase">砲弾間の位相</param>
    /// <param name="offset">配置開始位置のオフセット</param>
    /// <param name="interval">発射周期(待ち時間)</param>
    public IEnumerator BombardmentDiffusion(CannonBall cannon_ball, int n, float radius = 1.0f,
        int phase = 10, int offset = 0, float interval = 0)
    {
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSecondsRealtime(interval);
            var ball = Instantiate(cannon_ball) as CannonBall_Normal;
            ball.SetPositionLayer(is_enemy);
            //ラジアン計算
            float theta = (pi * (phase * i + offset)) / 180;

            var vec = radius * new Vector3(
                Mathf.Cos(theta),
                Mathf.Sin(theta));
            
            ball.transform.LookAt(vec);
            
            //発射先ターゲットの指定
            ball.SetTarget(vec);
            //砲弾配置(弾幕生成)
            ball.SetPosition(MyShip.transform.position + vec);
        }
    }

    public virtual async void Bombardment2() { }//基底　砲撃メソッド
    public virtual void TorpedoLaunch() { ship_state = State.Battle;}//基底　魚雷発射メソッド
    public virtual void AirStrike(){ ship_state = State.Move; }//基底　空爆メソッド

    private void OnTriggerEnter2D(Collider2D col)
    {
        HitDamage(col);
    }

    /// <summary>
    /// ダメージ判定
    /// </summary>
    /// <param name="col">衝突オブジェクト</param>
    private void HitDamage(Collider2D col)
    {
        //当たった弾の火力分ダメージを受ける。
        try
        {
            int damage = col.gameObject.GetComponent<CannonBall>().attack.power;
            Debug.Log(damage);
            //ダメージヒット
            durable -= 10;
            //耐久値バーの更新
            DurableBar.value = durable / (float)MaxDurable;
        }
        catch (Exception e)
        {
            return;
        }
    }
}
