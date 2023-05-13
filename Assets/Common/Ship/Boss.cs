using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Manager;

public class Boss : Ship
{
    private void Start()
    {
        InitStatus();
    }

    //ステータス初期化
    private void InitStatus()
    {
        //ボス用の弾幕拡張用待ち時間
        Array.Resize(ref bom_time, 5);
        //改めて初期化
        bom_time = new[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    }

    private void Update()
    {
        UpdateGame();
    }
    
    /// <summary>
    /// ゲーム更新
    /// </summary>
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
            //通常砲撃(砲弾オブジェクト配置) 8連砲
            StartCoroutine(Bombardment(CannonBalls[0], 8,0.15f));
        }
        //拡散弾発射バージョン1 +3秒
        if (bom_time[1] > (Charge + 3))
        {
            bom_time[1] = 0;
            StartCoroutine(BombardmentDiffusion(CannonBalls[1], 14, 1.5f, 5, -10, 0.0f));
        }
        
        //拡散弾発射バージョン2 +5秒
        if (bom_time[2] > (Charge + 5))
        {
            bom_time[2] = 0;
            StartCoroutine(BombardmentDiffusion(CannonBalls[1], 4, 1.5f, 15, -30, 0.2f));
        }
        
        //全段発射バージョン1 +7秒
        if (bom_time[3] > (Charge + 7))
        {
            bom_time[3] = 0;
            //固有弾幕展開(全方位弾)
            StartCoroutine(BombardmentDiffusion(CannonBalls[1], 18, 1.0f, 20, 0, 0.2f));
        }

        //固有弾幕展開(全方位弾)
        //全段発射バージョン2
        if (bom_time[4] > UniqueCharge)
        {
            bom_time[4] = 0;
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(BombardmentDiffusion(CannonBalls[1], 36, 1.0f, 10, 0, 0.0f));
            }
        }
    }
}
