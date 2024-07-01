using UnityEngine;
using UnityEngine.UI;

public class RoulettePickImage : MonoBehaviour
{
    private Image _image = null;

    private void OnEnable()
    {
        _image = GetComponent<Image>();

        int gunImgNum = Random.Range(0, (int)Define.GunType.MaxCount);

        _image.sprite = Managers.Resource.Load<Sprite>($"Img_Gun{gunImgNum}");

        Managers.Game.GameSceneUI.Roulette.PickImg((Define.GunType)gunImgNum);
    }
}
