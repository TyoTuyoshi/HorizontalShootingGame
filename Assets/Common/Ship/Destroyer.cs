using System.Collections;
using System.Linq;

using UnityEngine;

//駆逐艦クラス
public class Destroyer : Ship
{
    //砲撃する砲弾
    [SerializeField] private CannonBall[] cannon_ball;
    
    //砲撃間隔カウンタ
    //{通常砲撃チャージタイム,固有弾幕チャージタイム,仮}
    private float[] bom_time = {0, 0, 0};
    private void Start()
    {
        InitStatus();
    }

    //ステータス初期化
    private void InitStatus()
    {
       
    }

    private void Update()
    {
        UpdateGame();
    }

    //ゲーム更新
    private void UpdateGame()
    {
        //砲撃時間の加算
        bom_time = bom_time.Select(i => i + Time.deltaTime).ToArray();
        
        //通常砲撃
        if (bom_time[0] > Charge)
        {
            bom_time[0] = 0;
            //通常砲撃(砲弾オブジェクト配置) 6連砲
            StartCoroutine(Bombardment(cannon_ball[0], 6));
        }
        
        //固有砲撃
        if (bom_time[1] > UniqueCharge)
        {
            bom_time[1] = 0;
            //固有弾幕展開
            StartCoroutine(Bombardment_CrossCos());
        }
    }


    //二連Cos波形連続砲撃(固有弾幕)
    private IEnumerator Bombardment_CrossCos()
    {
        ShipState = Ship.State.Battle;
        Debug.Log(Name + ":固有弾幕展開!");
        for (int i = 0; i < cannon_ball[0].ContinuousCanon.contisous; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            var ball = Instantiate(cannon_ball[0]) as CannonBall;
            ball.Create(new Vector2(2, 2 / (i + 1)), 10);
            //砲弾配置(弾幕生成)
            ball.transform.position =
                this.gameObject.transform.position +
                new Vector3(0, Mathf.Cos(Mathf.PI / 180 * i * 15));

            ball = Instantiate(cannon_ball[0]) as CannonBall;
            ball.Create(new Vector2(2, -2 / (i + 1)), 10);
            //砲弾配置(弾幕生成)
            ball.transform.position =
                this.gameObject.transform.position +
                new Vector3(0, -Mathf.Cos(Mathf.PI / 180 * i * 15));
        }
    }

    //魚雷攻撃
    public override void TorpedoLaunch()
    {
        
    }
}
