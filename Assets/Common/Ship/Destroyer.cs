using System.Collections;
using System.Linq;
using UnityEngine;

//駆逐艦クラス
public class Destroyer : Ship
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
        //砲撃時間の加算
        bom_time = bom_time.Select(i => i + Time.deltaTime).ToArray();
        
        //通常砲撃
        if (bom_time[0] > Charge)
        {
            bom_time[0] = 0;
            //通常砲撃(砲弾オブジェクト配置) 6連砲
            StartCoroutine(Bombardment(CannonBalls[0], 6, 0.05f));
        }
        //拡散弾発射
        if (bom_time[1] > (Charge+3))
        {
            bom_time[1] = 0;
            StartCoroutine(BombardmentDiffusion(CannonBalls[1], 6, 1.5f, 5, -15, 0.0f));
        }
        //固有砲撃
        if (bom_time[2] > UniqueCharge && !IsEnemy)
        {
            bom_time[2] = 0;
            //固有弾幕展開
            StartCoroutine(BombardmentCrossCoswave());
        }
    }
    
    //二連Cos波形連続砲撃(固有弾幕)
    private IEnumerator BombardmentCrossCoswave()
    {
        //ShipState = Ship.State.Battle;
        Debug.Log(Name + ":固有弾幕展開!");
        //発射時の座標

        //連射回数
        int n = CannonBalls[0].ContinuousCanon.contisous;

        //演算用円周率
        float pi = Mathf.PI;

        //弾幕展開
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            for (int j = 0; j < 2; j++)
            {
                //現在位置
                Vector3 pos = MyShip.transform.position;

                //発射する弾
                var ball = Instantiate(CannonBalls[0]) as CannonBall_Normal;
                ball.SetPositionLayer(IsEnemy);
                ball.SetTarget(Vector2.right);
                
                //Y軸上下弾配置のための変数
                //int y_upper = (j == 0) ? 1 : -1;
                int y_upper = (int)Mathf.Pow(-1, j);

                //砲弾配置(弾幕生成)
                ball.SetPosition(pos +
                                 new Vector3(
                                     0, y_upper * Mathf.Cos(pi / 180 * i * 15)), MyShip);
                //追従のための親子関係
                ball.transform.parent = MyShip.transform;
            }
        }
    }

    //魚雷攻撃
    public override void TorpedoLaunch()
    {
        
    }
}
