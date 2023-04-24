using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

//Enemyの艦隊チームのクラス
[System.Serializable]
public class Phase
{
   [SerializeField] public List<Ship> EnemyShip = new List<Ship>();
}
public class EnemyCommander : MonoBehaviour
{
   //敵艦隊の形態リスト
   [SerializeField] private List<Phase> phase = new List<Phase>();

   //敵艦隊の番号(フェーズ)
   private const int current_index = 0;

   //破棄対象のオブジェクトリスト
   private List<Ship> disposal_list = new List<Ship>();
   
   void Update()
   {
      UpdateGame();
      DestoryObject();
   }

   private void UpdateGame()
   {
      Phase current_phase = phase[current_index];
      //艦隊フェーズのシフト
      if (IsAnnihilation(current_phase))
      {
         Debug.Log("敵艦隊全滅...");
         phase.Remove(phase[current_index]);
      }

      var current_ship = current_phase.EnemyShip;

      for (int i = current_ship.Count - 1; i >= 0; i--)
      {
         if (current_ship[i].ShipState == Ship.State.Sunk)
         {
            //破棄リスト送り
            disposal_list.Add(current_ship[i]);
            current_ship.Remove(current_ship[i]);
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
   /// 全滅したか判定する関数
   /// </summary>
   /// <param name="ships">判定する艦隊</param>
   /// <returns></returns>
   private bool IsAnnihilation(Phase phases)
   {
      return (phases.EnemyShip.Count == 0);
   }
}
