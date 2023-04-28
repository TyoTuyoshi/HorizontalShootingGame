using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//重巡洋艦クラス
public class HeavyCruiser : Ship
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
        if (Durable <= 0)
        {
            Durable = 0;
            ShipState = State.Sunk;
        }

        //砲撃時間の加算
        bom_time = bom_time.Select(i => i + Time.deltaTime).ToArray();
        
        //通常砲撃
        if (bom_time[0] > Charge)
        {
            bom_time[0] = 0;
            //通常砲撃(砲弾オブジェクト配置) 6連砲
            StartCoroutine(Bombardment(cannon_ball[0], 4,0.05f));
        }
        
        //固有砲撃(敵は発射させない)
        if (bom_time[1] > UniqueCharge && !IsEnemy)
        {
            bom_time[1] = 0;
            //固有弾幕展開
            StartCoroutine(Bombardment_GoldenRectangle());
        }
    }


    //黄金長方形弾幕展開(固有弾幕)
    private IEnumerator Bombardment_GoldenRectangle()
    {
        //ShipState = Ship.State.Battle;
        Debug.Log(Name + ":固有弾幕展開!");

        //連射回数
        int n = cannon_ball[1].ContinuousCanon.contisous;

        //弾幕展開
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            var ball = Instantiate(cannon_ball[1]) as CannonBall_GoldenRectangle;
            ball.SetPositionLayer(IsEnemy);
            ball.Create(this.gameObject.transform.position, 1);
        }
    }

    //魚雷攻撃
    public override void TorpedoLaunch()
    {
        
    }
}
