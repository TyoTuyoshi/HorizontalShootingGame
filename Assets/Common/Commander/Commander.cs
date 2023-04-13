using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Commander : MonoBehaviour
{
    //艦隊の編隊
    public List<Ship> KAN_SEN = new List<Ship>();

    //ジョイスティック入力
    private Vector2 joystick_input = new Vector2();

    private void Update()
    {
        ProcessInput();
        UpdateGame();
        DestoryObject();
    }

    //総合速力
    private float speed = 0;
    private void Awake()
    {
        speed = ShipSpeed(KAN_SEN);
    }

    //操作入力
    private void ProcessInput()
    {
        //joystick_input.x = Input.GetAxis("Horizontal");
        //joystick_input.y = Input.GetAxis("Vertical");
        joystick_input = Gamepad.current.leftStick.ReadValue();
    }
    
    //ゲーム更新
    private void UpdateGame()
    {
        //各艦船の移動指揮
        foreach (var ship in KAN_SEN)
        {
            ship.Move(joystick_input, speed, Random.Range(1f, 1.1f));
        }

        //Debug.Log(speed);
    }

    //オブジェクト破棄
    private void DestoryObject()
    {
        
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
