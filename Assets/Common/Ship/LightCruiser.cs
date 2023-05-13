using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Manager;

//軽巡洋艦クラス
public class LightCruiser :Ship
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
        //拡散弾発射 +3秒
        if (bom_time[1] > (Charge + 3))
        {
            bom_time[1] = 0;
            StartCoroutine(BombardmentDiffusion(CannonBalls[1], 4, 1.5f, 5, -10, 0.0f));
        }
        
        //固有砲撃(敵は発射させない)
        if (bom_time[2] > UniqueCharge && !IsEnemy)
        {
            bom_time[2] = 0;
            //固有弾幕展開
            StartCoroutine(BombardmentLateBall());
        }
    }

    //黄金長方形弾幕展開(固有弾幕)
    private IEnumerator BombardmentLateBall()
    {
        //ShipState = Ship.State.Battle;
        Debug.Log(Name + ":固有弾幕展開!");

        //連射回数
        int n = CannonBalls[2].attack.contisous;

        int offset = -90;

        //弾幕展開
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            //現在位置
            Vector3 pos = this.gameObject.transform.position;

            //発射する弾
            var ball = Instantiate(CannonBalls[2]) as CannonBall_Late;
            ball.SetPositionLayer(IsEnemy);
            try
            {
                ball.SetTarget(Target.gameObject, this.gameObject);
                //円形状に弾を配置
                ball.SetPosition(pos +
                                 new Vector3(
                                     2 * Mathf.Cos((i * 10 + offset) * pi / 180),
                                     2 * Mathf.Sin((i * 10 + offset) * pi / 180)));
                //発射するまで親子関係で追従
                ball.transform.parent = this.gameObject.transform;
            }
            catch (Exception e)
            {
                break;
            }
        }
    }
}
