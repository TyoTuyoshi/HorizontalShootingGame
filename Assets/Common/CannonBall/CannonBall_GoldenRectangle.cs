using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall_GoldenRectangle : CannonBall
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector2 pos = new Vector2();
    private float velocity = 0.0f;

    
    /// <summary>
    /// 砲弾作成セットアップ
    /// </summary>
    /// <param name="pos">基準座標</param>
    /// <param name="velocity">速度</param>
    private void Create(Vector2 pos, float velocity)
    {
        this.pos = pos;
        this.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGame();
    }

    //更新
    private void UpdateGame()
    {
        
    }
}
