using System.Collections.Generic;
using UnityEngine;

public class RouletteManager : MonoBehaviour
{
    private RouletteSlot[] _slots = null;

    private readonly int _maxSlotCount = 3;
    public float EndSpawnY => _slots[0].EndSpawnY;

    private List<Define.GunType> _gunTypeList = new List<Define.GunType>();
    public List<Define.GunType> GunTypeList => _gunTypeList;

    public void Init()
    {
        _slots = new RouletteSlot[_maxSlotCount];

        for (int i = 0; i < _maxSlotCount; i++)
        {
            _slots[i] = transform.GetChild(i).GetComponent<RouletteSlot>();
            _slots[i].Init();
        }

        _gunTypeList.Clear();
    }


    public void GameStartMatch()
    {
        for (int i = 0; i < _maxSlotCount; i++)
            _slots[i].GameStartMatch(12 + i * 5, i);
    }

    public void PickImg(Define.GunType gunType)
    {
        _gunTypeList.Add(gunType);
    }

    public void Clear()
    {
        _gunTypeList.Clear();

        for (int i = 0; i < _maxSlotCount; i++)
            _slots[i].Clear();
    }
}
