using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SortOption
{
    Level,//レベル
    Power,//火力
    Name//名前
}
/// <summary>
/// 艦船のデータベース(スクリプト)
/// 全ての艦船プレハブはここに置いてます。
/// 編隊時にここのデータベースから艦船を取得します。
/// </summary>
public class ShipDataBase : Singleton<ShipDataBase>
{
    /// <summary>
    /// ソートするオプション
    /// </summary>
    [SerializeField] private List<Ship> shipDB = null;

    public List<Ship> ShipDB
    {
        get { return shipDB; }
    }

    public void Add(Ship ship)
    {
        shipDB.Add(ship);
    }

    public void Remove(Ship ship)
    {
        shipDB.Remove(ship);
    }

    public void Sort(SortOption option)
    {
        switch (option)
        {
            default:
            case SortOption.Level:
                shipDB.Sort((a, b) => a.Level - b.Level);
                break;
            case SortOption.Power:
                shipDB.Sort((a, b) => a.Power - b.Power);
                break;
            case SortOption.Name:
                shipDB.Sort((a, b) => string.Compare(a.name, b.name));
                break;
        }
        shipDB.Sort();
    }
}