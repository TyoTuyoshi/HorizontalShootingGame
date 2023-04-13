using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//駆逐艦クラス
public class Destroyer : Ship
{
    //砲撃する砲弾
    public GameObject Canon;
    private CannonBall CanonSource;
    
    private void Start()
    {
        InitStatus();
        CanonSource = Canon.GetComponent<CannonBall>();
    }

    //ステータス初期化
    private void InitStatus()
    {
        //Debug.Log(ShipType.GetHashCode());
    }

    private float bom_time = 0.0f;//砲撃間隔カウンタ
    private void Update()
    {
        UpdateGame();
    }

    //ゲーム更新
    private void UpdateGame()
    {
        //砲撃間隔
        bom_time += Time.deltaTime;
        if (bom_time > Charge)
        {
            bom_time = 0;
            Bombardment();
        }
    }

    //砲撃
    public override void Bombardment()
    {
        ShipState = Ship.State.Battle;
        Debug.Log(Name + ":こうげき！");
        if (CanonSource.CanContinous)
        {
            for (int i = 0; i < CanonSource.ContinuousCanon.contisous; i++)
            {
                var canon_ball = Instantiate(Canon) as GameObject;
                canon_ball.transform.position =
                    this.gameObject.transform.position +
                    new Vector3(Mathf.Cos(2 * Mathf.PI / 180 * (i * 45)), Mathf.Sin(2 * Mathf.PI / 180 * (i * 10)), 0);
            }
        }
        else
        {
        }
    }

    //魚雷攻撃
    public override void TorpedoLaunch()
    {
        
    }
}
