using UnityEngine;
using UnityEngine.UI;

public class MatchInfo : MonoBehaviour
{
    private Image _gunImage = null;
    private Text _gunCountText = null;

    private int _count = 1;

    public void Init(Define.GunType gunType)
    {
        if (_gunImage == null) _gunImage = Util.FindChild<Image>(gameObject, "Img_Gun");
        if (_gunCountText == null) _gunCountText = Util.FindChild<Text>(gameObject, "Text_GunCount");

        _count = 1;
        _gunCountText.text = _count.ToString();

        switch (gunType)
        {
            case Define.GunType.HandGun:
                _gunImage.sprite = Managers.Resource.Load<Sprite>("Img_Gun0");
                break;

            case Define.GunType.Shotgun:
                _gunImage.sprite = Managers.Resource.Load<Sprite>("Img_Gun1");
                break;

            case Define.GunType.AutoRifle:
                _gunImage.sprite = Managers.Resource.Load<Sprite>("Img_Gun2");
                break;
        }
    }

    public void Counting()
    {
        _count += 1;
        _gunCountText.text = _count.ToString();
    }
}
