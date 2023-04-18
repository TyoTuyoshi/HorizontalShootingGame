using System.Collections;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    //敵扱いフラグ
    //falseなら味方　trueなら敵
    [SerializeField] private bool is_enemy = false;
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

    private GameObject KANSEN; //艦船
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

        if (!is_unique)
        {
            UniqueCharge *= 5;
        }
    }
    
    //砲撃間隔カウンタ
    //{通常砲撃チャージタイム,固有弾幕チャージタイム,仮}
    private float[] bom_time = {0, 0, 0};

    private void Update()
    {
        UpdateGame();
    }

    //更新
    private void UpdateGame()
    {
        //耐久値の監視(撃沈)
        if (durable <= 0)
        {
            durable = 0;
            ship_state = State.Sunk;
        }
    }

    //移動
    public void Move(Vector2 pos, float speed)
    {
        //ship_state = State.Move;
        const float coefficient = 1.5f; //速力係数
        rbody.MovePosition(KANSEN.transform.position + (Vector3)pos * coefficient * speed * Time.fixedDeltaTime);
        //rbody.AddForce(pos * coefficient * speed * Time.deltaTime, ForceMode2D.Impulse);
        KANSEN.transform.position = PositionRange(KANSEN.transform.position, move_range.MinPos, move_range.MaxPos);
    }

    //追従移動
    public void MoveFollowing(Vector2 pos,float distance)
    {
        //ship_state = State.Move;
        const float speed = 1.5f;
        //一定距離外であるときに追従
        if (Vector2.Distance(pos, KANSEN.transform.position) > distance)
        {
            float present_pos = (Time.deltaTime * speed) / distance;
            KANSEN.transform.position = Vector2.Lerp(KANSEN.transform.position, pos, present_pos);
        }
    }

    //破棄
    public void Destory()
    {
        UnityEngine.Object.Destroy(this.gameObject);
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

    public virtual IEnumerator Bombardment() { yield return null;}//基底　砲撃メソッド
    public virtual async void Bombardment2() { }//基底　砲撃メソッド

    public virtual void TorpedoLaunch() { ship_state = State.Battle;}//基底　魚雷発射メソッド
    public virtual void AirStrike(){ ship_state = State.Move; }//基底　空爆メソッド

    public void OnTriggerEnter(Collider other)
    {
        //ダメージ判定
        HitDamage(other);
    }
    
    //ダメージ判定
    private void HitDamage(Collider other)
    {
        //味方目線 ダメージ判定
        if (other.CompareTag("Enemy") && is_enemy)
        {
            //durable -= ;
        }
        //敵目線　ダメージ判定
        else if (other.CompareTag("Player") && !is_enemy)
        {
            
        }
    }
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