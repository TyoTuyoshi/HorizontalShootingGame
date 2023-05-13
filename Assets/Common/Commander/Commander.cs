using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Commander : MonoBehaviour
{
    //敵の艦隊リスト取得のため
    [SerializeField] private EnemyCommander enemy_commander;

    //艦隊の編隊
    [FormerlySerializedAs("KANTAI")] [FormerlySerializedAs("KAN_SEN")] [Header("艦隊")] public List<Ship> MyShips = new List<Ship>();

    //ジョイスティック入力
    private Vector2 move = new Vector2();

    //全滅フラグ
    [System.NonSerialized] public bool Annihilation = false;
    
    //追従開始距離(艦隊距離間隔)
    [Header("艦隊間隔距離"),SerializeField, Range(0.1f, 1.0f)] private float distance = 0.75f;
    [Header("間隔変動許可"),SerializeField] private bool can_ramble = false;

    //艦隊の移動可能範囲
    [Header("移動制御範囲"), SerializeField] private Ship.RangePos range;

    private InputAction move_input;
    private void Start()
    {
        //ゲームマネージャから艦船元を取得
        var ships = GameManager.Instance.PlayAbleShip;
        //インスタンスの生成
        foreach (var ship in ships)
        {
            MyShips.Add(Instantiate(ship) as Ship);
        }

        Debug.Log("debug");
        foreach (var s in ships)
        {
            Debug.Log(s.name);
        }
        
        PlayerInput playerInput = GetComponent<PlayerInput>();
        move_input = playerInput.currentActionMap["Move"];
        Debug.Log(move_input.ReadValue<Vector2>());

        //移動制御可能範囲の指定
        foreach (var ship in MyShips)
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
        //joystick_input = Gamepad.current.leftStick.ReadValue();
        move = move_input.ReadValue<Vector2>();
        //joystick_input = Vector2.one;
    }

    //破棄対象のオブジェクトリスト
    private List<Ship> disposal_list = new List<Ship>();

    //ターゲットリストから最近距離にいる敵のインデックスを返す。
    public int ClosestTargetIndex(Ship ship,List<Ship> targets)
    {
        List<float> distances = new List<float>();

        for (int i = 0; i < targets.Count; i++)
        {
            float distance =
                (targets[i].transform.position -
                 ship.transform.position).magnitude;
            distances.Add(distance);
        }

        //最近距離ターゲットのインデックスを返す
        int target_index = distances.IndexOf(distances.Min());

        return target_index;
    }

    //ゲーム更新
    private void UpdateGame()
    {
        //全滅フラグオン
        if (MyShips.Count == 0) Annihilation = true;
        //バトルが終了ならリターン
        if (GameSceneManager.Instance.State == GameSceneManager.BattleState.Finish) return;
        //艦隊が減るほどスピードが早くなる
        speed = ShipSpeed(MyShips);
        time += Time.deltaTime;

        //カメラの追従対象に指定
        try
        {
            TrackingCamera.Instance.Target = MyShips[0].gameObject;
        }
        catch (Exception e)
        {
            return;
        }

        //砲撃対象の指揮
        //各艦船からの最近距離の敵をマーク
        {
            //敵艦隊のフェーズは全滅する度に、先頭のフェーズにシフトするため参照可能
            try
            {
                var try_targets = enemy_commander.EnemyPhases[0].EnemyShips;
            }
            catch (Exception e)
            {
                return;
            }
            var targets = enemy_commander.EnemyPhases[0].EnemyShips;

            if (targets.Count != 0)
            {
                foreach (var ship in MyShips)
                {
                    //最近距離ターゲットのインデックスを取得
                    int index = ClosestTargetIndex(ship, targets);
                    ship.Target = targets[index];
                }
            }
        }
        //撃沈判定
        for (int i = MyShips.Count - 1; i >= 0; i--)
        {
            if (MyShips[i].ShipState == Ship.State.Sunk)
            {
                //破棄リスト送り
                //disposal_list.Add(KANTAI[i].KANSEN);
                disposal_list.Add(MyShips[i]);
                MyShips.Remove(MyShips[i]);
            }
        }
        
        //艦隊の並びに描画順変更
        {
            //上向き移動時最奥順
            if (move.y <= 0)
            {
                int i = MyShips.Count - 1;
                foreach (var ship in MyShips)
                {
                    ship.Renderer.sortingOrder = i;
                    i--;
                }
            }
            //下向き移動(アイドル含む)時描画順
            else if (move.y > 0)
            {
                int i = 0;
                foreach (var ship in MyShips)
                {
                    ship.Renderer.sortingOrder = i;
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
        //バトルが終了ならリターン
        if (GameSceneManager.Instance.State == GameSceneManager.BattleState.Finish) return;
        
        //各艦船の移動指揮
        for (int i = 0; i < MyShips.Count; i++)
        {
            //先頭艦のみ操作
            if (i == 0)
            {
                MyShips[i].Move(move, speed);
            }
            else
            {
                //任意の距離を設定し、その距離内であれば追従しない。距離を越えた場合追従を始める。
                MyShips[i].MoveFollowing(MyShips[i - 1].transform.position, distance);
            }
        }
    }

    //艦船オブジェクト破棄
    private void DestoryObject()
    {
        foreach (var disp_obj in disposal_list)
        {
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
