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
        
        //固有砲撃
        if (bom_time[1] > UniqueCharge && !IsEnemy)
        {
            bom_time[1] = 0;
            //固有弾幕展開
            StartCoroutine(Bombardment_CrossCoswave());
        }
    }
    
    //二連Cos波形連続砲撃(固有弾幕)
    private IEnumerator Bombardment_CrossCoswave()
    {
        //ShipState = Ship.State.Battle;
        Debug.Log(Name + ":固有弾幕展開!");
        //発射時の座標
        //Vector3 set_pos = transform.position;

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
                Vector3 pos = this.gameObject.transform.position;

                //発射する弾
                var ball = Instantiate(CannonBalls[0]) as CannonBall_Normal;
                ball.SetPositionLayer(IsEnemy);
                //Debug.Log("LayerName = " + LayerMask.LayerToName(ball.gameObject.layer));
               
                ball.target = Vector2.right;
                //Y軸上下弾配置のための変数
                //int y_upper = (j == 0) ? 1 : -1;
                int y_upper = (int)Mathf.Pow(-1, j);

                //砲弾配置(弾幕生成)
                ball.transform.position = pos +
                                          new Vector3(
                                              0,
                                              y_upper * Mathf.Cos(pi / 180 * i * 15));
                //追従のための親子関係
                ball.transform.parent = this.gameObject.transform;
            }
        }
    }

    //魚雷攻撃
    public override void TorpedoLaunch()
    {
        
    }
}
