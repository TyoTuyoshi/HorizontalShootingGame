using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Manager;

//重巡洋艦クラス
public class HeavyCruiser : Ship
{
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
        if (GameSceneManager.Instance.State == GameSceneManager.BattleState.Finish) return;

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
            StartCoroutine(Bombardment(CannonBalls[0], 4,0.05f));
        }
        
        //固有砲撃(敵は発射させない)
        if (bom_time[1] > UniqueCharge && !IsEnemy)
        {
            bom_time[1] = 0;
            //固有弾幕展開
            StartCoroutine(BombardmentGoldenRectangle());
        }
    }


    //黄金長方形弾幕展開(固有弾幕)
    private IEnumerator BombardmentGoldenRectangle()
    {
        Debug.Log(Name + ":固有弾幕展開!");

        //連射回数
        int n = CannonBalls[1].attack.contisous;

        //弾幕展開
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            var ball = Instantiate(CannonBalls[1]) as CannonBall_GoldenRectangle;
            ball.SetPositionLayer(IsEnemy);
            ball.Create(MyShip.transform.position, 1);
        }
    }
}
