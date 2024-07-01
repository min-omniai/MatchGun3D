using UnityEngine;
using System.Collections.Generic;

public class MatchPlayManager : MonoBehaviour
{
    private const int SpawnIncreasePerRound = 4;

    private int _currentGunCount = Define.SpawnCount;
    private int _round = 1;

    private Transform _gunPos1 = null;
    private Transform _gunPos2 = null;
    private Transform _matchPos = null;

    private GameObject[] _selectedObjects = null;
    private MatchGun[] _selectedGuns = null;

    public static Vector3 MatchScale = Vector3.one * 2.1f;
    public static Vector3 OriginScale = Vector3.one * 3.0f;

    private List<GameObject> _gunInfoList = new List<GameObject>();
    private Dictionary<Define.GunType, GameObject> _gunInfoByType = new Dictionary<Define.GunType, GameObject>();


    public void Init()
    {
        if (_gunPos1 == null) _gunPos1 = Util.FindChild<Transform>(gameObject, "GunPos1", true);
        if (_gunPos2 == null) _gunPos2 = Util.FindChild<Transform>(gameObject, "GunPos2", true);
        if (_matchPos == null) _matchPos = Util.FindChild<Transform>(gameObject, "MatchPos", true);

        _selectedObjects = new GameObject[2];
        _selectedGuns = new MatchGun[2];

        _currentGunCount = Define.SpawnCount;
        _round = 1;

        Clear();
    }

    public void Selected(GameObject gunObj)
    {
        if (_selectedObjects[0] == null)
        {
            _selectedObjects[0] = gunObj;
            _selectedGuns[0] = _selectedObjects[0].GetComponent<MatchGun>();
            _selectedGuns[0].Selected(_gunPos1);
        }
        else if (_selectedObjects[1] == null)
        {// Judge Match
            _selectedObjects[1] = gunObj;
            _selectedGuns[1] = _selectedObjects[1].GetComponent<MatchGun>();
            _selectedGuns[1].Selected(_gunPos2);

            Match(_selectedObjects);
        }
        else
        {
            Debug.Log("Match 판별중");
        }
    }

    private async void Match(GameObject[] matchObj)
    {
        await System.Threading.Tasks.Task.Delay(250);

        Define.GunType gunType1 = _selectedGuns[0].GunType;
        Define.GunType gunType2 = _selectedGuns[1].GunType;

        if (gunType1 == gunType2)
        {// Match !
            _selectedGuns[0].Match(_matchPos);
            _selectedGuns[1].Match(_matchPos);

            _currentGunCount -= 2;

            if (_currentGunCount <= 0)
                SpawnNextRound();

            CheckDicInfo(gunType1);
        }
        else
        {// Mismatch..
            _selectedGuns[0].Mismatch(Managers.Game.GunSpawnManager.transform);
            _selectedGuns[1].Mismatch(Managers.Game.GunSpawnManager.transform);
        }


        for (int i = 0; i < _selectedObjects.Length; i++)
        {
            if (_selectedObjects[i] != null)
            {
                _selectedObjects[i] = null;
                _selectedGuns[i] = null;
            }
        }
    }

    ///<summary>필드를 다 비우면 라운드를 올리고, 이전 라운드보다 SpawnIncreasePerRound정 많이 스폰</summary>
    private void SpawnNextRound()
    {
        _round++;

        int spawnCount = Define.SpawnCount + (_round - 1) * SpawnIncreasePerRound;
        Managers.Game.GunSpawnManager.SpawnGun(spawnCount);

        _currentGunCount = spawnCount;
    }

    private void CheckDicInfo(Define.GunType gunType)
    {
        if (!_gunInfoByType.ContainsKey(gunType))
        {
            GameObject matchInfo = Managers.Resource.Instantiate("UI_MatchInfo", Managers.Game.GameSceneUI.MatchInfoRoot.transform);
            matchInfo.GetComponent<MatchInfo>().Init(gunType);

            _gunInfoList.Add(matchInfo);
            _gunInfoByType.Add(gunType, matchInfo);
        }
        else
        {
            _gunInfoByType[gunType].GetComponent<MatchInfo>().Counting();
        }
    }

    private void Clear()
    {
        for (int i = 0; i < _selectedObjects.Length; i++)
        {
            if (_selectedObjects[i] != null)
            {
                _selectedObjects[i] = null;
                _selectedGuns[i] = null;
            }
        }

        foreach (var list in _gunInfoList)
        {
            if (list != null)
                Managers.Pool.Push(list.GetOrAddComponent<Poolable>());
        }

        _gunInfoList.Clear();
        _gunInfoByType.Clear();
    }
}
