using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Enemyの艦隊チームのクラス
[System.Serializable]
public class Phase
{
   [SerializeField] public List<Ship> EnemyShips = new List<Ship>();
}
public class EnemyCommander : MonoBehaviour
{
   //プレイヤーの艦隊リスト取得のため
   [SerializeField] private Commander player_commander;
   
   //敵艦隊の形態リスト
   public List<Phase> EnemyPhases = new List<Phase>();

   //敵艦隊の番号(フェーズ)
   private const int current_index = 0;

   //破棄対象のオブジェクトリスト
   private List<Ship> disposal_list = new List<Ship>();

   void Start()
   {
      //フェーズの初期化
      InitPhase();
   }

   void Update()
   {
      UpdateGame();
      DestoryObject();
   }

   /// <summary>
   ///ゲームの更新 
   /// </summary>
   private void UpdateGame()
   {
      Phase current_phase = EnemyPhases[current_index];
      var current_ship = current_phase.EnemyShips;

      //フェーズ全滅時の処理
      if (IsAnnihilation(current_phase))
      {
         Debug.Log("敵艦隊全滅...");
         EnemyPhases.Remove(EnemyPhases[current_index]);
         //艦隊フェーズのシフト
         ShiftCurrentPhase();
      }

      //現在のフェーズの艦隊リスト
      //撃沈判定
      for (int i = current_ship.Count - 1; i >= 0; i--)
      {
         if (current_ship[i].ShipState == Ship.State.Sunk)
         {
            //破棄リスト送り
            disposal_list.Add(current_ship[i]);
            current_ship.Remove(current_ship[i]);
         }
      }
      
      //砲撃対象の指揮
      //各艦船からの最近距離の敵をマーク
      {
         var targets = player_commander.KANTAI;
         if (targets.Count != 0)
         {
            foreach (var ship in current_ship)
            {
               //最近距離ターゲットのインデックスを取得
               int index = player_commander.ClosestTargetIndex(ship, targets);
               ship.Target = targets[index];
            }
         }
         else
         {
            
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

   /// <summary>
   /// フェーズの初期化(艦隊の"Active"を"false")
   /// </summary>
   private void InitPhase()
   {
      //第一フェーズの艦隊は表示するため[0]をスキップ
      for (int i = 1; i < EnemyPhases.Count; i++)
      {
         //各フェーズの艦隊
         var phase_ships = EnemyPhases[i].EnemyShips; 
         for (int j = 0; j < phase_ships.Count; j++)
         {
            phase_ships[j].gameObject.SetActive(false);
         }
      }
   }

   /// <summary>
   ///艦隊全滅時の次の敵艦隊フェーズの出現
   /// </summary>
   private void ShiftCurrentPhase()
   {
      //カレントフェーズの艦隊
      var phase_ships = EnemyPhases[current_index].EnemyShips;
      //次のフェーズの艦隊の"Active"を"true"にする
      for (int i = 0; i < phase_ships.Count; i++)
      {
         phase_ships[i].gameObject.SetActive(true);
      }
   }

   /// <summary>
   /// 全滅したか判定する関数
   /// </summary>
   /// <param name="ships">判定する艦隊</param>
   /// <returns>艦隊が全滅したか?true:false</returns>
   private bool IsAnnihilation(Phase phases)
   {
      return (phases.EnemyShips.Count == 0);
   }
}
