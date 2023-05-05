using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall_GoldenRectangle : CannonBall
{
    private Vector2 pos = new Vector2();
    private float velocity = 0.0f;

    /// <summary>
    /// 黄金回転用の砲弾作成セットアップ
    /// </summary>
    /// <param name="pos">基準座標</param>
    /// <param name="velocity">速度</param>
    public override void Create(Vector2 pos, float velocity)
    {
        this.pos = pos;
        this.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGame();
    }

    private int rad = 1;
    private float theta = 0.0f;
    
    //フィボナッチ数列用
    private int f0 = 0, f1 = 1;

    private int cycle = 0;
    //更新
    private void UpdateGame()
    {
        if (rad >= 21.0f)
        {
            Destroy(MyBom);
        }
        theta += 3.0f;
        //いい感じの式が思い浮かばなかった。
        if ((int) theta % 180 == 0)
        {
            rad = f1 + f0;
            f0 = f1;
            f1 = rad;
            switch (rad)
            {
                case 2:
                    pos.x++;
                    break;
                case 3:
                    pos.y++;
                    break;
                case 5:
                    pos.x -= 2;
                    break;
                case 8:
                    pos.y -= 3;
                    break;
                case 13:
                    pos.x += 5;
                    break;
                case 21:
                    pos.y += 8;
                    break;
                case 34:
                    pos.x -= 13;
                    break;
            }
        }
        //移動
        rbody.MovePosition((Vector3) pos + new Vector3(Mathf.Cos(theta / 360 * Mathf.PI),
            Mathf.Sin(theta / 360 * Mathf.PI)) * rad);
    }
}
