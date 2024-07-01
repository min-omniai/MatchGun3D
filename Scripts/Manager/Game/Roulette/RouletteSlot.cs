using UnityEngine;

public class RouletteSlot : MonoBehaviour
{
    private Transform _imageStartPoint = null;
    private Transform _imageEndPoint = null;
    public float EndSpawnY => _imageEndPoint.localPosition.y;

    private GameObject _startImgPrefab = null;
    private GameObject _pickImgPrefab = null;

    public void Init()
    {
        if (_imageStartPoint == null) _imageStartPoint = Util.FindChild<Transform>(gameObject, "StartSpawnTf");
        if (_imageEndPoint == null) _imageEndPoint = Util.FindChild<Transform>(gameObject, "EndSpawnTf");

        Clear();
    }

    public void GameStartMatch(int forCount, int slotNum)
    {
        if (_startImgPrefab != null) Managers.Resource.Destroy(_startImgPrefab.gameObject);

        StartCoroutine(SpawnGunImagesRoutine(forCount, slotNum));
    }

    private System.Collections.IEnumerator SpawnGunImagesRoutine(int forCount, int slotNum)
    {
        for (int i = 0; i < forCount; i++)
        {
            GameObject obj = Managers.Resource.Instantiate("Img_Gun", transform);
            obj.transform.localPosition = _imageStartPoint.localPosition;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(true);

            yield return Util.GetWaitForSeconds(.075f);
        }

        PickImg();

        if (slotNum == 2)
        {
            yield return Util.GetWaitForSeconds(1f);

            Managers.Game.GunSpawnManager.SpawnGun();
            Managers.Game.GameSceneUI.SetRouletteActive(false);
        }
    }

    private void PickImg()
    {
        if (_pickImgPrefab == null) Managers.Resource.Instantiate("PickImg", transform);
    }

    public void Clear()
    {
        if (_startImgPrefab == null) _startImgPrefab = Managers.Resource.Instantiate("StartImg", transform);
        if (_pickImgPrefab != null) Managers.Resource.Destroy(_pickImgPrefab.gameObject);
    }
}
