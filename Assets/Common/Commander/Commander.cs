using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Commander : MonoBehaviour
{
    //艦隊の編隊
    [FormerlySerializedAs("KAN_SEN")] [Header("艦隊")] public List<Ship> KANTAI = new List<Ship>();

    //ジョイスティック入力
    private Vector2 joystick_input = new Vector2();

    //全滅フラグ
    [System.NonSerialized] public bool Annihilation = false;
    
    //追従開始距離(艦隊距離間隔)
    [Header("艦隊間隔距離"),SerializeField, Range(0.1f, 1.0f)] private float distance = 0.75f;
    [Header("間隔変動許可"),SerializeField] private bool can_ramble = false;

    //艦隊の移動可能範囲
    [Header("移動制御範囲"), SerializeField] private Ship.RangePos range;

    private void Start()
    {
        //移動制御可能範囲の指定
        foreach (var ship in KANTAI)
        { ship.Range = range; }
    }

    private void Update()
    {
        ProcessInput();
        UpdateGame();
        DestoryObject();
    }

    private void FixedUpdate()
    {
        FixedGameUpdate();
    }

    //総合速力
    private float speed = 0.0f;
    private float time = 0.0f;

    //操作入力
    private void ProcessInput()
    {
        joystick_input = Gamepad.current.leftStick.ReadValue();
    }

    //破棄対象のオブジェクトリスト
    //private List<GameObject> disposal_list = new List<GameObject>();
    private List<Ship> disposal_list = new List<Ship>();

    
    //ゲーム更新
    private void UpdateGame()
    {
        //艦隊が減るほどスピードが早くなる
        speed = ShipSpeed(KANTAI);
        time += Time.deltaTime;
       
        //全滅判定
        if (KANTAI.Count == 0)
        {
            Debug.Log("全滅...");
            Annihilation = true;
        }

        //撃沈判定
        for (int i = KANTAI.Count - 1; i >= 0; i--)
        {
            if (KANTAI[i].ShipState == Ship.State.Sunk)
            {
                //破棄リスト送り
                //disposal_list.Add(KANTAI[i].KANSEN);
                disposal_list.Add(KANTAI[i]);
                KANTAI.Remove(KANTAI[i]);
            }
        }
        
        //艦隊の並びに描画順変更
        {
            //上向き移動時最奥順
            if (joystick_input.y <= 0)
            {
                int i = KANTAI.Count - 1;
                foreach (var ship in KANTAI)
                {
                    ship.GetComponent<Renderer>().sortingOrder = i;
                    i--;
                }
            }
            //下向き移動(アイドル含む)時描画順
            else if (joystick_input.y > 0)
            {
                int i = 0;
                foreach (var ship in KANTAI)
                {
                    ship.GetComponent<Renderer>().sortingOrder = i;
                    i++;
                }
            }
        }

        //艦隊の間隔がSin周期で変動(ユニークオプション)
        if (can_ramble)
        {
            distance = 0.3f + (1.0f - Mathf.Abs(Mathf.Sin(time)));
        }
    }

    //物理系コンポーネント更新
    private void FixedGameUpdate()
    {
        //各艦船の移動指揮
        for (int i = 0; i < KANTAI.Count; i++)
        {
            //先頭艦のみ操作
            if (i == 0)
            {
                KANTAI[i].Move(joystick_input, speed);
            }
            else
            {
                //任意の距離を設定し、その距離内であれば追従しない。距離を越えた場合追従を始める。
                KANTAI[i].MoveFollowing(KANTAI[i - 1].transform.position, distance);
            }
        }
    }

    //オブジェクト破棄
    private void DestoryObject()
    {
        foreach (var disp_obj in disposal_list)
        {
            //Destroy(disp_obj);
            disp_obj.Destory();
        }
        disposal_list.Clear();
    }
    
    //速力コスト(前衛艦隊のみ)
    private float ShipSpeed(List<Ship> _ships)
    {
        float weight = 0;//重さ
        foreach (var ship in _ships)
        {
            switch (ship.ShipType)
            {
                case Ship.Type.Destroyer: //駆逐艦
                    weight += 1.0f;
                    break;
                case Ship.Type.LCruiser: //軽巡洋艦
                    weight += 2.0f;
                    break;
                case Ship.Type.HCruiser: //重巡洋艦
                    weight += 3.0f;
                    break;
            }
        }
        //総合速力
        float ships_speed = 9.0f / weight;
        return ships_speed;
    }
}
