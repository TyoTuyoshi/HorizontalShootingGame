using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

//駆逐艦クラス
public class Destroyer : Ship
{
    //砲撃する砲弾
    [SerializeField] private CannonBall cannon_ball;

    private void Start()
    {
        InitStatus();
    }

    //ステータス初期化
    private void InitStatus()
    {
    }

    private float bom_time = 0.0f; //砲撃間隔カウンタ

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
            StartCoroutine(Bombardment());
        }
    }

    //砲撃(砲弾オブジェクト配置)
    public override IEnumerator Bombardment()
    {
        ShipState = Ship.State.Battle;
        Debug.Log(Name + ":こうげき！");

        for (int i = 0; i <= cannon_ball.ContinuousCanon.contisous; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            Debug.Log(Name + ":発射!");
            var ball = Instantiate(cannon_ball) as CannonBall;
            ball.Create(new Vector2(2, 0), 1);
            //砲弾配置(弾幕生成)
            ball.transform.position =
                this.gameObject.transform.position +
                new Vector3(0, Mathf.Cos(Mathf.PI / 180 * i * 15), 0);

            ball = Instantiate(cannon_ball) as CannonBall;
            ball.Create(new Vector2(2, 0), 20);
            //砲弾配置(弾幕生成)
            ball.transform.position =
                this.gameObject.transform.position +
                new Vector3(0, -Mathf.Cos(Mathf.PI / 180 * i * 15), 0);
        }
    }

    //魚雷攻撃
    public override void TorpedoLaunch()
    {
        
    }
}
