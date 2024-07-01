using System.Collections.Generic;
using UnityEngine;

public class GunSpawnManager : MonoBehaviour
{
    private List<GameObject> _gunList = new List<GameObject>();

    public void SpawnGun()
    {
        SpawnGun(Define.SpawnCount);
    }

    ///<summary>spawnCount정을 같은 종류 2정씩 짝지어 스폰</summary>
    public void SpawnGun(int spawnCount)
    {
        List<Define.GunType> gunTypeList = Managers.Game.GameSceneUI.GunTypeList;

        for (int i = 0; i < spawnCount / 2; i++)
        {
            Define.GunType gunType = gunTypeList[Random.Range(0, 3)];

            GameObject gun1 = Managers.Resource.Instantiate(gunType.ToString(), transform);
            gun1.transform.position += GetSpawnOffset();
            gun1.GetOrAddComponent<MatchGun>().Init(gunType);

            GameObject gun2 = Managers.Resource.Instantiate(gunType.ToString(), transform);
            gun2.transform.position += GetSpawnOffset();
            gun2.GetOrAddComponent<MatchGun>().Init(gunType);

            _gunList.Add(gun1);
            _gunList.Add(gun2);
        }
    }

    private Vector3 GetSpawnOffset()
    {
        float x = Random.Range(-4, 4);
        float z = Random.Range(-5, 5);

        return new Vector3(x, 0, z);
    }
}
