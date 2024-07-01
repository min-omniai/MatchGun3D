using UnityEngine;

public class RouletteImage : MonoBehaviour
{
    private void Update()
    {
        if (transform.localPosition.y > Managers.Game.GameSceneUI.Roulette.EndSpawnY)
            transform.Translate(Vector3.down * Time.deltaTime * Define.RouletteGunImageSpeed);
        else
            Managers.Resource.Destroy(gameObject);
    }
}
